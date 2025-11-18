using BlazorDiagrams.Core.Geometry;
using BlazorDiagrams.Core.Models;
using System.Text;

namespace BlazorDiagrams.Core.Rendering;

/// <summary>
/// SVG rendering engine for diagram elements
/// </summary>
public class SvgRenderer
{
    private readonly RenderContext _context;
    
    public SvgRenderer(RenderContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Renders a node to SVG
    /// </summary>
    public string RenderNode(Node node)
    {
        if (!node.IsVisible)
            return string.Empty;
        
        var sb = new StringBuilder();
        var cssClass = node.IsSelected ? "diagram-node diagram-node-selected" : "diagram-node";
        
        sb.AppendLine(SvgBuilder.GroupStart(
            id: $"node-{node.Id}",
            cssClass: cssClass
        ));
        
        // Node background
        sb.AppendLine(SvgBuilder.Rect(
            node.Position.X,
            node.Position.Y,
            node.Size.Width,
            node.Size.Height,
            fill: node.Fill,
            stroke: node.Stroke,
            strokeWidth: node.StrokeWidth,
            cornerRadius: node.CornerRadius
        ));
        
        // Node label (if data has a string representation)
        if (node.Data != null)
        {
            var text = GetNodeText(node);
            if (!string.IsNullOrEmpty(text))
            {
                var textX = node.Position.X + node.Size.Width / 2;
                var textY = node.Position.Y + node.Size.Height / 2 + 5; // +5 for vertical centering
                
                sb.AppendLine(SvgBuilder.Text(
                    textX,
                    textY,
                    text,
                    fill: "black",
                    fontSize: 14,
                    fontFamily: "Arial",
                    textAnchor: "middle"
                ));
            }
        }
        
        // Render ports
        foreach (var port in node.Ports)
        {
            sb.AppendLine(RenderPort(port));
        }
        
        sb.AppendLine(SvgBuilder.GroupEnd());
        
        return sb.ToString();
    }
    
    /// <summary>
    /// Renders a port to SVG
    /// </summary>
    public string RenderPort(Port port)
    {
        if (!port.IsVisible)
            return string.Empty;
        
        var position = port.GetAbsolutePosition();
        var portRadius = 5.0;
        
        return SvgBuilder.Circle(
            position.X,
            position.Y,
            portRadius,
            fill: "white",
            stroke: "blue",
            strokeWidth: 2
        );
    }
    
    /// <summary>
    /// Renders a link to SVG
    /// </summary>
    public string RenderLink(Link link)
    {
        if (!link.IsVisible)
            return string.Empty;
        
        var sb = new StringBuilder();
        var cssClass = link.IsSelected ? "diagram-link diagram-link-selected" : "diagram-link";
        
        sb.AppendLine(SvgBuilder.GroupStart(
            id: $"link-{link.Id}",
            cssClass: cssClass
        ));
        
        // Render the path
        var path = link.GetSvgPath();
        if (!string.IsNullOrEmpty(path))
        {
            var markerEnd = link.ShowArrowhead ? $"url(#arrowhead-{link.ArrowheadStyle})" : "";
            
            sb.Append($"<path d=\"{path}\" fill=\"none\" stroke=\"{link.Stroke}\" stroke-width=\"{link.StrokeWidth:F2}\"");
            
            if (!string.IsNullOrEmpty(link.StrokeDashArray))
            {
                sb.Append($" stroke-dasharray=\"{link.StrokeDashArray}\"");
            }
            
            if (!string.IsNullOrEmpty(markerEnd))
            {
                sb.Append($" marker-end=\"{markerEnd}\"");
            }
            
            sb.AppendLine(" />");
        }
        
        // Render label if present
        if (!string.IsNullOrEmpty(link.Label))
        {
            var points = link.GetAllPoints();
            if (points.Count >= 2)
            {
                var midPoint = GeometryHelper.Lerp(points[0], points[^1], 0.5);
                sb.AppendLine(SvgBuilder.Text(
                    midPoint.X,
                    midPoint.Y - 5,
                    link.Label,
                    fill: "black",
                    fontSize: 12
                ));
            }
        }
        
        sb.AppendLine(SvgBuilder.GroupEnd());
        
        return sb.ToString();
    }
    
    /// <summary>
    /// Renders a group to SVG
    /// </summary>
    public string RenderGroup(Group group)
    {
        if (!group.IsVisible)
            return string.Empty;
        
        var sb = new StringBuilder();
        var cssClass = group.IsSelected ? "diagram-group diagram-group-selected" : "diagram-group";
        
        sb.AppendLine(SvgBuilder.GroupStart(
            id: $"group-{group.Id}",
            cssClass: cssClass
        ));
        
        // Group background
        sb.AppendLine(SvgBuilder.Rect(
            group.Position.X,
            group.Position.Y,
            group.Size.Width,
            group.Size.Height,
            fill: group.Fill,
            stroke: group.Stroke,
            strokeWidth: group.StrokeWidth
        ));
        
        // Group title bar
        if (!string.IsNullOrEmpty(group.Title))
        {
            // Title bar background
            sb.AppendLine(SvgBuilder.Rect(
                group.Position.X,
                group.Position.Y,
                group.Size.Width,
                30,
                fill: "#d0d0d0",
                stroke: group.Stroke,
                strokeWidth: group.StrokeWidth
            ));
            
            // Title text
            sb.AppendLine(SvgBuilder.Text(
                group.Position.X + 10,
                group.Position.Y + 20,
                group.Title,
                fill: "black",
                fontSize: 14,
                fontFamily: "Arial",
                textAnchor: "start"
            ));
            
            // Expand/collapse indicator
            if (group.IsCollapsible)
            {
                var indicator = group.IsExpanded ? "−" : "+";
                sb.AppendLine(SvgBuilder.Text(
                    group.Position.X + group.Size.Width - 15,
                    group.Position.Y + 20,
                    indicator,
                    fill: "black",
                    fontSize: 16,
                    fontFamily: "Arial",
                    textAnchor: "middle"
                ));
            }
        }
        
        sb.AppendLine(SvgBuilder.GroupEnd());
        
        return sb.ToString();
    }
    
    /// <summary>
    /// Renders a grid background
    /// </summary>
    public string RenderGrid()
    {
        if (!_context.ShowGrid)
            return string.Empty;
        
        var sb = new StringBuilder();
        var gridSize = _context.GridSize;
        var viewport = _context.Viewport;
        
        // Calculate visible grid range
        var startX = Math.Floor(viewport.X / gridSize) * gridSize;
        var startY = Math.Floor(viewport.Y / gridSize) * gridSize;
        var endX = viewport.Right;
        var endY = viewport.Bottom;
        
        sb.AppendLine(SvgBuilder.GroupStart(id: "grid", cssClass: "diagram-grid"));
        
        // Vertical lines
        for (var x = startX; x <= endX; x += gridSize)
        {
            sb.AppendLine(SvgBuilder.Line(x, startY, x, endY, _context.GridColor, 0.5));
        }
        
        // Horizontal lines
        for (var y = startY; y <= endY; y += gridSize)
        {
            sb.AppendLine(SvgBuilder.Line(startX, y, endX, y, _context.GridColor, 0.5));
        }
        
        sb.AppendLine(SvgBuilder.GroupEnd());
        
        return sb.ToString();
    }
    
    /// <summary>
    /// Renders arrowhead markers
    /// </summary>
    public string RenderMarkers()
    {
        var sb = new StringBuilder();
        sb.AppendLine("<defs>");
        
        // Standard arrowhead
        sb.AppendLine(SvgBuilder.StandardArrowhead("arrowhead-Standard"));
        
        // Open arrowhead
        sb.AppendLine(SvgBuilder.Marker("arrowhead-Open", "M 0 0 L 10 5 L 0 10", 10, 10, 10, 5, "none"));
        
        // Diamond
        sb.AppendLine(SvgBuilder.Marker("arrowhead-Diamond", "M 0 5 L 5 0 L 10 5 L 5 10 Z", 10, 10, 10, 5));
        
        // Circle
        sb.AppendLine(SvgBuilder.Marker("arrowhead-Circle", "M 5 5 m -5 0 a 5 5 0 1 0 10 0 a 5 5 0 1 0 -10 0", 10, 10, 5, 5));
        
        sb.AppendLine("</defs>");
        
        return sb.ToString();
    }
    
    private string GetNodeText(Node node)
    {
        if (node.Data == null)
            return string.Empty;
        
        // Try to get common properties
        var dataType = node.Data.GetType();
        
        // Check for Title property
        var titleProp = dataType.GetProperty("Title");
        if (titleProp != null)
        {
            var value = titleProp.GetValue(node.Data);
            if (value != null)
                return value.ToString() ?? string.Empty;
        }
        
        // Check for Name property
        var nameProp = dataType.GetProperty("Name");
        if (nameProp != null)
        {
            var value = nameProp.GetValue(node.Data);
            if (value != null)
                return value.ToString() ?? string.Empty;
        }
        
        // Check for Label property
        var labelProp = dataType.GetProperty("Label");
        if (labelProp != null)
        {
            var value = labelProp.GetValue(node.Data);
            if (value != null)
                return value.ToString() ?? string.Empty;
        }
        
        // Default to ToString
        return node.Data.ToString() ?? string.Empty;
    }
}

