using System;
using Microsoft.Maui.Controls;
using HesapMakinesi.Servisler;

namespace HesapMakinesi.Sayfalar
{
    public partial class AyarlarSayfasi : ContentPage
    {
        // Sayfa ve XAML bileşenleri yüklenirken CheckedChanged olayının
        // erken tetiklenmesini ve mükerrer çalışmasını önleyen kontrol bayrağı
        private bool _ayarYukleniyor = true;

        public AyarlarSayfasi()
        {
            InitializeComponent();
            _ayarYukleniyor = false;
        }

        /// <summary>
        /// Sayfa ekrana her geldiğinde çalışır.
        /// Kayıtlı temayı Preferences üzerinden okuyup doğru radyo butonunu işaretler.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            _ayarYukleniyor = true;

            // Tercihlerden (Preferences) mevcut temayı al
            TemaSecenegi aktifTema = TemaHizmeti.MevcutTemaTercihiniGetir();

            if (RadyoAcikTema != null && RadyoKoyuTema != null && RadyoSistemTemasi != null)
            {
                switch (aktifTema)
                {
                    case TemaSecenegi.Acik:
                        RadyoAcikTema.IsChecked = true;
                        break;
                    case TemaSecenegi.Koyu:
                        RadyoKoyuTema.IsChecked = true;
                        break;
                    case TemaSecenegi.Sistem:
                    default:
                        RadyoSistemTemasi.IsChecked = true;
                        break;
                }
            }

            _ayarYukleniyor = false;
        }

        /// <summary>
        /// Kullanıcı tema radyo butonlarından birini değiştirdiğinde anında tetiklenir.
        /// Uygulama temasını yeniden başlatmaya gerek kalmadan anında tüm sayfalara yansıtır.
        /// </summary>
        private void OnTemaSecimiDegisti(object? sender, CheckedChangedEventArgs e)
        {
            // Sayfa yükleniyorsa, radyo butonları henüz hazır değilse veya seçim kaldırıldıysa işlem yapma
            if (_ayarYukleniyor || !e.Value)
                return;

            if (RadyoAcikTema == null || RadyoKoyuTema == null || RadyoSistemTemasi == null)
                return;

            TemaSecenegi secilenTema;

            if (sender == RadyoAcikTema || RadyoAcikTema.IsChecked)
            {
                secilenTema = TemaSecenegi.Acik;
            }
            else if (sender == RadyoKoyuTema || RadyoKoyuTema.IsChecked)
            {
                secilenTema = TemaSecenegi.Koyu;
            }
            else
            {
                secilenTema = TemaSecenegi.Sistem;
            }

            // Temayı anında değiştir ve Preferences ile kalıcı olarak kaydet
            TemaHizmeti.TemayiDegistir(secilenTema);
        }
    }
}
