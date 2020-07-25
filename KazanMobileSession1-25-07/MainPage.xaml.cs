using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static KazanMobileSession1_25_07.GlobalClass;

namespace KazanMobileSession1_25_07
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private List<CustomListView> _assetList;
        public MainPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await LoadAssets();
        }

        private async Task LoadAssets()
        {
            var apiCaller = new WebApi();
            var results = await apiCaller.PostAsync("Assets/GetCustomView", null);
            _assetList = JsonConvert.DeserializeObject<List<CustomListView>>(results);
            lvAssets.ItemsSource = _assetList;
        }

        private void pDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pAssetGroups_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dpStart_DateSelected(object sender, DateChangedEventArgs e)
        {

        }

        private void dpEnd_DateSelected(object sender, DateChangedEventArgs e)
        {

        }

        private void sbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnAdd_Clicked(object sender, EventArgs e)
        {

        }

        private void btnEdit_Clicked(object sender, EventArgs e)
        {

        }

        private void btnMove_Clicked(object sender, EventArgs e)
        {

        }

        private void btnHistory_Clicked(object sender, EventArgs e)
        {

        }
    }
}
