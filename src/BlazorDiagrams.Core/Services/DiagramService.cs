using BlazorDiagrams.Core.Models;
using System.Text.Json;

namespace BlazorDiagrams.Core.Services;

/// <summary>
/// Main service for diagram operations
/// </summary>
public class DiagramService
{
    public UndoManager UndoManager { get; } = new();
    
    /// <summary>
    /// Serializes the diagram model to JSON
    /// </summary>
    public string SerializeToJson(DiagramModel model, JsonSerializerOptions? options = null)
    {
        options ??= new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        var data = new
        {
            nodes = model.Nodes.Select(n => new
            {
                id = n.Id,
                position = new { x = n.Position.X, y = n.Position.Y },
                size = new { width = n.Size.Width, height = n.Size.Height },
                fill = n.Fill,
                stroke = n.Stroke,
                strokeWidth = n.StrokeWidth,
                data = n.Data,
                category = n.Category,
                isCollapsed = n.IsCollapsed
            }),
            links = model.Links.Select(l => new
            {
                id = l.Id,
                from = l.FromNode?.Id,
                to = l.ToNode?.Id,
                fromPort = l.FromPort?.Id,
                toPort = l.ToPort?.Id,
                routing = l.Routing.ToString(),
                stroke = l.Stroke,
                strokeWidth = l.StrokeWidth,
                label = l.Label
            }),
            groups = model.Groups.Select(g => new
            {
                id = g.Id,
                title = g.Title,
                position = new { x = g.Position.X, y = g.Position.Y },
                size = new { width = g.Size.Width, height = g.Size.Height },
                isExpanded = g.IsExpanded
            })
        };
        
        return JsonSerializer.Serialize(data, options);
    }
    
    /// <summary>
    /// Exports the diagram to SVG format
    /// </summary>
    public string ExportToSvg(DiagramModel model)
    {
        var bounds = model.GetBoundingBox();
        var padding = 20;
        
        var viewBox = $"{bounds.X - padding} {bounds.Y - padding} {bounds.Width + 2 * padding} {bounds.Height + 2 * padding}";
        
        var svg = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<svg xmlns=""http://www.w3.org/2000/svg"" 
     xmlns:xlink=""http://www.w3.org/1999/xlink""
     viewBox=""{viewBox}""
     width=""{bounds.Width + 2 * padding}""
     height=""{bounds.Height + 2 * padding}"">
  <defs>
    <marker id=""arrowhead"" markerWidth=""10"" markerHeight=""10"" refX=""9"" refY=""3"" orient=""auto"">
      <polygon points=""0 0, 10 3, 0 6"" fill=""black"" />
    </marker>
  </defs>
  <g id=""diagram"">
";
        
        // Render links
        foreach (var link in model.Links)
        {
            if (!link.IsVisible) continue;
            
            var path = link.GetSvgPath();
            if (!string.IsNullOrEmpty(path))
            {
                var markerEnd = link.ShowArrowhead ? " marker-end=\"url(#arrowhead)\"" : "";
                svg += $"    <path d=\"{path}\" fill=\"none\" stroke=\"{link.Stroke}\" stroke-width=\"{link.StrokeWidth}\"{markerEnd} />\n";
            }
        }
        
        // Render nodes
        foreach (var node in model.Nodes)
        {
            if (!node.IsVisible) continue;
            
            svg += $"    <rect x=\"{node.Position.X}\" y=\"{node.Position.Y}\" width=\"{node.Size.Width}\" height=\"{node.Size.Height}\" fill=\"{node.Fill}\" stroke=\"{node.Stroke}\" stroke-width=\"{node.StrokeWidth}\" />\n";
            
            // Add text if data is available
            if (node.Data != null)
            {
                var text = node.Data.ToString();
                var textX = node.Position.X + node.Size.Width / 2;
                var textY = node.Position.Y + node.Size.Height / 2;
                svg += $"    <text x=\"{textX}\" y=\"{textY}\" text-anchor=\"middle\" dominant-baseline=\"middle\">{System.Net.WebUtility.HtmlEncode(text)}</text>\n";
            }
        }
        
        svg += @"  </g>
</svg>";
        
        return svg;
    }
}

