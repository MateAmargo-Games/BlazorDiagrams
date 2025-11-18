using BlazorDiagrams.Core.Geometry;
using BlazorDiagrams.Core.Models;

namespace BlazorDiagrams.Core.Rendering;

/// <summary>
/// Context information for rendering operations
/// </summary>
public class RenderContext
{
    public DiagramModel Model { get; set; } = null!;
    public Rect Viewport { get; set; }
    public double Zoom { get; set; } = 1.0;
    public Point PanOffset { get; set; } = Point.Zero;
    public bool ShowGrid { get; set; } = true;
    public double GridSize { get; set; } = 20;
    public string GridColor { get; set; } = "#e0e0e0";
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

