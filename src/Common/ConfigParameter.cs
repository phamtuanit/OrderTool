#region Reference



#endregion

namespace Common
{
    using System.Configuration;

    public static class ConfigParameter
    {
        private const string DefaultDataFile = @"\\hanoi\temp\TuanPham\DrinkData.txt";
        private const string DefaultExportFile = @"\\hanoi\temp\TuanPham\OrderResult.txt";
        private const string DefaultOutputPath = @"\\hanoi\temp\TuanPham\Temp";
        private const bool DefaultMultiSelect = false;

        /// <summary>
        /// Gets the temporary path.
        /// </summary>
        /// <value>
        /// The temporary path.
        /// </value>
        public static string OrderOutput
        {
            get
            {
                var tmpPath = ConfigurationManager.AppSettings.Get("Output");
                if (string.IsNullOrWhiteSpace(tmpPath))
                {
                    tmpPath = DefaultOutputPath;
                }
                return tmpPath;
            }
        }

        /// <summary>
        /// Gets the welcome message.
        /// </summary>
        /// <value>
        /// The welcome message.
        /// </value>
        public static string WelcomeMessage
        {
            get
            {
                var message = ConfigurationManager.AppSettings.Get("WelcomeMessage");
                if (string.IsNullOrWhiteSpace(message))
                {
                    message = "Welcome to Food Ordering Application!";
                }
                return message;
            }
        }

        /// <summary>
        /// Gets the data file.
        /// </summary>
        /// <value>
        /// The data file.
        /// </value>
        public static string DataFile
        {
            get
            {
                var fileName = ConfigurationManager.AppSettings.Get("DataFileName");
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = DefaultDataFile;
                }
                return fileName;
            }
        }

        /// <summary>
        /// Gets the export file.
        /// </summary>
        /// <value>
        /// The export file.
        /// </value>
        public static string ExportFile
        {
            get
            {
                var fileName = ConfigurationManager.AppSettings.Get("ExportFile");
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = DefaultExportFile;
                }
                return fileName;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [multi select].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [multi select]; otherwise, <c>false</c>.
        /// </value>
        public static bool MultiSelect
        {
            get
            {
                var multiSelectStr = ConfigurationManager.AppSettings.Get("MultiSelect").ToUpper();
                if (multiSelectStr == "TRUE")
                {
                    return true;
                }

                return DefaultMultiSelect;
            }
        }
    }
}