using Microsoft.Maui.Controls;

namespace MyCollectionMobileApp
{
    public partial class SettingsPage : ContentPage
    {
        private bool _isNavigating = false;
        private SemaphoreSlim _navigationLock = new SemaphoreSlim(1, 1);

        public SettingsPage()
        {
            InitializeComponent();
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            if (!await AcquireNavigationLock()) return;

            try
            {
                await Navigation.PopAsync(false);
            }
            finally
            {
                ReleaseNavigationLock();
            }
        }

        // Новые методы для блоков "Внешний вид"
        private async void OnThemeClicked(object sender, EventArgs e)
        {
            if (!await AcquireNavigationLock()) return;
            try
            {
                await DisplayAlert("🎨 Тема", "Выбрать тему? Хорошая попытка! Здесь только светлая... пока что", "Ок, буду ждать");
            }
            finally
            {
                ReleaseNavigationLock();
            }
        }

        private async void OnAppIconClicked(object sender, EventArgs e)
        {
            if (!await AcquireNavigationLock()) return;
            try
            {
                await DisplayAlert("📱 Иконка приложения", "Хотите сменить иконку? Мечтайте! Пока только классическая", "Мечтать не вредно");
            }
            finally
            {
                ReleaseNavigationLock();
            }
        }

        // Заглушки для кнопок настроек с креативными сообщениями
        private async void OnProfileClicked(object sender, EventArgs e)
        {
            if (!await AcquireNavigationLock()) return;
            try
            {
                await DisplayAlert("👤 Профиль", "Здесь будет ваш профиль... если бы он у вас был", "Ок, я скромный");
            }
            finally
            {
                ReleaseNavigationLock();
            }
        }

        private async void OnNotificationsClicked(object sender, EventArgs e)
        {
            if (!await AcquireNavigationLock()) return;
            try
            {
                await DisplayAlert("🔔 Уведомления", "Тишина - золото! Уведомлений пока нет", "Наслаждаться тишиной");
            }
            finally
            {
                ReleaseNavigationLock();
            }
        }

        private async void OnSecurityClicked(object sender, EventArgs e)
        {
            if (!await AcquireNavigationLock()) return;
            try
            {
                await DisplayAlert("🔐 Безопасность", "Ваши данные в безопасности! Настолько, что даже мы не можем к ним подступиться", "Спать спокойно");
            }
            finally
            {
                ReleaseNavigationLock();
            }
        }

        private async void OnPrivacyClicked(object sender, EventArgs e)
        {
            if (!await AcquireNavigationLock()) return;
            try
            {
                await DisplayAlert("🕵️ Приватность", "Мы знаем о вас всё... шутка! Мы ничего не знаем и не хотим знать", "Дышать свободно");
            }
            finally
            {
                ReleaseNavigationLock();
            }
        }

        private async void OnHelpCenterClicked(object sender, EventArgs e)
        {
            if (!await AcquireNavigationLock()) return;
            try
            {
                await DisplayAlert("🆘 Центр помощи", "Вам поможет только бог... или гугл. Мы бессильны", "Молиться и гуглить");
            }
            finally
            {
                ReleaseNavigationLock();
            }
        }

        private async void OnContactUsClicked(object sender, EventArgs e)
        {
            if (!await AcquireNavigationLock()) return;
            try
            {
                await DisplayAlert("📞 Связаться с нами",
                    "Кричите в подушку - мы услышим! Или пишите на: nobody@nowhere.com\n\nP.S. Почтовый голубь в отпуске",
                    "Кричать в подушку");
            }
            finally
            {
                ReleaseNavigationLock();
            }
        }

        private async void OnTermsClicked(object sender, EventArgs e)
        {
            if (!await AcquireNavigationLock()) return;
            try
            {
                await DisplayAlert("📜 Условия использования",
                    "Условия просты:\n1. Не ломайте\n2. Не жалуйтесь\n3. Принимайте как есть\n\nВ случае проблем - см. пункт 2",
                    "Принимаю всё");
            }
            finally
            {
                ReleaseNavigationLock();
            }
        }

        private async void OnPrivacyPolicyClicked(object sender, EventArgs e)
        {
            if (!await AcquireNavigationLock()) return;
            try
            {
                await DisplayAlert("🤫 Политика конфиденциальности",
                    "Мы собираем:\n- Ничего\n- Абсолютно ничего\n- Вообще ничего\n\nВаши секреты в безопасности... пока вы их нам не расскажете",
                    "Хранить секреты");
            }
            finally
            {
                ReleaseNavigationLock();
            }
        }

        // Навигация по нижнему меню
        private async void OnHomeNavTapped(object sender, EventArgs e)
        {
            if (!await AcquireNavigationLock()) return;
            try
            {
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
                await Navigation.PushAsync(new StatisticsPage(), false);
            }
            finally
            {
                ReleaseNavigationLock();
            }
        }

        private async void OnSettingsNavTapped(object sender, EventArgs e)
        {
            // Уже на странице настроек
            if (!await AcquireNavigationLock()) return;
            try
            {
                // Просто пульсация
                if (sender is VerticalStackLayout layout)
                {
                    await layout.ScaleTo(0.95, 50);
                    await layout.ScaleTo(1.0, 50);
                }
            }
            finally
            {
                ReleaseNavigationLock();
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
}