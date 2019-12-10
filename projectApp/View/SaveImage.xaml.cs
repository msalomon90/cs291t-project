using Plugin.Media.Abstractions;
using projectApp.ViewModel;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace projectApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SaveImage : ContentPage
    {
        SaveImageViewModel vm;   // Viewmodel instance 
        public SaveImage(string name, string timestamp, string coordinates, MediaFile photo)
        {
            // Initialize and set binding
            InitializeComponent();
            vm = new SaveImageViewModel(photo.Path);
            BindingContext = vm;

            CategoryButton.IsEnabled = false;

            // Set image preview and details
            imageIcon.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
            imageName_label.Text = name;
            timestamp_label.Text = timestamp;
            location_label.Text = "(" + coordinates + ")";

        }
        /*** Captures image ***/
        // Goes to the next page (save image)
        private void ConfirmSaveButton_Clicked(object sender, EventArgs e)
        {
            vm.SaveImageToDevice();
            Navigation.PushAsync(vm.GoToMainPage());
        }
        
        /*** Displays alert showing current categories ***/
        // Kind of annoying for the user, could be a picker instead
        public async void CategoryButton_Clicked(object sender, EventArgs e)
        {
            CategoryButton.IsEnabled = false;
            await DisplayAlert("Categories", vm.CategoryList(), "OK");
            category_label.Text = "";
        }


    }
}