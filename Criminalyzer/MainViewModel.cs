using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public MainViewModel()
        {

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

                CapturedImage = bitmapImage;
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
