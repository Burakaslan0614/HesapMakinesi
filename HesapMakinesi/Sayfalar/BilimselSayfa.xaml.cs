using System;
using Microsoft.Maui.Controls;
using HesapMakinesi.Modeller;

namespace HesapMakinesi.Sayfalar
{
    public partial class BilimselSayfa : ContentPage
    {
        // Bilimsel mod hesaplama motoru nesnesi
        private readonly HesapMakinesiMotoru _hesapMotoru;

        // 2nd (İkinci işlevler) tuşu aktif mi?
        private bool _ikinciIslevAktif = false;

        public BilimselSayfa()
        {
            InitializeComponent();
            _hesapMotoru = new HesapMakinesiMotoru();
            EkraniGuncelle();
        }

        #region Buton Olay Yöneticileri (Event Handlers)

        /// <summary>
        /// Rakam butonlarına (0-9) tıklandığında çalışır.
        /// </summary>
        private void OnRakamTiklandi(object? sender, EventArgs e)
        {
            if (sender is Button buton)
            {
                _hesapMotoru.RakamEkle(buton.Text);
                EkraniGuncelle();
            }
        }

        /// <summary>
        /// Ondalık virgül (,) butonuna tıklandığında çalışır.
        /// </summary>
        private void OnVirgulTiklandi(object? sender, EventArgs e)
        {
            _hesapMotoru.OndalikVirgulEkle();
            EkraniGuncelle();
        }

        /// <summary>
        /// İkili işlem butonlarına (+, -, ×, ÷, mod, x^y) tıklandığında çalışır.
        /// </summary>
        private void OnIslemTiklandi(object? sender, EventArgs e)
        {
            if (sender is Button buton)
            {
                _hesapMotoru.IslemAyarla(buton.Text);
                EkraniGuncelle();
            }
        }

        /// <summary>
        /// Eşittir (=) butonuna tıklandığında sonucu hesaplar.
        /// </summary>
        private void OnEsittirTiklandi(object? sender, EventArgs e)
        {
            _hesapMotoru.HesaplaSonuc();
            EkraniGuncelle();
        }

        /// <summary>
        /// Bilimsel tekli fonksiyonlara (sin, cos, tan, log, ln, √x, x², |x|, 1/x, exp, n!) tıklandığında çalışır.
        /// </summary>
        private void OnFonksiyonTiklandi(object? sender, EventArgs e)
        {
            if (sender is Button buton)
            {
                string fonksiyonAdi = buton.Text;

                // Eğer 2nd (ikinci işlev) aktifse ters trigonometrik fonksiyonları uygula
                if (_ikinciIslevAktif)
                {
                    fonksiyonAdi = fonksiyonAdi.ToLowerInvariant() switch
                    {
                        "sin" => "asin",
                        "cos" => "acos",
                        "tan" => "atan",
                        _ => fonksiyonAdi
                    };
                }

                _hesapMotoru.TekliFonksiyonUygula(fonksiyonAdi);
                EkraniGuncelle();
            }
        }

        /// <summary>
        /// Pi (π) sabitini ekler.
        /// </summary>
        private void OnPiTiklandi(object? sender, EventArgs e)
        {
            _hesapMotoru.PiSabitiEkle();
            EkraniGuncelle();
        }

        /// <summary>
        /// Euler (e) sabitini ekler.
        /// </summary>
        private void OnEulerTiklandi(object? sender, EventArgs e)
        {
            _hesapMotoru.EulerSabitiEkle();
            EkraniGuncelle();
        }

        /// <summary>
        /// DEG / RAD açı birimi butonuna tıklandığında açı modunu değiştirir.
        /// </summary>
        private void OnAciBirimiTiklandi(object? sender, EventArgs e)
        {
            _hesapMotoru.AciBiriminiDegistir();
            AciBirimiButonu.Text = _hesapMotoru.SeciliAciBirimi == AciBirimi.Derece ? "DEG" : "RAD";
        }

        /// <summary>
        /// F-E (Üstel / Bilimsel format) butonuna tıklandığında gösterim modunu değiştirir.
        /// </summary>
        private void OnUstelGosterimTiklandi(object? sender, EventArgs e)
        {
            _hesapMotoru.UstelGosterimGecisi();
            EkraniGuncelle();
        }

        /// <summary>
        /// 2nd (İkinci işlevler) tuşu açılıp kapatılır.
        /// </summary>
        private void OnIkinciFonksiyonTiklandi(object? sender, EventArgs e)
        {
            _ikinciIslevAktif = !_ikinciIslevAktif;
            IkinciFonksiyonButonu.Opacity = _ikinciIslevAktif ? 1.0 : 0.7;
        }

        /// <summary>
        /// Parantez butonları tıklandığında.
        /// </summary>
        private void OnParantezTiklandi(object? sender, EventArgs e)
        {
            // Parantez tıklamasında temel işlem tetikleme veya görsel geri bildirim
            if (sender is Button buton)
            {
                if (buton.Text == ")")
                {
                    _hesapMotoru.HesaplaSonuc();
                    EkraniGuncelle();
                }
            }
        }

        /// <summary>
        /// İşaret değiştirme (+/-) butonuna tıklandığında çalışır.
        /// </summary>
        private void OnIsaretDegistirTiklandi(object? sender, EventArgs e)
        {
            _hesapMotoru.IsaretDegistir();
            EkraniGuncelle();
        }

        /// <summary>
        /// Geri alma / Silme (⌫) butonuna tıklandığında çalışır.
        /// </summary>
        private void OnSilmeTiklandi(object? sender, EventArgs e)
        {
            _hesapMotoru.SonKarakteriSil();
            EkraniGuncelle();
        }

        /// <summary>
        /// C (Clear All) butonuna tıklandığında tüm hafızayı ve işlemi temizler.
        /// </summary>
        private void OnTumuTemizleTiklandi(object? sender, EventArgs e)
        {
            _hesapMotoru.TemizleTumu();
            EkraniGuncelle();
        }

        #endregion

        #region Bellek (Memory) Olay Yöneticileri

        private void OnBellekTemizleTiklandi(object? sender, EventArgs e)
        {
            _hesapMotoru.BellekTemizle();
        }

        private void OnBellekGetirTiklandi(object? sender, EventArgs e)
        {
            _hesapMotoru.BellekGetir();
            EkraniGuncelle();
        }

        private void OnBellekEkleTiklandi(object? sender, EventArgs e)
        {
            _hesapMotoru.BellekEkle();
        }

        private void OnBellekCikarTiklandi(object? sender, EventArgs e)
        {
            _hesapMotoru.BellekCikar();
        }

        private void OnBellekKaydetTiklandi(object? sender, EventArgs e)
        {
            _hesapMotoru.BellekKaydet();
        }

        #endregion

        #region Arayüz Güncelleme

        /// <summary>
        /// Ekrandaki etiketleri motorun son durumuna göre günceller.
        /// </summary>
        private void EkraniGuncelle()
        {
            SonucEtiketi.Text = _hesapMotoru.EkranMetni;
            GecmisEtiketi.Text = _hesapMotoru.IslemGecmisiMetni;

            // Metin uzunsa buton ve font taşmasını önlemek için dinamik boyutlandırma
            if (SonucEtiketi.Text.Length > 11)
            {
                SonucEtiketi.FontSize = 26;
            }
            else if (SonucEtiketi.Text.Length > 8)
            {
                SonucEtiketi.FontSize = 32;
            }
            else
            {
                SonucEtiketi.FontSize = 44;
            }
        }

        #endregion
    }
}
