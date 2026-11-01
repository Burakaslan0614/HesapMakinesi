using System;
using Microsoft.Maui.Controls;
using HesapMakinesi.Modeller;

namespace HesapMakinesi.Sayfalar
{
    public partial class StandartSayfa : ContentPage
    {
        // Standart mod hesaplama motoru nesnesi
        private readonly HesapMakinesiMotoru _hesapMotoru;

        public StandartSayfa()
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
        /// Dört işlem butonlarına (+, -, ×, ÷) tıklandığında çalışır.
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
        /// Tekli fonksiyon butonlarına (1/x, x², √x, %) tıklandığında çalışır.
        /// </summary>
        private void OnFonksiyonTiklandi(object? sender, EventArgs e)
        {
            if (sender is Button buton)
            {
                _hesapMotoru.TekliFonksiyonUygula(buton.Text);
                EkraniGuncelle();
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
        /// CE (Clear Entry) butonuna tıklandığında mevcut sayıyı sıfırlar.
        /// </summary>
        private void OnGirisTemizleTiklandi(object? sender, EventArgs e)
        {
            _hesapMotoru.TemizleGiris();
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
                SonucEtiketi.FontSize = 28;
            }
            else if (SonucEtiketi.Text.Length > 8)
            {
                SonucEtiketi.FontSize = 34;
            }
            else
            {
                SonucEtiketi.FontSize = 46;
            }
        }

        #endregion
    }
}
