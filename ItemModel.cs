using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyCollectionMobileApp;

public class ItemModel
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public string StorageLocation { get; set; } = string.Empty;
    public DateTime AcquisitionDate { get; set; } = DateTime.Now;
    public decimal Cost { get; set; } = 0;
    public string Currency { get; set; } = "USD";
    public Dictionary<string, string> CustomFields { get; set; } = new Dictionary<string, string>();

    // Добавьте коллекции для Picker'ов
    public ObservableCollection<string> Conditions { get; } = new ObservableCollection<string>
    {
        "Новое", "Отличное", "Хорошее", "Удовлетворительное", "Плохое"
    };

    public ObservableCollection<string> Currencies { get; } = new ObservableCollection<string>
    {
        "RUB", "USD", "EUR", "GBP"
    };
}