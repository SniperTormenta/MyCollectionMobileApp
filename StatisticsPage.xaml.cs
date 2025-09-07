using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace MyCollectionMobileApp;

public partial class StatisticsPage : ContentPage
{
    public StatisticsPage()
    {
        InitializeComponent();
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
        await Navigation.PopAsync();
    }
}