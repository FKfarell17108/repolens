using RepoLens.Models;

namespace RepoLens.Services;

public class RepoScannerService
{
    private static readonly HashSet<string> IgnoredFolders = new(StringComparer.OrdinalIgnoreCase)
    {
        ".git", "node_modules", "bin", "obj", "dist", "build",
        ".next", "__pycache__", ".venv", "venv", "env",
        ".idea", ".vscode", "coverage", ".nyc_output",
        "target", ".gradle", "vendor", "packages",
        "{models,services}", "{models}", "{services}"
    };

    public RepoInfo Scan(string projectPath)
    {
        if (!Directory.Exists(projectPath))
            throw new DirectoryNotFoundException($"Path tidak ditemukan: {projectPath}");

        var repoInfo = new RepoInfo
        {
            ProjectPath = Path.GetFullPath(projectPath),
            ProjectName = Path.GetFileName(Path.GetFullPath(projectPath))
        };

        Console.WriteLine($"[Scanner] Memindai repository: {repoInfo.ProjectPath}");

        ScanDirectory(repoInfo.ProjectPath, repoInfo);

        Console.WriteLine($"[Scanner] Selesai. Ditemukan {repoInfo.Files.Count} file dan {repoInfo.Folders.Count} folder.");

        return repoInfo;
    }

    private void ScanDirectory(string currentPath, RepoInfo repoInfo)
    {
        try
        {
            foreach (var file in Directory.GetFiles(currentPath))
            {
                repoInfo.Files.Add(file);
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"[Scanner] Akses ditolak: {currentPath}");
            return;
        }

        foreach (var subDir in Directory.GetDirectories(currentPath))
        {
            var folderName = Path.GetFileName(subDir);

            if (IgnoredFolders.Contains(folderName))
            {
                Console.WriteLine($"[Scanner] Melewati folder: {folderName}");
                continue;
            }

            repoInfo.Folders.Add(subDir);
            ScanDirectory(subDir, repoInfo);
        }
    }
}
