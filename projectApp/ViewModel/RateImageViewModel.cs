using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using SwipeCards.Controls.Arguments;
using System.Text;

namespace projectApp.ViewModel
{
    class RateImageViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void RaisePropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        ObservableCollection<Model.Image> cards;
        List<Model.Image> imgList { get; set; }
        public ObservableCollection<Model.Image> Cards   // collection for cards
        {
            get { return cards; }
            set { cards = value; RaisePropertyChanged(); }
        }

        /*** Writes in object to json ***/
        public void SerializeImageObject()
        {
            // Gets json path
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var jsonpath = Path.Combine(documents, "AppImages.json");

            string jsonData = JsonConvert.SerializeObject(imgList, Formatting.Indented); // Write into json

            File.WriteAllText(jsonpath, jsonData);
        }
        /*** Read json to object ***/
        // Should be a utility function
        public void DeserializeImageJson()
        {
            // Get json path
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var jsonpath = Path.Combine(documents, "AppImages.json");

            if (!File.Exists(jsonpath))
            {
                using (File.Create(jsonpath)) ;
            }
            string jsonData = File.ReadAllText(jsonpath);

            if (jsonData != "")
            {

                imgList = JsonConvert.DeserializeObject<List<Model.Image>>(jsonData);
            }

        }
        /*** Constructor ***/
        // Initialize cards and imgList objects
        public RateImageViewModel()
        {
            cards = new ObservableCollection<Model.Image>();
            imgList = new List<Model.Image>();

            DeserializeImageJson();

            foreach(Model.Image img in imgList)   // print for testing
            {
                Console.WriteLine("IMGDIR: {0}", img.Directory);
                cards.Add(img);
            }
        }

        /*** Changes rating of image based on direction ***/
        public void AddImageRating(Model.Image ratedImg, SwipeDirection direction)
        {
            imgList.Remove(ratedImg);

            if(direction == SwipeDirection.Left)
            {
                ratedImg.Rating = 0;
            }
            if(direction == SwipeDirection.Right)
            {
                ratedImg.Rating = 1;
            }

            imgList.Add(ratedImg);

            SerializeImageObject();

        }

    }
}
