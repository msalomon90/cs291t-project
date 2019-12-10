using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using projectApp.ViewModel;
using projectApp.View;
using System.Collections.Generic;

namespace projectApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BrowseLayout : ContentPage
    {
        public BrowseLayout()
        {
            InitializeComponent();
        }

        public void PicView_Clicked(object sender, System.EventArgs e)
        {

            ImageButton button = (ImageButton)sender;
            try
            {
                List<string> category = new List<string>(); //category_label.Text
                SaveImageViewModel.SaveImage(imageName_entry.Text, timestamp_label.Text, coordinates_label.Text,category , imageName_entry.Text, Convert.ToInt32(rating_label.Text));
            }
            catch (Exception exc)
            {
                Console.WriteLine("Did not save changes to image ----------" + exc);
            }
        }

        public void OnSwiped(object sender, SwipedEventArgs e)
        {
            int i = Convert.ToInt32(rating_label.Text); ;
            rating_label.Text = new BrowseLayoutViewModel().SwipeRate(e, rating_label.Text, i);
            String tmp = category_label.Text;
            category_label.Text = new BrowseLayoutViewModel().SwipeCat(e, tmp);
            // rat.Text = $"{e.Direction.ToString()}";
        }
    }
}