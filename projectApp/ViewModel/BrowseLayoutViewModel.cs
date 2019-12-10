using System;
using System.ComponentModel;
using Xamarin.Forms;
namespace projectApp.ViewModel
{
    public class BrowseLayoutViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public BrowseLayoutViewModel()
        {

        }

        public Model.Image SelectedPic(Model.Image selectedItem)
        {
            return selectedItem;
        }

        public String SwipeRate(SwipedEventArgs e, String rat, int i)
        {

            if (e.Direction.ToString() == "Right")
            {
                i++;
                rat = i.ToString();
                return rat;
            }
            if (e.Direction.ToString() == "Left")
            {
                i--;
                rat = i.ToString();
                return rat;

            }
            return rat;
        }

        public String SwipeCat(SwipedEventArgs e, String tmp)
        {

            if (e.Direction.ToString() == "Up")
            {

                tmp = "Category UP";
                return tmp;
            }
            if (e.Direction.ToString() == "Down")
            {

                tmp = "Category DOWN";
                return tmp;

            }
            return tmp;
        }
    }
}
