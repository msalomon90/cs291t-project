using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace projectApp.ViewModel
{
    public class BrowseImagesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Location loc { get; set; }
        public List<Model.Image> pics { get; set; }

        ObservableCollection<Model.Image> _pictures = new ObservableCollection<Model.Image>();
        public List<Model.Image> Pictures { get; set; }

        public BrowseImagesViewModel()
        {
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var jsonpath = Path.Combine(documents, "AppImages.json");

            String fileName = jsonpath;//"/storage/emulated/0/Android/data/com.companyname.projectapp/files/jsonFile.txt";
            string text = File.ReadAllText(fileName);
            List<Model.Image> pics = new List<Model.Image>();
            pics = JsonConvert.DeserializeObject<List<Model.Image>>(text);
            GetLocation(pics);
            Pictures = pics;
            //     Pictures;
            //     Images = pics;
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
                                 orderby p.Category descending
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
