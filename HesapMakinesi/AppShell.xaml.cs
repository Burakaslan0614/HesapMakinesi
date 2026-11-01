using Microsoft.Maui.Controls;
using HesapMakinesi.Sayfalar;

namespace HesapMakinesi
{
    /// <summary>
    /// Standart Mod, Bilimsel Mod ve Ayarlar sayfası arasındaki Shell geçişlerini yöneten ana Shell sınıfı.
    /// </summary>
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Shell rotalarının (Navigation Routes) tanımlanması
            Routing.RegisterRoute(nameof(StandartSayfa), typeof(StandartSayfa));
            Routing.RegisterRoute(nameof(BilimselSayfa), typeof(BilimselSayfa));
            Routing.RegisterRoute(nameof(AyarlarSayfasi), typeof(AyarlarSayfasi));
        }
    }
}
