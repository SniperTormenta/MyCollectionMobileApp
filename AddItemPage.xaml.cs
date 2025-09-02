using Microsoft.Maui.Controls;
using Microsoft.Maui.Media;
using Microsoft.Maui.Graphics;

namespace MyCollectionMobileApp;

public partial class AddItemPage : ContentPage
{
    private ItemModel _itemModel;
    private Aspect _currentAspect = Aspect.AspectFill;

    public AddItemPage()
    {
        InitializeComponent();
        _itemModel = new ItemModel();
        BindingContext = _itemModel;
    }

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
        try
        {
            // Проверяем разрешения
            var status = await Permissions.RequestAsync<Permissions.Photos>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Внимание", "Разрешение на доступ к фото необходимо для добавления изображений", "OK");
                return;
            }

            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Выберите изображение"
            });

            if (result != null)
            {
                // Проверяем размер файла (максимум 10MB)
                var fileInfo = new FileInfo(result.FullPath);
                if (fileInfo.Length > 10 * 1024 * 1024)
                {
                    await DisplayAlert("Ошибка", "Размер изображения не должен превышать 10MB", "OK");
                    return;
                }

                _itemModel.ImagePath = result.FullPath;
                SelectedImage.Source = ImageSource.FromFile(result.FullPath);
                ImageContainer.IsVisible = true;

                // Безопасный доступ к ImageHintLabel
                var hintLabel = this.FindByName<Label>("ImageHintLabel");
                if (hintLabel != null)
                    hintLabel.IsVisible = true;

                // Анимация появления изображения
                await ImageContainer.ScaleTo(0.8, 0);
                await ImageContainer.ScaleTo(1.1, 250, Easing.SpringOut);
                await ImageContainer.ScaleTo(1, 150, Easing.SpringIn);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", "Не удалось загрузить изображение", "OK");
        }
    }

    // Обработчик смены режима отображения изображения
    private void OnChangeImageAspectClicked(object sender, EventArgs e)
    {
        // Переключаем между режимами
        _currentAspect = _currentAspect == Aspect.AspectFill ? Aspect.AspectFit : Aspect.AspectFill;
        SelectedImage.Aspect = _currentAspect;

        // Анимация переключения
        ImageContainer.ScaleTo(0.95, 100, Easing.SpringOut)
                     .ContinueWith(t => ImageContainer.ScaleTo(1, 100, Easing.SpringIn));

        // Показываем подсказку о текущем режиме
        var mode = _currentAspect == Aspect.AspectFill ? "Обрезать" : "Вписать";
        DisplayAlert("Режим изображения", $"Текущий режим: {mode}\n\nНажмите ещё раз для смены", "OK");
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

    // Дополнительный метод для выбора из нескольких режимов
    private async void OnImageSettingsClicked(object sender, EventArgs e)
    {
        var action = await DisplayActionSheet("Режим отображения", "Отмена", null,
            "Обрезать (AspectFill)", "Вписать (AspectFit)", "Растянуть (Fill)");

        switch (action)
        {
            case "Обрезать (AspectFill)":
                SelectedImage.Aspect = Aspect.AspectFill;
                break;
            case "Вписать (AspectFit)":
                SelectedImage.Aspect = Aspect.AspectFit;
                break;
            case "Растянуть (Fill)":
                SelectedImage.Aspect = Aspect.Fill;
                break;
        }
    }
}