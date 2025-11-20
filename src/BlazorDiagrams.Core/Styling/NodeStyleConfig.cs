namespace BlazorDiagrams.Core.Styling;

/// <summary>
/// Configuration for node visual styling
/// </summary>
public class NodeStyleConfig
{
    /// <summary>
    /// Shape of the node
    /// </summary>
    public NodeShape Shape { get; set; } = NodeShape.Rectangle;
    
    /// <summary>
    /// Fill color of the node
    /// </summary>
    public string FillColor { get; set; } = "#ffffff";
    
    /// <summary>
    /// Stroke (border) color of the node
    /// </summary>
    public string StrokeColor { get; set; } = "#000000";
    
    /// <summary>
    /// Stroke width in pixels
    /// </summary>
    public double StrokeWidth { get; set; } = 1;
    
    /// <summary>
    /// Stroke dash array (for dashed borders)
    /// </summary>
    public string? StrokeDashArray { get; set; }
    
    /// <summary>
    /// Corner radius for rounded rectangles
    /// </summary>
    public double CornerRadius { get; set; } = 0;
    
    /// <summary>
    /// Fill pattern (solid, gradient, etc.)
    /// </summary>
    public FillPattern FillPattern { get; set; } = FillPattern.Solid;
    
    /// <summary>
    /// Gradient start color (if FillPattern is Gradient)
    /// </summary>
    public string? GradientStartColor { get; set; }
    
    /// <summary>
    /// Gradient end color (if FillPattern is Gradient)
    /// </summary>
    public string? GradientEndColor { get; set; }
    
    /// <summary>
    /// Gradient direction in degrees (0 = horizontal, 90 = vertical)
    /// </summary>
    public double GradientAngle { get; set; } = 90;
    
    /// <summary>
    /// Shadow color
    /// </summary>
    public string? ShadowColor { get; set; }
    
    /// <summary>
    /// Shadow blur radius
    /// </summary>
    public double ShadowBlur { get; set; } = 0;
    
    /// <summary>
    /// Shadow offset X
    /// </summary>
    public double ShadowOffsetX { get; set; } = 0;
    
    /// <summary>
    /// Shadow offset Y
    /// </summary>
    public double ShadowOffsetY { get; set; } = 0;
    
    /// <summary>
    /// Opacity (0.0 to 1.0)
    /// </summary>
    public double Opacity { get; set; } = 1.0;
    
    /// <summary>
    /// Font family for node text
    /// </summary>
    public string FontFamily { get; set; } = "Arial, sans-serif";
    
    /// <summary>
    /// Font size in pixels
    /// </summary>
    public double FontSize { get; set; } = 14;
    
    /// <summary>
    /// Font weight (normal, bold, etc.)
    /// </summary>
    public string FontWeight { get; set; } = "normal";
    
    /// <summary>
    /// Text color
    /// </summary>
    public string TextColor { get; set; } = "#000000";
    
    /// <summary>
    /// Text alignment (left, center, right)
    /// </summary>
    public string TextAlign { get; set; } = "center";
    
    /// <summary>
    /// Padding inside the node
    /// </summary>
    public double Padding { get; set; } = 10;
    
    /// <summary>
    /// Creates a copy of this style configuration
    /// </summary>
    public NodeStyleConfig Clone()
    {
        return (NodeStyleConfig)MemberwiseClone();
    }
}

/// <summary>
/// Shape of the node
/// </summary>
public enum NodeShape
{
    /// <summary>
    /// Rectangle shape
    /// </summary>
    Rectangle,
    
    /// <summary>
    /// Rounded rectangle shape
    /// </summary>
    RoundedRectangle,
    
    /// <summary>
    /// Circle shape
    /// </summary>
    Circle,
    
    /// <summary>
    /// Ellipse shape
    /// </summary>
    Ellipse,
    
    /// <summary>
    /// Diamond shape
    /// </summary>
    Diamond,
    
    /// <summary>
    /// Triangle shape
    /// </summary>
    Triangle,
    
    /// <summary>
    /// Hexagon shape
    /// </summary>
    Hexagon,
    
    /// <summary>
    /// Parallelogram shape
    /// </summary>
    Parallelogram,
    
    /// <summary>
    /// Custom shape
    /// </summary>
    Custom
}

/// <summary>
/// Fill pattern for nodes
/// </summary>
public enum FillPattern
{
    /// <summary>
    /// Solid color fill
    /// </summary>
    Solid,
    
    /// <summary>
    /// Gradient fill
    /// </summary>
    Gradient,
    
    /// <summary>
    /// Striped pattern
    /// </summary>
    Striped,
    
    /// <summary>
    /// Dotted pattern
    /// </summary>
    Dotted,
    
    /// <summary>
    /// No fill
    /// </summary>
    None
}

