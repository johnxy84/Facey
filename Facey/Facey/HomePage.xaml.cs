using Facey.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Facey.Helpers;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Face;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Facey
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private string result = " ";
        private EmotionServiceClient emotionClient;
        MediaFile photo = null;
        private IFaceServiceClient faceServiceClient;

        public HomePage()
        {
            InitializeComponent();
            emotionClient = new EmotionServiceClient(Constants.EmotionApiKey);
            faceServiceClient = new FaceServiceClient(Constants.FaceApiKey);
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            var choice = await DisplayActionSheet(" ", "Cncel", " ", "Take Photo", "Pick Photo");

            if (choice == "Take Photo")
            {
                await CrossMedia.Current.Initialize();

                // Take photo
                if (CrossMedia.Current.IsCameraAvailable || CrossMedia.Current.IsTakePhotoSupported)
                {
                    photo = await CrossMedia.Current.TakePhotoAsync(
                        new Plugin.Media.Abstractions.StoreCameraMediaOptions

                        {
                            Name = "emotion.jpg",
                            PhotoSize = PhotoSize.Small
                        });
                }
                else
                {
                    await DisplayAlert("No Camera", "Camera unavailable.", "OK");
                }
            }
            else
            {
                photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    CompressionQuality = 70,
                    PhotoSize = PhotoSize.Medium
                });
            }

            if (photo != null)
            {
                Image.Source = ImageSource.FromStream(photo.GetStream);
            }
            ANalyzeButton.IsVisible = true;
            TakePhotoButton.Text = "Retake Photo?";
        }

        private async void Analyze_Button_OnClicked(object sender, EventArgs e)
        {
            Loader.IsEnabled = Loader.IsRunning = Loader.IsVisible = true;
            var person = new PersonModel
            {
                Date = DateTime.Now,
                Emotion = result,
                FilePath = string.Empty,
                FaceData = new FaceData()
            };

            using (var photoStream = photo.GetStream())
            {
                var emotionResult = await emotionClient.RecognizeAsync(photoStream);
                if (emotionResult.Any())
                {
                    // Emotions detected are happiness, sadness, surprise, anger, fear, contempt, disgust, or neutral.
                    person.Emotion = emotionResult.FirstOrDefault()?.Scores.ToRankedList().FirstOrDefault().Key;
                }
            }

            var facedata = await MakeAnalysisRequest(photo);
            if (facedata == null)
            {
                await DisplayAlert("Error", "Couldn't analyze Face", "Try Again");
                Loader.IsEnabled = Loader.IsRunning = Loader.IsVisible = false;
                return;
            }

            person.FaceData = facedata;
            person.FilePath = photo.Path;

            var history = JsonConvert.DeserializeObject<List<PersonModel>>(Settings.FaceHistory);

            history.Add(person);

            Settings.FaceHistory = JsonConvert.SerializeObject(history);

            var page = new AnalysisPage(person);

            Loader.IsEnabled = Loader.IsRunning = Loader.IsVisible = false;

            await Navigation.PushAsync(page, true);
        }

        /// <summary>
        /// Gets the analysis of the specified image file by using the Computer Vision REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file.</param>
        async Task<FaceData> MakeAnalysisRequest(MediaFile file)
        {
            HttpClient client = new HttpClient();
            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Constants.FaceApiKey);
            // Request parameters. A third optional parameter is "details".
            string requestParameters =
                "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";
            // Assemble the URI for the REST API Call.
            string uri = Constants.baseUrl + "?" + requestParameters;
            // Request body. Posts a locally stored JPEG image.
            byte[] byteData = Helper.GetByte(file);
            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                // Execute the REST API call.
                var response = await client.PostAsync(uri, content);
                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                var faceData = JsonConvert.DeserializeObject<List<FaceData>>(contentString);

                return faceData.FirstOrDefault();
            }
        }
    }
}