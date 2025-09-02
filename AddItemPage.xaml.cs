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
            // ��������� ����������
            var status = await Permissions.RequestAsync<Permissions.Photos>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("��������", "���������� �� ������ � ���� ���������� ��� ���������� �����������", "OK");
                return;
            }

            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "�������� �����������"
            });

            if (result != null)
            {
                // ��������� ������ ����� (�������� 10MB)
                var fileInfo = new FileInfo(result.FullPath);
                if (fileInfo.Length > 10 * 1024 * 1024)
                {
                    await DisplayAlert("������", "������ ����������� �� ������ ��������� 10MB", "OK");
                    return;
                }

                _itemModel.ImagePath = result.FullPath;
                SelectedImage.Source = ImageSource.FromFile(result.FullPath);
                ImageContainer.IsVisible = true;

                // ���������� ������ � ImageHintLabel
                var hintLabel = this.FindByName<Label>("ImageHintLabel");
                if (hintLabel != null)
                    hintLabel.IsVisible = true;

                // �������� ��������� �����������
                await ImageContainer.ScaleTo(0.8, 0);
                await ImageContainer.ScaleTo(1.1, 250, Easing.SpringOut);
                await ImageContainer.ScaleTo(1, 150, Easing.SpringIn);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("������", "�� ������� ��������� �����������", "OK");
        }
    }

    // ���������� ����� ������ ����������� �����������
    private void OnChangeImageAspectClicked(object sender, EventArgs e)
    {
        // ����������� ����� ��������
        _currentAspect = _currentAspect == Aspect.AspectFill ? Aspect.AspectFit : Aspect.AspectFill;
        SelectedImage.Aspect = _currentAspect;

        // �������� ������������
        ImageContainer.ScaleTo(0.95, 100, Easing.SpringOut)
                     .ContinueWith(t => ImageContainer.ScaleTo(1, 100, Easing.SpringIn));

        // ���������� ��������� � ������� ������
        var mode = _currentAspect == Aspect.AspectFill ? "��������" : "�������";
        DisplayAlert("����� �����������", $"������� �����: {mode}\n\n������� ��� ��� ��� �����", "OK");
    }

    private async void OnNextButtonClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_itemModel.Name) || string.IsNullOrWhiteSpace(_itemModel.Category))
        {
            await DisplayAlert("������", "�������� � ��������� �����������", "OK");
            return;
        }
        await Navigation.PushAsync(new AddItemDetailsPage(_itemModel));
    }

    // �������������� ����� ��� ������ �� ���������� �������
    private async void OnImageSettingsClicked(object sender, EventArgs e)
    {
        var action = await DisplayActionSheet("����� �����������", "������", null,
            "�������� (AspectFill)", "������� (AspectFit)", "��������� (Fill)");

        switch (action)
        {
            case "�������� (AspectFill)":
                SelectedImage.Aspect = Aspect.AspectFill;
                break;
            case "������� (AspectFit)":
                SelectedImage.Aspect = Aspect.AspectFit;
                break;
            case "��������� (Fill)":
                SelectedImage.Aspect = Aspect.Fill;
                break;
        }
    }
}