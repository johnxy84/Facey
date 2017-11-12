using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facey.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Facey
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterMenuPage : ContentPage
	{
	    public ListView Menu => MenuList;
		public MasterMenuPage ()
		{
			InitializeComponent ();
           

		    var list = new ObservableCollection<MasterMenuItem>
		    {
		        new MasterMenuItem
		        {
		            IconSource = "home.png",
		            Title = " Home", 
                    TargetType = typeof(HomePage)
		        },
		        new MasterMenuItem
		        {
		            IconSource = "history.png",
		            Title = "History",
                    TargetType=typeof(HistoryPage)
		        }

            };

		    MenuList.ItemsSource = list;
		    TitleImage.Source = "menu.png";
		}
	}
}