namespace FoodOrdering.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using Caliburn.Micro;
    using Common;
    using MaterialDesignThemes.Wpf;
    using Models;
    using Views;
    using OderBehavior = OderBehavior;
    using System.Threading.Tasks;

    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class FoodSubmitionViewModel : Screen
    {
        /// <summary>
        /// The selected count
        /// </summary>
        private int selectedCount = 0;

        /// <summary>
        /// Gets or sets the drink collection.
        /// </summary>
        /// <value>
        /// The drink collection.
        /// </value>
        public ObservableCollection<DrinkItem> DrinkCollection { set; get; } = new ObservableCollection<DrinkItem>();

        /// <summary>
        /// The selected count
        /// </summary>
        public int SelectedCount
        {
            get { return this.selectedCount; }
            set { this.selectedCount = value; this.NotifyOfPropertyChange(); }
        }

        /// <summary>
        /// The user name
        /// </summary>
        private string userName = Environment.UserName;

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName
        {
            get { return this.userName; }
            set
            {
                this.userName = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// The comment
        /// </summary>
        private string comment = string.Empty;

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string Comment
        {
            get { return this.comment; }
            set
            {
                this.comment = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// The refresh vis
        /// </summary>
        private Visibility refreshVis = Visibility.Visible;

        /// <summary>
        /// Gets or sets the refresh vis.
        /// </summary>
        /// <value>
        /// The refresh vis.
        /// </value>
        public Visibility RefreshVis
        {
            get { return this.refreshVis; }
            set
            {
                this.refreshVis = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Selections the changed.
        /// </summary>
        /// <param name="item">The index.</param>
        public void Checked(DrinkItem item)
        {
            if (item.IsSelected)
            {
                this.SelectedCount++;
            }
            else
            {
                this.SelectedCount--;
            }
        }

        /// <summary>
        /// Gets the selected items.
        /// </summary>
        /// <value>
        /// The selected items.
        /// </value>
        private ObservableCollection<DrinkItem> SelectedItems
        {
            get
            {
                var list = this.DrinkCollection.Where(item => item.IsSelected);
                return new ObservableCollection<DrinkItem>(list);
            }
        }

        /// <summary>
        /// Raises a change notification indicating that all bindings should be refreshed.
        /// </summary>
        public void RefreshData()
        {
            var main = IoC.Get<MainWindowViewModel>();
            main.SnackbarMessageQueue.Enqueue("Refreshing data...");

            try
            {
                this.LoadDrinkData();
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message, "Error Was Occurred When Load Drink Data");
            }
        }

        /// <summary>
        /// Selections the changed.
        /// </summary>
        /// <param name="source">The source.</param>
        public void SelectionChanged(object source)
        {
            var listView = source as ListView;
            if (listView != null && this.DrinkCollection.Count > 1)
            {
                if (listView.SelectedIndex >= this.DrinkCollection.Count - 1)
                {
                    this.RefreshVis = Visibility.Collapsed;
                }
                else
                {
                    this.RefreshVis = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Submits this instance.
        /// </summary>
        public void Submit()
        {
            if (!ConfigParameter.MultiSelect && this.SelectedItems.Count > 1)
            {
                this.ShowError("You can only select one drink/food.", "Warning");
                return;
            }

            if (string.IsNullOrWhiteSpace(this.UserName))
            {
                this.ShowError("Please put your name before submitting your request.", "Warning");
                return;
            }

            if (this.SelectedItems == null || this.SelectedItems.Count <= 0)
            {
                this.ShowError("Please select at least one food/drink.", "Warning");
                return;
            }

            try
            {
                OderBehavior.SubMitRequest(ConfigParameter.OrderOutput, this.UserName, this.Comment, this.SelectedItems);
                var main = IoC.Get<MainWindowViewModel>();
                main.SnackbarMessageQueue.Enqueue("Your request has been submitted successfully.");
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message, "Error");
            }
        }

        /// <summary>
        /// Called when an attached view's Loaded event fires.
        /// </summary>
        /// <param name="view"></param>
        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                this.LoadDrinkData();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Loads the drink data.
        /// </summary>
        private void LoadDrinkData()
        {
            var data = OderBehavior.GetDrinkData(ConfigParameter.DataFile);
            this.SelectedCount = 0;
            this.DrinkCollection.Clear();

            foreach (var item in data)
            {
                this.DrinkCollection.Add(item);
            };
        }

        /// <summary>
        /// Shows the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        private void ShowError(string message, string title)
        {
            var main = IoC.Get<MainWindowViewModel>();
            main.ShowError(message, title);
        }
    }
}
