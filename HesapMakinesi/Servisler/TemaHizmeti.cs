using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace HesapMakinesi.Servisler
{
    /// <summary>
    /// Kullanıcının seçebileceği tema seçeneklerini temsil eder.
    /// </summary>
    public enum TemaSecenegi
    {
        Sistem = 0,
        Acik = 1,
        Koyu = 2
    }

    /// <summary>
    /// Uygulama temasının Preferences (Tercihler) ile saklanmasını,
    /// uygulama açılışında geri yüklenmesini ve çalışma anında anında değiştirilmesini yöneten servis.
    /// </summary>
    public static class TemaHizmeti
    {
        // Preferences dosyasında temanın saklanacağı anahtar kelime
        private const string TemaTercihAnahtari = "KullaniciTemaTercihi";

        /// <summary>
        /// Uygulama başlatılırken çağrılır.
        /// Preferences'ta saklanan son tema tercihini okur ve anında uygular.
        /// </summary>
        public static void KayitliTemayiUygula()
        {
            TemaSecenegi kayitliTema = MevcutTemaTercihiniGetir();
            TemayiUygula(kayitliTema);
        }

        /// <summary>
        /// Kullanıcının seçtiği yeni temayı Preferences ile saklar ve
        /// uygulamanın mevcut çalışan temasına (UserAppTheme) anında yansıtır.
        /// </summary>
        /// <param name="secilenTema">Kullanıcının belirlediği tema (Sistem, Açık veya Koyu)</param>
        public static void TemayiDegistir(TemaSecenegi secilenTema)
        {
            // Tercihi kalıcı olarak cihaza kaydet
            Preferences.Default.Set(TemaTercihAnahtari, (int)secilenTema);

            // Uygulamanın anlık temasını güncelle
            TemayiUygula(secilenTema);
        }

        /// <summary>
        /// Preferences üzerinden daha önce kaydedilmiş tema tercihini okur.
        /// Eğer henüz bir kayıt yoksa varsayılan olarak "Sistem" (Use system setting) döner.
        /// </summary>
        public static TemaSecenegi MevcutTemaTercihiniGetir()
        {
            int deger = Preferences.Default.Get(TemaTercihAnahtari, (int)TemaSecenegi.Sistem);
            return (TemaSecenegi)deger;
        }

        /// <summary>
        /// MAUI Application.Current.UserAppTheme özelliğini hedef temaya göre ayarlar.
        /// </summary>
        private static void TemayiUygula(TemaSecenegi tema)
        {
            if (Application.Current == null)
                return;

            Application.Current.UserAppTheme = tema switch
            {
                TemaSecenegi.Acik => AppTheme.Light,
                TemaSecenegi.Koyu => AppTheme.Dark,
                _ => AppTheme.Unspecified // Sistem ayarını kullan
            };
        }
    }
}
