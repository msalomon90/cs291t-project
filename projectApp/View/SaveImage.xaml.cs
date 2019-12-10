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
        SaveImageViewModel vm;
        public SaveImage(string name, string timestamp, string coordinates, MediaFile photo)
        {
            InitializeComponent();
            vm = new SaveImageViewModel(photo.Path);
            BindingContext = vm;

            CategoryButton.IsEnabled = false;

            imageIcon.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
            imageName_label.Text = name; 
            timestamp_label.Text = timestamp;
            location_label.Text = "(" + coordinates + ")";//string.Join(",", coordinates.ToArray()); //coordinates; 

        }

        private void ConfirmSaveButton_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("ConfirmSaveButton Clicked");
            vm.SaveImageToDevice();
            Navigation.PushAsync(vm.GoToMainPage());
        }

        public async void CategoryButton_Clicked(object sender, EventArgs e)
        {
            CategoryButton.IsEnabled = false;
            await DisplayAlert("Categories", vm.CategoryList(), "OK");
            category_label.Text = "";
        }


    }
}