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
            Placeholder = " люч пол€",
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
            Placeholder = "«начение",
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
        // —охран€ем элемент в коллекцию
        CollectionService.Instance.AddItem(_itemModel);

        await DisplayAlert("”спех", "Ёлемент добавлен в коллекцию!", "OK");
        await Navigation.PopToRootAsync();
    }
}