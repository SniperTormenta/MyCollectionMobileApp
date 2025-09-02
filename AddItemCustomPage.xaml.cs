using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace MyCollectionMobileApp;

public partial class AddItemCustomPage : ContentPage
{
    private ItemModel _itemModel;

    public AddItemCustomPage(ItemModel itemModel)
    {
        InitializeComponent();
        _itemModel = itemModel;
        BindingContext = _itemModel;
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void OnAddCustomFieldClicked(object sender, EventArgs e)
    {
        var keyEntry = new Entry
        {
            Placeholder = "Ключ поля",
            PlaceholderColor = Color.FromArgb("#51946c"), 
            TextColor = Color.FromArgb("#0e1a13"),        
            BackgroundColor = Color.FromArgb("#e8f2ec"),  
            HeightRequest = 56,
            FontSize = 16,
            Margin = new Thickness(0, 4),
            ClearButtonVisibility = ClearButtonVisibility.WhileEditing
        };

        var valueEntry = new Entry
        {
            Placeholder = "Значение",
            PlaceholderColor = Color.FromArgb("#51946c"), 
            TextColor = Color.FromArgb("#0e1a13"),        
            BackgroundColor = Color.FromArgb("#e8f2ec"),  
            HeightRequest = 56,
            FontSize = 16,
            Margin = new Thickness(0, 4),
            ClearButtonVisibility = ClearButtonVisibility.WhileEditing
        };

        valueEntry.TextChanged += (s, args) =>
        {
            if (!string.IsNullOrEmpty(keyEntry.Text))
                _itemModel.CustomFields[keyEntry.Text] = valueEntry.Text;
        };

        CustomFieldsLayout.Children.Add(keyEntry);
        CustomFieldsLayout.Children.Add(valueEntry);
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Успех", "Элемент сохранен", "OK");
        await Navigation.PopToRootAsync();
    }
}