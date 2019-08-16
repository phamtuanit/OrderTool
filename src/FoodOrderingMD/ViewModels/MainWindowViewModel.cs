namespace FoodOrdering.ViewModels
{
    using System;
    using System.ComponentModel.Composition;
    using Caliburn.Micro;
    using Common;
    using MaterialDesignThemes.Wpf;
    using Views;

    [Export(typeof(MainWindowViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MainWindowViewModel : Screen
    {
        /// <summary>
        /// The snackbar message queue
        /// </summary>
        private SnackbarMessageQueue snackbarMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));

        /// <summary>
        /// Gets the snackbar message queue.
        /// </summary>
        /// <value>
        /// The snackbar message queue.
        /// </value>
        public SnackbarMessageQueue SnackbarMessageQueue
        {
            get { return this.snackbarMessageQueue; }
            set { this.snackbarMessageQueue = value; }
        }

        /// <summary>
        /// The active item
        /// </summary>
        private object activeItem;

        /// <summary>
        /// Gets or sets the active item.
        /// </summary>
        /// <value>
        /// The active item.
        /// </value>
        public object ActiveItem
        {
            get { return this.activeItem; }
            set
            {
                this.activeItem = value; this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// The food submition
        /// </summary>
        private FoodSubmitionViewModel foodSubmition;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
        }

        /// <summary>
        /// Activates the specified item.
        /// </summary>
        /// <param name="item">The item to activate.</param>
        public void ActivateItem(Screen item)
        {
            this.ActiveItem = item;
        }

        /// <summary>
        /// Abouts this instance.
        /// </summary>
        public async void About()
        {
            var aboutDialog = new AboutDialog();
            await DialogHost.Show(aboutDialog, "RootDialog");
        }

        /// <summary>
        /// Called when an attached view's Loaded event fires.
        /// </summary>
        /// <param name="view"></param>
        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            Execute.OnUIThread(() =>
            {
                var message = ConfigParameter.WelcomeMessage;
                this.SnackbarMessageQueue.Enqueue(message);

                this.foodSubmition = IoC.Get<FoodSubmitionViewModel>();
                this.ActivateItem(this.foodSubmition);
            });
        }

        /// <summary>
        /// Exits this instance.
        /// </summary>
        public void Exit()
        {
            Environment.Exit(-1);
        }

        /// <summary>
        /// Orders this instance.
        /// </summary>
        public void Order()
        {
            this.ActivateItem(this.foodSubmition);
        }

        /// <summary>
        /// Recent the request.
        /// </summary>
        public void RecentRequest()
        {
            var recentViewModel = IoC.Get<RecentRequestViewModel>();
            this.ActivateItem(recentViewModel);
        }

        /// <summary>
        /// Shows the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        public async void ShowError(string message, string title)
        {
            var aboutDialog = new ErrorDialog(title, message);
            await DialogHost.Show(aboutDialog, "RootDialog");
        }
    }
}
