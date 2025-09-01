using Microsoft.Maui.Controls;

namespace MyCollectionMobileApp;

/// <summary>
/// �������� ��� ����� ��������� ���������� � ����� �������� ���������.
/// ��������� ������� ��������, ���������, ����� ��������, ���� � ���������.
/// </summary>
public partial class AddItemDetailsPage : ContentPage
{
    /// <summary>
    /// ������ ��������, ���������� ������ ��� �������� ����� ����������.
    /// </summary>
    private ItemModel _itemModel;

    /// <summary>
    /// �������������� �������� � ������� ��������.
    /// </summary>
    /// <param name="itemModel">������ �������� ��� ����������.</param>
    public AddItemDetailsPage(ItemModel itemModel)
    {
        InitializeComponent();
        _itemModel = itemModel;
        BindingContext = _itemModel;
    }

    /// <summary>
    /// ���������� ������� ������ ��������. ��������� ����� �� ���������.
    /// </summary>
    /// <param name="sender">�������� �������.</param>
    /// <param name="e">��������� �������.</param>
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    /// <summary>
    /// ���������� ������� ������ "Next". ��������� � ��������� ��������.
    /// </summary>
    /// <param name="sender">�������� �������.</param>
    /// <param name="e">��������� �������.</param>
    private async void OnNextButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddItemCustomPage(_itemModel));
    }
}