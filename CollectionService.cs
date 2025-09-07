using System.Collections.ObjectModel;
using System.Linq;

namespace MyCollectionMobileApp;

public class CollectionService
{
    private static CollectionService _instance;
    public static CollectionService Instance => _instance ??= new CollectionService();

    public ObservableCollection<ItemModel> Items { get; } = new ObservableCollection<ItemModel>();
    public ObservableCollection<ItemModel> FilteredItems { get; private set; } = new ObservableCollection<ItemModel>();
    public FilterOptions CurrentFilters { get; private set; } = new FilterOptions();

    private CollectionService()
    {
        // Загрузка тестовых данных
        LoadSampleData();
        FilteredItems = new ObservableCollection<ItemModel>(Items);
    }

    // ДОБАВЛЕНО: Метод добавления элемента
    public void AddItem(ItemModel item)
    {
        Items.Add(item);
        ApplyFilters(); // Обновляем фильтры после добавления
    }

    // ДОБАВЛЕНО: Метод удаления элемента
    public void RemoveItem(ItemModel item)
    {
        Items.Remove(item);
        ApplyFilters(); // Обновляем фильтры после удаления
    }

    // ДОБАВЛЕНО: Метод получения всех категорий
    public List<string> GetAllCategories()
    {
        return Items
            .Select(item => item.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToList();
    }

    public void ApplyFilters(FilterOptions filters)
    {
        CurrentFilters = filters;
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var query = Items.AsQueryable();

        // Фильтрация по категории
        if (!string.IsNullOrEmpty(CurrentFilters.Category) && CurrentFilters.Category != "Все")
        {
            query = query.Where(item => item.Category == CurrentFilters.Category);
        }

        // Фильтрация по цене
        if (CurrentFilters.MinPrice.HasValue)
        {
            query = query.Where(item => item.Cost >= CurrentFilters.MinPrice.Value);
        }

        if (CurrentFilters.MaxPrice.HasValue)
        {
            query = query.Where(item => item.Cost <= CurrentFilters.MaxPrice.Value);
        }

        // Сортировка
        switch (CurrentFilters.SortBy)
        {
            case "Name":
                query = query.OrderBy(item => item.Name);
                break;
            case "Date":
                query = query.OrderByDescending(item => item.AcquisitionDate);
                break;
            case "Price":
                query = query.OrderBy(item => item.Cost);
                break;
            default:
                query = query.OrderBy(item => item.Name);
                break;
        }

        // Обновляем отфильтрованную коллекцию
        FilteredItems.Clear();
        foreach (var item in query.ToList())
        {
            FilteredItems.Add(item);
        }
    }

    private void LoadSampleData()
    {
        // Тестовые данные для демонстрации
        Items.Add(new ItemModel
        {
            Name = "Винтажное вино",
            Category = "Вино",
            ImagePath = "https://lh3.googleusercontent.com/aida-public/AB6AXuCvtfujHFYeKvx30XQ59a1lP0NUam8RgnX8oWdMuLWpCcJUqBRXXCbtRrFvzoTGDpdGZxgstPYSUgqxVUyPIyH0a3uY7UCQb2hUr-nnXmJDmTinHfa8tFL6v1znM6gdW7C5Ff_v5AoeT9FnJ36YN29nTkS0w9mutQY2ItVyDabC_lqjCUKbTYC87jtdO5dFREWaxBAN9ZFZwvdIfWsGy3LUApeBVR-jSmsrIjq-e-t7YHK-gHzHYdQDZRDP8KqU95CZuWjvLixD4w",
            Description = "Отличное винтажное вино из Франции",
            Condition = "Отличное",
            Cost = 2500,
            Currency = "RUB"
        });

        Items.Add(new ItemModel
        {
            Name = "Редкий комикс",
            Category = "Комиксы",
            ImagePath = "https://lh3.googleusercontent.com/aida-public/AB6AXuCbWrxC48hVvIaqooumq_KKm4waZwNQnvKPP3-Lqo1LRAJGQcwP-LUu3oVCsy7hzUeiEo2j2NlVy7QgRDgXfF6FyPxjxsP0FVnZ3hiT_lN_F4ttIuwpPTt49f6fzFfYHFIY7muxYmnS8G9oz8cG6w8ESBCJIkKBvTEv2_zjOJjErFg6AHZzred_Oq5UmDpmhIv2znZLbC1bVciog4beDtFS0qdWXpXbYUUhWnNIqBnmJw1zzfekc5gzxXTkVdLYh9Tr28Q-cXr7oA",
            Description = "Редкое первое издание комикса",
            Condition = "Хорошее",
            Cost = 1500,
            Currency = "RUB"
        });
    }
}