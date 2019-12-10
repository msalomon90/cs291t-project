
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using projectApp.ViewModel;
using System;

namespace projectApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CaptureImage : ContentPage
    {
        CaptureImageViewModel vm;
        public CaptureImage()
        {
            InitializeComponent();
            vm = new ViewModel.CaptureImageViewModel();
            BindingContext = vm;

            CaptureImage_Clicked();             // opens the camera on initialization
            Console.WriteLine("NEWIMAGE SOURCE {0}", saveButton.IsVisible);
        }
        public void SaveButton_Clicked(object sender, System.EventArgs e)
        {
            // call function with img as parameter
            //Navigation.PushAsync(new View.SaveImage(photo, timeStamp, imageLocation));

            Navigation.PushAsync(vm.SaveImage_NextPage());
        }
        public void CaptureImage_Clicked()
        {
            //PhotoImage.Source = vm.GetImage();
            vm.DisplayImage();
        }
    }
}