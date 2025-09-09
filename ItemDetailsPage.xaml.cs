using Microsoft.Maui.Controls;

namespace MyCollectionMobileApp;

public partial class ItemDetailsPage : ContentPage
{
    private ItemModel _item;

    public ItemDetailsPage(ItemModel item)
    {
        InitializeComponent();
        _item = item;
        BindingContext = _item;

        LoadCustomFields();
    }

    private void LoadCustomFields()
    {
        if (_item?.CustomFields != null && _item.CustomFields.Count > 0)
        {
            foreach (var field in _item.CustomFields)
            {
                var fieldGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }
                    },
                    Margin = new Thickness(0, 16, 0, 16)
                };

                // Ключ поля (зеленый текст)
                var keyLabel = new Label
                {
                    Text = $"{field.Key}:",
                    TextColor = Color.FromArgb("#51946c"),
                    FontSize = 14,
                    VerticalOptions = LayoutOptions.Center
                };

                // Значение поля (основной текст)
                var valueLabel = new Label
                {
                    Text = field.Value,
                    TextColor = Color.FromArgb("#0e1a13"),
                    FontSize = 14,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.End
                };

                fieldGrid.Add(keyLabel, 0, 0);
                fieldGrid.Add(valueLabel, 1, 0);

                CustomFieldsContainer.Children.Add(fieldGrid);
            }
        }
        else
        {
            // Если нет кастомных полей
            var noFieldsLabel = new Label
            {
                Text = "Дополнительные поля не добавлены",
                TextColor = Color.FromArgb("#51946c"),
                FontSize = 14,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 20, 0, 20)
            };

            CustomFieldsContainer.Children.Add(noFieldsLabel);
        }
    }

    private async void OnEditButtonClicked(object sender, EventArgs e)
    {
        // Переход на страницу редактирования
        await DisplayAlert("Редактирование", "Функция редактирования будет реализована позже", "OK");
    }

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Подтверждение",
            "Вы уверены, что хотите удалить этот элемент?", "Да", "Нет");

        if (answer)
        {
            CollectionService.Instance.RemoveItem(_item);
            await DisplayAlert("Успех", "Элемент удален", "OK");

            // Возврат на главную страницу
            await Navigation.PopToRootAsync();
        }
    }
}