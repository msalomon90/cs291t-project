using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace projectApp.ViewModel
{
    class MapImagesViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void RaisePropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        ObservableCollection<string> _CategoryList;
        List<Model.Image> mapList { get; set; }
        public string _MapCategory;
        public string MapCategory
        {
            get { return _MapCategory; }
            set
            {
                _MapCategory = value;
                RaisePropertyChanged("MapCategory");
            }
        }
        public ObservableCollection<string> CategoryList
        {
            get { return _CategoryList; }
            set { _CategoryList = value; RaisePropertyChanged(); }
        }

        public MapImagesViewModel()
        {
            _CategoryList = new ObservableCollection<string>();
            mapList = new List<Model.Image>();

            DeserializeImageJson();
            CreateCategoryList();
            

        }
        public void CreateCategoryList()
        {
            foreach(Model.Image img in mapList)   // could use linq here -__-
            {
                foreach(string category in img.Category)
                {
                    if(!_CategoryList.Contains(category))
                    {
                        _CategoryList.Add(category);
                    }
                }
            }
            foreach(string c in _CategoryList)
            {
                
                Console.WriteLine("CATEGORIES: {0}", c);
            }
        }
        public void CreateMapList()   // Could use linq here -__-
        {
            List<Model.Image> tempList = new List<Model.Image>(mapList);
            foreach(Model.Image img in tempList)
            {
                if(!img.Category.Contains(_MapCategory))
                {
                    mapList.Remove(img);
                }
            }
        }
        public void SerializeImageObject()
        {
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var jsonpath = Path.Combine(documents, "AppImages.json");

            string jsonData = JsonConvert.SerializeObject(mapList, Formatting.Indented);

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
                mapList = JsonConvert.DeserializeObject<List<Model.Image>>(jsonData);
            }

        }
        public List<Model.Image> GetImagesToPin()
        {
            CreateMapList();
            Console.WriteLine("MAPLIST: {0}", mapList.Count);
            return mapList;
        }
    }
}
