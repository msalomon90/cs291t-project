using projectApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace projectApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapImages : ContentPage
    {
        MapImagesViewModel vm;
        public MapImages()
        {
            InitializeComponent();
            vm = new MapImagesViewModel();
            BindingContext = vm;
            
            // initial view california yeahhhhhh!
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(36.7783, -119.4179), Distance.FromMiles(100)));

        }
        public void AddPinsToMap()
        {
            map.Pins.Clear();
            List<Model.Image> PinList = vm.GetImagesToPin();
            Pin pin;
            double ImageLatitude;
            double ImageLongitude;
            bool tempFlag = true; // change location for testing
            foreach (Model.Image img in PinList)
            {
                ImageLatitude = Convert.ToDouble(img.Coordinates.Split(',')[0]);
                ImageLongitude = Convert.ToDouble(img.Coordinates.Split(',')[1]);

                if(tempFlag)
                {
                    ImageLatitude = 36.7378;
                    ImageLongitude = -119.7871;
                    tempFlag = false;
                }
                pin = new Pin
                {
                    Label = img.Name,
                    Type = PinType.Place,
                    Position = new Position(ImageLatitude, ImageLongitude)
                };

                map.Pins.Add(pin);

                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(ImageLatitude, ImageLongitude), Distance.FromMiles(100)));
            }
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            AddPinsToMap();
        }
    }
}