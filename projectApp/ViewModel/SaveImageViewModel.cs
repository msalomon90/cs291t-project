using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using Xamarin.Forms;
using projectApp.View;
using System.Linq;

namespace projectApp.ViewModel
{
    class SaveImageViewModel : INotifyPropertyChanged
    {
        public string _ImageName;
        public string _ImageTimeStamp;
        public string _ImageCoordinates;
        public int _ImageRating;
        public List<string> _ImageCategoryList;
        public string _ImageCategory;
        public string _CategoryButtonEnabled;


        Model.Image NewImage;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /*** Binding properties for image details  ***/
        public string ImageName
        {
            get { return _ImageName; }
            set
            {
                _ImageName = value;
                OnPropertyChanged("ImageName");
            }
        }
        public string ImageTimeStamp
        {
            get { return _ImageTimeStamp; }
            set
            {
                _ImageTimeStamp = value;
                OnPropertyChanged("ImageTimeStamp");
            }
        }
        public int ImageRating
        {
            get { return _ImageRating; }
            set
            {
                _ImageRating = value;
                OnPropertyChanged("ImageRating");
            }
        }
        public string ImageCoordinates
        {
            get { return _ImageCoordinates; }
            set
            {
                _ImageCoordinates = value;
                OnPropertyChanged("ImageRating");
            }
        }
        public string ImageCategory
        {
            get { return _ImageCategory; }
            set
            {
                _ImageCategory = value;
                CategoryButtonEnabled = "True";
                OnPropertyChanged("ImageCategory");
            }
        }
        public string CategoryButtonEnabled
        {
            get { return _CategoryButtonEnabled; }
            set
            {
                _CategoryButtonEnabled = value;
                OnPropertyChanged("CategoryButtonEnabled");
            }
        }
        /*** Constructor ***/
        // Initialize all binding properties
        public SaveImageViewModel(string imagePath)
        {
            _ImageName = "";
            _ImageCoordinates = "";
            _ImageRating = 0;
            _ImageTimeStamp = "";
            _ImageTimeStamp = "";
            _ImageCategoryList = new List<string>();
            NewImage = new Model.Image(imagePath);

        }

        /*** Second Constructor ***/
        // To be used with browse images (probably could have been made to utility function)
        public static void SaveImage(String Name, String TimeStamp, String Location, List<string> Category, String Directory, int Rating)
        {
            // Get path to image
            String documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var jsonpath = Path.Combine(documents, "AppImages.json");
            String dir = jsonpath;

            // Object to read from json object
            Model.Image savedPic = new Model.Image();
            savedPic.Name = Name;
            savedPic.TimeStamp = TimeStamp;
            savedPic.Category = Category;
            savedPic.Coordinates = Location;
            savedPic.Directory = dir + "Pictures/cs191t/" + Directory;
            savedPic.Rating = Rating;
            string fileName = dir + "jsonFile.txt";
            List<Model.Image> pics = new List<Model.Image>();

            if (File.Exists(dir + "jsonFile.txt"))
            {
                string text = File.ReadAllText(fileName);
                try
                {

                    pics = JsonConvert.DeserializeObject<List<Model.Image>>(text);

                }
                catch (Exception e)
                {
                    Console.WriteLine("--------------Json did not save--------------");
                }

                //Linq to find if already saved
                var match = from p in pics
                            where p.TimeStamp == savedPic.TimeStamp
                            select p;
                Model.Image firstmatch = null;
                try
                {
                    firstmatch = match.First();
                }
                catch (Exception e)
                {

                }

                if (firstmatch != null)
                {
                    savedPic.Directory = firstmatch.Directory;
                    pics.Remove(firstmatch);
                    pics.Add(savedPic);
                    string json = JsonConvert.SerializeObject(pics);
                    File.WriteAllText(fileName, json);
                }

                else
                {
                    pics.Add(savedPic);
                    string json = JsonConvert.SerializeObject(pics);
                    File.WriteAllText(fileName, json);

                }
            }
            else
            {
                pics.Add(savedPic);
                string json = JsonConvert.SerializeObject(pics);
                File.WriteAllText(fileName, json);

            }

        }

        /*** Adds an object with all properties to json ***/
        public void SaveImageToDevice()
        {
            // NOTE: Images always get saved to device even if you back out and dont press save button.
            //       The only difference is that it will not be added to the json which is good, but we
            //       also shouldn't be storing the picture :/ 
          

            // Initialize object information
            NewImage.Name = _ImageName;
            NewImage.TimeStamp = _ImageTimeStamp;
            NewImage.Coordinates = _ImageCoordinates.Substring(1, _ImageCoordinates.Length - 2); // removes parantheses
            NewImage.Rating = _ImageRating;
            NewImage.Category = _ImageCategoryList;

            // Get json file path
            String documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var jsonpath = Path.Combine(documents, "AppImages.json");

            // Create file if not existing
            if (!File.Exists(jsonpath))
            {
                using (File.Create(jsonpath)) ;
            }
            string jsonData = File.ReadAllText(jsonpath);
            var imgList = new List<Model.Image>();


            //Used to reset file for testing

            /*
            bool resetJson = true;
            if(resetJson)
            {
                File.Delete(jsonpath);
                resetJson = false;
            }
            return;*/

            // Add object to json
            if (jsonData != "")
            {
                imgList = JsonConvert.DeserializeObject<List<Model.Image>>(jsonData);
                imgList.Add(NewImage);

            }

            jsonData = JsonConvert.SerializeObject(imgList, Formatting.Indented);
            File.WriteAllText(jsonpath, jsonData);

        }
        /*** Returns a string of categories for display dialogue box ***/
        public string CategoryList()
        {
            _ImageCategoryList.Add(_ImageCategory);
            Console.WriteLine("IMAGECATEGORY: {0}", _ImageCategory);

            string categories = "";
            foreach (string c in _ImageCategoryList)
            {
                categories += c + "\n";  // each category on its own line
            }

            return categories;
        }
        public Page GoToMainPage()
        {
            return new MainPage();
        }
    }

}