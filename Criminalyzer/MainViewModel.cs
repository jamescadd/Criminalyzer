using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using PropertyChanged;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Criminalyzer
{
    [ImplementPropertyChanged]
    class MainViewModel
    {
        public RelayCommand GetMugshotsCommand {  get { return new RelayCommand(OnGetMugshots); } }

        public RelayCommand TakePictureCommand { get { return new RelayCommand(OnTakePicture); } }

        public ObservableCollection<Record> Records = new ObservableCollection<Record>();

        public BitmapImage CapturedImage { get; private set; }

        public string CapturedAge { get; private set; }

        public string CapturedGender { get; private set; }

        private FaceServiceClient _faceService;

        public MainViewModel()
        {
            _faceService = new FaceServiceClient("9b9e6f57f27a4ce9b949c3a22dee8630");
        }

        protected void OnGetMugshots()
        {

        }

        protected async void OnTakePicture()
        {
            //CameraCaptureUI dialog = new CameraCaptureUI();
            //var photo = await dialog.CaptureFileAsync(CameraCaptureUIMode.Photo);

            //if (photo == null)
            //{
            //    return;
            //}


            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                //OutputTextBlock.Text = "Picked photo: " + file.Name;
                BitmapImage bitmapImage = new BitmapImage();
                using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    bitmapImage.SetSource(fileStream);
                }

                using (var stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    var faceResult = await _faceService.DetectAsync(stream.AsStream(), analyzesAge:true, analyzesGender: true);
                    var face = faceResult.FirstOrDefault();

                    if (face != null)
                    {
                        CapturedAge = face.Attributes.Age.ToString();
                        CapturedGender = face.Attributes.Gender;
                    }
                }

                CapturedImage = bitmapImage;

                // Oxford face detection
            }
            else
            {
                //OutputTextBlock.Text = "Operation cancelled.";
            }



            //var picker = new Windows.Storage.Pickers.FileOpenPicker();
            //var photo = await picker.PickSingleFileAsync();

            //if (photo == null)
            //    return;

            //BitmapImage bitmapImage = new BitmapImage();
            //using (IRandomAccessStream fileStream = await photo.OpenAsync(FileAccessMode.Read))
            //{
            //    bitmapImage.SetSource(fileStream);
            //}

            //CapturedImage = bitmapImage;
        }
    }
}
