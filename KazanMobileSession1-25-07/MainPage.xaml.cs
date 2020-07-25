using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
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
            await LoadPickers();
        }

        private async Task LoadPickers()
        {
            pDepartment.Items.Clear();
            pAssetGroups.Items.Clear();
            pAssetGroups.Items.Add("No Filter");
            pDepartment.Items.Add("No Filter");
            var apiCaller = new WebApi();
            var departmentStrings = await apiCaller.PostAsync("Departments", null);
            var departmentList = JsonConvert.DeserializeObject<List<Department>>(departmentStrings);
            foreach (var item in departmentList)
            {
                pDepartment.Items.Add(item.Name);
            }

            var assetGroupsStrings = await apiCaller.PostAsync("AssetGroups", null);
            var assetGroupsList = JsonConvert.DeserializeObject<List<AssetGroup>>(assetGroupsStrings);
            foreach (var item in assetGroupsList)
            {
                pAssetGroups.Items.Add(item.Name);
            }
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
            if (pDepartment.SelectedItem.ToString() == "No Filter")
            {
                if (pAssetGroups.SelectedItem == null || pAssetGroups.SelectedItem.ToString() == "No Filter")
                {
                    lvAssets.ItemsSource = _assetList;
                }
                else
                {
                    var filteredList = (from x in _assetList
                                        where x.AssetGroup == pAssetGroups.SelectedItem.ToString() 
                                        select x).ToList();
                    lvAssets.ItemsSource = filteredList;
                }
            }
            else
            {
                if (pAssetGroups.SelectedItem == null || pAssetGroups.SelectedItem.ToString() == "No Filter")
                {
                    var filteredList = (from x in _assetList
                                        where x.AssetDepartment == pDepartment.SelectedItem.ToString()
                                        select x).ToList();
                    lvAssets.ItemsSource = filteredList;
                }
                else
                {
                    var filteredList = (from x in _assetList
                                        where x.AssetDepartment == pDepartment.SelectedItem.ToString() && x.AssetGroup == pAssetGroups.SelectedItem.ToString() 
                                        select x).ToList();
                    lvAssets.ItemsSource = filteredList;
                }
            }

        }

        private void pAssetGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pAssetGroups.SelectedItem.ToString() == "No Filter")
            {
                if (pDepartment.SelectedItem == null || pDepartment.SelectedItem.ToString() == "No Filter")
                {
                    lvAssets.ItemsSource = _assetList;
                }
                else
                {
                    var filteredList = (from x in _assetList
                                        where x.AssetDepartment == pDepartment.SelectedItem.ToString() 
                                        select x).ToList();
                    lvAssets.ItemsSource = filteredList;
                }
            }
            else
            {
                if (pDepartment.SelectedItem == null || pDepartment.SelectedItem.ToString() == "No Filter")
                {
                    var filteredList = (from x in _assetList
                                        where x.AssetGroup == pAssetGroups.SelectedItem.ToString() 
                                        select x).ToList();
                    lvAssets.ItemsSource = filteredList;
                }
                else
                {
                    var filteredList = (from x in _assetList
                                        where x.AssetDepartment == pDepartment.SelectedItem.ToString() && x.AssetGroup == pAssetGroups.SelectedItem.ToString() 
                                        select x).ToList();
                    lvAssets.ItemsSource = filteredList;
                }
            }
        }

        private void dpStart_DateSelected(object sender, DateChangedEventArgs e)
        {

        }

        private void dpEnd_DateSelected(object sender, DateChangedEventArgs e)
        {

        }

        private void sbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(sbSearch.Text))
            {
                lvAssets.ItemsSource = _assetList;
            }
            else
            {
                var filteredList = (from x in _assetList
                                    where x.AssetName.Contains(sbSearch.Text) || x.AssetSN.Contains(sbSearch.Text)
                                    select x).ToList();
                lvAssets.ItemsSource = filteredList;
            }
        }

        private async void btnAdd_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AssetInformation(0));
        }

        private async void btnEdit_Clicked(object sender, EventArgs e)
        {
            var imageButton = (ImageButton)sender;
            var getHStack = (StackLayout)imageButton.Parent;
            var getLabelsStack = (StackLayout)((Grid)(getHStack.Parent)).Children[0];
            var getAssetID = (Label)getLabelsStack.Children[0];
            Console.WriteLine(getAssetID.Text);
            await Navigation.PushAsync(new AssetInformation(Int32.Parse(getAssetID.Text)));
        }

        private void btnMove_Clicked(object sender, EventArgs e)
        {
            var imageButton = (ImageButton)sender;
            var getHStack = (StackLayout)imageButton.Parent;
            var getLabelsStack = (StackLayout)((Grid)(getHStack.Parent)).Children[0];
            var getAssetID = (Label)getLabelsStack.Children[0];
            Console.WriteLine(getAssetID.Text);
        }

        private void btnHistory_Clicked(object sender, EventArgs e)
        {
            var imageButton = (ImageButton)sender;
            var getHStack = (StackLayout)imageButton.Parent;
            var getLabelsStack = (StackLayout)((Grid)(getHStack.Parent)).Children[0];
            var getAssetID = (Label)getLabelsStack.Children[0];
            Console.WriteLine(getAssetID.Text);
        }
    }
}
