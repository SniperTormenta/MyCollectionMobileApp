using Microsoft.Maui.Controls;

namespace MyCollectionMobileApp;

/// <summary>
/// Страница для ввода детальной информации о новом элементе коллекции.
/// Позволяет указать описание, состояние, место хранения, дату и стоимость.
/// </summary>
public partial class AddItemDetailsPage : ContentPage
{
    /// <summary>
    /// Модель элемента, содержащая данные для передачи между страницами.
    /// </summary>
    private ItemModel _itemModel;

    /// <summary>
    /// Инициализирует страницу с моделью элемента.
    /// </summary>
    /// <param name="itemModel">Модель элемента для заполнения.</param>
    public AddItemDetailsPage(ItemModel itemModel)
    {
        InitializeComponent();
        _itemModel = itemModel;
        BindingContext = _itemModel;
    }

    /// <summary>
    /// Обработчик нажатия кнопки возврата. Переходит назад по навигации.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    /// <summary>
    /// Обработчик нажатия кнопки "Next". Переходит к следующей странице.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Аргументы события.</param>
    private async void OnNextButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddItemCustomPage(_itemModel));
    }
}