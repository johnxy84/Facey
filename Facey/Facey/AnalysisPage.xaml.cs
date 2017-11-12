using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facey.Models;
using Microsoft.ProjectOxford.Emotion;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Facey
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnalysisPage : ContentPage


    {
        private PersonModel file;

        public AnalysisPage(PersonModel file)
        {
            InitializeComponent();
            this.file = file;
            MainImage.Source = file.FilePath;
            AgeEntry.Text += file.FaceData.FaceAttributes.Age.ToString();
            GenderEntry.Text += file.FaceData.FaceAttributes.Gender;
            EmotionEntry.Text += file.Emotion;
            ExposureEntry.Text += file.FaceData.FaceAttributes.Exposure.ExposureLevel;
            GlassesEntry.Text += file.FaceData.FaceAttributes.Glasses;
        }

      
    }
}