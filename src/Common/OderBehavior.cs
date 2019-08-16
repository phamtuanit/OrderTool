namespace Common
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// 
    /// </summary>
    public static class OderBehavior
    {
        public const string SearchParten = "*.Order";
        private const string DefaultExtention = ".Order";
        private const string NoComment = "No comment";

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

        /// <summary>
        /// Collects the result.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <returns></returns>
        public static ObservableCollection<OrderItem> CollectOrder(string folderPath, string searchPattern = SearchParten)
        {
            if (!Directory.Exists(folderPath))
            {
                return new ObservableCollection<OrderItem>(); ;
            }

            string[] files = Directory.GetFiles(folderPath, searchPattern);
            IList<OrderItem> orderList = new List<OrderItem>();

            foreach (string file in files)
            {
                GetOrderFromFile(file, orderList);
            }

            orderList = (from drinkItem in orderList
                         orderby drinkItem.DrinkName
                         select drinkItem).ToList();

            return new ObservableCollection<OrderItem>(orderList);
        }

        /// <summary>
        /// Gets the order from file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="orderList">The order list.</param>
        private static void GetOrderFromFile(string filePath, IList<OrderItem> orderList)
        {
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var stream = new StreamReader(fileStream);
            try
            {
                var orderItem = BuildOrderItem(stream);
                orderItem.FilePath = filePath;
                if (orderItem != null)
                {
                    orderList.Add(orderItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                stream.Close();
                stream.Dispose();
                fileStream.Close();
                fileStream.Dispose();
            }
        }

        /// <summary>
        /// Builds the order item.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        private static OrderItem BuildOrderItem(StreamReader stream)
        {
            OrderItem orderItem = new OrderItem();
            string line = stream.ReadLine();
            while (!string.IsNullOrWhiteSpace(line))
            {
                if (line.Contains("Name"))
                {
                    orderItem.UserName = stream.ReadLine();
                }
                else if (line.Contains("Comment"))
                {
                    var comment = stream.ReadLine();
                    if (string.IsNullOrEmpty(comment))
                    {
                        comment = NoComment;
                    }
                    orderItem.Comments = comment;
                }
                else if (line.Contains("Order count"))
                {
                    //TODO: do nothing
                }
                else if (line.Contains("Order"))
                {
                    var listDrink = BuildDrinkItems(stream);

                    if (listDrink != null)
                    {
                        orderItem.DrinkItems = listDrink;
                    }
                    else
                    {
                        MessageBox.Show($"Cannot build drink request for [{orderItem.UserName}]");
                        return null;
                    }
                }

                line = stream.ReadLine();
            }

            return orderItem;
        }

        /// <summary>
        /// Builds the drink items.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        private static IList<DrinkItem> BuildDrinkItems(StreamReader stream)
        {
            IList<DrinkItem> drinkItems = new List<DrinkItem>();
            string line = stream.ReadLine();
            char[] separator = { ';' };

            while (!string.IsNullOrWhiteSpace(line))
            {
                try
                {
                    var items = line.Split(separator);
                    var rinkItem = new DrinkItem
                    {
                        Cost = items[0].Trim(),
                        Name = items[1].Trim(),
                        Note = items[2].Trim()
                    };

                    drinkItems.Add(rinkItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning");
                    return null;
                }

                line = stream.ReadLine();
            }

            return drinkItems;
        }
    }
}
