using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using projectApp.ViewModel;

namespace projectApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RateImages : ContentPage
    {
        RateImageViewModel vm;
        public RateImages()
        {
            // Initialize and bind
            InitializeComponent();
            vm = new ViewModel.RateImageViewModel();
            BindingContext = vm;
        }

        /*** Sends rating of swipe direction ***/
        private void CardStackView_Swiped(object sender, SwipeCards.Controls.Arguments.SwipedEventArgs e)
        {
            
            Model.Image img = (Model.Image)e.Item;
            Console.WriteLine("SWIPED: {0}", img.Name);
            vm.AddImageRating(img, e.Direction);
            
        }
    }
}