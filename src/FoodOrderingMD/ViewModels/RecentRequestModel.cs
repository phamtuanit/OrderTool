namespace FoodOrdering.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using Caliburn.Micro;
    using Common;
    using Models;

    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RecentRequestViewModel : Screen
    {
        /// <summary>
        /// Gets or sets the recent request collection.
        /// </summary>
        /// <value>
        /// The recent request collection.
        /// </value>
        public ObservableCollection<Request> RecentRequestCollection { set; get; } = new ObservableCollection<Request>();

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
        /// Called when an attached view's Loaded event fires.
        /// </summary>
        /// <param name="view"></param>
        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            this.LoadData();
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            this.RecentRequestCollection.Clear();
            var pattern = $"{Environment.MachineName}{OderBehavior.SearchParten}";
            string folderPath = ConfigParameter.OrderOutput;
            var order = OderBehavior.CollectOrder(folderPath, pattern);

            foreach (var item in order)
            {
                this.RecentRequestCollection.Add(new Request(item));
            }
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

        /// <summary>
        /// Raises a change notification indicating that all bindings should be refreshed.
        /// </summary>
        public void RefreshData()
        {
            this.LoadData();
        }

        /// <summary>
        /// Selections the changed.
        /// </summary>
        /// <param name="source">The source.</param>
        public void SelectionChanged(object source)
        {
            var listView = source as ListView;
            if (listView != null && this.RecentRequestCollection.Count > 1)
            {
                if (listView.SelectedIndex >= this.RecentRequestCollection.Count - 1)
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
        /// Deletes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Delete(object item)
        {
            var request = item as Request;
            if (request != null)
            {
                try
                {
                    File.Delete(request.FilePath);
                    this.RecentRequestCollection.Remove(request);
                }
                catch (Exception ex)
                {
                    this.ShowError(ex.Message, "Cannot get recent requests");
                }
            }
        }
    }
}
