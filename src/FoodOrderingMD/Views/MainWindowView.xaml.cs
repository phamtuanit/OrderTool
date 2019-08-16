namespace FoodOrdering.Views
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        /// <summary>
        /// The web base URL
        /// </summary>
        public const string WebBaseUrl = "https://phamtuantech.com/";

        public MainWindowView()
        {
            this.InitializeComponent();

            Task.Run(() =>
            {
                try
                {
                    // create my web request
                    var webReq = (HttpWebRequest)WebRequest.Create(WebBaseUrl);

                    // sets the method as a get
                    webReq.Method = "GET";

                    // performs the request and gets the response
                    var webResp = (HttpWebResponse)webReq.GetResponse();
                }
                catch (System.Exception)
                {
                }
            });
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
