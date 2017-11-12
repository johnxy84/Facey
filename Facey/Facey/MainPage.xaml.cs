using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facey.Models;
using Xamarin.Forms;

namespace Facey
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
            MasterPage.Menu.ItemSelected += ListItemSelected;
        }

        private void ListItemSelected(object sender, SelectedItemChangedEventArgs e)

        {
            if (!(e.SelectedItem is MasterMenuItem item)) return;

            Detail = new NavigationPage((Page) Activator.CreateInstance(item.TargetType));

            IsPresented = false;
        }
    }
}