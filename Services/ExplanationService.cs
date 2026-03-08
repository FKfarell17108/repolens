using RepoLens.Models;

namespace RepoLens.Services;

public class ExplanationService
{
    private static readonly Dictionary<string, string> FolderPurposeMap = new(StringComparer.OrdinalIgnoreCase)
    {
        { "frontend",         "Aplikasi web frontend (UI layer)" },
        { "client",           "Aplikasi client-side / frontend" },
        { "web",              "Aplikasi web / halaman statis" },
        { "backend",          "Layanan backend / business logic" },
        { "server",           "Server-side application" },
        { "api",              "REST API / endpoint service" },
        { "services",         "Kumpulan layanan/service modular" },
        { "microservices",    "Arsitektur microservices" },
        { "machine-learning", "Modul pelatihan model machine learning" },
        { "ml",               "Modul machine learning" },
        { "ai",               "Modul kecerdasan buatan" },
        { "data",             "Data pipeline / ETL processing" },
        { "dataset",          "Lokasi data pelatihan / dataset" },
        { "datasets",         "Kumpulan dataset pelatihan" },
        { "models",           "Model machine learning yang telah dilatih" },
        { "model",            "Definisi model (ML atau ORM)" },
        { "schemas",          "Definisi schema data / validasi" },
        { "docs",             "Dokumentasi project" },
        { "documentation",    "Dokumentasi teknis project" },
        { "src",              "Source code utama aplikasi" },
        { "lib",              "Library / modul yang dapat digunakan ulang" },
        { "utils",            "Utilitas dan helper functions" },
        { "helpers",          "Helper functions dan utilities" },
        { "components",       "Komponen UI yang dapat digunakan ulang" },
        { "pages",            "Halaman / views aplikasi" },
        { "views",            "Tampilan / view layer" },
        { "layouts",          "Template layout halaman" },
        { "controllers",      "Controller layer (MVC pattern)" },
        { "routes",           "Definisi routing aplikasi" },
        { "middleware",       "Middleware layer (auth, logging, dll)" },
        { "config",           "Konfigurasi aplikasi dan environment" },
        { "configs",          "File-file konfigurasi" },
        { "scripts",          "Script otomatisasi dan build tools" },
        { "tests",            "Test suite (unit, integration, e2e)" },
        { "test",             "File-file testing" },
        { "__tests__",        "Jest / unit test files" },
        { "spec",             "Spesifikasi / test files" },
        { "database",         "Skema database dan migrasi" },
        { "db",               "Database layer" },
        { "migrations",       "Database migration files" },
        { "seeds",            "Database seed data" },
        { "public",           "Asset statis yang dapat diakses publik" },
        { "static",           "File statis (gambar, font, dll)" },
        { "assets",           "Asset project (gambar, font, icon)" },
        { "images",           "Gambar dan media files" },
        { "styles",           "CSS / styling files" },
        { "css",              "Stylesheet files" },
        { "hooks",            "Custom React hooks" },
        { "context",          "React context providers" },
        { "store",            "State management (Redux, Zustand, dll)" },
        { "redux",            "Redux store, actions, reducers" },
        { "types",            "TypeScript type definitions" },
        { "interfaces",       "Interface definitions" },
        { "constants",        "Konstanta dan nilai tetap" },
        { "enums",            "Enumeration definitions" },
        { "validators",       "Input validation logic" },
        { "auth",             "Autentikasi dan otorisasi" },
        { "security",         "Security layer dan enkripsi" },
        { "cache",            "Caching layer" },
        { "queue",            "Message queue / job queue" },
        { "jobs",             "Background jobs dan tasks" },
        { "workers",          "Worker processes" },
        { "notifications",    "Sistem notifikasi" },
        { "email",            "Email templates dan service" },
        { "logs",             "Log files" },
        { "infrastructure",   "Infrastructure as Code (IaC)" },
        { "terraform",        "Terraform infrastructure definitions" },
        { "kubernetes",       "Kubernetes manifests" },
        { "k8s",              "Kubernetes configuration files" },
        { "helm",             "Helm charts untuk Kubernetes" },
        { "docker",           "Docker configuration files" },
        { "ci",               "CI/CD pipeline configuration" },
        { ".github",          "GitHub Actions workflows dan templates" },
        { "notebooks",        "Jupyter notebooks untuk eksplorasi data" },
        { "experiments",      "Eksperimen dan proof of concept" },
        { "reports",          "Laporan analisis dan hasil" },
        { "output",           "Output dan hasil proses" },
        { "build",            "Build artifacts (auto-generated)" },
        { "dist",             "Distribution files (auto-generated)" },
        { "apps",             "Aplikasi dalam monorepo" },
        { "packages",         "Package-package dalam monorepo" },
    };

    public void BuildFolderDescriptions(RepoInfo repoInfo)
    {
        var rootFolders = repoInfo.Folders
            .Where(f => Path.GetDirectoryName(f) == repoInfo.ProjectPath)
            .Select(f => Path.GetFileName(f))
            .Where(name => !string.IsNullOrEmpty(name))
            .OrderBy(name => name)
            .ToList();

        foreach (var folder in rootFolders)
        {
            if (FolderPurposeMap.TryGetValue(folder, out var description))
                repoInfo.FolderDescriptions[folder] = description;
            else
                repoInfo.FolderDescriptions[folder] = DetectFolderPurpose(folder, repoInfo);
        }
    }

    public string GenerateReport(RepoInfo repoInfo)
    {
        BuildFolderDescriptions(repoInfo);

        var sb = new System.Text.StringBuilder();
        var separator = new string('═', 60);
        var thinSep = new string('─', 60);

        sb.AppendLine();
        sb.AppendLine(separator);
        sb.AppendLine($"  REPOLENS — Analisis Repository");
        sb.AppendLine(separator);
        sb.AppendLine($"  Project  : {repoInfo.ProjectName}");
        sb.AppendLine($"  Lokasi   : {repoInfo.ProjectPath}");
        sb.AppendLine($"  Arsitektur: {repoInfo.TechStack.ArchitectureType ?? "Unknown"}");
        sb.AppendLine($"  File     : {repoInfo.Files.Count} | Folder: {repoInfo.Folders.Count}");
        sb.AppendLine(separator);

        sb.AppendLine();
        sb.AppendLine("  TECH STACK");
        sb.AppendLine(thinSep);

        if (repoInfo.Languages.Count > 0)
        {
            sb.AppendLine("  Bahasa Pemrograman:");
            foreach (var lang in repoInfo.Languages)
                sb.AppendLine($"     • {lang}");
        }
        else
        {
            sb.AppendLine("  Bahasa Pemrograman: (tidak terdeteksi)");
        }

        sb.AppendLine();
        if (repoInfo.Frameworks.Count > 0)
        {
            sb.AppendLine("  Framework & Libraries:");
            foreach (var fw in repoInfo.Frameworks)
                sb.AppendLine($"     • {fw}");
        }
        else
        {
            sb.AppendLine("  Framework: (tidak terdeteksi)");
        }

        sb.AppendLine();
        if (repoInfo.Tools.Count > 0)
        {
            sb.AppendLine("  Development Tools:");
            foreach (var tool in repoInfo.Tools)
                sb.AppendLine($"     • {tool}");
        }
        else
        {
            sb.AppendLine("  Tools: (tidak terdeteksi)");
        }

        sb.AppendLine();
        sb.AppendLine(thinSep);
        sb.AppendLine("  STRUKTUR UTAMA");
        sb.AppendLine(thinSep);

        if (repoInfo.FolderDescriptions.Count > 0)
        {
            foreach (var kv in repoInfo.FolderDescriptions)
                sb.AppendLine($"  📂 {kv.Key,-25} → {kv.Value}");
        }
        else
        {
            sb.AppendLine("  (Tidak ada subfolder utama yang terdeteksi)");
        }

        if (repoInfo.Components.Routes.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine(thinSep);
            sb.AppendLine("  ROUTES");
            sb.AppendLine(thinSep);
            foreach (var route in repoInfo.Components.Routes)
                sb.AppendLine($"  • {route}");
        }

        if (repoInfo.Components.ApiEndpoints.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine(thinSep);
            sb.AppendLine("  API ENDPOINTS");
            sb.AppendLine(thinSep);
            foreach (var endpoint in repoInfo.Components.ApiEndpoints)
                sb.AppendLine($"  • {endpoint}");
        }

        if (repoInfo.Components.UiComponents.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine(thinSep);
            sb.AppendLine("  UI COMPONENTS");
            sb.AppendLine(thinSep);
            var shown = repoInfo.Components.UiComponents.Take(20).ToList();
            foreach (var comp in shown)
                sb.AppendLine($"  • {comp}");
            if (repoInfo.Components.UiComponents.Count > 20)
                sb.AppendLine($"  ... dan {repoInfo.Components.UiComponents.Count - 20} component lainnya");
        }

        sb.AppendLine();
        sb.AppendLine(separator);
        sb.AppendLine("  RINGKASAN");
        sb.AppendLine(thinSep);
        sb.AppendLine(GenerateSummary(repoInfo));
        sb.AppendLine(separator);
        sb.AppendLine();

        return sb.ToString();
    }

    private string DetectFolderPurpose(string folderName, RepoInfo repoInfo)
    {
        var lower = folderName.ToLower();

        if (lower.Contains("test") || lower.Contains("spec")) return "File-file testing";
        if (lower.Contains("doc")) return "Dokumentasi project";
        if (lower.Contains("config")) return "Konfigurasi aplikasi";
        if (lower.Contains("util") || lower.Contains("helper")) return "Utilitas dan helper functions";
        if (lower.Contains("component")) return "Komponen UI";
        if (lower.Contains("model")) return "Model layer";
        if (lower.Contains("service")) return "Service layer / business logic";
        if (lower.Contains("controller")) return "Controller layer";
        if (lower.Contains("repo") || lower.Contains("repository")) return "Repository / data access layer";
        if (lower.Contains("db") || lower.Contains("database")) return "Database layer";
        if (lower.Contains("auth")) return "Autentikasi dan otorisasi";
        if (lower.Contains("api")) return "API service layer";

        return "Folder project";
    }

    private string GenerateSummary(RepoInfo repoInfo)
    {
        var lines = new List<string>();
        var arch = repoInfo.TechStack.ArchitectureType ?? "Single Application";

        if (repoInfo.Languages.Count == 0)
            lines.Add($"  Repository \"{repoInfo.ProjectName}\" adalah sebuah project software.");
        else
        {
            var langs = string.Join(", ", repoInfo.Languages.Take(3));
            lines.Add($"  Repository \"{repoInfo.ProjectName}\" adalah project {arch} yang ditulis dalam {langs}.");
        }

        if (repoInfo.Frameworks.Count > 0)
        {
            var fws = string.Join(", ", repoInfo.Frameworks.Take(4));
            lines.Add($"  Project ini menggunakan framework dan library: {fws}.");
        }

        if (repoInfo.Tools.Count > 0)
        {
            var tools = string.Join(", ", repoInfo.Tools.Take(5));
            lines.Add($"  Development tools yang digunakan mencakup: {tools}.");
        }

        if (repoInfo.Components.Routes.Count > 0 || repoInfo.Components.ApiEndpoints.Count > 0)
        {
            lines.Add($"  Terdeteksi {repoInfo.Components.Routes.Count} routes dan " +
                      $"{repoInfo.Components.ApiEndpoints.Count} API endpoints.");
        }

        if (repoInfo.Components.UiComponents.Count > 0)
            lines.Add($"  Project memiliki {repoInfo.Components.UiComponents.Count} UI component yang teridentifikasi.");

        lines.Add(string.Empty);
        lines.Add("  Jalankan RepoLens dengan flag --ai untuk mendapatkan penjelasan mendalam via AI.");

        return string.Join(Environment.NewLine, lines);
    }
}
