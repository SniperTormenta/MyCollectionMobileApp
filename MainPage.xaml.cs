namespace MyCollectionMobileApp
{
    public partial class MainPage : ContentPage
    {
        private Label _currentActiveNav;
        private CollectionService _collectionService;
        private bool _isNavigating = false;
        private SemaphoreSlim _navigationLock = new SemaphoreSlim(1, 1);
        private bool _isGridView = true; // По умолчанию сетка


        public MainPage()
        {
            InitializeComponent();
            _currentActiveNav = HomeLabel;
            _collectionService = CollectionService.Instance;
            BindingContext = _collectionService;


            // Устанавливаем начальный вид отображения
            UpdateViewLayout();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ItemsCollectionView.ItemsSource = null;
            ItemsCollectionView.ItemsSource = _collectionService.FilteredItems;
        }

        private void OnGridTabTapped(object sender, EventArgs e)
        {
            if (!_isGridView)
            {
                _isGridView = true;
                UpdateViewLayout();
            }
        }

        private void OnListTabTapped(object sender, EventArgs e)
        {
            if (_isGridView)
            {
                _isGridView = false;
                UpdateViewLayout();
            }
        }

        private void UpdateViewLayout()
        {
            if (_isGridView)
            {
                // Режим сетки (2 колонки)
                ItemsCollectionView.ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical)
                {
                    HorizontalItemSpacing = 12,
                    VerticalItemSpacing = 12
                };
            }
            else
            {
                // Режим списка (1 колонка)
                ItemsCollectionView.ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    ItemSpacing = 12
                };
            }
        }


        private async void OnItemTapped(object sender, EventArgs e)
        {
            if (!await AcquireNavigationLock()) return;

            try
            {
                if (sender is Border border && border.BindingContext is ItemModel item)
                {
                    // Простая пульсация
                    await border.ScaleTo(0.95, 80);
                    await border.ScaleTo(1, 80);
                    await Navigation.PushAsync(new ItemDetailsPage(item), false);
                }
            }
            finally
            {
                ReleaseNavigationLock();
            }
        }

        private async void OnHomeNavTapped(object sender, EventArgs e)
        {
            if (!await AcquireNavigationLock()) return;

            try
            {
                // Пульсация
                await PulseAnimation(HomeLabel);
                _currentActiveNav = HomeLabel;
                ResetNavColors();
                HomeLabel.TextColor = Color.FromArgb("#0e1a13");
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
                // Пульсация и переход
                await PulseAnimation(AddItemLabel);
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
                // Пульсация и переход
                await PulseAnimation(FiltersLabel);
                _currentActiveNav = FiltersLabel;
                ResetNavColors();
                FiltersLabel.TextColor = Color.FromArgb("#0e1a13");
                await Navigation.PushAsync(new FiltersPage(), false);
            }
            finally
            {
                ReleaseNavigationLock();
            }
        }

        private async void OnStatsNavTapped(object sender, EventArgs e)
        {
            if (!await AcquireNavigationLock()) return;

            try
            {
                // Пульсация и переход
                await PulseAnimation(StatsLabel);
                _currentActiveNav = StatsLabel;
                ResetNavColors();
                StatsLabel.TextColor = Color.FromArgb("#0e1a13");
                await Navigation.PushAsync(new StatisticsPage(), false);
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
                // Пульсация
                await PulseAnimation(SettingsLabel);
                _currentActiveNav = SettingsLabel;
                ResetNavColors();
                SettingsLabel.TextColor = Color.FromArgb("#0e1a13");
                await Navigation.PushAsync(new SettingsPage(), false);
            }
            finally
            {
                ReleaseNavigationLock();
            }
        }

        // Универсальная анимация пульсации
        private async Task PulseAnimation(VisualElement element)
        {
            await element.ScaleTo(1.1, 100);
            await element.ScaleTo(1.0, 100);
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

        // Сброс цветов навигации
        private void ResetNavColors()
        {
            HomeLabel.TextColor = Color.FromArgb("#51946c");
            AddItemLabel.TextColor = Color.FromArgb("#51946c");
            FiltersLabel.TextColor = Color.FromArgb("#51946c");
            StatsLabel.TextColor = Color.FromArgb("#51946c");
            SettingsLabel.TextColor = Color.FromArgb("#51946c");
        }
    }
}