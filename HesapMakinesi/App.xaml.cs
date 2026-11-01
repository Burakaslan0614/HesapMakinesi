namespace HesapMakinesi
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            // Preferences'ta kayıtlı temayı uygulama başlarken yükle
            HesapMakinesi.Servisler.TemaHizmeti.KayitliTemayiUygula();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var pencere = new Window(new AppShell());

#if WINDOWS
            // Windows üzerinde pencere başlık çubuğunun (TitleBar) rengini metalik gri yap
            pencere.Created += (sender, args) =>
            {
                var platformPenceresi = pencere.Handler?.PlatformView as Microsoft.UI.Xaml.Window;
                if (platformPenceresi != null)
                {
                    var pencereTutamaci = WinRT.Interop.WindowNative.GetWindowHandle(platformPenceresi);
                    var pencereKimligi = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(pencereTutamaci);
                    var uygulamaPenceresi = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(pencereKimligi);

                    if (uygulamaPenceresi?.TitleBar != null)
                    {
                        var metalikGri = global::Windows.UI.Color.FromArgb(255, 189, 196, 204); // #BDC4CC
                        var koyuYazi = global::Windows.UI.Color.FromArgb(255, 26, 32, 44);      // #1A202C

                        uygulamaPenceresi.TitleBar.BackgroundColor = metalikGri;
                        uygulamaPenceresi.TitleBar.ForegroundColor = koyuYazi;
                        uygulamaPenceresi.TitleBar.ButtonBackgroundColor = metalikGri;
                        uygulamaPenceresi.TitleBar.ButtonForegroundColor = koyuYazi;
                        uygulamaPenceresi.TitleBar.ButtonHoverBackgroundColor = global::Windows.UI.Color.FromArgb(255, 205, 210, 217);
                        uygulamaPenceresi.TitleBar.InactiveBackgroundColor = metalikGri;
                        uygulamaPenceresi.TitleBar.ButtonInactiveBackgroundColor = metalikGri;
                    }
                }
            };
#endif

            return pencere;
        }
    }
}