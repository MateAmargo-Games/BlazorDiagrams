using BlazorDiagrams.Core.Geometry;
using BlazorDiagrams.Core.Models;

namespace BlazorDiagrams.Core.Rendering;

/// <summary>
/// Context information for rendering operations
/// </summary>
public class RenderContext
{
    /// <summary>
    /// The diagram model being rendered
    /// </summary>
    public DiagramModel Model { get; set; } = null!;

    /// <summary>
    /// The visible viewport area
    /// </summary>
    public Rect Viewport { get; set; }

    /// <summary>
    /// Current zoom level
    /// </summary>
    public double Zoom { get; set; } = 1.0;

    /// <summary>
    /// Current pan offset
    /// </summary>
    public Point PanOffset { get; set; } = Point.Zero;

    /// <summary>
    /// Whether to show the background grid
    /// </summary>
    public bool ShowGrid { get; set; } = true;

    /// <summary>
    /// Size of the grid cells
    /// </summary>
    public double GridSize { get; set; } = 20;

    /// <summary>
    /// Color of the grid lines
    /// </summary>
    public string GridColor { get; set; } = "#e0e0e0";

    /// <summary>
    /// Background color of the diagram
    /// </summary>
    public string BackgroundColor { get; set; } = "#ffffff";
    
    /// <summary>
    /// Converts diagram coordinates to screen coordinates
    /// </summary>
    public Point DiagramToScreen(Point point)
    {
        return new Point(
            (point.X + PanOffset.X) * Zoom,
            (point.Y + PanOffset.Y) * Zoom
        );
    }
    
    /// <summary>
    /// Converts screen coordinates to diagram coordinates
    /// </summary>
    public Point ScreenToDiagram(Point point)
    {
        return new Point(
            point.X / Zoom - PanOffset.X,
            point.Y / Zoom - PanOffset.Y
        );
    }
    
    /// <summary>
    /// Gets the SVG transform string for the current zoom and pan
    /// </summary>
    public string GetTransformString()
    {
        return $"translate({PanOffset.X:F2}, {PanOffset.Y:F2}) scale({Zoom:F4})";
    }
}

