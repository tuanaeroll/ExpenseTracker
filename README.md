# 💸 Expense Tracker API

Papara & Patika.NET Bootcamp final projesi olarak geliştirilen bu sistem, personelin masraf taleplerini yönetime ilettiği ve onaylanan masrafların hayali bir banka API’si üzerinden simüle edildiği bir masraf takip uygulamasıdır.

---

## 📦 Kullanılan Teknolojiler

- .NET 9  
- ASP.NET Core Web API (→ `ExpenseTracker.Api`)  
- Entity Framework Core (Code-First)  
- Dapper (View & SP üzerinden raporlama)  
- SQL Server (MSSQL)  
- AutoMapper  
- FluentValidation  
- Swagger  
- JWT Authentication  
- Repository & Unit of Work Pattern  
- HttpClient (Banka API bağlantısı)  
- Custom Middleware (GlobalExceptionHandling)  
- ILoggingService<T> ile merkezi loglama  

---

## ⚙️ Kurulum Adımları

### 1️⃣ Veritabanı Bağlantısı

`appsettings.json` dosyasındaki `ConnectionStrings` ayarı:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ExpenseTrackerDb;User Id=sa;Password=yourStrong(!)Password;"
}
```

### 2️⃣ Migration ve Veritabanı Oluşturma

```bash
dotnet ef database update
```

> Bu işlem tüm tabloları ve default kullanıcıları oluşturur.

### 3️⃣ View ve SP Kurulumu

`ExpenseTracker.Data/Database/InitView.sql` dosyasını SSMS veya terminal üzerinden çalıştırın:

```bash
sqlcmd -S localhost -d ExpenseDb -i ExpenseTracker.Data/Database/InitView.sql
```

> Raporlama için zorunludur.

### 4️⃣ Banka API Bağlantısı
ExpenseTracker.Api/appsettings.json içerisindeki;

```json
"BankApi": {
  "BaseUrl": "http://localhost:5050"
}
```

> Eğer farklı bir portta çalıştırıyorsanız bu değeri değiştirin.
 ---
> Bankaya ödeme simülasyonu yapılabilmesi için:
Hem ExpenseTracker.Api (ana API)
Hem de ExpenseTrackerBank.Api (banka simülasyon API’si) projelerinde “Set as Startup Project” ayarı yapılmalıdır.
---
### 📑 Postman API Dokümantasyonu

API'ye ait tüm uç noktalar, örnek istek/yanıt gövdeleri ve açıklamalarıyla birlikte aşağıdaki Postman dokümantasyonunda yer almaktadır:

🔗 **Postman Dokümantasyon Linki:**  
[https://documenter.getpostman.com/view/39507792/2sB2j7c9Wh](https://documenter.getpostman.com/view/39507792/2sB2j7c9Wh)

> 📌 Geliştirici veya jüri, bu bağlantıyı kullanarak API'nin tüm işlevlerini kolayca test edebilir.

## 👤 Varsayılan Kullanıcılar

| Rol       | E-mail              | Şifre      |
|-----------|---------------------|------------|
| Admin     | admin@example.com    | 123456  |
| Personel  | test@example.com     | 123456  |

---

## 🔐 JWT Authentication & Authorization

- Bearer Token kullanılır: `Authorization: Bearer {token}`
- Role-based erişim: `Admin`, `User`

---

## 🧾 Masraf Süreci

- Personel masraf oluşturabilir, kendi masraflarını görüntüleyebilir ve filtreleyebilir.
- Admin tüm masrafları yönetebilir, onaylayabilir, reddedebilir.
- Red edilen taleplerde `ResponseNote` girilmesi zorunludur.
- Onaylanan masraflar Banka API’ye gönderilir.

---

## 📊 Raporlama

View ve SP’ler ile aşağıdaki raporlar sunulur:

- Personel bazlı haftalık/aylık masraflar
- Şirket genel masraf durumu
- Onay/red bazlı raporlar

> Tüm sorgular `Dapper` üzerinden çalışır. Script: `InitView.sql`

---

## ✅ FluentValidation

### `ExpenseCreateDtoValidator`

- Tutar sıfırdan büyük olmalı  
- Tarih bugünden ileri olmamalı  
- Kategori ve ödeme yöntemi zorunlu  

### `ExpenseRejectDtoValidator`

- `ResponseNote` boş geçilemez, minimum 10 karakter olmalı  

### `UserRegisterDtoValidator`

- E-posta format kontrolü  
- Şifre: büyük harf, rakam ve özel karakter içermeli  
- Telefon: 10 haneli sayı  

> Tüm validator’lar otomatik olarak DI container’a kayıtlıdır.

---

## 🧱 Entity Yapısı ve İlişkiler
<img width="561" alt="sql" src="https://github.com/user-attachments/assets/023464f7-9db2-418d-babd-e3f2030e4b2b" />

Tüm tablolar `BaseEntity` soyut sınıfını miras alır:

```csharp
public abstract class BaseEntity
{
    public int Id { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
```

### `User`

- Roller: `Admin`, `Personnel`
- Navigation: `ICollection<Expense>`

### `Expense`

- Durum: `Pending`, `Approved`, `Rejected`
- Navigation: `User`, `Category`, `PaymentMethod`
- Alan: `ResponseNote` (Red açıklaması)

### `Category` ve `PaymentMethod`

- Sadece Admin erişebilir
- Her biri birçok masrafla ilişkilidir

### İlişkiler

| Entity          | İlişkili Entity     | Türü   |
|-----------------|---------------------|--------|
| User            | Expense              | 1 → n  |
| Category        | Expense              | 1 → n  |
| PaymentMethod   | Expense              | 1 → n  |

---

## 🧠 Logging & Middleware

- `GlobalExceptionMiddleware`: Tüm hataları yakalar  
- `ILoggingService<T>`: Katmanlı loglama çözümü  

---

## 🧪 Swagger UI

Projeyi çalıştırdıktan sonra:

📎 `http://localhost:5122/swagger`

> Bu port `launchSettings.json` içindeki `applicationUrl` ayarından gelir:

```json
"applicationUrl": "http://localhost:5122"
```
<img width="454" alt="projeçalışırhali1" src="https://github.com/user-attachments/assets/a8bab101-ed80-4807-ada7-f68286bf9384" />


<img width="458" alt="projeçalışırhal2" src="https://github.com/user-attachments/assets/3efb624b-7093-44c4-b6f0-207fff3df6ed" />
 
## 📄 Lisans ve 👩‍💻 Geliştirici Bilgileri

Bu proje 🎓 **eğitim ve değerlendirme** amacıyla geliştirilmiştir.  

- 👤 **Geliştirici:** Tuana Erol  
- 📧 **E-posta:** erolltuana@gmail.com  
- 💼 **LinkedIn:** [linkedin.com/in/eroltuana](https://www.linkedin.com/in/eroltuana)  
- 🐙 **GitHub:** [github.com/tuanaeroll](https://github.com/tuanaeroll)

> 📌 Bu proje, Papara 💳 & Patika.dev 🚀 iş birliğinde düzenlenen .NET Bootcamp kapsamında geliştirilmiştir.
---
