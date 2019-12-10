using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using projectApp.Model;
using projectApp.ViewModel;

namespace projectApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        MainPageViewModel vm;
        public MainPage()
        {
            InitializeComponent();

            vm = new ViewModel.MainPageViewModel();   // will initialize the classid for each button
            BindingContext = vm;
        }
        void MainPage_Clicked(object sender, System.EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            Navigation.PushAsync(vm.MainPage_NextPage(button.ClassId));

        }
    }
    
}
