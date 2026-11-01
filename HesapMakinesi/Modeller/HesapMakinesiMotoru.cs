using System;
using System.Globalization;

namespace HesapMakinesi.Modeller
{
    /// <summary>
    /// Bilimsel modda açı hesaplamaları için açı birimleri.
    /// </summary>
    public enum AciBirimi
    {
        Derece, // DEG
        Radyan  // RAD
    }

    /// <summary>
    /// Standart ve Bilimsel hesap makinesi iş mantığını (logic) yöneten motor sınıfı.
    /// </summary>
    public class HesapMakinesiMotoru
    {
        // Türk kültürüne göre ondalık ayırıcı virgül (,)
        private static readonly CultureInfo Kultur = new CultureInfo("tr-TR");

        // Durum Değişkenleri
        private double _oncekiSayi = 0;
        private string _aktifIslem = "";
        private string _sonTamamlananIslem = "";
        private double _sonEklenenSayi = 0;

        private bool _yeniGirisBekleniyor = true;
        private bool _sonucGosteriliyor = false;
        private bool _hataVar = false;
        private bool _ustelMod = false;

        // Tekli fonksiyon geçmiş takibi için
        private string _fonksiyonGecmisi = "";

        // Bellek (Memory) değeri (M+, M-, MR, MS, MC)
        private double _bellek = 0;

        /// <summary>
        /// Ana ekranda gösterilen sayı veya hata metni.
        /// </summary>
        public string EkranMetni { get; private set; } = "0";

        /// <summary>
        /// Ekranın üst kısmında gösterilen işlem geçmişi (örneğin: 12 + 4 =).
        /// </summary>
        public string IslemGecmisiMetni { get; private set; } = "";

        /// <summary>
        /// Bilimsel modda seçili olan açı birimi (Derece veya Radyan).
        /// </summary>
        public AciBirimi SeciliAciBirimi { get; private set; } = AciBirimi.Derece;

        #region Rakam ve Karakter Girişi

        /// <summary>
        /// Rakam butonlarına (0-9) basıldığında çağrılır.
        /// </summary>
        public void RakamEkle(string rakam)
        {
            if (_hataVar)
            {
                TemizleTumu();
            }

            if (_yeniGirisBekleniyor || EkranMetni == "0")
            {
                EkranMetni = rakam;
                _yeniGirisBekleniyor = false;
                _sonucGosteriliyor = false;
                _fonksiyonGecmisi = "";
            }
            else
            {
                // Ekrandaki maksimum hane sınırını kontrol et (16 hane)
                if (EkranMetni.Replace("-", "").Replace(",", "").Length < 16)
                {
                    EkranMetni += rakam;
                }
            }
        }

        /// <summary>
        /// Ondalık virgül (,) butonuna basıldığında çağrılır.
        /// </summary>
        public void OndalikVirgulEkle()
        {
            if (_hataVar)
            {
                TemizleTumu();
            }

            if (_yeniGirisBekleniyor)
            {
                EkranMetni = "0,";
                _yeniGirisBekleniyor = false;
                _sonucGosteriliyor = false;
                _fonksiyonGecmisi = "";
            }
            else if (!EkranMetni.Contains(","))
            {
                EkranMetni += ",";
            }
        }

        /// <summary>
        /// Geri alma / Silme (⌫) butonuna basıldığında son girilen karakteri siler.
        /// </summary>
        public void SonKarakteriSil()
        {
            if (_hataVar)
            {
                TemizleTumu();
                return;
            }

            // Yeni giriş bekleniyorsa veya sonuç gösteriliyorsa silme işlemi yapılmaz
            if (_yeniGirisBekleniyor || _sonucGosteriliyor)
            {
                return;
            }

            if (EkranMetni.Length > 1)
            {
                EkranMetni = EkranMetni.Substring(0, EkranMetni.Length - 1);
                // Geriye sadece eksi işareti kaldıysa veya eksi sıfır olduysa sıfır yap
                if (EkranMetni == "-" || EkranMetni == "-0")
                {
                    EkranMetni = "0";
                    _yeniGirisBekleniyor = true;
                }
            }
            else
            {
                EkranMetni = "0";
                _yeniGirisBekleniyor = true;
            }
        }

        /// <summary>
        /// İşaret değiştirme (+/-) butonu: sayının işaretini tersine çevirir.
        /// </summary>
        public void IsaretDegistir()
        {
            if (_hataVar || EkranMetni == "0")
                return;

            if (EkranMetni.StartsWith("-"))
            {
                EkranMetni = EkranMetni.Substring(1);
            }
            else
            {
                EkranMetni = "-" + EkranMetni;
            }
        }

        /// <summary>
        /// CE (Clear Entry): Sadece mevcut ekrandaki sayıyı sıfırlar.
        /// </summary>
        public void TemizleGiris()
        {
            if (_hataVar)
            {
                TemizleTumu();
                return;
            }

            EkranMetni = "0";
            _yeniGirisBekleniyor = true;
        }

        /// <summary>
        /// C (Clear All): Tüm hesaplama geçmişini ve ekranı sıfırlar.
        /// </summary>
        public void TemizleTumu()
        {
            EkranMetni = "0";
            IslemGecmisiMetni = "";
            _oncekiSayi = 0;
            _aktifIslem = "";
            _sonTamamlananIslem = "";
            _sonEklenenSayi = 0;
            _yeniGirisBekleniyor = true;
            _sonucGosteriliyor = false;
            _hataVar = false;
            _fonksiyonGecmisi = "";
        }

        #endregion

        #region İkili İşlemler (+, -, ×, ÷, mod, x^y)

        /// <summary>
        /// İki sayıyı ilgilendiren bir işlem seçildiğinde çağrılır.
        /// </summary>
        public void IslemAyarla(string islem)
        {
            if (_hataVar)
                return;

            double mevcutSayi = SayiyaDonustur(EkranMetni);

            if (!string.IsNullOrEmpty(_aktifIslem) && !_yeniGirisBekleniyor)
            {
                // Zincirleme işlem (Örn: 5 + 3 - ...) önceki işlemi ara sonuç olarak hesapla
                double araSonuc = IkiSayiHesapla(_oncekiSayi, mevcutSayi, _aktifIslem);
                if (_hataVar) return;

                _oncekiSayi = araSonuc;
                EkranMetni = SayiyiBicimlendir(araSonuc);
            }
            else
            {
                _oncekiSayi = mevcutSayi;
            }

            _aktifIslem = islem;
            IslemGecmisiMetni = $"{SayiyiBicimlendir(_oncekiSayi)} {_aktifIslem}";
            _fonksiyonGecmisi = "";
            _yeniGirisBekleniyor = true;
            _sonucGosteriliyor = false;
        }

        /// <summary>
        /// Eşittir (=) butonuna basıldığında işlemi hesaplar ve sonucu gösterir.
        /// </summary>
        public void HesaplaSonuc()
        {
            if (_hataVar)
                return;

            if (!string.IsNullOrEmpty(_aktifIslem))
            {
                double ikinciSayi = SayiyaDonustur(EkranMetni);
                double sonuc = IkiSayiHesapla(_oncekiSayi, ikinciSayi, _aktifIslem);

                if (_hataVar) return;

                string sagIfade = string.IsNullOrEmpty(_fonksiyonGecmisi)
                    ? SayiyiBicimlendir(ikinciSayi)
                    : _fonksiyonGecmisi;

                IslemGecmisiMetni = $"{SayiyiBicimlendir(_oncekiSayi)} {_aktifIslem} {sagIfade} =";

                _sonTamamlananIslem = _aktifIslem;
                _sonEklenenSayi = ikinciSayi;
                _oncekiSayi = sonuc;
                EkranMetni = SayiyiBicimlendir(sonuc);
                _aktifIslem = "";
                _fonksiyonGecmisi = "";
                _yeniGirisBekleniyor = true;
                _sonucGosteriliyor = true;
            }
            else if (!string.IsNullOrEmpty(_sonTamamlananIslem))
            {
                // Tekrar eden eşittir tuşlaması (Örn: 5 + 3 = 8, tekrar = -> 8 + 3 = 11)
                double ilkSayi = SayiyaDonustur(EkranMetni);
                double sonuc = IkiSayiHesapla(ilkSayi, _sonEklenenSayi, _sonTamamlananIslem);

                if (_hataVar) return;

                IslemGecmisiMetni = $"{SayiyiBicimlendir(ilkSayi)} {_sonTamamlananIslem} {SayiyiBicimlendir(_sonEklenenSayi)} =";
                EkranMetni = SayiyiBicimlendir(sonuc);
                _yeniGirisBekleniyor = true;
                _sonucGosteriliyor = true;
            }
            else
            {
                double sayi = SayiyaDonustur(EkranMetni);
                IslemGecmisiMetni = $"{SayiyiBicimlendir(sayi)} =";
                _yeniGirisBekleniyor = true;
                _sonucGosteriliyor = true;
            }
        }

        private double IkiSayiHesapla(double s1, double s2, string islem)
        {
            switch (islem)
            {
                case "+":
                    return s1 + s2;
                case "-":
                    return s1 - s2;
                case "×":
                case "*":
                    return s1 * s2;
                case "÷":
                case "/":
                    if (s2 == 0)
                    {
                        HataBildir("Sıfıra bölünemez");
                        return 0;
                    }
                    return s1 / s2;
                case "mod":
                case "Mod":
                case "%":
                    if (s2 == 0)
                    {
                        HataBildir("Sıfıra bölünemez");
                        return 0;
                    }
                    return s1 % s2;
                case "x^y":
                case "^":
                    return Math.Pow(s1, s2);
                default:
                    return s2;
            }
        }

        #endregion

        #region Tekli Fonksiyonlar (Trigonometri, Kök, Kare, Log, Faktöriyel vb.)

        /// <summary>
        /// Tek parametreli matematiksel fonksiyonları uygular.
        /// </summary>
        public void TekliFonksiyonUygula(string fonksiyonAdi)
        {
            if (_hataVar)
                return;

            double x = SayiyaDonustur(EkranMetni);
            double sonuc = 0;
            string fonksiyonMetni = "";
            string mevcutGosterim = string.IsNullOrEmpty(_fonksiyonGecmisi) ? SayiyiBicimlendir(x) : _fonksiyonGecmisi;

            switch (fonksiyonAdi.Trim().ToLowerInvariant())
            {
                // Yüzde (%)
                case "%":
                    if (!string.IsNullOrEmpty(_aktifIslem))
                    {
                        sonuc = (_oncekiSayi * x) / 100.0;
                    }
                    else
                    {
                        sonuc = x / 100.0;
                    }
                    fonksiyonMetni = SayiyiBicimlendir(sonuc);
                    break;

                // 1/x
                case "1/x":
                    if (x == 0)
                    {
                        HataBildir("Sıfıra bölünemez");
                        return;
                    }
                    sonuc = 1.0 / x;
                    fonksiyonMetni = $"1/({mevcutGosterim})";
                    break;

                // x²
                case "x²":
                case "x^2":
                case "sqr":
                    sonuc = x * x;
                    fonksiyonMetni = $"sqr({mevcutGosterim})";
                    break;

                // √x
                case "√x":
                case "sqrt":
                    if (x < 0)
                    {
                        HataBildir("Geçersiz giriş");
                        return;
                    }
                    sonuc = Math.Sqrt(x);
                    fonksiyonMetni = $"√({mevcutGosterim})";
                    break;

                // |x| Mutlak Değer
                case "|x|":
                case "abs":
                    sonuc = Math.Abs(x);
                    fonksiyonMetni = $"abs({mevcutGosterim})";
                    break;

                // exp: e^x (Euler üssü x)
                case "exp":
                    sonuc = Math.Exp(x);
                    fonksiyonMetni = $"exp({mevcutGosterim})";
                    break;

                // 10^x
                case "10^x":
                    sonuc = Math.Pow(10, x);
                    fonksiyonMetni = $"10^({mevcutGosterim})";
                    break;

                // log (10 tabanında logaritma)
                case "log":
                    if (x <= 0)
                    {
                        HataBildir("Geçersiz giriş");
                        return;
                    }
                    sonuc = Math.Log10(x);
                    fonksiyonMetni = $"log({mevcutGosterim})";
                    break;

                // ln (Doğal logaritma - e tabanında)
                case "ln":
                    if (x <= 0)
                    {
                        HataBildir("Geçersiz giriş");
                        return;
                    }
                    sonuc = Math.Log(x);
                    fonksiyonMetni = $"ln({mevcutGosterim})";
                    break;

                // n! (Faktöriyel)
                case "n!":
                case "fact":
                    if (x < 0 || Math.Floor(x) != x || x > 170)
                    {
                        HataBildir("Geçersiz giriş");
                        return;
                    }
                    sonuc = FaktoriyelHesapla((int)x);
                    fonksiyonMetni = $"fact({mevcutGosterim})";
                    break;

                // Trigonometri: sin
                case "sin":
                    sonuc = SinusHesapla(x);
                    fonksiyonMetni = $"sin({mevcutGosterim})";
                    break;

                // Trigonometri: cos
                case "cos":
                    sonuc = KosinusHesapla(x);
                    fonksiyonMetni = $"cos({mevcutGosterim})";
                    break;

                // Trigonometri: tan
                case "tan":
                    sonuc = TanjantHesapla(x);
                    if (_hataVar) return;
                    fonksiyonMetni = $"tan({mevcutGosterim})";
                    break;

                // Trigonometri: cot
                case "cot":
                    sonuc = KotanjantHesapla(x);
                    if (_hataVar) return;
                    fonksiyonMetni = $"cot({mevcutGosterim})";
                    break;

                // Ters Trigonometri: asin
                case "asin":
                    if (x < -1 || x > 1)
                    {
                        HataBildir("Geçersiz giriş");
                        return;
                    }
                    sonuc = Math.Asin(x);
                    if (SeciliAciBirimi == AciBirimi.Derece)
                        sonuc = RadyandanDereceye(sonuc);
                    fonksiyonMetni = $"asin({mevcutGosterim})";
                    break;

                // Ters Trigonometri: acos
                case "acos":
                    if (x < -1 || x > 1)
                    {
                        HataBildir("Geçersiz giriş");
                        return;
                    }
                    sonuc = Math.Acos(x);
                    if (SeciliAciBirimi == AciBirimi.Derece)
                        sonuc = RadyandanDereceye(sonuc);
                    fonksiyonMetni = $"acos({mevcutGosterim})";
                    break;

                // Ters Trigonometri: atan
                case "atan":
                    sonuc = Math.Atan(x);
                    if (SeciliAciBirimi == AciBirimi.Derece)
                        sonuc = RadyandanDereceye(sonuc);
                    fonksiyonMetni = $"atan({mevcutGosterim})";
                    break;

                default:
                    return;
            }

            _fonksiyonGecmisi = fonksiyonMetni;
            EkranMetni = SayiyiBicimlendir(sonuc);

            if (!string.IsNullOrEmpty(_aktifIslem))
            {
                IslemGecmisiMetni = $"{SayiyiBicimlendir(_oncekiSayi)} {_aktifIslem} {_fonksiyonGecmisi}";
            }
            else
            {
                IslemGecmisiMetni = _fonksiyonGecmisi;
            }

            _yeniGirisBekleniyor = true;
        }

        private double DerecedenRadyana(double derece) => derece * (Math.PI / 180.0);
        private double RadyandanDereceye(double radyan) => radyan * (180.0 / Math.PI);

        private double SinusHesapla(double x)
        {
            if (SeciliAciBirimi == AciBirimi.Derece)
            {
                double mod = x % 360;
                if (mod < 0) mod += 360;
                if (mod == 0 || mod == 180) return 0;
                return Math.Sin(DerecedenRadyana(x));
            }
            return Math.Sin(x);
        }

        private double KosinusHesapla(double x)
        {
            if (SeciliAciBirimi == AciBirimi.Derece)
            {
                double mod = x % 360;
                if (mod < 0) mod += 360;
                if (mod == 90 || mod == 270) return 0;
                return Math.Cos(DerecedenRadyana(x));
            }
            return Math.Cos(x);
        }

        private double TanjantHesapla(double x)
        {
            if (SeciliAciBirimi == AciBirimi.Derece)
            {
                double mod = x % 360;
                if (mod < 0) mod += 360;
                if (mod == 90 || mod == 270)
                {
                    HataBildir("Geçersiz giriş");
                    return 0;
                }
                if (mod == 0 || mod == 180) return 0;
                return Math.Tan(DerecedenRadyana(x));
            }

            double cos = Math.Cos(x);
            if (Math.Abs(cos) < 1e-15)
            {
                HataBildir("Geçersiz giriş");
                return 0;
            }
            return Math.Tan(x);
        }

        private double KotanjantHesapla(double x)
        {
            double tan = TanjantHesapla(x);
            if (_hataVar) return 0;

            if (Math.Abs(tan) < 1e-15)
            {
                HataBildir("Geçersiz giriş");
                return 0;
            }
            return 1.0 / tan;
        }

        private double FaktoriyelHesapla(int n)
        {
            if (n <= 1) return 1;
            double sonuc = 1;
            for (int i = 2; i <= n; i++)
            {
                sonuc *= i;
            }
            return sonuc;
        }

        #endregion

        #region Sabitler ve Mod Ayarları (π, e, DEG/RAD, F-E)

        /// <summary>
        /// Pi (π) sabitini ekrana yazar.
        /// </summary>
        public void PiSabitiEkle()
        {
            if (_hataVar)
            {
                TemizleTumu();
            }

            EkranMetni = SayiyiBicimlendir(Math.PI);
            _yeniGirisBekleniyor = true;
            _sonucGosteriliyor = false;
        }

        /// <summary>
        /// Euler (e) sabitini ekrana yazar.
        /// </summary>
        public void EulerSabitiEkle()
        {
            if (_hataVar)
            {
                TemizleTumu();
            }

            EkranMetni = SayiyiBicimlendir(Math.E);
            _yeniGirisBekleniyor = true;
            _sonucGosteriliyor = false;
        }

        /// <summary>
        /// DEG ve RAD açı birimleri arasında geçiş yapar.
        /// </summary>
        public void AciBiriminiDegistir()
        {
            SeciliAciBirimi = SeciliAciBirimi == AciBirimi.Derece ? AciBirimi.Radyan : AciBirimi.Derece;
        }

        /// <summary>
        /// F-E (Üstel / Bilimsel gösterim) modunu açıp kapatır.
        /// </summary>
        public void UstelGosterimGecisi()
        {
            if (_hataVar)
                return;

            _ustelMod = !_ustelMod;
            double sayi = SayiyaDonustur(EkranMetni);
            EkranMetni = SayiyiBicimlendir(sayi);
        }

        #endregion

        #region Bellek (Memory) İşlemleri (MC, MR, M+, M-, MS)

        /// <summary>
        /// Belleği temizler (MC - Memory Clear).
        /// </summary>
        public void BellekTemizle()
        {
            _bellek = 0;
        }

        /// <summary>
        /// Bellekteki sayıyı ekrana getirir (MR - Memory Recall).
        /// </summary>
        public void BellekGetir()
        {
            if (_hataVar)
                TemizleTumu();

            EkranMetni = SayiyiBicimlendir(_bellek);
            _yeniGirisBekleniyor = true;
            _sonucGosteriliyor = false;
        }

        /// <summary>
        /// Ekrandaki sayıyı belleğe ekler (M+ - Memory Add).
        /// </summary>
        public void BellekEkle()
        {
            if (!_hataVar)
            {
                _bellek += SayiyaDonustur(EkranMetni);
                _yeniGirisBekleniyor = true;
            }
        }

        /// <summary>
        /// Ekrandaki sayıyı bellekten çıkarır (M- - Memory Subtract).
        /// </summary>
        public void BellekCikar()
        {
            if (!_hataVar)
            {
                _bellek -= SayiyaDonustur(EkranMetni);
                _yeniGirisBekleniyor = true;
            }
        }

        /// <summary>
        /// Ekrandaki sayıyı doğrudan belleğe kaydeder (MS - Memory Store).
        /// </summary>
        public void BellekKaydet()
        {
            if (!_hataVar)
            {
                _bellek = SayiyaDonustur(EkranMetni);
                _yeniGirisBekleniyor = true;
            }
        }

        #endregion

        #region Yardımcı Dönüştürme ve Biçimlendirme Metotları

        private double SayiyaDonustur(string metin)
        {
            if (string.IsNullOrWhiteSpace(metin))
                return 0;

            // Boşlukları temizle ve nokta/virgül uyumunu sağla
            string temizMetin = metin.Trim().Replace(" ", "");

            if (double.TryParse(temizMetin, NumberStyles.Any, Kultur, out double sonuc))
            {
                return sonuc;
            }

            // Invariant fallback
            if (double.TryParse(temizMetin.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double invSonuc))
            {
                return invSonuc;
            }

            return 0;
        }

        private string SayiyiBicimlendir(double sayi)
        {
            if (double.IsNaN(sayi) || double.IsInfinity(sayi))
            {
                HataBildir("Geçersiz giriş");
                return "Geçersiz giriş";
            }

            // Yuvarlama hatalarını engellemek için çok küçük sayıları sıfırla
            if (Math.Abs(sayi) < 1e-14)
            {
                sayi = 0;
            }

            if (_ustelMod)
            {
                return sayi.ToString("0.######e+0", Kultur);
            }

            // 15 basamağa kadar genel sayı biçimi
            return sayi.ToString("G15", Kultur);
        }

        private void HataBildir(string mesaj)
        {
            _hataVar = true;
            EkranMetni = mesaj;
            IslemGecmisiMetni = "";
            _aktifIslem = "";
            _fonksiyonGecmisi = "";
            _yeniGirisBekleniyor = true;
        }

        #endregion
    }
}
