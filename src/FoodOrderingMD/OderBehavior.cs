namespace FoodOrdering
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using Common;
    using Models;

    /// <summary>
    /// 
    /// </summary>
    public static class OderBehavior
    {
        private const string DefaultExtention = ".Order";

        /// <summary>
        /// Gets the drink data.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static IList<DrinkItem> GetDrinkData(string fileName)
        {
            var drinklist = new List<DrinkItem>();
            var ddataInFile = File.ReadAllLines(fileName);
            char[] separator = { ';' };

            foreach (var item in ddataInFile)
            {
                if (!item.StartsWith("//"))
                {
                    try
                    {
                        var items = item.Split(separator);
                        var rinkItem = new DrinkItem
                        {
                            Cost = items[0].Trim(),
                            Name = items[1].Trim(),
                            Available = bool.Parse(items[2].Trim()),
                            Note = items[3].Trim()
                        };

                        drinklist.Add(rinkItem);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Warning");
                    }
                }
            }

            return drinklist;
        }

        /// <summary>
        /// Submit request.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="username">The user-name.</param>
        /// <param name="comments">The comments.</param>
        /// <param name="selectedList">The selected list.</param>
        /// <returns></returns>
        public static bool SubMitRequest(string folderPath, string username, string comments,
            ObservableCollection<DrinkItem> selectedList)
        {
            var fileName = Environment.MachineName + "_" + username + DefaultExtention;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var filePath = Path.Combine(folderPath, fileName);

            string content = "------------------Name-------------------";
            content += Environment.NewLine + username;
            content += Environment.NewLine + "------------------Comment-------------------";
            content += Environment.NewLine + comments;
            content += Environment.NewLine + "------------------Order count---------------";
            content += Environment.NewLine + selectedList.Count;
            content += Environment.NewLine + "------------------Order---------------------";
            content = selectedList.Aggregate(content, (current, item) =>
                                                 current + (Environment.NewLine + item.Cost + "; " + item.Name + "; "
                                                 + item.Note));

            try
            {
                // Create result file
                using (var file = new StreamWriter(filePath, false))
                {
                    file.Write(content);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                return false;
            }

            return true;
        }
    }
}
