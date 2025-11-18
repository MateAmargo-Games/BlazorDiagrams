using BlazorDiagrams.Core.Geometry;
using System.Text;

namespace BlazorDiagrams.Core.Rendering;

/// <summary>
/// Helper class for building SVG elements
/// </summary>
public static class SvgBuilder
{
    /// <summary>
    /// Creates an SVG rect element
    /// </summary>
    public static string Rect(double x, double y, double width, double height, string fill = "white", string stroke = "black", double strokeWidth = 1, double cornerRadius = 0)
    {
        var sb = new StringBuilder();
        sb.Append($"<rect x=\"{x:F2}\" y=\"{y:F2}\" width=\"{width:F2}\" height=\"{height:F2}\"");
        sb.Append($" fill=\"{fill}\" stroke=\"{stroke}\" stroke-width=\"{strokeWidth:F2}\"");
        
        if (cornerRadius > 0)
        {
            sb.Append($" rx=\"{cornerRadius:F2}\" ry=\"{cornerRadius:F2}\"");
        }
        
        sb.Append(" />");
        return sb.ToString();
    }
    
    /// <summary>
    /// Creates an SVG circle element
    /// </summary>
    public static string Circle(double cx, double cy, double r, string fill = "white", string stroke = "black", double strokeWidth = 1)
    {
        return $"<circle cx=\"{cx:F2}\" cy=\"{cy:F2}\" r=\"{r:F2}\" fill=\"{fill}\" stroke=\"{stroke}\" stroke-width=\"{strokeWidth:F2}\" />";
    }
    
    /// <summary>
    /// Creates an SVG ellipse element
    /// </summary>
    public static string Ellipse(double cx, double cy, double rx, double ry, string fill = "white", string stroke = "black", double strokeWidth = 1)
    {
        return $"<ellipse cx=\"{cx:F2}\" cy=\"{cy:F2}\" rx=\"{rx:F2}\" ry=\"{ry:F2}\" fill=\"{fill}\" stroke=\"{stroke}\" stroke-width=\"{strokeWidth:F2}\" />";
    }
    
    /// <summary>
    /// Creates an SVG line element
    /// </summary>
    public static string Line(double x1, double y1, double x2, double y2, string stroke = "black", double strokeWidth = 1, string strokeDashArray = "")
    {
        var sb = new StringBuilder();
        sb.Append($"<line x1=\"{x1:F2}\" y1=\"{y1:F2}\" x2=\"{x2:F2}\" y2=\"{y2:F2}\"");
        sb.Append($" stroke=\"{stroke}\" stroke-width=\"{strokeWidth:F2}\"");
        
        if (!string.IsNullOrEmpty(strokeDashArray))
        {
            sb.Append($" stroke-dasharray=\"{strokeDashArray}\"");
        }
        
        sb.Append(" />");
        return sb.ToString();
    }
    
    /// <summary>
    /// Creates an SVG path element
    /// </summary>
    public static string Path(string d, string fill = "none", string stroke = "black", double strokeWidth = 1, string strokeDashArray = "")
    {
        var sb = new StringBuilder();
        sb.Append($"<path d=\"{d}\"");
        sb.Append($" fill=\"{fill}\" stroke=\"{stroke}\" stroke-width=\"{strokeWidth:F2}\"");
        
        if (!string.IsNullOrEmpty(strokeDashArray))
        {
            sb.Append($" stroke-dasharray=\"{strokeDashArray}\"");
        }
        
        sb.Append(" />");
        return sb.ToString();
    }
    
    /// <summary>
    /// Creates an SVG text element
    /// </summary>
    public static string Text(double x, double y, string text, string fill = "black", int fontSize = 14, string fontFamily = "Arial", string textAnchor = "middle")
    {
        return $"<text x=\"{x:F2}\" y=\"{y:F2}\" fill=\"{fill}\" font-size=\"{fontSize}\" font-family=\"{fontFamily}\" text-anchor=\"{textAnchor}\">{System.Net.WebUtility.HtmlEncode(text)}</text>";
    }
    
    /// <summary>
    /// Creates an SVG group element
    /// </summary>
    public static string GroupStart(string? id = null, string? transform = null, string? cssClass = null)
    {
        var sb = new StringBuilder("<g");
        
        if (!string.IsNullOrEmpty(id))
        {
            sb.Append($" id=\"{id}\"");
        }
        
        if (!string.IsNullOrEmpty(transform))
        {
            sb.Append($" transform=\"{transform}\"");
        }
        
        if (!string.IsNullOrEmpty(cssClass))
        {
            sb.Append($" class=\"{cssClass}\"");
        }
        
        sb.Append(">");
        return sb.ToString();
    }
    
    /// <summary>
    /// Closes an SVG group element
    /// </summary>
    public static string GroupEnd() => "</g>";
    
    /// <summary>
    /// Creates an SVG marker (for arrowheads)
    /// </summary>
    public static string Marker(string id, string path, double width = 10, double height = 10, double refX = 5, double refY = 5, string fill = "black")
    {
        return $"<marker id=\"{id}\" markerWidth=\"{width:F2}\" markerHeight=\"{height:F2}\" refX=\"{refX:F2}\" refY=\"{refY:F2}\" orient=\"auto\" markerUnits=\"strokeWidth\">" +
               $"<path d=\"{path}\" fill=\"{fill}\" />" +
               "</marker>";
    }
    
    /// <summary>
    /// Creates a standard arrowhead marker
    /// </summary>
    public static string StandardArrowhead(string id, string fill = "black")
    {
        return Marker(id, "M 0 0 L 10 5 L 0 10 Z", 10, 10, 10, 5, fill);
    }
    
    /// <summary>
    /// Creates a polyline element
    /// </summary>
    public static string Polyline(IEnumerable<Point> points, string fill = "none", string stroke = "black", double strokeWidth = 1, string strokeDashArray = "")
    {
        var sb = new StringBuilder();
        sb.Append("<polyline points=\"");
        
        foreach (var point in points)
        {
            sb.Append($"{point.X:F2},{point.Y:F2} ");
        }
        
        sb.Append("\"");
        sb.Append($" fill=\"{fill}\" stroke=\"{stroke}\" stroke-width=\"{strokeWidth:F2}\"");
        
        if (!string.IsNullOrEmpty(strokeDashArray))
        {
            sb.Append($" stroke-dasharray=\"{strokeDashArray}\"");
        }
        
        sb.Append(" />");
        return sb.ToString();
    }
    
    /// <summary>
    /// Creates a polygon element
    /// </summary>
    public static string Polygon(IEnumerable<Point> points, string fill = "white", string stroke = "black", double strokeWidth = 1)
    {
        var sb = new StringBuilder();
        sb.Append("<polygon points=\"");
        
        foreach (var point in points)
        {
            sb.Append($"{point.X:F2},{point.Y:F2} ");
        }
        
        sb.Append("\"");
        sb.Append($" fill=\"{fill}\" stroke=\"{stroke}\" stroke-width=\"{strokeWidth:F2}\"");
        sb.Append(" />");
        return sb.ToString();
    }
}

