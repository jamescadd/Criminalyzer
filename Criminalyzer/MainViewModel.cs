using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
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

        public ObservableCollection<Record> Records { get; private set; } = new ObservableCollection<Record>();

        public ObservableCollection<Face> RecordFaces { get; private set; } = new ObservableCollection<Face>();

        public BitmapImage CapturedImage { get; private set; }

        public Face CapturedFace { get; private set; }

        public string CapturedAge { get; private set; }

        public string CapturedGender { get; private set; }

        private FaceServiceClient _faceService;

        public Face MostSimilarFace { get; private set; }

        public Record MostSimilarRecord { get; private set; }

        public MainViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                CapturedAge = "32";
                CapturedGender = "Male";

                BitmapImage image = new BitmapImage(new Uri("http://lorempixel.com/200/200/"));

                Records.Add(new Record { mugshot = "http://lorempixel.com/200/200/", name = "FIRST LAST" });
                Records.Add(new Record { mugshot = "http://lorempixel.com/200/200/", name = "FIRST LAST" });
                Records.Add(new Record { mugshot = "http://lorempixel.com/200/200/", name = "FIRST LAST" });
                Records.Add(new Record { mugshot = "http://lorempixel.com/200/200/", name = "FIRST LAST" });
                Records.Add(new Record { mugshot = "http://lorempixel.com/200/200/", name = "FIRST LAST" });
                Records.Add(new Record { mugshot = "http://lorempixel.com/200/200/", name = "FIRST LAST" });
                Records.Add(new Record { mugshot = "http://lorempixel.com/200/200/", name = "FIRST LAST" });
            }

            _faceService = new FaceServiceClient("9b9e6f57f27a4ce9b949c3a22dee8630");
        }

        protected void OnGetMugshots()
        {

        }

        protected async void OnTakePicture()
        {
            var file = await GetPicture();
            if (file == null)
                return;

            CapturedImage = await LoadImage(file);
            var faces = await GetFaces(file);
            var face = faces.FirstOrDefault();

            if (face != null)
            {
                CapturedAge = face.Attributes.Age.ToString();
                CapturedGender = face.Attributes.Gender;
                CapturedFace = face;
            }

            await LoadMugshots();

            HttpClient client = new HttpClient();

            foreach(var record in Records)
            {
                var mugshot = await client.GetStreamAsync(record.mugshot);

                var recordFaces = await _faceService.DetectAsync(mugshot);
                var recordFace = recordFaces.FirstOrDefault();
                if (recordFace != null)
                {
                    recordFace.Tag = record;
                    RecordFaces.Add(recordFace);
                }
            }

            // Find similar.
            //var similar = await _faceService.FindSimilarAsync(CapturedFace.FaceId, RecordFaces.Select(f => f.FaceId).ToArray());
            //var mostSimilar = similar.FirstOrDefault();

            //var mostSimilarFace = RecordFaces.Where(f => f.FaceId == mostSimilar.FaceId).FirstOrDefault();

            MostSimilarFace = RecordFaces.First();
            MostSimilarRecord = Records.First();

        }

        private async Task LoadMugshots()
        {
            var jailbase = await JailbaseService.GetMugshots();
            Debug.WriteLine(jailbase.records.Count);

            foreach(var record in jailbase.records)
            {
                Records.Add(record);
            }
        }

        private async Task<BitmapImage> LoadImage(StorageFile file)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
            {
                bitmapImage.SetSource(fileStream);
            }

            return bitmapImage;
        }

        private async Task<Face[]> GetFaces(StorageFile file)
        {
            using (var stream = await file.OpenAsync(FileAccessMode.Read))
            {
                var faceResult = await _faceService.DetectAsync(stream.AsStream(), analyzesAge: true, analyzesGender: true);
                return faceResult;
            }
        }

        private async Task<StorageFile> GetPicture()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            StorageFile file = await openPicker.PickSingleFileAsync();
            return file;
            
            //CameraCaptureUI dialog = new CameraCaptureUI();
            //var photo = await dialog.CaptureFileAsync(CameraCaptureUIMode.Photo);

            //if (photo == null)
            //{
            //    return;
            //}

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
