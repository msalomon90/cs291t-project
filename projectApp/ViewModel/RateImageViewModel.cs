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
        public ObservableCollection<Model.Image> Cards
        {
            get { return cards; }
            set { cards = value; RaisePropertyChanged(); }
        }
        public void SerializeImageObject()
        {
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var jsonpath = Path.Combine(documents, "AppImages.json");

            string jsonData = JsonConvert.SerializeObject(imgList, Formatting.Indented);

            File.WriteAllText(jsonpath, jsonData);

            Console.WriteLine("UPDATEDJSON: {0}", jsonData);
        }
        public void DeserializeImageJson()
        {
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var jsonpath = Path.Combine(documents, "AppImages.json");
            if (!File.Exists(jsonpath))
            {
                using (File.Create(jsonpath)) ;
            }
            Console.WriteLine("JSONPATH: {0}", jsonpath);
            string jsonData = File.ReadAllText(jsonpath);

            if (jsonData != "")
            {

                imgList = JsonConvert.DeserializeObject<List<Model.Image>>(jsonData);
            }

        }
        public RateImageViewModel()
        {
            cards = new ObservableCollection<Model.Image>();
            imgList = new List<Model.Image>();

            DeserializeImageJson();

            foreach(Model.Image img in imgList)
            {
                Console.WriteLine("IMGDIR: {0}", img.Directory);
                cards.Add(img);
            }
        }

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

        // Implementation of INotifyPropertyChanged
    }
}
