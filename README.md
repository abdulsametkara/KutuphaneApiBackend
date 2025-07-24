# 📚 Kütüphane API - Backend

Bu proje, modern kütüphane yönetim sistemi için geliştirilmiş bir RESTful API'dir. .NET 8 ve Entity Framework Core kullanılarak Clean Architecture prensiplerine uygun olarak geliştirilmiştir.

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
- **SQL Server** - Veritabanı yönetim sistemi

### Güvenlik & Kimlik Doğrulama

- **JWT Bearer Authentication** - Token tabanlı kimlik doğrulama
- **ASP.NET Core Authorization** - Yetkilendirme sistemi

### Veritabanı & ORM

- **Entity Framework Core** - Code First yaklaşımı
- **Entity Framework Migrations** - Veritabanı şema yönetimi
- **Repository Pattern** - Veri erişim katmanı soyutlaması

### Mapping & Validation

- **AutoMapper 12.0.1** - Object-to-object mapping
- **DTO (Data Transfer Objects)** - Veri transfer nesneleri

### Logging & Monitoring

- **Serilog** - Yapılandırılabilir loglama sistemi
- **Serilog.AspNetCore** - ASP.NET Core entegrasyonu
- **Serilog.Sinks.File** - Dosya tabanlı log kayıtları

### API Documentation

- **Swagger/OpenAPI** - API dokümantasyonu ve test arayüzü
- **Swashbuckle.AspNetCore** - Swagger entegrasyonu

### Rate Limiting & Performance

- **ASP.NET Core Rate Limiting** - API isteklerini sınırlama

## 📊 Veri Modeli

### Ana Entity'ler

- **📖 Book (Kitap)**
  
  - Başlık, açıklama, sayfa sayısı
  - Yazar ve kategori ilişkileri
  - Dosya yükleme desteği

- **✍️ Author (Yazar)**
  
  - Ad, soyad, doğum yeri ve yılı
  - Kitaplarla one-to-many ilişki

- **📂 Category (Kategori)**
  
  - Kitap kategorileri
  - Kitaplarla one-to-many ilişki

- **👤 User (Kullanıcı)**
  
  - Kullanıcı yönetimi ve kimlik doğrulama

- **📁 UploadedFiles (Yüklenen Dosyalar)**
  
  - Dosya yükleme ve yönetimi

## 🔧 API Endpoints

### 📚 Kitap İşlemleri

- `GET /api/Book/ListAll` - Tüm kitapları listele
- `POST /api/Book` - Yeni kitap ekle
- `PUT /api/Book` - Kitap güncelle
- `DELETE /api/Book/{id}` - Kitap sil

### ✍️ Yazar İşlemleri

- `GET /api/Author/ListAll` - Tüm yazarları listele
- `POST /api/Author` - Yeni yazar ekle
- `PUT /api/Author` - Yazar güncelle
- `DELETE /api/Author/{id}` - Yazar sil

### 📂 Kategori İşlemleri

- `GET /api/Category/ListAll` - Tüm kategorileri listele
- `POST /api/Category` - Yeni kategori ekle
- `PUT /api/Category` - Kategori güncelle
- `DELETE /api/Category/{id}` - Kategori sil

### 👤 Kullanıcı İşlemleri

- Kullanıcı kaydı ve girişi
- JWT token yönetimi

### 📁 Dosya İşlemleri

- Dosya yükleme ve indirme
- Dosya yönetimi

## 📝 Öğrenilen Kavramlar

Bu proje geliştirme sürecinde aşağıdaki kavramlar ve teknolojiler öğrenilmiştir:

### 🏗️ Mimari Tasarım

- **Clean Architecture** - Katmanlı mimari yaklaşımı
- **Repository Pattern** - Veri erişim katmanı soyutlaması
- **Dependency Injection** - Bağımlılık enjeksiyonu
- **SOLID Principles** - Yazılım tasarım prensipleri

### 🔐 Güvenlik

- **JWT Authentication** - JSON Web Token kimlik doğrulama
- **Authorization** - Yetkilendirme mekanizmaları
- **Rate Limiting** - API isteklerini sınırlama

### 💾 Veritabanı

- **Entity Framework Core** - Code First yaklaşımı
- **Database Migrations** - Veritabanı şema yönetimi
- **LINQ** - Language Integrated Query
- **Relationships** - Entity ilişkileri (One-to-Many, Many-to-Many)

### 🌐 Web API

- **RESTful API Design** - REST mimari prensipleri
- **HTTP Methods** - GET, POST, PUT, DELETE
- **Status Codes** - HTTP durum kodları
- **Content Negotiation** - İçerik müzakeresi

### 🛠️ DevOps & Tools

- **Logging** - Serilog ile loglama
- **API Documentation** - Swagger/OpenAPI
- **Error Handling** - Hata yönetimi
- **Configuration Management** - Yapılandırma yönetimi

### 📦 Paket Yönetimi

- **NuGet Packages** - Paket yönetimi
- **AutoMapper** - Object mapping
- **Third-party Libraries** - Üçüncü parti kütüphaneler

## 📁 Proje Yapısı Detayları

```
📁 KutuphaneApi/
├── Controllers/          # API Controller'ları
├── Properties/          # Launch ayarları
├── Logs/               # Log dosyaları
└── Program.cs          # Uygulama başlangıç noktası

📁 KutuphaneCore/
└── Entities/           # Domain entity'leri
    ├── Author.cs
    ├── Book.cs
    ├── Category.cs
    ├── User.cs
    └── BaseEntity.cs

📁 KutuphaneDataAccess/
├── DTOs/               # Data Transfer Objects
├── Migrations/         # EF Core migrations
├── Repository/         # Repository implementasyonları
└── DatabaseConnection.cs

📁 KutuphaneServis/
├── Interfaces/         # Servis arayüzleri
├── Service/           # Servis implementasyonları
├── MapProfile/        # AutoMapper profilleri
└── Response/          # Response modelleri
```

## 📄 Lisans

Bu proje eğitim amaçlı geliştirilmiştir ve MIT lisansı altında lisanslanmıştır.

---

**Not:** Bu proje, modern .NET teknolojileri ile RESTful API geliştirme konularında pratik yapmak amacıyla oluşturulmuştur. Kütüphane yönetim sistemi senaryosu üzerinden gerçek dünya projeleri için gerekli teknolojiler ve yaklaşımlar öğrenilmiştir.
