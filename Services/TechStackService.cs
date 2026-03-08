using System.Text.Json;
using RepoLens.Models;

namespace RepoLens.Services;

public class TechStackService
{
    private static readonly Dictionary<string, string> ExtensionToLanguage = new(StringComparer.OrdinalIgnoreCase)
    {
        { ".py",    "Python" },
        { ".js",    "JavaScript" },
        { ".jsx",   "JavaScript (React)" },
        { ".ts",    "TypeScript" },
        { ".tsx",   "TypeScript (React)" },
        { ".html",  "HTML" },
        { ".css",   "CSS" },
        { ".scss",  "SCSS" },
        { ".sass",  "Sass" },
        { ".java",  "Java" },
        { ".cs",    "C#" },
        { ".cpp",   "C++" },
        { ".c",     "C" },
        { ".h",     "C/C++ Header" },
        { ".go",    "Go" },
        { ".rs",    "Rust" },
        { ".rb",    "Ruby" },
        { ".php",   "PHP" },
        { ".swift", "Swift" },
        { ".kt",    "Kotlin" },
        { ".dart",  "Dart" },
        { ".r",     "R" },
        { ".sql",   "SQL" },
        { ".sh",    "Shell Script" },
        { ".yaml",  "YAML" },
        { ".yml",   "YAML" },
        { ".json",  "JSON" },
        { ".toml",  "TOML" },
        { ".xml",   "XML" },
        { ".md",    "Markdown" },
        { ".ipynb", "Jupyter Notebook" },
    };

    private static readonly Dictionary<string, string> DependencyToFramework = new(StringComparer.OrdinalIgnoreCase)
    {
        { "react",          "React" },
        { "next",           "Next.js" },
        { "vue",            "Vue.js" },
        { "@nuxt/core",     "Nuxt.js" },
        { "nuxt",           "Nuxt.js" },
        { "@angular/core",  "Angular" },
        { "express",        "Express.js" },
        { "fastify",        "Fastify" },
        { "koa",            "Koa.js" },
        { "nestjs",         "NestJS" },
        { "@nestjs/core",   "NestJS" },
        { "svelte",         "Svelte" },
        { "@sveltejs/kit",  "SvelteKit" },
        { "gatsby",         "Gatsby" },
        { "remix",          "Remix" },
        { "vite",           "Vite" },
        { "webpack",        "Webpack" },
        { "electron",       "Electron" },
        { "socket.io",      "Socket.IO" },
        { "graphql",        "GraphQL" },
        { "prisma",         "Prisma ORM" },
        { "mongoose",       "Mongoose" },
        { "sequelize",      "Sequelize" },
        { "typeorm",        "TypeORM" },
        { "jest",           "Jest (Testing)" },
        { "vitest",         "Vitest (Testing)" },
        { "cypress",        "Cypress (Testing)" },
        { "playwright",     "@playwright/test" },
        { "tailwindcss",    "Tailwind CSS" },
        { "@mui/material",  "Material UI" },
        { "antd",           "Ant Design" },
        { "redux",          "Redux" },
        { "@reduxjs/toolkit","Redux Toolkit" },
        { "zustand",        "Zustand" },
        { "react-query",    "React Query" },
        { "@tanstack/react-query", "TanStack Query" },
        { "axios",          "Axios" },
        { "zod",            "Zod" },

        { "flask",          "Flask" },
        { "fastapi",        "FastAPI" },
        { "django",         "Django" },
        { "tornado",        "Tornado" },
        { "aiohttp",        "aiohttp" },
        { "tensorflow",     "TensorFlow" },
        { "torch",          "PyTorch" },
        { "scikit-learn",   "Scikit-Learn" },
        { "sklearn",        "Scikit-Learn" },
        { "pandas",         "Pandas" },
        { "numpy",          "NumPy" },
        { "matplotlib",     "Matplotlib" },
        { "seaborn",        "Seaborn" },
        { "keras",          "Keras" },
        { "transformers",   "Hugging Face Transformers" },
        { "langchain",      "LangChain" },
        { "sqlalchemy",     "SQLAlchemy" },
        { "pydantic",       "Pydantic" },
        { "celery",         "Celery" },

        { "spring-boot",    "Spring Boot" },
        { "spring-web",     "Spring Web" },
        { "hibernate",      "Hibernate" },
        { "quarkus",        "Quarkus" },
        { "micronaut",      "Micronaut" },

        { "microsoft.aspnetcore", "ASP.NET Core" },
        { "entityframeworkcore",  "Entity Framework Core" },
        { "maui",           "MAUI" },
        { "blazor",         "Blazor" },

        { "gin",            "Gin (Go)" },
        { "echo",           "Echo (Go)" },
        { "fiber",          "Fiber (Go)" },
        { "gorm",           "GORM" },

        { "rails",          "Ruby on Rails" },
        { "sinatra",        "Sinatra" },
    };

    private static readonly List<(string FileOrFolder, string Tool)> ToolDetectors = new()
    {
        ("Dockerfile",          "Docker"),
        ("docker-compose.yml",  "Docker Compose"),
        ("docker-compose.yaml", "Docker Compose"),
        (".github",             "GitHub Actions"),
        (".gitlab-ci.yml",      "GitLab CI/CD"),
        ("Jenkinsfile",         "Jenkins"),
        ("package-lock.json",   "npm"),
        ("yarn.lock",           "Yarn"),
        ("pnpm-lock.yaml",      "pnpm"),
        ("bun.lockb",           "Bun"),
        (".eslintrc",           "ESLint"),
        (".eslintrc.js",        "ESLint"),
        (".eslintrc.json",      "ESLint"),
        ("prettier.config.js",  "Prettier"),
        (".prettierrc",         "Prettier"),
        ("tsconfig.json",       "TypeScript Compiler"),
        ("jest.config.js",      "Jest"),
        ("jest.config.ts",      "Jest"),
        ("vitest.config.ts",    "Vitest"),
        ("vite.config.ts",      "Vite"),
        ("vite.config.js",      "Vite"),
        (".env",                "dotenv"),
        ("Makefile",            "Make"),
        ("terraform.tf",        "Terraform"),
        (".terraform",          "Terraform"),
        ("kubernetes",          "Kubernetes"),
        ("k8s",                 "Kubernetes"),
        ("helm",                "Helm"),
        ("nginx.conf",          "Nginx"),
        (".husky",              "Husky (Git Hooks)"),
        ("lefthook.yml",        "Lefthook"),
        ("sonar-project.properties", "SonarQube"),
        (".editorconfig",       "EditorConfig"),
        ("renovate.json",       "Renovate"),
        (".dependabot",         "Dependabot"),
    };

    public void Detect(RepoInfo repoInfo)
    {
        Console.WriteLine("[TechStack] Mendeteksi tech stack...");

        DetectLanguages(repoInfo);
        DetectFrameworks(repoInfo);
        DetectTools(repoInfo);
        DetectArchitecture(repoInfo);

        repoInfo.Languages = repoInfo.TechStack.Languages;
        repoInfo.Frameworks = repoInfo.TechStack.Frameworks;
        repoInfo.Tools = repoInfo.TechStack.Tools;

        Console.WriteLine($"[TechStack] Ditemukan: {repoInfo.Languages.Count} bahasa, {repoInfo.Frameworks.Count} framework, {repoInfo.Tools.Count} tools.");
    }

    private void DetectLanguages(RepoInfo repoInfo)
    {
        var detected = new HashSet<string>();

        foreach (var file in repoInfo.Files)
        {
            var ext = Path.GetExtension(file);
            if (!string.IsNullOrEmpty(ext) && ExtensionToLanguage.TryGetValue(ext, out var lang))
                detected.Add(lang);
        }

        var programmingLanguages = detected
            .Where(l => !new[] { "YAML", "JSON", "XML", "Markdown", "TOML" }.Contains(l))
            .OrderBy(l => l)
            .ToList();

        repoInfo.TechStack.Languages = programmingLanguages;
    }

    private void DetectFrameworks(RepoInfo repoInfo)
    {
        var frameworks = new HashSet<string>();

        foreach (var file in repoInfo.Files)
        {
            var fileName = Path.GetFileName(file);

            if (fileName.Equals("package.json", StringComparison.OrdinalIgnoreCase))
                DetectFromPackageJson(file, frameworks);

            else if (fileName.Equals("requirements.txt", StringComparison.OrdinalIgnoreCase))
                DetectFromRequirementsTxt(file, frameworks);

            else if (fileName.Equals("Pipfile", StringComparison.OrdinalIgnoreCase))
                DetectFromPipfile(file, frameworks);

            else if (fileName.Equals("pyproject.toml", StringComparison.OrdinalIgnoreCase))
                DetectFromPyprojectToml(file, frameworks);

            else if (fileName.Equals("pom.xml", StringComparison.OrdinalIgnoreCase))
                DetectFromPomXml(file, frameworks);

            else if (fileName.Equals("build.gradle", StringComparison.OrdinalIgnoreCase) ||
                     fileName.Equals("build.gradle.kts", StringComparison.OrdinalIgnoreCase))
                DetectFromGradle(file, frameworks);

            else if (fileName.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase))
                DetectFromCsproj(file, frameworks);

            else if (fileName.Equals("go.mod", StringComparison.OrdinalIgnoreCase))
                DetectFromGoMod(file, frameworks);

            else if (fileName.Equals("Gemfile", StringComparison.OrdinalIgnoreCase))
                DetectFromGemfile(file, frameworks);

            else if (fileName.Equals("Cargo.toml", StringComparison.OrdinalIgnoreCase))
                DetectFromCargoToml(file, frameworks);
        }

        repoInfo.TechStack.Frameworks = frameworks.OrderBy(f => f).ToList();
    }

    private void DetectFromPackageJson(string filePath, HashSet<string> frameworks)
    {
        try
        {
            var content = File.ReadAllText(filePath);
            var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;

            ExtractJsonDependencies(root, "dependencies", frameworks);
            ExtractJsonDependencies(root, "devDependencies", frameworks);
        }
        catch {  }
    }

    private void ExtractJsonDependencies(JsonElement root, string section, HashSet<string> frameworks)
    {
        if (!root.TryGetProperty(section, out var deps)) return;

        foreach (var dep in deps.EnumerateObject())
        {
            var name = dep.Name.ToLower();
            foreach (var kv in DependencyToFramework)
            {
                if (name == kv.Key.ToLower() || name.Contains(kv.Key.ToLower()))
                {
                    frameworks.Add(kv.Value);
                    break;
                }
            }
        }
    }

    private void DetectFromRequirementsTxt(string filePath, HashSet<string> frameworks)
    {
        try
        {
            foreach (var line in File.ReadAllLines(filePath))
            {
                var pkg = line.Split("==")[0].Split(">=")[0].Split("<=")[0].Trim().ToLower();
                if (string.IsNullOrEmpty(pkg) || pkg.StartsWith("#")) continue;

                foreach (var kv in DependencyToFramework)
                {
                    if (pkg == kv.Key.ToLower() || pkg.Contains(kv.Key.ToLower()))
                    {
                        frameworks.Add(kv.Value);
                        break;
                    }
                }
            }
        }
        catch { }
    }

    private void DetectFromPipfile(string filePath, HashSet<string> frameworks)
    {
        try
        {
            foreach (var line in File.ReadAllLines(filePath))
            {
                var pkg = line.Split("=")[0].Trim().Trim('"').ToLower();
                if (string.IsNullOrEmpty(pkg) || pkg.StartsWith("[") || pkg.StartsWith("#")) continue;

                foreach (var kv in DependencyToFramework)
                {
                    if (pkg == kv.Key.ToLower())
                    {
                        frameworks.Add(kv.Value);
                        break;
                    }
                }
            }
        }
        catch { }
    }

    private void DetectFromPyprojectToml(string filePath, HashSet<string> frameworks)
    {
        try
        {
            var content = File.ReadAllText(filePath).ToLower();
            foreach (var kv in DependencyToFramework)
            {
                if (content.Contains(kv.Key.ToLower()))
                    frameworks.Add(kv.Value);
            }
        }
        catch { }
    }

    private void DetectFromPomXml(string filePath, HashSet<string> frameworks)
    {
        try
        {
            var content = File.ReadAllText(filePath).ToLower();
            foreach (var kv in DependencyToFramework)
            {
                if (content.Contains(kv.Key.ToLower()))
                    frameworks.Add(kv.Value);
            }
        }
        catch { }
    }

    private void DetectFromGradle(string filePath, HashSet<string> frameworks)
    {
        try
        {
            var content = File.ReadAllText(filePath).ToLower();
            foreach (var kv in DependencyToFramework)
            {
                if (content.Contains(kv.Key.ToLower()))
                    frameworks.Add(kv.Value);
            }
        }
        catch { }
    }

    private void DetectFromCsproj(string filePath, HashSet<string> frameworks)
    {
        try
        {
            var content = File.ReadAllText(filePath).ToLower();
            foreach (var kv in DependencyToFramework)
            {
                if (content.Contains(kv.Key.ToLower()))
                    frameworks.Add(kv.Value);
            }
        }
        catch { }
    }

    private void DetectFromGoMod(string filePath, HashSet<string> frameworks)
    {
        try
        {
            foreach (var line in File.ReadAllLines(filePath))
            {
                var lower = line.Trim().ToLower();
                foreach (var kv in DependencyToFramework)
                {
                    if (lower.Contains(kv.Key.ToLower()))
                    {
                        frameworks.Add(kv.Value);
                        break;
                    }
                }
            }
        }
        catch { }
    }

    private void DetectFromGemfile(string filePath, HashSet<string> frameworks)
    {
        try
        {
            foreach (var line in File.ReadAllLines(filePath))
            {
                var lower = line.Trim().ToLower();
                if (!lower.StartsWith("gem")) continue;
                foreach (var kv in DependencyToFramework)
                {
                    if (lower.Contains(kv.Key.ToLower()))
                    {
                        frameworks.Add(kv.Value);
                        break;
                    }
                }
            }
        }
        catch { }
    }

    private void DetectFromCargoToml(string filePath, HashSet<string> frameworks)
    {
        try
        {
            var content = File.ReadAllText(filePath).ToLower();
            foreach (var kv in DependencyToFramework)
            {
                if (content.Contains(kv.Key.ToLower()))
                    frameworks.Add(kv.Value);
            }
        }
        catch { }
    }

    private int CountIndependentSubProjects(RepoInfo repoInfo)
    {
        var projectFileNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "package.json", "pom.xml", "build.gradle", "go.mod", "Cargo.toml", "Gemfile"
        };

        return repoInfo.Files
            .Where(f => projectFileNames.Contains(Path.GetFileName(f)))
            .Where(f => {
                var rel = Path.GetRelativePath(repoInfo.ProjectPath, f);
                return rel.Contains(Path.DirectorySeparatorChar) || rel.Contains('/');
            })
            .Select(f => Path.GetDirectoryName(f))
            .Distinct()
            .Count();
    }

    private void DetectTools(RepoInfo repoInfo)
    {
        var tools = new HashSet<string>();

        foreach (var (fileOrFolder, tool) in ToolDetectors)
        {
            bool foundInFiles = repoInfo.Files.Any(p =>
                Path.GetFileName(p).Equals(fileOrFolder, StringComparison.OrdinalIgnoreCase));

            bool foundInFolders = repoInfo.Folders.Any(p =>
            {
                var name = new DirectoryInfo(p).Name;
                return name.Equals(fileOrFolder, StringComparison.OrdinalIgnoreCase);
            });

            bool foundDotFile = repoInfo.Files.Any(p =>
            {
                var name = Path.GetFileName(p);
                return name.StartsWith(fileOrFolder.TrimStart('.'), StringComparison.OrdinalIgnoreCase)
                    && fileOrFolder.StartsWith(".");
            });

            if (foundInFiles || foundInFolders || foundDotFile)
                tools.Add(tool);
        }

        repoInfo.TechStack.Tools = tools.OrderBy(t => t).ToList();
    }

    private void DetectArchitecture(RepoInfo repoInfo)
    {
        var rootFolders = repoInfo.Folders
            .Where(f => Path.GetDirectoryName(f) == repoInfo.ProjectPath)
            .Select(f => Path.GetFileName(f).ToLower())
            .ToHashSet();

        var hasDockerCompose = repoInfo.Files
            .Any(f => Path.GetFileName(f).StartsWith("docker-compose", StringComparison.OrdinalIgnoreCase));

        var multiServiceFolders = new[] { "microservices", "apps", "packages" };
        bool hasMultiService = rootFolders.Any(f => multiServiceFolders.Contains(f))
            || CountIndependentSubProjects(repoInfo) >= 3;

        var monoFolders = new[] { "frontend", "backend", "api", "client", "server" };
        int monoCount = rootFolders.Count(f => monoFolders.Contains(f));

        bool hasWorkspace = repoInfo.Files
            .Any(f => Path.GetFileName(f).Equals("lerna.json", StringComparison.OrdinalIgnoreCase)
                   || Path.GetFileName(f).Equals("nx.json", StringComparison.OrdinalIgnoreCase)
                   || Path.GetFileName(f).Equals("turbo.json", StringComparison.OrdinalIgnoreCase));

        if (hasWorkspace || hasMultiService)
            repoInfo.TechStack.ArchitectureType = "Monorepo";
        else if (hasDockerCompose && monoCount >= 2)
            repoInfo.TechStack.ArchitectureType = "Microservices";
        else if (monoCount >= 2)
            repoInfo.TechStack.ArchitectureType = "Full-Stack Monolith";
        else
            repoInfo.TechStack.ArchitectureType = "Single Application";
    }
}
