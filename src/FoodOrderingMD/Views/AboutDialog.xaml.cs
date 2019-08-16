namespace FoodOrdering.Views
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Interaction logic for SampleMessageDialog.xaml
    /// </summary>
    public partial class AboutDialog : UserControl
    {
        public AboutDialog()
        {
            this.InitializeComponent();

            var index = DateTime.Now.Minute % 5;
            var imagePath = $"../Resources/AboutHeader{index}.jpg";
            this.HeaderImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }
    }
}
