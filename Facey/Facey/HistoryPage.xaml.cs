using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facey.Helpers;
using Facey.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Facey
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {
        public HistoryPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var items = JsonConvert.DeserializeObject<List<PersonModel>>(Settings.FaceHistory);


            HistoryList.ItemsSource = items;
        }

        private void HistoryList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (sender is ListView listview)
            {
                var person = listview.SelectedItem as PersonModel;
                Navigation.PushAsync(new AnalysisPage(person));
            }
        }
    }
}