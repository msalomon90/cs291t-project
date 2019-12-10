using System;
using Plugin.Media.Abstractions;
using Plugin.Media;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Globalization;
using System.ComponentModel;

namespace projectApp.ViewModel
{
    class CaptureImageViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public CaptureImageViewModel()
        {
            ImageInfo = new Model.Image();
            CapturedImage = null;
            DiscardButton_isVisible = "False";
            SaveButton_isVisible = "False";
        }

        public Model.Image ImageInfo { get; set; }
        public MediaFile Photo { get; set; }
        
        public ImageSource _CapturedImage;
        public string _discardButton_isVisible;
        public string _saveButton_isVisible;
        public ImageSource CapturedImage
        {
            get { return _CapturedImage; }
            set
            {
                _CapturedImage = value;
                OnPropertyChanged("CapturedImage");
            }
        }
        public string DiscardButton_isVisible
        {
            get { return _discardButton_isVisible; }
            set
            {
                _discardButton_isVisible = value;
                OnPropertyChanged("DiscardButton_isVisible");
            }
        }
        public string SaveButton_isVisible
        {
            get { return _saveButton_isVisible; }
            set
            {
                _saveButton_isVisible = value;
                OnPropertyChanged("SaveButton_isVisible");
            }
        }

        
        public Page SaveImage_NextPage()
        {
            return new View.SaveImage(ImageInfo.Name, ImageInfo.TimeStamp, ImageInfo.Coordinates, Photo); // fix parameters
        }
        public async void DisplayImage()
        {
            await CrossMedia.Current.Initialize();

            //String storageDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);
            //Console.WriteLine(storageDir);

            //string timeStamp;
            //timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            ImageInfo.TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            ImageInfo.Name = "IMG_" + (new Random()).Next().ToString() + ".jpg";//"IMG_" + ImageInfo.TimeStamp + ".jpg";

            Plugin.Media.Abstractions.Location imageLocation;
            imageLocation = new Plugin.Media.Abstractions.Location();

            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    imageLocation.Latitude = location.Latitude;
                    imageLocation.Longitude = location.Longitude;
                    //ImageInfo.Coordinates = "(" + imageLocation.Latitude + ", " + imageLocation.Longitude + ")";
                    ImageInfo.Coordinates.Add(imageLocation.Latitude);
                    ImageInfo.Coordinates.Add(imageLocation.Longitude);
                }
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

            
            Photo = await CrossMedia.Current.TakePhotoAsync(
              new Plugin.Media.Abstractions.StoreCameraMediaOptions
              {
                  SaveToAlbum = true,
                  Directory = "cs191t",
                  Name = ImageInfo.Name,
                  Location = imageLocation,
                  SaveMetaData = true,
              });

            // Check file is available
            if (Photo != null)
            {
                ImageInfo.Directory = Photo.Path;
                CapturedImage = ImageSource.FromStream(() => { return Photo.GetStream(); });
                DiscardButton_isVisible = "True";
                SaveButton_isVisible = "True";
            }


            /*
            if (!CrossMedia.Current.IsCameraAvailable || CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "No camera available.", "OK");
                return;
            }
            */
            /*if (file == null)
                return;
            await DisplayAlert("File Location", file.Path, "OK");
            */
        }

    }
}
