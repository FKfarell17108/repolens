using RepoLens.Models;

namespace RepoLens.Services;

public class ComponentDetectorService
{
    private static readonly string[] RouteFolderNames = { "pages", "app", "routes", "views" };

    private static readonly string[] ApiFolderNames = { "api", "controllers", "endpoints", "handlers" };

    private static readonly string[] ComponentFolderNames = { "components", "widgets", "ui", "elements", "shared" };

    public void Detect(RepoInfo repoInfo)
    {
        Console.WriteLine("[Components] Mendeteksi komponen kode...");

        DetectRoutes(repoInfo);
        DetectApiEndpoints(repoInfo);
        DetectUiComponents(repoInfo);

        Console.WriteLine($"[Components] Ditemukan: {repoInfo.Components.Routes.Count} routes, " +
                          $"{repoInfo.Components.ApiEndpoints.Count} endpoints, " +
                          $"{repoInfo.Components.UiComponents.Count} UI components.");
    }

    private void DetectRoutes(RepoInfo repoInfo)
    {
        var routes = new List<string>();
        bool isNextJs = repoInfo.Frameworks.Contains("Next.js");

        foreach (var file in repoInfo.Files)
        {
            var parts = file.Replace("\\", "/").Split('/');
            var fileName = Path.GetFileNameWithoutExtension(file);
            var ext = Path.GetExtension(file);

            if (!IsCodeFile(ext)) continue;

            if (isNextJs && parts.Any(p => p.Equals("app", StringComparison.OrdinalIgnoreCase)))
            {
                if (fileName.Equals("page", StringComparison.OrdinalIgnoreCase))
                {
                    var route = ExtractNextJsRoute(file, repoInfo.ProjectPath, "app");
                    if (route != null) routes.Add($"[Next.js App] {route}");
                }
            }

            else if (isNextJs && parts.Any(p => p.Equals("pages", StringComparison.OrdinalIgnoreCase)))
            {
                var route = ExtractNextJsRoute(file, repoInfo.ProjectPath, "pages");
                if (route != null && !route.Contains("/_"))
                    routes.Add($"[Next.js Page] {route}");
            }

            else if (parts.Any(p => p.Equals("routes", StringComparison.OrdinalIgnoreCase)))
            {
                routes.Add($"[Routes] {fileName}");
            }

            else if ((ext == ".tsx" || ext == ".jsx") && parts.Any(p => p.Equals("views", StringComparison.OrdinalIgnoreCase)))
            {
                routes.Add($"[View] {fileName}");
            }
        }

        repoInfo.Components.Routes = routes.Distinct().Take(50).ToList();
    }

    private string? ExtractNextJsRoute(string filePath, string projectPath, string baseFolder)
    {
        try
        {
            var relative = Path.GetRelativePath(projectPath, filePath)
                               .Replace("\\", "/");
            var idx = relative.IndexOf($"/{baseFolder}/", StringComparison.OrdinalIgnoreCase);
            if (idx < 0)
                idx = relative.StartsWith(baseFolder + "/", StringComparison.OrdinalIgnoreCase) ? -1 : -999;

            string routePart;
            if (idx >= 0)
                routePart = relative.Substring(idx + baseFolder.Length + 2);
            else if (idx == -1)
                routePart = relative.Substring(baseFolder.Length + 1);
            else
                return null;

            var dir = Path.GetDirectoryName(routePart)?.Replace("\\", "/") ?? "";
            return "/" + (string.IsNullOrEmpty(dir) ? "" : dir);
        }
        catch { return null; }
    }

    private void DetectApiEndpoints(RepoInfo repoInfo)
    {
        var endpoints = new List<string>();

        foreach (var file in repoInfo.Files)
        {
            var parts = file.Replace("\\", "/").Split('/');
            var fileName = Path.GetFileNameWithoutExtension(file);
            var ext = Path.GetExtension(file);

            if (!IsCodeFile(ext)) continue;

            bool isInPagesApi = parts.Any(p => p.Equals("pages", StringComparison.OrdinalIgnoreCase))
                             && parts.Any(p => p.Equals("api", StringComparison.OrdinalIgnoreCase));

            bool isInAppApi = parts.Any(p => p.Equals("app", StringComparison.OrdinalIgnoreCase))
                           && parts.Any(p => p.Equals("api", StringComparison.OrdinalIgnoreCase));

            if (isInPagesApi || isInAppApi)
            {
                var route = ExtractApiRoute(file, repoInfo.ProjectPath);
                if (route != null)
                    endpoints.Add($"[API] {route}");
                continue;
            }

            if (parts.Any(p => p.Equals("controllers", StringComparison.OrdinalIgnoreCase)))
            {
                endpoints.Add($"[Controller] {fileName}Controller");
                continue;
            }

            if (parts.Any(p => p.Equals("handlers", StringComparison.OrdinalIgnoreCase)
                            || p.Equals("endpoints", StringComparison.OrdinalIgnoreCase)))
            {
                endpoints.Add($"[Handler] {fileName}");
            }

            if ((ext == ".py") && IsApiFile(file))
            {
                endpoints.Add($"[Python API] {fileName}");
            }
        }

        repoInfo.Components.ApiEndpoints = endpoints.Distinct().Take(50).ToList();
    }

    private string? ExtractApiRoute(string filePath, string projectPath)
    {
        try
        {
            var relative = Path.GetRelativePath(projectPath, filePath).Replace("\\", "/").TrimStart('/');
            relative = "/" + relative;

            var apiIdx = relative.IndexOf("/api/", StringComparison.OrdinalIgnoreCase);
            if (apiIdx < 0)
            {
                return "/api/" + Path.GetFileNameWithoutExtension(filePath);
            }

            var route = relative.Substring(apiIdx);
            var ext = Path.GetExtension(filePath);
            if (!string.IsNullOrEmpty(ext) && route.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                route = route.Substring(0, route.Length - ext.Length);

            return route;
        }
        catch { return null; }
    }

    private bool IsApiFile(string filePath)
    {
        try
        {
            var content = File.ReadAllText(filePath);
            return content.Contains("@app.route") || content.Contains("@router.") ||
                   content.Contains("app.get(") || content.Contains("app.post(") ||
                   content.Contains("@Get(") || content.Contains("@Post(") ||
                   content.Contains("APIRouter") || content.Contains("@api_view");
        }
        catch { return false; }
    }

    private void DetectUiComponents(RepoInfo repoInfo)
    {
        var components = new List<string>();
        bool isReact = repoInfo.Frameworks.Any(f => f.Contains("React") || f.Contains("Next") || f.Contains("Vue"));

        foreach (var file in repoInfo.Files)
        {
            var ext = Path.GetExtension(file);
            var fileName = Path.GetFileNameWithoutExtension(file);
            var parts = file.Replace("\\", "/").Split('/');

            if (new[] { ".tsx", ".jsx", ".vue", ".svelte" }.Contains(ext))
            {
                bool inComponentFolder = parts.Any(p =>
                    ComponentFolderNames.Any(cf => p.Equals(cf, StringComparison.OrdinalIgnoreCase)));

                if (inComponentFolder)
                {
                    components.Add($"[Component] {fileName}");
                }
                else if (isReact && fileName.Length > 1 && char.IsUpper(fileName[0]))
                {
                    components.Add($"[Component] {fileName}");
                }
            }
        }

        repoInfo.Components.UiComponents = components.Distinct().Take(50).ToList();
    }

    private bool IsCodeFile(string ext)
    {
        return new[] { ".ts", ".tsx", ".js", ".jsx", ".py", ".cs", ".java",
                       ".go", ".rb", ".php", ".vue", ".svelte" }.Contains(ext);
    }
}
