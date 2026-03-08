namespace RepoLens.Models;

public class RepoInfo
{
    public string ProjectPath { get; set; } = string.Empty;

    public string ProjectName { get; set; } = string.Empty;

    public List<string> Files { get; set; } = new();

    public List<string> Folders { get; set; } = new();

    public List<string> Languages { get; set; } = new();

    public List<string> Frameworks { get; set; } = new();

    public List<string> Tools { get; set; } = new();

    public TechStack TechStack { get; set; } = new();

    public CodeComponents Components { get; set; } = new();

    public Dictionary<string, string> FolderDescriptions { get; set; } = new();
}

public class TechStack
{
    public List<string> Languages { get; set; } = new();
    public List<string> Frameworks { get; set; } = new();
    public List<string> Tools { get; set; } = new();
    public List<string> Dependencies { get; set; } = new();
    public string? ArchitectureType { get; set; }
}

public class CodeComponents
{
    public List<string> Routes { get; set; } = new();

    public List<string> ApiEndpoints { get; set; } = new();

    public List<string> UiComponents { get; set; } = new();
}
