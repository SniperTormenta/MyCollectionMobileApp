namespace MyCollectionMobileApp
{
    public partial class MainPage : ContentPage
    {
        private Label _currentActiveNav;

        public MainPage()
        {
            InitializeComponent();
            _currentActiveNav = HomeLabel;
        }

        // Обработчик нажатия на вкладку Grid
        private void OnGridTabTapped(object sender, EventArgs e)
        {
            // Активируем Grid вкладку
            VisualStateManager.GoToState(GridLabel, "Pressed");

            // Деактивируем List вкладку
            VisualStateManager.GoToState(ListLabel, "Normal");

            // Меняем внешний вид Border'ов
            GridTab.Stroke = Color.FromArgb("#38e07b");
            ListTab.Stroke = Colors.Transparent;

            // Меняем цвета текста (дополнительная визуализация)
            GridLabel.TextColor = Color.FromArgb("#0e1a13");
            ListLabel.TextColor = Color.FromArgb("#51946c");
        }

        // Обработчик нажатия на вкладку List
        private void OnListTabTapped(object sender, EventArgs e)
        {
            // Активируем List вкладку
            VisualStateManager.GoToState(ListLabel, "Pressed");

            // Деактивируем Grid вкладку
            VisualStateManager.GoToState(GridLabel, "Normal");

            // Меняем внешний вид Border'ов
            ListTab.Stroke = Color.FromArgb("#38e07b");
            GridTab.Stroke = Colors.Transparent;

            // Меняем цвета текста (дополнительная визуализация)
            ListLabel.TextColor = Color.FromArgb("#0e1a13");
            GridLabel.TextColor = Color.FromArgb("#51946c");
        }

        // Анимированное нажатие для навигационных иконок
        private async Task AnimateNavIcon(Label icon, bool isActivating)
        {
            // Анимация нажатия
            await icon.ScaleTo(0.8, 100, Easing.SpringOut);
            await icon.ScaleTo(1, 100, Easing.SpringIn);

            if (isActivating)
            {
                // Анимация активации - пульсация
                await icon.ScaleTo(1.1, 150, Easing.SinOut);
                await icon.ScaleTo(1, 150, Easing.SinIn);

                // Анимация изменения цвета
                await icon.FadeTo(1, 200);
                icon.TextColor = Color.FromArgb("#0e1a13");
            }
            else
            {
                // Анимация деактивации
                icon.TextColor = Color.FromArgb("#51946c");
                await icon.FadeTo(0.8, 200);
            }
        }

        // Анимация для Home
        private async void OnHomeNavTapped(object sender, EventArgs e)
        {
            if (_currentActiveNav != HomeLabel)
            {
                await AnimateNavIcon(_currentActiveNav, false);
                _currentActiveNav = HomeLabel;
                await AnimateNavIcon(HomeLabel, true);

                // Дополнительная логика перехода на главную
                await HomeLabel.RotateTo(360, 300, Easing.SpringOut);
                HomeLabel.Rotation = 0;
            }
        }

        // Анимация для Add Item - ПЕРЕХОД НА СТРАНИЦУ ДОБАВЛЕНИЯ
        private async void OnAddItemNavTapped(object sender, EventArgs e)
        {
            // Анимация нажатия
            await AddItemLabel.ScaleTo(0.8, 100, Easing.SpringOut);
            await AddItemLabel.ScaleTo(1, 100, Easing.SpringIn);

            // Анимация вращения для плюсика
            await AddItemLabel.RotateTo(90, 200, Easing.BounceOut);
            await AddItemLabel.RotateTo(0, 200, Easing.BounceOut);

            // ИЗМЕНИТЕ эту строку - используйте конструктор без параметров
            await Navigation.PushAsync(new AddItemPage());

            // Возвращаем цвет к исходному после перехода
            AddItemLabel.TextColor = Color.FromArgb("#51946c");
        }

        // Анимация для Filters
        private async void OnFiltersNavTapped(object sender, EventArgs e)
        {
            if (_currentActiveNav != FiltersLabel)
            {
                await AnimateNavIcon(_currentActiveNav, false);
                _currentActiveNav = FiltersLabel;
                await AnimateNavIcon(FiltersLabel, true);

                // Анимация "встряски" для инструмента
                await FiltersLabel.RotateTo(-15, 100, Easing.SinOut);
                await FiltersLabel.RotateTo(15, 100, Easing.SinOut);
                await FiltersLabel.RotateTo(-10, 80, Easing.SinOut);
                await FiltersLabel.RotateTo(10, 80, Easing.SinOut);
                await FiltersLabel.RotateTo(0, 100, Easing.SpringOut);
            }
        }

        // Анимация для Statistics
        private async void OnStatsNavTapped(object sender, EventArgs e)
        {
            if (_currentActiveNav != StatsLabel)
            {
                await AnimateNavIcon(_currentActiveNav, false);
                _currentActiveNav = StatsLabel;
                await AnimateNavIcon(StatsLabel, true);

                // Анимация "пульсации" для статистики
                await StatsLabel.ScaleTo(1.2, 150, Easing.SinOut);
                await StatsLabel.ScaleTo(1, 150, Easing.SinIn);
                await StatsLabel.ScaleTo(1.1, 100, Easing.SinOut);
                await StatsLabel.ScaleTo(1, 100, Easing.SinIn);
            }
        }

        // Анимация для Settings
        private async void OnSettingsNavTapped(object sender, EventArgs e)
        {
            if (_currentActiveNav != SettingsLabel)
            {
                await AnimateNavIcon(_currentActiveNav, false);
                _currentActiveNav = SettingsLabel;
                await AnimateNavIcon(SettingsLabel, true);

                // Анимация вращения шестеренки
                await SettingsLabel.RotateTo(360, 400, Easing.CubicInOut);
                SettingsLabel.Rotation = 0;

                // Дополнительная пульсация
                await SettingsLabel.ScaleTo(1.15, 100, Easing.SinOut);
                await SettingsLabel.ScaleTo(1, 100, Easing.SinIn);
            }
        }

        // Дополнительный метод для сброса анимаций при необходимости
        private void ResetAllNavStates()
        {
            VisualStateManager.GoToState(HomeLabel, "Normal");
            VisualStateManager.GoToState(AddItemLabel, "Normal");
            VisualStateManager.GoToState(FiltersLabel, "Normal");
            VisualStateManager.GoToState(StatsLabel, "Normal");
            VisualStateManager.GoToState(SettingsLabel, "Normal");
        }
    }
}