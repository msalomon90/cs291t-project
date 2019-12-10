using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using projectApp.ViewModel;

namespace projectApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BrowseImages : ContentPage
    {
        public BrowseImages()
        {
            InitializeComponent();
        }

        public void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // pass selected item to pic view
            var item = e.CurrentSelection.FirstOrDefault() as Model.Image;
            var picViewPage = new BrowseLayout();
            picViewPage.BindingContext = item;
            Navigation.PushAsync(picViewPage);
        }
        public void time_button_Clicked(object sender, EventArgs e)
        {
            //  BrowseImagesViewModel.;

            colView.ItemsSource = new BrowseImagesViewModel().Sorter(true, false, false, false);
        }

        public void distance_button_Clicked(object sender, EventArgs e)
        {
            colView.ItemsSource = new BrowseImagesViewModel().Sorter(false, true, false, false);
        }

        public void category_button_Clicked(object sender, EventArgs e)
        {
            colView.ItemsSource = new BrowseImagesViewModel().Sorter(false, false, true, false);


        }

        public void rating_button_Clicked(object sender, EventArgs e)
        {
            colView.ItemsSource = new BrowseImagesViewModel().Sorter(false, false, false, true);
        }
    }
}