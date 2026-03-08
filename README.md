██████╗ ███████╗██████╗  ██████╗ ██╗     ███████╗███╗   ██╗███████╗
██╔══██╗██╔════╝██╔══██╗██╔═══██╗██║     ██╔════╝████╗  ██║██╔════╝
██████╔╝█████╗  ██████╔╝██║   ██║██║     █████╗  ██╔██╗ ██║███████╗
██╔══██╗██╔══╝  ██╔═══╝ ██║   ██║██║     ██╔══╝  ██║╚██╗██║╚════██║
██║  ██║███████╗██║     ╚██████╔╝███████╗███████╗██║ ╚████║███████║
╚═╝  ╚═╝╚══════╝╚═╝      ╚═════╝ ╚══════╝╚══════╝╚═╝  ╚═══╝╚══════╝

# RepoLens

> **Repository Code Analysis Tool**: Membantu developer memahami struktur codebase dengan cepat, tanpa harus membaca semua file secara manual.

---

## Cara Menjalankan

### Prasyarat
- [.NET 8 SDK](https://dotnet.microsoft.com/download) atau lebih baru

### Clone & Build
```bash
git clone <your-repo-url>
cd RepoLens
dotnet build
```

### Jalankan Analisis
```bash
# Analisis repository manapun
dotnet run -- /path/to/your/project

# Simpan hasil ke file
dotnet run -- /path/to/project --output report.txt

# Tampilkan log detail
dotnet run -- /path/to/project --verbose

# Lewati deteksi routes/components (lebih cepat)
dotnet run -- /path/to/project --no-components
```

---

## 🏗️ Struktur Project

```
RepoLens/
├── Models/
│   └── RepoInfo.cs              # Model data repository (RepoInfo, TechStack, CodeComponents)
├── Services/
│   ├── RepoScannerService.cs    # Recursive scanning seluruh file & folder
│   ├── TechStackService.cs      # Deteksi bahasa, framework, dan tools
│   ├── ComponentDetectorService.cs  # Deteksi routes, API endpoints, UI components
│   └── ExplanationService.cs    # Generate laporan teks terstruktur
├── Program.cs                   # Entry point & CLI argument parsing
├── RepoLens.csproj              # Project configuration
└── README.md
```

---

## ✨ Fitur

| Fitur | Status |
|-------|--------|
| Recursive folder scanning | ✅ |
| Deteksi 20+ bahasa pemrograman | ✅ |
| Deteksi framework (React, Next.js, Vue, Django, FastAPI, dll) | ✅ |
| Deteksi tools (Docker, GitHub Actions, npm, Yarn, dll) | ✅ |
| Deteksi arsitektur (Monolith, Microservices, Monorepo) | ✅ |
| Deteksi routes (Next.js, Express) | ✅ |
| Deteksi API endpoints | ✅ |
| Deteksi UI components (React, Vue) | ✅ |
| Deskripsi otomatis setiap folder | ✅ |
| Export laporan ke file .txt | ✅ |
| Skip folder irrelevant (node_modules, .git, dll) | ✅ |

---

## 📝 Lisensi

MIT License — bebas digunakan dan dimodifikasi.
