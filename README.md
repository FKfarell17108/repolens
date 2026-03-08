<div align="center">

<pre>
██████╗ ███████╗██████╗  ██████╗ ██╗     ███████╗███╗   ██╗███████╗
██╔══██╗██╔════╝██╔══██╗██╔═══██╗██║     ██╔════╝████╗  ██║██╔════╝
██████╔╝█████╗  ██████╔╝██║   ██║██║     █████╗  ██╔██╗ ██║███████╗
██╔══██╗██╔══╝  ██╔═══╝ ██║   ██║██║     ██╔══╝  ██║╚██╗██║╚════██║
██║  ██║███████╗██║     ╚██████╔╝███████╗███████╗██║ ╚████║███████║
╚═╝  ╚═╝╚══════╝╚═╝      ╚═════╝ ╚══════╝╚══════╝╚═╝  ╚═══╝╚══════╝
</pre>

**Repository Code Analysis Tool | v1.0.0 BETA**

*Helps developers understand codebase structure quickly, without reading every file manually.*

</div>

---

## Prerequisites
[.NET 8 SDK](https://dotnet.microsoft.com/download) or later

## Clone & Build
```bash
git clone https://github.com/FKfarell17108/repolens.git
cd repolens
dotnet build
```

### Run Analysis
```bash
# Analyze any repository
dotnet run -- /path/to/your/project

# Save output to a file
dotnet run -- /path/to/project --output report.txt

# Show detailed scan logs
dotnet run -- /path/to/project --verbose

# Skip route/component detection (faster)
dotnet run -- /path/to/project --no-components
```

---

## Project Structure

```
RepoLens/
├── Models/
│   └── RepoInfo.cs
├── Services/
│   ├── RepoScannerService.cs
│   ├── TechStackService.cs
│   ├── ComponentDetectorService.cs
│   └── ExplanationService.cs
├── Program.cs
├── RepoLens.csproj
└── README.md
```

---

## Features

| Feature | Status |
|---|---|
| Recursive folder scanning | ✅ |
| Detects 20+ programming languages | ✅ |
| Detects frameworks (React, Next.js, Vue, Django, FastAPI, etc.) | ✅ |
| Detects tools (Docker, GitHub Actions, npm, Yarn, etc.) | ✅ |
| Architecture detection (Monolith, Microservices, Monorepo) | ✅ |
| Route detection (Next.js, Express) | ✅ |
| API endpoint detection | ✅ |
| UI component detection (React, Vue) | ✅ |
| Automatic folder descriptions | ✅ |
| Export report to `.txt` file | ✅ |
| Skips irrelevant folders (node_modules, .git, etc.) | ✅ |

---


## © 2026 Farell Kurniawan

Copyright © 2026 Farell Kurniawan. All rights reserved.  
Distribution and use of this code is permitted under the terms of the **MIT** license.
