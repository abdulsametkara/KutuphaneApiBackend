# 📚 Kütüphane API - Backend

Bu proje, modern kütüphane yönetim sistemi için geliştirilmiş kapsamlı bir RESTful API'dir. .NET 8 ve Entity Framework Core kullanılarak Clean Architecture prensiplerine uygun olarak geliştirilmiştir. Kitap yönetimi, kullanıcı sistemi, kitap ödünç alma, dosya yükleme ve gelişmiş güvenlik özellikleri sunar.

## 🏗️ Proje Mimarisi

Proje, aşağıdaki katmanlardan oluşan Clean Architecture yaklaşımı benimser:

```
📁 KutuphaneApi-Backend/
├── 🎯 KutuphaneApi/          # Web API Katmanı (Controllers, Program.cs)
├── 📋 KutuphaneCore/         # Domain Katmanı (Entities, Interfaces)
├── 💾 KutuphaneDataAccess/   # Data Access Katmanı (Repository, DbContext, DTOs)
└── 🔧 KutuphaneServis/       # Business Logic Katmanı (Services, AutoMapper)
```

### Katman Açıklamaları

- **KutuphaneApi**: Web API controller'ları, middleware konfigürasyonları ve uygulama başlangıç ayarları
- **KutuphaneCore**: Domain entity'leri ve temel arayüzleri içerir
- **KutuphaneDataAccess**: Veritabanı işlemleri, Entity Framework migrations ve Repository Pattern implementasyonu
- **KutuphaneServis**: İş mantığı servisleri ve AutoMapper profilleri

## 🚀 Kullanılan Teknolojiler

### Framework & Platform

- **.NET 8.0** - Modern C# framework
- **ASP.NET Core Web API** - RESTful API geliştirme
- **Entity Framework Core 9.0.7** - ORM ve veritabanı işlemleri
- **SQL Server LocalDB** - Geliştirme ortamı veritabanı

### Güvenlik & Kimlik Doğrulama

- **JWT Bearer Authentication** - Token tabanlı kimlik doğrulama
- **ASP.NET Core Authorization** - Rol tabanlı yetkilendirme sistemi
- **Secure Token Management** - Güvenli token yönetimi

### Caching & Performance

- **Redis Cache** - Yüksek performanslı in-memory cache
- **StackExchange.Redis** - Redis istemci kütüphanesi
- **ASP.NET Core Rate Limiting** - API isteklerini sınırlama (5 istek/10 saniye)

### Veritabanı & ORM

- **Entity Framework Core** - Code First yaklaşımı
- **Entity Framework Migrations** - Veritabanı şema yönetimi
- **Repository Pattern** - Veri erişim katmanı soyutlaması
- **Generic Repository** - Tekrar kullanılabilir veri erişim katmanı

### Mapping & Validation

- **AutoMapper 12.0.1** - Object-to-object mapping
- **DTO (Data Transfer Objects)** - Veri transfer nesneleri
- **MapProfile** - AutoMapper profil konfigürasyonları

### Logging & Monitoring

- **Serilog 4.3.0** - Yapılandırılabilir loglama sistemi
- **Serilog.AspNetCore** - ASP.NET Core entegrasyonu
- **Serilog.Sinks.File** - Günlük log dosyaları

### API Documentation

- **Swagger/OpenAPI** - Interaktif API dokümantasyonu
- **Swashbuckle.AspNetCore** - Swagger entegrasyonu
- **JWT Authentication UI** - Swagger ile JWT token testi

### Cross-Origin & Configuration

- **CORS** - Cross-Origin Resource Sharing desteği
- **Configuration Management** - appsettings.json konfigürasyonu

## 📊 Veri Modeli

### Ana Entity'ler

- **📖 Book (Kitap)**
  
  ```csharp
  - Id: int
  - Title: string
  - Description: string?
  - CountofPage: int
  - FileKey: string?
  - AuthorId: int (Foreign Key)
  - CategoryId: int (Foreign Key)
  - TotalCopies: int
  - AvailableCopies: int
  ```
  
  - Kitap stok yönetimi desteği
  - Dosya ekleme özelliği
  - Yazar ve kategori ilişkileri

- **✍️ Author (Yazar)**
  
  ```csharp
  - Id: int
  - Name: string
  - Surname: string
  - BirthPlace: string?
  - BirthYear: int?
  ```
  
  - Kitaplarla one-to-many ilişki

- **📂 Category (Kategori)**
  
  ```csharp
  - Id: int
  - Name: string
  ```
  
  - Kitap kategorileri
  - Kitaplarla one-to-many ilişki

- **👤 User (Kullanıcı)**
  
  ```csharp
  - Id: int
  - Username: string
  - Password: string
  - Email: string
  - Role: string
  ```
  
  - JWT tabanlı kimlik doğrulama
  - Rol tabanlı yetkilendirme

- **📚 BookLoan (Kitap Ödünç Alma)**
  
  ```csharp
  - Id: int
  - BookId: int
  - UserId: int
  - BookTitle: string
  - UserName: string
  - LoanDate: DateTime
  - ExpectedReturnDate: DateTime?
  - ActualReturnDate: DateTime?
  - IsReturned: bool
  - Notes: string?
  ```
  
  - Kitap ödünç alma sistemi
  - Teslim tarih takibi
  - İade durumu yönetimi

- **📁 UploadedFiles (Yüklenen Dosyalar)**
  
  ```csharp
  - Id: int
  - FileName: string
  - FileKey: string
  - FilePath: string
  - ContentType: string
  ```
  
  - Dosya yükleme ve yönetimi
  - Kitap dosyaları ile ilişkilendirme



## 🔧 API Endpoints

### 📚 Kitap İşlemleri

> **Authorization:** Bearer Token gerekli

- `GET /api/Book/ListAll` - Tüm kitapları listele (Rate Limit: 5/10sn)
- `GET /api/Book/GetById?id={id}` - ID'ye göre kitap getir
- `POST /api/Book/Create` - Yeni kitap ekle (🔒 Admin rolü gerekli)
- `PUT /api/Book/Update` - Kitap güncelle (🔒 Admin rolü gerekli)
- `DELETE /api/Book/Delete?id={id}` - Kitap sil (🔒 Admin rolü gerekli)

### ✍️ Yazar İşlemleri

> **Authorization:** Bearer Token gerekli

- `GET /api/Author/ListAll` - Tüm yazarları listele
- `GET /api/Author/GetById?id={id}` - ID'ye göre yazar getir
- `POST /api/Author/Create` - Yeni yazar ekle (🔒 Admin rolü gerekli)
- `PUT /api/Author/Update` - Yazar güncelle (🔒 Admin rolü gerekli)
- `DELETE /api/Author/Delete?id={id}` - Yazar sil (🔒 Admin rolü gerekli)

### 📂 Kategori İşlemleri

> **Authorization:** Bearer Token gerekli

- `GET /api/Category/ListAll` - Tüm kategorileri listele
- `GET /api/Category/GetById?id={id}` - ID'ye göre kategori getir
- `POST /api/Category/Create` - Yeni kategori ekle (🔒 Admin rolü gerekli)
- `PUT /api/Category/Update` - Kategori güncelle (🔒 Admin rolü gerekli)
- `DELETE /api/Category/Delete?id={id}` - Kategori sil (🔒 Admin rolü gerekli)

### 📚 Kitap Ödünç Alma İşlemleri

> **Authorization:** Bearer Token gerekli

- `GET /api/BookLoan/ListAll` - Tüm ödünç alınan kitapları listele
- `GET /api/BookLoan/GetById?id={id}` - ID'ye göre ödünç alma kaydı getir
- `POST /api/BookLoan/Create` - Yeni kitap ödünç al
- `PUT /api/BookLoan/Update` - Ödünç alma kaydını güncelle
- `PUT /api/BookLoan/Return?id={id}` - Kitap iade et
- `DELETE /api/BookLoan/Delete?id={id}` - Ödünç alma kaydını sil

### 👤 Kullanıcı İşlemleri

- `POST /api/User/Register` - Kullanıcı kaydı
- `POST /api/User/Login` - Kullanıcı girişi (JWT token döner)
- `GET /api/User/Profile` - Kullanıcı profili (🔒 Token gerekli)
- `PUT /api/User/Update` - Profil güncelle (🔒 Token gerekli)

### 📁 Dosya İşlemleri

> **Authorization:** Bearer Token gerekli

- `POST /api/Upload/UploadFile` - Dosya yükleme
- `GET /api/Upload/Download?fileKey={key}` - Dosya indirme
- `DELETE /api/Upload/Delete?id={id}` - Dosya silme

### 🔐 Kimlik Doğrulama

API'ye erişim için JWT token gereklidir. Token'ı aşağıdaki şekilde kullanın:

```bash
Authorization: Bearer {your-jwt-token}
```

**Roller:**

- **User**: Temel okuma ve kendi verilerini güncelleme
- **Admin**: Tüm CRUD işlemleri ve kullanıcı yönetimi

## 🚀 Kurulum ve Çalıştırma

### Ön Gereksinimler

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) (Visual Studio ile birlikte gelir)
- [Redis Server](https://redis.io/download) (Opsiyonel - cache için)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) veya [Visual Studio Code](https://code.visualstudio.com/)

### Adım Adım Kurulum

1. **Projeyi klonlayın:**
   
   ```bash
   git clone <repository-url>
   cd KutuphaneApi-Backend
   ```

2. **Bağımlılıkları yükleyin:**
   
   ```bash
   dotnet restore
   ```

3. **Veritabanını oluşturun:**
   
   ```bash
   cd KutuphaneApi
   dotnet ef database update
   ```

4. **Redis'i başlatın (Opsiyonel):**
   
   ```bash
   redis-server
   ```

5. **Uygulamayı çalıştırın:**
   
   ```bash
   dotnet run --project KutuphaneApi
   ```

6. **Swagger UI'ya erişin:**
   
   - Tarayıcınızda `https://localhost:7XXX/swagger` adresine gidin
   - (Port numarası konsol çıktısında görünecektir)

### Yapılandırma

`appsettings.json` dosyasında aşağıdaki ayarları yapabilirsiniz:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Kutuphane2;Integrated Security=True",
    "Redis": "localhost:6379"
  },
  "Jwt": {
    "Key": "your-secret-key",
    "Issuer": "kütüphaneApi",
    "Audience": "kütüphaneApiUsers"
  }
}
```

### İlk Kullanım

1. **Admin kullanıcısı oluşturun:**
   
   - `/api/User/Register` endpoint'ini kullanarak admin rolü ile kullanıcı oluşturun

2. **JWT Token alın:**
   
   - `/api/User/Login` endpoint'i ile giriş yapın
   - Dönen token'ı Swagger'da "Authorize" butonuna ekleyin

3. **Test verileri ekleyin:**
   
   - Swagger UI üzerinden yazar, kategori ve kitap ekleyebilirsiniz

## 📝 Öğrenilen Kavramlar ve Teknolojiler

Bu proje geliştirme sürecinde aşağıdaki kavramlar ve teknolojiler öğrenilmiştir:

### 🏗️ Mimari Tasarım

- **Clean Architecture** - Katmanlı mimari yaklaşımı
- **Repository Pattern** - Veri erişim katmanı soyutlaması
- **Generic Repository Pattern** - Tekrar kullanılabilir repository
- **Dependency Injection** - Bağımlılık enjeksiyonu
- **SOLID Principles** - Yazılım tasarım prensipleri
- **Separation of Concerns** - Sorumlulukların ayrılması

### 🔐 Güvenlik ve Kimlik Doğrulama

- **JWT Authentication** - JSON Web Token kimlik doğrulama
- **Role-based Authorization** - Rol tabanlı yetkilendirme
- **Bearer Token Security** - Token güvenliği
- **Rate Limiting** - API isteklerini sınırlama
- **CORS Policy** - Cross-Origin Resource Sharing

### 💾 Veritabanı ve ORM

- **Entity Framework Core** - Code First yaklaşımı
- **Database Migrations** - Veritabanı şema yönetimi
- **LINQ** - Language Integrated Query
- **Entity Relationships** - One-to-Many, One-to-One ilişkileri
- **Database Context** - Veritabanı bağlamı yönetimi

### 🌐 Web API Geliştirme

- **RESTful API Design** - REST mimari prensipleri
- **HTTP Methods** - GET, POST, PUT, DELETE
- **HTTP Status Codes** - Doğru durum kodları kullanımı
- **Content Negotiation** - İçerik müzakeresi
- **API Versioning** - API sürüm yönetimi

### ⚡ Performans ve Cache

- **Redis Cache** - In-memory cache kullanımı
- **Caching Strategies** - Cache stratejileri
- **Rate Limiting** - İstek sınırlama
- **Asynchronous Programming** - Async/await kullanımı

### 🛠️ DevOps ve Araçlar

- **Serilog** - Yapılandırılabilir loglama sistemi
- **Swagger/OpenAPI** - API dokümantasyonu
- **Error Handling** - Global hata yönetimi
- **Configuration Management** - Yapılandırma yönetimi
- **Environment Variables** - Ortam değişkenleri

### 📦 Paket ve Kütüphane Yönetimi

- **NuGet Package Management** - Paket yönetimi
- **AutoMapper** - Object-to-object mapping
- **Third-party Integration** - Üçüncü parti entegrasyonlar
- **Dependency Management** - Bağımlılık yönetimi

## 📁 Proje Yapısı Detayları

```
📁 KutuphaneApi-Backend/
├── 🎯 KutuphaneApi/                    # Web API Katmanı
│   ├── Controllers/                    # API Controller'ları
│   │   ├── AuthorController.cs         # Yazar işlemleri
│   │   ├── BookController.cs           # Kitap işlemleri
│   │   ├── BookLoanController.cs       # Kitap ödünç alma
│   │   ├── CategoryController.cs       # Kategori işlemleri
│   │   ├── UploadController.cs         # Dosya yükleme
│   │   └── UserController.cs           # Kullanıcı işlemleri
│   ├── Properties/                     # Launch ayarları
│   ├── Logs/                          # Serilog dosyaları
│   ├── Program.cs                     # Uygulama başlangıç noktası
│   ├── appsettings.json               # Yapılandırma dosyası
│   └── KutuphaneApi.csproj           # Proje dosyası
│
├── 📋 KutuphaneCore/                   # Domain Katmanı
│   └── Entities/                       # Domain entity'leri
│       ├── Author.cs                   # Yazar entity
│       ├── Book.cs                     # Kitap entity
│       ├── BookLoan.cs                 # Kitap ödünç alma entity
│       ├── Category.cs                 # Kategori entity
│       ├── User.cs                     # Kullanıcı entity
│       ├── UploadedFiles.cs           # Dosya entity
│       └── BaseEntity.cs              # Temel entity
│
├── 💾 KutuphaneDataAccess/            # Data Access Katmanı
│   ├── DTOs/                          # Data Transfer Objects
│   ├── Migrations/                    # Entity Framework migrations
│   ├── Repository/                    # Repository implementasyonları
│   ├── DatabaseConnection.cs          # DbContext
│   └── KutuphaneDataAccess.csproj    # Proje dosyası
│
├── 🔧 KutuphaneServis/                # Business Logic Katmanı
│   ├── Interfaces/                    # Servis arayüzleri
│   ├── Service/                       # Servis implementasyonları
│   ├── MapProfile/                    # AutoMapper profilleri
│   ├── Response/                      # Response modelleri
│   └── KutuphaneServis.csproj        # Proje dosyası
│
├── KutuphaneApi-Backend.sln          # Solution dosyası
├── .gitignore                        # Git ignore dosyası
└── README.md                         # Bu dosya
```

## 🔧 Geliştirme Ortamı

### Visual Studio Code Ayarları

Proje `KutuphaneApi-Backend.code-workspace` dosyası ile VS Code'da açılabilir:

```json
{
  "folders": [
    {
      "path": "."
    }
  ],
  "settings": {
    "dotnet.defaultSolution": "KutuphaneApi-Backend.sln"
  }
}
```

### Debug ve Test

```bash
# Debug modunda çalıştırma
dotnet run --project KutuphaneApi --environment Development

# Release modunda çalıştırma
dotnet run --project KutuphaneApi --environment Production

# Unit testler (eğer eklenirse)
dotnet test
```

## 🐛 Sorun Giderme

### Yaygın Sorunlar

1. **Veritabanı bağlantı hatası:**
   
   - SQL Server LocalDB'nin çalıştığından emin olun
   - Connection string'i kontrol edin

2. **Migration hatası:**
   
   ```bash
   # Migration'ları sıfırlama
   dotnet ef database drop
   dotnet ef database update
   ```

3. **Redis bağlantı hatası:**
   
   - Redis server'ın çalıştığından emin olun
   - Redis'i devre dışı bırakmak için Program.cs'i düzenleyin

4. **JWT token hatası:**
   
   - appsettings.json'da JWT ayarlarını kontrol edin
   - Token'ın doğru formatta gönderildiğinden emin olun

## 📄 Lisans

Bu proje eğitim amaçlı geliştirilmiştir ve MIT lisansı altında lisanslanmıştır.

## 📞 İletişim

Proje hakkında sorularınız için:

- 📧 E-mail: abdulsamedkara7@gmail.com
- 💬 Issues: GitHub Issues sekmesini kullanabilirsiniz

---
