using Microsoft.Maui.Controls;
using Microsoft.Maui.Media;

namespace MyCollectionMobileApp;

public partial class AddItemPage : ContentPage
{
    private ItemModel _itemModel;

    // ДОБАВЬТЕ конструктор без параметров
    public AddItemPage()
    {
        InitializeComponent();
        _itemModel = new ItemModel();
        BindingContext = _itemModel;
    }

    // Или измените существующий конструктор на необязательный параметр
    public AddItemPage(ItemModel itemModel = null)
    {
        InitializeComponent();
        _itemModel = itemModel ?? new ItemModel();
        BindingContext = _itemModel;
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnAddImageClicked(object sender, EventArgs e)
    {
        var result = await MediaPicker.PickPhotoAsync();
        if (result != null)
        {
            _itemModel.ImagePath = result.FullPath;
            // Убедитесь, что у вас есть элемент SelectedImage в XAML
            // SelectedImage.Source = ImageSource.FromFile(result.FullPath);
            // SelectedImage.IsVisible = true;
        }
    }

    private async void OnNextButtonClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_itemModel.Name) || string.IsNullOrWhiteSpace(_itemModel.Category))
        {
            await DisplayAlert("Ошибка", "Название и категория обязательны", "OK");
            return;
        }
        await Navigation.PushAsync(new AddItemDetailsPage(_itemModel));
    }
}