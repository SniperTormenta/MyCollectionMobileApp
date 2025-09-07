using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;

namespace MyCollectionMobileApp;

public partial class FiltersPage : ContentPage
{
    private FiltersViewModel _viewModel;

    public FiltersPage()
    {
        InitializeComponent();
        _viewModel = new FiltersViewModel();
        BindingContext = _viewModel;
    }

    private async void OnCloseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnApplyFiltersClicked(object sender, EventArgs e)
    {
        // Сохраняем выбранные фильтры
        var filters = new FilterOptions
        {
            Category = _viewModel.SelectedCategory == "Все" ? null : _viewModel.SelectedCategory,
            MinPrice = string.IsNullOrEmpty(_viewModel.PriceFrom) ? null : decimal.Parse(_viewModel.PriceFrom),
            MaxPrice = string.IsNullOrEmpty(_viewModel.PriceTo) ? null : decimal.Parse(_viewModel.PriceTo),
            SortBy = _viewModel.GetSelectedSort()
        };

        // Сохраняем фильтры в сервисе
        CollectionService.Instance.ApplyFilters(filters);

        await DisplayAlert("Фильтры", "Фильтры применены", "OK");
        await Navigation.PopAsync();
    }

    private void OnSortByNameTapped(object sender, EventArgs e)
    {
        _viewModel.IsSortByName = true;
        _viewModel.IsSortByDate = false;
        _viewModel.IsSortByPrice = false;
        _viewModel.UpdateBorderColors();
    }

    private void OnSortByDateTapped(object sender, EventArgs e)
    {
        _viewModel.IsSortByName = false;
        _viewModel.IsSortByDate = true;
        _viewModel.IsSortByPrice = false;
        _viewModel.UpdateBorderColors();
    }

    private void OnSortByPriceTapped(object sender, EventArgs e)
    {
        _viewModel.IsSortByName = false;
        _viewModel.IsSortByDate = false;
        _viewModel.IsSortByPrice = true;
        _viewModel.UpdateBorderColors();
    }
}

public class FilterOptions
{
    public string Category { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string SortBy { get; set; }
}

public class FiltersViewModel : BindableObject
{
    private ObservableCollection<string> _categories;
    private string _selectedCategory = "Все";
    private string _priceFrom;
    private string _priceTo;
    private bool _isSortByName = true;
    private bool _isSortByDate;
    private bool _isSortByPrice;
    private Color _sortByNameBorderColor = Color.FromArgb("#38e07b");
    private Color _sortByDateBorderColor = Color.FromArgb("#d1e6d9");
    private Color _sortByPriceBorderColor = Color.FromArgb("#d1e6d9");

    public FiltersViewModel()
    {
        // Загрузка категорий из сервиса + добавляем "Все"
        var categories = CollectionService.Instance.GetAllCategories();
        categories.Insert(0, "Все");
        Categories = new ObservableCollection<string>(categories);
    }

    public ObservableCollection<string> Categories
    {
        get => _categories;
        set
        {
            _categories = value;
            OnPropertyChanged();
        }
    }

    public string SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            _selectedCategory = value;
            OnPropertyChanged();
        }
    }

    public string PriceFrom
    {
        get => _priceFrom;
        set
        {
            _priceFrom = value;
            OnPropertyChanged();
        }
    }

    public string PriceTo
    {
        get => _priceTo;
        set
        {
            _priceTo = value;
            OnPropertyChanged();
        }
    }

    public bool IsSortByName
    {
        get => _isSortByName;
        set
        {
            _isSortByName = value;
            OnPropertyChanged();
        }
    }

    public bool IsSortByDate
    {
        get => _isSortByDate;
        set
        {
            _isSortByDate = value;
            OnPropertyChanged();
        }
    }

    public bool IsSortByPrice
    {
        get => _isSortByPrice;
        set
        {
            _isSortByPrice = value;
            OnPropertyChanged();
        }
    }

    public Color SortByNameBorderColor
    {
        get => _sortByNameBorderColor;
        set
        {
            _sortByNameBorderColor = value;
            OnPropertyChanged();
        }
    }

    public Color SortByDateBorderColor
    {
        get => _sortByDateBorderColor;
        set
        {
            _sortByDateBorderColor = value;
            OnPropertyChanged();
        }
    }

    public Color SortByPriceBorderColor
    {
        get => _sortByPriceBorderColor;
        set
        {
            _sortByPriceBorderColor = value;
            OnPropertyChanged();
        }
    }

    public void UpdateBorderColors()
    {
        SortByNameBorderColor = IsSortByName ? Color.FromArgb("#38e07b") : Color.FromArgb("#d1e6d9");
        SortByDateBorderColor = IsSortByDate ? Color.FromArgb("#38e07b") : Color.FromArgb("#d1e6d9");
        SortByPriceBorderColor = IsSortByPrice ? Color.FromArgb("#38e07b") : Color.FromArgb("#d1e6d9");
    }

    public string GetSelectedSort()
    {
        if (IsSortByName) return "Name";
        if (IsSortByDate) return "Date";
        if (IsSortByPrice) return "Price";
        return "Name";
    }
}