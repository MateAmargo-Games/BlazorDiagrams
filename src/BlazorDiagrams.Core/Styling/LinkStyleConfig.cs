namespace BlazorDiagrams.Core.Styling;

/// <summary>
/// Configuration for link visual styling
/// </summary>
public class LinkStyleConfig
{
    /// <summary>
    /// Stroke color of the link
    /// </summary>
    public string StrokeColor { get; set; } = "#000000";
    
    /// <summary>
    /// Stroke width in pixels
    /// </summary>
    public double StrokeWidth { get; set; } = 2;
    
    /// <summary>
    /// Stroke dash array (for dashed lines)
    /// </summary>
    public string? StrokeDashArray { get; set; }
    
    /// <summary>
    /// Opacity (0.0 to 1.0)
    /// </summary>
    public double Opacity { get; set; } = 1.0;
    
    /// <summary>
    /// Whether to show arrowhead
    /// </summary>
    public bool ShowArrowhead { get; set; } = true;
    
    /// <summary>
    /// Arrowhead size multiplier
    /// </summary>
    public double ArrowheadSize { get; set; } = 1.0;
    
    /// <summary>
    /// Arrowhead style
    /// </summary>
    public ArrowheadStyle ArrowheadStyle { get; set; } = ArrowheadStyle.Filled;
    
    /// <summary>
    /// Line cap style (butt, round, square)
    /// </summary>
    public string LineCap { get; set; } = "butt";
    
    /// <summary>
    /// Line join style (miter, round, bevel)
    /// </summary>
    public string LineJoin { get; set; } = "miter";
    
    /// <summary>
    /// Shadow color
    /// </summary>
    public string? ShadowColor { get; set; }
    
    /// <summary>
    /// Shadow blur radius
    /// </summary>
    public double ShadowBlur { get; set; } = 0;
    
    /// <summary>
    /// Label background color
    /// </summary>
    public string? LabelBackgroundColor { get; set; }
    
    /// <summary>
    /// Label text color
    /// </summary>
    public string LabelTextColor { get; set; } = "#000000";
    
    /// <summary>
    /// Label font family
    /// </summary>
    public string LabelFontFamily { get; set; } = "Arial, sans-serif";
    
    /// <summary>
    /// Label font size
    /// </summary>
    public double LabelFontSize { get; set; } = 12;
    
    /// <summary>
    /// Label padding
    /// </summary>
    public double LabelPadding { get; set; } = 4;
    
    /// <summary>
    /// Creates a copy of this style configuration
    /// </summary>
    public LinkStyleConfig Clone()
    {
        return (LinkStyleConfig)MemberwiseClone();
    }
}

/// <summary>
/// Arrowhead style for links
/// </summary>
public enum ArrowheadStyle
{
    Filled,
    Open,
    Diamond,
    Circle,
    Square,
    None
}

