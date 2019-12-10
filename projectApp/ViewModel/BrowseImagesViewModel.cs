using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using System.Runtime.CompilerServices;

namespace projectApp.ViewModel
{
    public class BrowseImagesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        void RaisePropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public Location loc { get; set; }
        public List<Model.Image> pics { get; set; }

        ObservableCollection<string> _CategoryList;
        public string _SelectedCategory;

        public ObservableCollection<string> CategoryList
        {
            get { return _CategoryList; }
            set { _CategoryList = value; RaisePropertyChanged("CategoryList"); }
        }
        public string SelectedCategory
        {
            get { return _SelectedCategory; }
            set
            {
                _SelectedCategory = value;
                Console.WriteLine("SELECTEDCATEGORY!!{0}", _SelectedCategory);
                RaisePropertyChanged("SelectedCategory");
            }
        }
       
        ObservableCollection<Model.Image> _pictures = new ObservableCollection<Model.Image>();
        public List<Model.Image> Pictures { get; set; }

        public BrowseImagesViewModel()
        {
            _CategoryList = new ObservableCollection<string>();
            CreateCategoryList();
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var jsonpath = Path.Combine(documents, "AppImages.json");
            String fileName = jsonpath;

            string text = File.ReadAllText(fileName);
            List<Model.Image> pics = new List<Model.Image>();
            pics = JsonConvert.DeserializeObject<List<Model.Image>>(text);
            GetLocation(pics);
            Pictures = pics;
        }
        public void CreateCategoryList()
        {

            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var jsonpath = Path.Combine(documents, "AppImages.json");

            String fileName = jsonpath;
            List<Model.Image> pics = new List<Model.Image>();
            string text = File.ReadAllText(fileName);
            pics = JsonConvert.DeserializeObject<List<Model.Image>>(text);

            foreach (Model.Image img in pics)   // could use linq here -__-
            {
                foreach (string category in img.Category)
                {
                    if (!_CategoryList.Contains(category))
                    {
                        _CategoryList.Add(category);
                    }
                }
            }
            foreach (string c in _CategoryList)
            {

                Console.WriteLine("CATEGORIES: {0}", c);
            }
        }
        public async void GetLocation(List<Model.Image> pics)
        {
            try
            {
                string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var jsonpath = Path.Combine(documents, "AppImages.json");

                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                Location loc = new Location();
                loc = await Geolocation.GetLocationAsync(request);
                Console.WriteLine($"Latitude: {loc.Latitude}, Longitude: {loc.Longitude}, Altitude: {loc.Altitude}");
                foreach (Model.Image p in pics)
                {
                    p.Distance = GetDistance(p, loc);
                }
                string json = JsonConvert.SerializeObject(pics);
                File.WriteAllText(jsonpath, json);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        public Double GetDistance(Model.Image p, Location location)
        {
            string[] tmp = p.Coordinates.Split(',');
            Double picLatitude = Convert.ToDouble(tmp[0].Substring(1) + "," + tmp[1]);
            Double picLongitude = Convert.ToDouble(tmp[2] + "," + tmp[3].Substring(0, tmp[3].Length - 1));
            Location picLocation = new Location(picLatitude, picLongitude);
            Double Distance = Location.CalculateDistance(picLocation, location, DistanceUnits.Kilometers);
            return Distance;
        }

        public List<Model.Image> Sorter(bool time, bool dist, bool cat, bool rat)
        {
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var jsonpath = Path.Combine(documents, "AppImages.json");

            String fileName = jsonpath;
            string text = File.ReadAllText(fileName);
            List<Model.Image> pics = new List<Model.Image>();
            pics = JsonConvert.DeserializeObject<List<Model.Image>>(text);
            if (time)
            {
                var orderByTime = from p in pics
                                  orderby p.TimeStamp descending
                                  select p;

                Pictures = orderByTime.ToList();
                return Pictures;
            }
            else if (dist)
            {
                var orderByDistance = from p in pics
                                      orderby p.Distance descending
                                      select p;
                Pictures = orderByDistance.ToList();
                return Pictures;
            }
            else if (cat)
            {
                var orderByCat = from p in pics
                                 where p.Category.Contains(_SelectedCategory)
                                 select p;
                Pictures = orderByCat.ToList();
                return Pictures;
            }
            else if (rat)
            {
                var orderByDistance = from p in pics
                                      orderby p.Rating descending
                                      select p;
                Pictures = orderByDistance.Take(5).ToList();
                return Pictures;
            }
            else
            {
                return pics;
            }
        }



    }
}
