using RepoLens.Services;

namespace RepoLens;

class Program
{
    static int Main(string[] args)
    {
        PrintBanner();

        if (args.Length == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ⚠️  Penggunaan: dotnet run -- <path-to-repository> [options]");
            Console.WriteLine();
            Console.WriteLine("  Options:");
            Console.WriteLine("    --verbose        Tampilkan log detail proses scanning");
            Console.WriteLine("    --output <file>  Simpan laporan ke file teks");
            Console.WriteLine("    --no-components  Lewati deteksi routes/endpoints/components");
            Console.WriteLine();
            Console.WriteLine("  Contoh:");
            Console.WriteLine("    dotnet run -- ./my-project");
            Console.WriteLine("    dotnet run -- /home/user/projects/my-app --output report.txt");
            Console.ResetColor();
            return 1;
        }

        var projectPath = args[0];
        var verbose = args.Contains("--verbose");
        var noComponents = args.Contains("--no-components");
        string? outputFile = null;

        var outputIdx = Array.IndexOf(args, "--output");
        if (outputIdx >= 0 && outputIdx + 1 < args.Length)
            outputFile = args[outputIdx + 1];

        if (!Directory.Exists(projectPath))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  Error: Folder tidak ditemukan: {projectPath}");
            Console.ResetColor();
            return 1;
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"  Menganalisis repository: {Path.GetFullPath(projectPath)}");
        Console.WriteLine();
        Console.ResetColor();

        try
        {
            var scanner = new RepoScannerService();
            var repoInfo = scanner.Scan(projectPath);

            var techStack = new TechStackService();
            techStack.Detect(repoInfo);

            if (!noComponents)
            {
                var componentDetector = new ComponentDetectorService();
                componentDetector.Detect(repoInfo);
            }

            var explanation = new ExplanationService();
            var report = explanation.GenerateReport(repoInfo);

            Console.WriteLine(report);

            if (outputFile != null)
            {
                File.WriteAllText(outputFile, report);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"  Laporan disimpan ke: {outputFile}");
                Console.ResetColor();
            }

            return 0;
        }
        catch (DirectoryNotFoundException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  ❌ {ex.Message}");
            Console.ResetColor();
            return 1;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  Error tak terduga: {ex.Message}");
            if (verbose) Console.WriteLine(ex.StackTrace);
            Console.ResetColor();
            return 1;
        }
    }

    private static void PrintBanner()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine();
        Console.WriteLine("  ██████╗ ███████╗██████╗  ██████╗ ██╗     ███████╗███╗   ██╗███████╗");
        Console.WriteLine("  ██╔══██╗██╔════╝██╔══██╗██╔═══██╗██║     ██╔════╝████╗  ██║██╔════╝");
        Console.WriteLine("  ██████╔╝█████╗  ██████╔╝██║   ██║██║     █████╗  ██╔██╗ ██║███████╗");
        Console.WriteLine("  ██╔══██╗██╔══╝  ██╔═══╝ ██║   ██║██║     ██╔══╝  ██║╚██╗██║╚════██║");
        Console.WriteLine("  ██║  ██║███████╗██║     ╚██████╔╝███████╗███████╗██║ ╚████║███████║");
        Console.WriteLine("  ╚═╝  ╚═╝╚══════╝╚═╝      ╚═════╝ ╚══════╝╚══════╝╚═╝  ╚═══╝╚══════╝");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("  Repository Code Analysis Tool  |  v1.0.0 BETA");
        Console.WriteLine("  Membantu developer memahami codebase dengan cepat");
        Console.WriteLine("  StorSync Technology  |  Farell Kurniawan ");
        Console.ResetColor();
        Console.WriteLine();
    }
}
