# BSM352 / BSM322 Görsel Programlama — Ödev 1 Video Sunum ve Değerlendirme Rehberi

Bu belge, **Windows Hesap Makinesi (.NET MAUI)** ödevinizin değerlendirme kriterlerinden **100/100 tam puan** alabilmeniz için video çekimi sırasında ekranda göstermeniz gereken dosyaları, tıklayacağınız butonları ve söyleyeceğiniz konuşma metnini adım adım içermektedir.

---

## 1. Değerlendirme Kriterleri (Rubrik) ve Proje Durumu

| Bölüm | Puan | İstenen Şart | Projedeki Durumu |
| :--- | :---: | :--- | :--- |
| **1. Standart Mod** | 15 | Dört işlem, %, 1/x, $x^2$, $\sqrt{x}$, bellek, geçmiş | Eksiksiz ve hatasız çalışıyor. |
| **2. Bilimsel Mod** | 15 | $\sin, \cos, \tan, \cot$, $\log, \ln$, $n!$, üs, DEG/RAD, F-E, $\pi, e$ | Eksiksiz ve hatasız çalışıyor. |
| **3. Shell Geçişi** | 5 | Standart, Bilimsel ve Ayarlar arası Shell geçişi | `AppShell.xaml` Flyout ile sorunsuz. |
| **4. Arayüz (XAML)** | 10 | Grid, Button, Label, RadioButton, Border | Tamamen temiz XAML ile oluşturuldu. |
| **5. Stiller** | 10 | Ayrı sözlük, örtük (implicit) ve `BasedOn` açık stiller | `HesapMakinesiStilleri.xaml` içinde tanımlı. |
| **6. Ayarlar & Tema** | 15 | Açık/Koyu/Sistem, anında geçiş, `Preferences` kalıcılığı | `TemaHizmeti.cs` ile anında ve kalıcı. |
| **7. Hata Yönetimi** | 10 | Sıfıra bölme, negatif kök; uygulama çökmemeli | "Sıfıra bölünemez", "Geçersiz giriş" mesajları veriliyor. |
| **8. Kod Kalitesi** | 5 | Modüler katmanlar, anlamlı isimlendirme, yorum satırları | Modeller, Servisler, Sayfalar ayrıldı. |
| **9. Video Sunumu** | 10 | Max 5 dk, akıcı, tüm bileşen ve adımların anlatımı | Aşağıdaki senaryo ile garanti altına alındı. |
| **10. GitHub Teslimi** | 5 | Eksiksiz repo, herkese açık video bağlantısı | Repo ve video linki kurallarına uygun. |
| **TOPLAM** | **100** | | **TAM PUAN** |

---

## 2. Video Öncesi Kontrol Listesi (Checklist)

- [ ] Visual Studio'yu açın ve projeyi `Windows Machine` modunda hazır bulundurun.
- [ ] Sol taraftaki Solution Explorer'da şu dosyaları kolay erişebileceğiniz sekmelerde açın:
  1. `HesapMakinesiStilleri.xaml`
  2. `AppShell.xaml`
  3. `HesapMakinesiMotoru.cs`
  4. `AyarlarSayfasi.xaml`
- [ ] Ekran kayıt programınızı (OBS Studio veya Windows `Win + Alt + R`) hazır edin.
- [ ] Mikrofon sesinizin net geldiğini kontrol edin.
- [ ] Sürenin **maksimum 5 dakika** olduğunu unutmayın (İdeal süre: 3.5 - 4.5 dakika).

---

## 3. Dakika Dakika Video Konuşma ve Uygulama Senaryosu

### Bölüm 1: Kendini Tanıtma (00:00 - 00:25)
* **Ekranda Görünecek Yer:** Visual Studio Solution Explorer ve açık kod editörü.
* **Söylenecek Metin:**
> *"Merhaba hocam, ben [Adınız Soyadınız], öğrenci numaram [Öğrenci Numaranız]. Bartın Üniversitesi Bilgisayar Mühendisliği Görsel Programlama dersi 1. ödevi olan .NET MAUI Windows Hesap Makinesi uygulamasını sunuyorum. Projemde Standart Mod, Bilimsel Mod ve Ayarlar sayfası yer almaktadır."*

---

### Bölüm 2: XAML, Shell ve Stil Mimarisi (00:25 - 01:30)

#### 1. Shell Sayfa Geçişi
* **Açılacak Dosya:** `AppShell.xaml`
* **Söylenecek Metin:**
> *"Sayfalar arası gezinmeyi .NET MAUI Shell yapısıyla sağladım. `AppShell.xaml` içerisinde Standart, Bilimsel ve Ayarlar sayfalarını `FlyoutItem` olarak ekledim. Böylece sol üstteki menü üzerinden sayfalar arasında veri kaybı olmadan geçiş yapılabilmektedir."*

#### 2. Stiller (Örtük, Açık ve BasedOn)
* **Açılacak Dosya:** `HesapMakinesiStilleri.xaml`
* **Söylenecek Metin:**
> *"Tasarım kriterlerine uygun olarak sayfalarda sabit renk veya font değeri yazmadım; tüm tasarımı `HesapMakinesiStilleri.xaml` kaynak sözlüğünde topladım.*
> * *Tüm butonların ortak özellikleri için `x:Key` olmadan bir **Örtük (Implicit)** buton stili tanımladım.*
> * *Rakam, İşlem, Eşittir ve Bellek butonları için `BasedOn` anahtar kelimesiyle bu temel stilden türetilen **Açık (Explicit)** stiller (`RakamButonStili`, `IslemButonStili`, `EsittirButonStili`) oluşturdum.*
> * *Kullanıcı arayüzünde sayı göstergeleri için `Label`, butonlar için `Button`, tema ayarları için `RadioButton` ve kartlar için `Border` kullandım."*

#### 3. Hesaplama Motoru
* **Açılacak Dosya:** `HesapMakinesiMotoru.cs`
* **Söylenecek Metin:**
> *"İş mantığını `HesapMakinesiMotoru` sınıfı üzerinden yönettim. Dört işlem, zincirleme hesaplamalar, hafıza (memory) fonksiyonları ve bilimsel matematiksel işlemler burada merkezi olarak işletilmektedir."*

---

### Bölüm 3: Canlı Uygulama Demosu (01:30 - 03:00)
* **Yapılacak İşlem:** Visual Studio'dan **F5** veya **Windows Machine** butonuna basarak uygulamayı çalıştırın.

#### Standart Mod Testleri:
1. $25 \times 4 =$ tuşlayın $\rightarrow$ Ekranda **100** göründüğünü ve yukarıda `25 × 4 =` geçmişini gösterin.
2. $16$ yazıp $\sqrt{x}$ butonuna basın $\rightarrow$ Ekranda **4** sonucunu gösterin.
3. $5$ yazıp $x^2$ butonuna basın $\rightarrow$ Ekranda **25** sonucunu gösterin.
4. Ekranda 25 varken **MS** (Hafızaya Kaydet) butonuna basın. **C** ile ekranı temizleyin. Ardından **MR** (Hafızadan Getir) butonuna basarak 25 sayısının geri geldiğini gösterin.
* **Söylenecek Metin:**
> *"Uygulamamız Standart modda açıldı. Dört işlem, karekök, kare alma ve bellek butonlarının doğru çalıştığını ve üst tarafta işlem geçmişinin anlık takip edildiğini görüyoruz."*

#### Bilimsel Mod Testleri:
* Sol üstteki üç çizgi menüsüne (☰) tıklayın ve **"Bilimsel"** seçeneğine geçin.
1. Açı birimi **DEG** modundayken $90$ yazıp **sin** butonuna basın $\rightarrow$ Ekranda **1** sonucunu gösterin.
2. $5$ yazıp **n!** (faktöriyel) butonuna basın $\rightarrow$ Ekranda **120** sonucunu gösterin.
3. $100$ yazıp **log** butonuna basın $\rightarrow$ Ekranda **2** sonucunu gösterin.
4. **DEG** butonuna basarak **RAD** moduna geçtiğini gösterin.
* **Söylenecek Metin:**
> *"Shell menüsünden Bilimsel moda geçiş yapıyorum. Trigonometri, faktöriyel, logaritma ve açı birimi değiştirme (DEG/RAD) özellikleri eksiksiz çalışmaktadır."*

---

### Bölüm 4: Hata Yönetimi Gösterimi (03:00 - 03:45)

1. Ekrana $15 \div 0 =$ yazın $\rightarrow$ Ekranda kırmızı/belirgin **"Sıfıra bölünemez"** yazısını gösterin. Programın **çökmediğini** vurgulayın.
2. Ekrana $9$ yazın, $+/-$ butonuyla sayıyı $-9$ yapın ve $\sqrt{x}$ tuşuna basın $\rightarrow$ Ekranda **"Geçersiz giriş"** uyarısını gösterin.
* **Söylenecek Metin:**
> *"Hata yönetimi kriteri kapsamında; herhangi bir sayıyı sıfıra böldüğümüzde program kapanmıyor veya çökmüyor; ekranda kullanıcı dostu 'Sıfıra bölünemez' uyarısı çıkıyor. Negatif sayının karekökü gibi tanımsız durumlarda ise 'Geçersiz giriş' mesajıyla hata kontrollü şekilde yakalanıyor."*

---

### Bölüm 5: Ayarlar Sayfası ve Canlı Tema Değişimi (03:45 - 04:40)

1. Menüden **"Ayarlar"** sayfasına geçin.
2. Açık tema seçiliyken **Koyu (Dark)** seçeneğine tıklayın. Uygulamanın **hiç kapanmadan anında koyu temaya geçtiğini** gösterin.
3. Menüden Standart sayfaya geri dönüp onun da anında koyu temaya uyum sağladığını gösterin.
4. **En Kritik Adım:** Uygulamayı sağ üstteki çarpıdan (`X`) kapatın.
5. Visual Studio'dan uygulamayı **tekrar başlatın**. Uygulamanın az önce seçtiğiniz **Koyu Temada** başladığını gösterin!
* **Söylenecek Metin:**
> *"Ayarlar sayfamızda Açık, Koyu ve Sistem ayarı olmak üzere 3 tema bulunmaktadır. Koyu temayı seçtiğimde `Application.Current.UserAppTheme` tetiklenerek tüm sayfalara anında yansır.*
> *Ayrıca kullanıcının seçimi .NET MAUI `Preferences` servisiyle yerel belleğe kaydedilmektedir. Uygulamayı tamamen kapatıp yeniden başlattığımda, en son seçtiğim koyu temanın otomatik olarak korunduğunu ve uygulamanın bu temayla açıldığını görebiliyoruz."*

---

### Bölüm 6: Kapanış (04:40 - 05:00)
* **Söylenecek Metin:**
> *"Ödevin tüm işlevsel ve tasarımsal gereksinimlerini tamamladım. Kaynak kodları eksiksiz olarak GitHub depoma yükledim. Beni dinlediğiniz için teşekkür ederim."*

---

## 4. GitHub ve Teslim Hatırlatmaları

1. **Reponun Görünürlüğü:** GitHub deposunu mutlaka **Public (Herkese Açık)** yapın.
2. **Gereksiz Dosyalar:** `bin` ve `obj` klasörlerini repoya yüklemeyin (Visual Studio varsayılan .gitignore dosyasını kullanır).
3. **Video Bağlantısı:**
   - YouTube'a yüklüyorsanız: **"Liste Dışı (Unlisted)"** veya **"Herkese Açık (Public)"** yapın. *"Gizli (Private)"* yaparsanız hoca videoyu izleyemez ve 0 verir!
   - Google Drive'a yüklüyorsanız: Bağlantı paylaşım ayarını **"Bağlantıya sahip herkes görüntüleyebilir"** yapın.

