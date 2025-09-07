using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyCollectionMobileApp;

public class ItemModel
{
    public string CostFormatted => $"{Cost:N0} {Currency}"; // для правильного отображения валюты
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public string StorageLocation { get; set; } = string.Empty;
    public DateTime AcquisitionDate { get; set; } = DateTime.Now;
    public decimal Cost { get; set; } = 0;
    public string Currency { get; set; } = "RUB";
    public Dictionary<string, string> CustomFields { get; set; } = new Dictionary<string, string>();

    // Коллекции для Picker'ов
    public ObservableCollection<string> Categories { get; } = new ObservableCollection<string>
    {
        "Вино", "Книги", "Комиксы", "Фигурки", "Растения", "Другое"
    };

    public ObservableCollection<string> Conditions { get; } = new ObservableCollection<string>
    {
        "Новое", "Отличное", "Хорошее", "Удовлетворительное", "Плохое"
    };

    public ObservableCollection<string> Currencies { get; } = new ObservableCollection<string>
    {
        "RUB", "USD", "EUR", "GBP"
    };
}