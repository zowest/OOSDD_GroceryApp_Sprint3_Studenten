using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Grocery.App.ViewModels
{
    [QueryProperty(nameof(GroceryList), nameof(GroceryList))]
    public partial class GroceryListItemsViewModel : BaseViewModel
    {
        private readonly IGroceryListItemsService _groceryListItemsService;
        private readonly IProductService _productService;
        private readonly IFileSaverService _fileSaverService;

        private readonly List<Product> _allAvailableProducts = new();

        public ObservableCollection<GroceryListItem> MyGroceryListItems { get; set; } = [];
        public ObservableCollection<Product> AvailableProducts { get; set; } = [];

        [ObservableProperty]
        private GroceryList groceryList = new(0, "None", DateOnly.MinValue, "", 0);

        [ObservableProperty]
        private string themeToggleText = "Donker"; // toont doelmodus (actie)

        [ObservableProperty]
        private string myMessage = string.Empty;

        public GroceryListItemsViewModel(
            IGroceryListItemsService groceryListItemsService,
            IProductService productService,
            IFileSaverService fileSaverService)
        {
            _groceryListItemsService = groceryListItemsService;
            _productService = productService;
            _fileSaverService = fileSaverService;

            InitThemeText();
            Load(groceryList.Id);
        }

        private void InitThemeText()
        {
            var effective = Application.Current.UserAppTheme == AppTheme.Unspecified
                ? Application.Current.RequestedTheme
                : Application.Current.UserAppTheme;

            ThemeToggleText = effective == AppTheme.Dark ? "Licht" : "Donker";
        }

        private void Load(int id)
        {
            MyGroceryListItems.Clear();
            foreach (var item in _groceryListItemsService.GetAllOnGroceryListId(id))
                MyGroceryListItems.Add(item);

            GetAvailableProducts();
            Search(string.Empty);
        }

        private void GetAvailableProducts()
        {
            _allAvailableProducts.Clear();
            AvailableProducts.Clear();

            foreach (var p in _productService.GetAll())
                if (MyGroceryListItems.FirstOrDefault(g => g.ProductId == p.Id) == null && p.Stock > 0)
                    _allAvailableProducts.Add(p);

            if (_allAvailableProducts.Count == 0)
                MyMessage = "Alle producten zijn al toegevoegd.";
            else
                MyMessage = string.Empty;
        }

        partial void OnGroceryListChanged(GroceryList value)
        {
            Load(value.Id);
        }

        [RelayCommand]
        public async Task ChangeColor()
        {
            Dictionary<string, object> paramater = new() { { nameof(GroceryList), GroceryList } };
            await Shell.Current.GoToAsync($"{nameof(ChangeColorView)}?Name={GroceryList.Name}", true, paramater);
        }

        [RelayCommand]
        public void AddProduct(Product product)
        {
            if (product == null) return;

            GroceryListItem item = new(0, GroceryList.Id, product.Id, 1);
            _groceryListItemsService.Add(item);

            product.Stock--;
            _productService.Update(product);

            _allAvailableProducts.Remove(product);
            AvailableProducts.Remove(product);

            OnGroceryListChanged(GroceryList);
        }

        [RelayCommand]
        public async Task ShareGroceryList(CancellationToken cancellationToken)
        {
            if (GroceryList == null || MyGroceryListItems == null) return;

            string jsonString = JsonSerializer.Serialize(MyGroceryListItems);
            try
            {
                await _fileSaverService.SaveFileAsync("Boodschappen.json", jsonString, cancellationToken);
                await Toast.Make("Boodschappenlijst is opgeslagen.").Show(cancellationToken);
            }
            catch (Exception ex)
            {
                await Toast.Make($"Opslaan mislukt: {ex.Message}").Show(cancellationToken);
            }
        }

        [RelayCommand]
        private void ToggleTheme()
        {
            var effective = Application.Current.UserAppTheme == AppTheme.Unspecified
                ? Application.Current.RequestedTheme
                : Application.Current.UserAppTheme;

            var next = effective == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark;
            Application.Current.UserAppTheme = next;

            ThemeToggleText = next == AppTheme.Dark ? "Licht" : "Donker";
        }

        [RelayCommand]
        public void Search(string searchTerm)
        {
            searchTerm = (searchTerm ?? string.Empty).Trim();
            AvailableProducts.Clear();

            IEnumerable<Product> src = _allAvailableProducts;
            if (!string.IsNullOrWhiteSpace(searchTerm))
                src = src.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

            foreach (var p in src)
                AvailableProducts.Add(p);

            if (!src.Any())
                MyMessage = string.IsNullOrWhiteSpace(searchTerm)
                    ? "Geen producten beschikbaar."
                    : $"Geen producten gevonden voor '{searchTerm}'.";
            else
                MyMessage = string.Empty;
        }
    }
}
