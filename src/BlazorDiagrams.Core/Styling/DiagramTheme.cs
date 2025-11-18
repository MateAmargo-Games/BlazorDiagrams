namespace BlazorDiagrams.Core.Styling;

/// <summary>
/// Defines a complete theme for the diagram
/// </summary>
public class DiagramTheme
{
    /// <summary>
    /// Name of the theme
    /// </summary>
    public string Name { get; set; } = "Default";
    
    /// <summary>
    /// Background color of the diagram canvas
    /// </summary>
    public string BackgroundColor { get; set; } = "#ffffff";
    
    /// <summary>
    /// Grid color
    /// </summary>
    public string GridColor { get; set; } = "#e0e0e0";
    
    /// <summary>
    /// Grid size in pixels
    /// </summary>
    public double GridSize { get; set; } = 20;
    
    /// <summary>
    /// Grid style (dots, lines, crosshairs)
    /// </summary>
    public GridStyle GridStyle { get; set; } = GridStyle.Dots;
    
    /// <summary>
    /// Default node style configuration
    /// </summary>
    public NodeStyleConfig DefaultNodeStyle { get; set; } = new();
    
    /// <summary>
    /// Default link style configuration
    /// </summary>
    public LinkStyleConfig DefaultLinkStyle { get; set; } = new();
    
    /// <summary>
    /// Selected node style configuration
    /// </summary>
    public NodeStyleConfig SelectedNodeStyle { get; set; } = new()
    {
        StrokeColor = "#007bff",
        StrokeWidth = 2,
        ShadowColor = "rgba(0, 123, 255, 0.5)",
        ShadowBlur = 8
    };
    
    /// <summary>
    /// Selected link style configuration
    /// </summary>
    public LinkStyleConfig SelectedLinkStyle { get; set; } = new()
    {
        StrokeColor = "#007bff",
        StrokeWidth = 3
    };
    
    /// <summary>
    /// Hovered node style configuration
    /// </summary>
    public NodeStyleConfig? HoveredNodeStyle { get; set; }
    
    /// <summary>
    /// Creates a light theme
    /// </summary>
    public static DiagramTheme Light => new()
    {
        Name = "Light",
        BackgroundColor = "#ffffff",
        GridColor = "#e0e0e0",
        DefaultNodeStyle = new()
        {
            FillColor = "#ffffff",
            StrokeColor = "#333333",
            StrokeWidth = 1,
            FontFamily = "Arial, sans-serif",
            FontSize = 14,
            TextColor = "#333333"
        },
        DefaultLinkStyle = new()
        {
            StrokeColor = "#666666",
            StrokeWidth = 2
        }
    };
    
    /// <summary>
    /// Creates a dark theme
    /// </summary>
    public static DiagramTheme Dark => new()
    {
        Name = "Dark",
        BackgroundColor = "#1e1e1e",
        GridColor = "#333333",
        DefaultNodeStyle = new()
        {
            FillColor = "#2d2d2d",
            StrokeColor = "#666666",
            StrokeWidth = 1,
            FontFamily = "Arial, sans-serif",
            FontSize = 14,
            TextColor = "#e0e0e0"
        },
        DefaultLinkStyle = new()
        {
            StrokeColor = "#999999",
            StrokeWidth = 2
        },
        SelectedNodeStyle = new()
        {
            StrokeColor = "#4a9eff",
            StrokeWidth = 2,
            ShadowColor = "rgba(74, 158, 255, 0.5)",
            ShadowBlur = 8
        },
        SelectedLinkStyle = new()
        {
            StrokeColor = "#4a9eff",
            StrokeWidth = 3
        }
    };
    
    /// <summary>
    /// Creates a blueprint theme
    /// </summary>
    public static DiagramTheme Blueprint => new()
    {
        Name = "Blueprint",
        BackgroundColor = "#0d47a1",
        GridColor = "#1565c0",
        GridStyle = GridStyle.Lines,
        DefaultNodeStyle = new()
        {
            FillColor = "#0d47a1",
            StrokeColor = "#ffffff",
            StrokeWidth = 2,
            FontFamily = "Courier New, monospace",
            FontSize = 12,
            TextColor = "#ffffff"
        },
        DefaultLinkStyle = new()
        {
            StrokeColor = "#ffffff",
            StrokeWidth = 2,
            StrokeDashArray = "5,5"
        }
    };
}

/// <summary>
/// Grid style for the diagram background
/// </summary>
public enum GridStyle
{
    Dots,
    Lines,
    Crosshairs,
    None
}

