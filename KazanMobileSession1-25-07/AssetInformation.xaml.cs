using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static KazanMobileSession1_25_07.GlobalClass;

namespace KazanMobileSession1_25_07
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssetInformation : ContentPage
    {
        int _assetID = 0;
        List<Department> _departmentList;
        List<DepartmentLocation> _departmentLocationsList;
        List<Location> _locationList;
        List<AssetGroup> _assetGroupsList;
        List<string> _uniqueSNs;
        List<Employee> _accountableList;
        public AssetInformation(int AssetID)
        {
            InitializeComponent();
            _assetID = AssetID;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await LoadPickers();
            if (_assetID != 0)
            {
                await LoadDetails();
            }
            await GetAssets();
        }

        private async Task GetAssets()
        {
            var apiCaller = new WebApi();
            var getAssetSN = await apiCaller.PostAsync("Assets/GetUniqueSNs", null);
            _uniqueSNs = JsonConvert.DeserializeObject<List<string>>(getAssetSN);
        }

        private async Task LoadPickers()
        {
            var apiCaller = new WebApi();
            var departmentStrings = await apiCaller.PostAsync("Departments", null);
            _departmentList = JsonConvert.DeserializeObject<List<Department>>(departmentStrings);
            foreach (var item in _departmentList)
            {
                pDepartment.Items.Add(item.Name);
            }

            var departmentLocationsStrings = await apiCaller.PostAsync("DepartmentLocations", null);
            _departmentLocationsList = JsonConvert.DeserializeObject<List<DepartmentLocation>>(departmentLocationsStrings);

            var locationStrings = await apiCaller.PostAsync("Locations", null);
            _locationList = JsonConvert.DeserializeObject<List<Location>>(locationStrings);

            var assetGroupsStrings = await apiCaller.PostAsync("AssetGroups", null);
            _assetGroupsList = JsonConvert.DeserializeObject<List<AssetGroup>>(assetGroupsStrings);
            foreach (var item in _assetGroupsList)
            {
                pAssetGroup.Items.Add(item.Name);
            }

            var accountableStrings = await apiCaller.PostAsync("Employees", null);
            _accountableList = JsonConvert.DeserializeObject<List<Employee>>(accountableStrings);
            foreach (var item in _accountableList)
            {
                pAccountable.Items.Add(item.FirstName + " " + item.LastName);
            }
        }

        private async Task LoadDetails()
        {
            var apiCaller = new WebApi();
        }

        private void pDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            pLocation.Items.Clear();
            var getDepartmentID = (from x in _departmentList
                                   where x.Name == pDepartment.SelectedItem.ToString()
                                   select x.ID).First();
            var getDepartmentLocations = (from x in _departmentLocationsList
                                          where x.DepartmentID == getDepartmentID && x.EndDate == null
                                          select x.LocationID).ToList();
            foreach (var item in getDepartmentLocations)
            {
                var getLocationNames = (from x in _locationList
                                        where x.ID == item
                                        select x.Name).FirstOrDefault();
                pLocation.Items.Add(getLocationNames);
            }
            pLocation.IsEnabled = true;
            if (pAssetGroup.SelectedItem != null)
            {
                var DepartmentID = (from x in _departmentList
                                    where x.Name == pDepartment.SelectedItem.ToString()
                                    select x.ID).First();
                var assetGroupID = (from x in _assetGroupsList
                                    where x.Name == pAssetGroup.SelectedItem.ToString()
                                    select x.ID).First();

                var dd = DepartmentID.ToString().PadLeft(2, '0');
                var gg = assetGroupID.ToString().PadLeft(2, '0');
                var ddgg = $"{dd}/{gg}";
                var getLastestSN = (from x in _uniqueSNs
                                    where x.Contains(ddgg)
                                    orderby x descending
                                    select x).FirstOrDefault();
                if (getLastestSN == null)
                {
                    var newSN = 1.ToString().PadLeft(4, '0');
                    lblAssetSN.Text = $"{ddgg}/{newSN}";
                }
                else
                {
                    var newSN = (Int32.Parse(getLastestSN.Split('/')[2]) + 1).ToString().PadLeft(4, '0');
                    lblAssetSN.Text = $"{ddgg}/{newSN}";
                }
            }
        }

        private void pAssetGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pDepartment.SelectedItem != null)
            {
                var DepartmentID = (from x in _departmentList
                                    where x.Name == pDepartment.SelectedItem.ToString()
                                    select x.ID).First();
                var assetGroupID = (from x in _assetGroupsList
                                    where x.Name == pAssetGroup.SelectedItem.ToString()
                                    select x.ID).First();

                var dd = DepartmentID.ToString().PadLeft(2, '0');
                var gg = assetGroupID.ToString().PadLeft(2, '0');
                var ddgg = $"{dd}/{gg}";
                var getLastestSN = (from x in _uniqueSNs
                                    where x.Contains(ddgg)
                                    orderby x descending
                                    select x).FirstOrDefault();
                if (getLastestSN == null)
                {
                    var newSN = 1.ToString().PadLeft(4, '0');
                    lblAssetSN.Text = $"{ddgg}/{newSN}";
                }
                else
                {
                    var newSN = (Int32.Parse(getLastestSN.Split('/')[2]) + 1).ToString().PadLeft(4, '0');
                    lblAssetSN.Text = $"{ddgg}/{newSN}";
                }
            }
        }

        private async void btnSubmit_Clicked(object sender, EventArgs e)
        {
            var getDepartmentID = (from x in _departmentList
                                   where x.Name == pDepartment.SelectedItem.ToString()
                                   select x.ID).First();
            var getLocationID = (from x in _locationList
                                 where x.Name == pLocation.SelectedItem.ToString()
                                 select x.ID).First();
            var assetGroupID = (from x in _assetGroupsList
                                where x.Name == pAssetGroup.SelectedItem.ToString()
                                select x.ID).First();
            var getDepartmentLocationID = (from x in _departmentLocationsList
                                           where x.DepartmentID == getDepartmentID && x.LocationID == getLocationID
                                           select x.ID).First();
            var getEmployeeID = (from x in _accountableList
                                 where pAccountable.SelectedItem.ToString() == x.FirstName + " " + x.LastName
                                 select x.ID).First();
            var newAsset = new Asset()
            {
                AssetSN = lblAssetSN.Text,
                WarrantyDate = dpExpired.Date,
                Description = editorDescription.Text,
                AssetName = entryAssetName.Text,
                AssetGroupID = assetGroupID,
                DepartmentLocationID = getDepartmentLocationID,
                EmployeeID = getEmployeeID
            };
            var JsonData = JsonConvert.SerializeObject(newAsset);
            var apiCaller = new WebApi();
            var response = await apiCaller.PostAsync("Assets/Create", JsonData);
            if (response == "\"Asset created successfully!\"")
            {
                await DisplayAlert("Asset Creation/Modifications", "Asset created successfully!", "Ok");
                await Navigation.PopAsync();
            }
                
        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}