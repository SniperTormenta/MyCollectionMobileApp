using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace MyCollectionMobileApp;

public partial class StatisticsPage : ContentPage
{
    private bool _isNavigating = false;
    private SemaphoreSlim _navigationLock = new SemaphoreSlim(1, 1);

    public StatisticsPage()
    {
        InitializeComponent();
        LoadStatistics();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Обновляем статистику при каждом открытии страницы
        LoadStatistics();
    }

    private void LoadStatistics()
    {
        var items = CollectionService.Instance.Items;

        // Основные метрики
        var totalItems = items.Count;
        var totalValue = items.Sum(item => item.Cost);
        var averageValue = totalItems > 0 ? totalValue / totalItems : 0;
        var lastAddedDate = items.Any() ? items.Max(item => item.AcquisitionDate) : DateTime.Now;

        TotalItemsLabel.Text = totalItems.ToString();
        TotalValueLabel.Text = $"{totalValue:N0} ₽";
        AverageValueLabel.Text = $"{averageValue:N0} ₽";
        LastAddedDateLabel.Text = lastAddedDate.ToString("dd.MM.yyyy");

        // Статистика по категориям
        LoadCategoryStats(items, totalItems);

        // Топ-5 самых ценных
        LoadMostValuableItems(items);

        // Топ-5 самых старых
        LoadOldestItems(items);
    }

    private void LoadCategoryStats(ObservableCollection<ItemModel> items, int totalItems)
    {
        CategoryStatsContainer.Children.Clear();

        var categoryGroups = items
            .GroupBy(item => item.Category)
            .Select(g => new
            {
                Category = g.Key,
                Count = g.Count(),
                Percentage = totalItems > 0 ? (int)Math.Round((double)g.Count() / totalItems * 100) : 0
            })
            .OrderByDescending(c => c.Count)
            .ToList();

        foreach (var category in categoryGroups)
        {
            var grid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto }
                },
                ColumnSpacing = 12,
                Padding = new Thickness(8, 4)
            };

            var categoryLabel = new Label
            {
                Text = category.Category,
                TextColor = Color.FromArgb("#0e1a13"),
                FontSize = 14,
                VerticalOptions = LayoutOptions.Center
            };
            grid.Add(categoryLabel, 0, 0);

            var countLabel = new Label
            {
                Text = $"{category.Count} шт",
                TextColor = Color.FromArgb("#51946c"),
                FontSize = 14,
                VerticalOptions = LayoutOptions.Center
            };
            grid.Add(countLabel, 1, 0);

            var percentLabel = new Label
            {
                Text = $"{category.Percentage}%",
                TextColor = Color.FromArgb("#38e07b"),
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center
            };
            grid.Add(percentLabel, 2, 0);

            CategoryStatsContainer.Children.Add(grid);
        }
    }

    private void LoadMostValuableItems(ObservableCollection<ItemModel> items)
    {
        MostValuableContainer.Children.Clear();

        var mostValuable = items
            .OrderByDescending(item => item.Cost)
            .Take(5)
            .ToList();

        foreach (var item in mostValuable)
        {
            var grid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                },
                ColumnSpacing = 12,
                Padding = new Thickness(8, 4)
            };

            var nameLabel = new Label
            {
                Text = item.Name,
                TextColor = Color.FromArgb("#0e1a13"),
                FontSize = 14,
                LineBreakMode = LineBreakMode.TailTruncation,
                VerticalOptions = LayoutOptions.Center
            };
            grid.Add(nameLabel, 0, 0);

            var costLabel = new Label
            {
                Text = $"{item.Cost:N0} ₽",
                TextColor = Color.FromArgb("#38e07b"),
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center
            };
            grid.Add(costLabel, 1, 0);

            MostValuableContainer.Children.Add(grid);
        }
    }

    private void LoadOldestItems(ObservableCollection<ItemModel> items)
    {
        OldestItemsContainer.Children.Clear();

        var oldestItems = items
            .OrderBy(item => item.AcquisitionDate)
            .Take(5)
            .ToList();

        foreach (var item in oldestItems)
        {
            var grid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                },
                ColumnSpacing = 12,
                Padding = new Thickness(8, 4)
            };

            var nameLabel = new Label
            {
                Text = item.Name,
                TextColor = Color.FromArgb("#0e1a13"),
                FontSize = 14,
                LineBreakMode = LineBreakMode.TailTruncation,
                VerticalOptions = LayoutOptions.Center
            };
            grid.Add(nameLabel, 0, 0);

            var dateLabel = new Label
            {
                Text = item.AcquisitionDate.ToString("dd.MM.yyyy"),
                TextColor = Color.FromArgb("#51946c"),
                FontSize = 14,
                VerticalOptions = LayoutOptions.Center
            };
            grid.Add(dateLabel, 1, 0);

            OldestItemsContainer.Children.Add(grid);
        }
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        if (!await AcquireNavigationLock()) return;

        try
        {
            // Простая пульсация кнопки назад
            if (sender is ImageButton backButton)
            {
                await backButton.ScaleTo(0.9, 80);
                await backButton.ScaleTo(1.0, 80);
            }

            await Navigation.PopAsync(false);
        }
        finally
        {
            ReleaseNavigationLock();
        }
    }

    // Обработчики для навигации внизу страницы
    private async void OnHomeNavTapped(object sender, EventArgs e)
    {
        if (!await AcquireNavigationLock()) return;

        try
        {
            await PulseAnimation(sender);
            await Navigation.PopToRootAsync(false);
        }
        finally
        {
            ReleaseNavigationLock();
        }
    }

    private async void OnAddItemNavTapped(object sender, EventArgs e)
    {
        if (!await AcquireNavigationLock()) return;

        try
        {
            await PulseAnimation(sender);
            await Navigation.PushAsync(new AddItemPage(), false);
        }
        finally
        {
            ReleaseNavigationLock();
        }
    }

    private async void OnFiltersNavTapped(object sender, EventArgs e)
    {
        if (!await AcquireNavigationLock()) return;

        try
        {
            await PulseAnimation(sender);
            await Navigation.PushAsync(new FiltersPage(), false);
        }
        finally
        {
            ReleaseNavigationLock();
        }
    }

    private async void OnStatsNavTapped(object sender, EventArgs e)
    {
        // Уже на странице статистики, просто пульсация
        if (!await AcquireNavigationLock()) return;

        try
        {
            await PulseAnimation(sender);
        }
        finally
        {
            ReleaseNavigationLock();
        }
    }

    private async void OnSettingsNavTapped(object sender, EventArgs e)
    {
        if (!await AcquireNavigationLock()) return;

        try
        {
            await PulseAnimation(sender);

            // Проверяем, существует ли страница настроек
            // Если нет - просто игнорируем нажатие или показываем сообщение
            await DisplayAlert("Информация", "Страница настроек в разработке", "OK");

            // Если у вас есть SettingsPage, раскомментируйте строку ниже:
            // await Navigation.PushAsync(new SettingsPage(), false);
        }
        finally
        {
            ReleaseNavigationLock();
        }
    }

    // Универсальная анимация пульсации
    private async Task PulseAnimation(object sender)
    {
        if (sender is VisualElement element)
        {
            await element.ScaleTo(1.1, 80);
            await element.ScaleTo(1.0, 80);
        }
    }

    private async Task<bool> AcquireNavigationLock()
    {
        if (_isNavigating) return false;
        if (!await _navigationLock.WaitAsync(0)) return false;

        _isNavigating = true;
        return true;
    }

    private void ReleaseNavigationLock()
    {
        _isNavigating = false;
        _navigationLock.Release();
    }
}