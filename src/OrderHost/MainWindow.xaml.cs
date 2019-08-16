namespace Collector
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows;
    using Common;
    using Properties;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const string SearchParten = "*.Order";
        private const string NoComment = "No comment";
        private ObservableCollection<OrderItem> orderItems;

        /// <summary>
        /// Gets or sets the order items.
        /// </summary>
        /// <value>
        /// The order items.
        /// </value>
        public ObservableCollection<OrderItem> OrderItems
        {
            set
            {
                this.orderItems = value;
                this.OnPropertyChanged();
            }
            get
            {
                return this.orderItems;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            this.OrderItems = new ObservableCollection<OrderItem>();
        }

        /// <summary>
        /// Handles the OnClick event of the Collect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Collect_OnClick(object sender, RoutedEventArgs e)
        {
            string folderPath = ConfigParameter.OrderOutput;
            this.OrderItems.Clear();
            this.OrderItems = OderBehavior.CollectOrder(folderPath);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            string pathFile = ConfigParameter.ExportFile;
            var fileStream = new FileStream(pathFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            var stream = new StreamWriter(fileStream, Encoding.UTF8);

            // Write data
            stream.WriteLine(string.Format("------------------- Count: {0} ------------------", this.OrderItems.Count));
            foreach (var orderItem in this.OrderItems)
            {
                var line = orderItem.UserName + ": \t\t\t" + orderItem.DrinkName;
                if (!string.IsNullOrWhiteSpace(orderItem.Comments) && orderItem.Comments.Trim() != NoComment)
                {
                    line += "(" + orderItem.Comments.Trim() + ")";
                }
                stream.WriteLine(line);
            }

            var groupByName = this.OrderItems.GroupBy((item) =>
            {
                return item.DrinkName;
            });

            stream.WriteLine();
            stream.WriteLine("------------------- Summary --------------------");
            foreach (var item in groupByName)
            {
                stream.Write("{0} | {1}:", item.Count(), item.Key);

                foreach (var drkItem in item)
                {
                    var comment = string.Empty;
                    if (!string.IsNullOrWhiteSpace(drkItem.Comments) && drkItem.Comments.Trim() != NoComment)
                    {
                        comment = "(" + drkItem.Comments.Trim() + ")";
                    }
                    stream.Write("{0}{1},", drkItem.UserName, comment);
                }
                stream.WriteLine();
            }

            // End
            stream.Close();
            stream.Dispose();
            fileStream.Close();
            fileStream.Dispose();
        }
    }
}
