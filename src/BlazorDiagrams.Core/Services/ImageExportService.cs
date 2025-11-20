using BlazorDiagrams.Core.Models;
using BlazorDiagrams.Core.Rendering;
using Microsoft.JSInterop;

namespace BlazorDiagrams.Core.Services;

/// <summary>
/// Service for exporting diagrams as images
/// </summary>
public class ImageExportService
{
    private readonly IJSRuntime _jsRuntime;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ImageExportService"/> class.
    /// </summary>
    public ImageExportService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }
    
    /// <summary>
    /// Exports the diagram as a PNG image
    /// </summary>
    public async Task<string> ExportToPngAsync(DiagramModel model, RenderContext context, int width = 0, int height = 0)
    {
        // Generate SVG
        var svgRenderer = new SvgRenderer(context);
        var svgContent = RenderCompleteSvg(model, svgRenderer, context);
        
        // Calculate dimensions if not provided
        if (width == 0 || height == 0)
        {
            var bounds = model.GetBoundingBox();
            width = (int)(bounds.Width + 40); // Add padding
            height = (int)(bounds.Height + 40);
        }
        
        // Convert SVG to PNG using JavaScript
        var dataUrl = await _jsRuntime.InvokeAsync<string>(
            "BlazorDiagrams.exportToPng",
            svgContent,
            width,
            height
        );
        
        return dataUrl;
    }
    
    /// <summary>
    /// Renders a complete SVG document
    /// </summary>
    private string RenderCompleteSvg(DiagramModel model, SvgRenderer renderer, RenderContext context)
    {
        var bounds = model.GetBoundingBox();
        var svg = new System.Text.StringBuilder();
        
        svg.AppendLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{bounds.Width + 40}\" height=\"{bounds.Height + 40}\" viewBox=\"{bounds.X - 20} {bounds.Y - 20} {bounds.Width + 40} {bounds.Height + 40}\">");
        
        // Add markers
        svg.AppendLine(renderer.RenderMarkers());
        
        // Add background
        svg.AppendLine($"<rect x=\"{bounds.X - 20}\" y=\"{bounds.Y - 20}\" width=\"{bounds.Width + 40}\" height=\"{bounds.Height + 40}\" fill=\"{context.BackgroundColor}\" />");
        
        // Add grid if enabled
        svg.AppendLine(renderer.RenderGrid());
        
        // Render all groups
        foreach (var group in model.Groups)
        {
            svg.AppendLine(renderer.RenderGroup(group));
        }
        
        // Render all links
        foreach (var link in model.Links.Where(l => l.IsVisible))
        {
            svg.AppendLine(renderer.RenderLink(link));
        }
        
        // Render all nodes
        foreach (var node in model.Nodes.Where(n => n.IsVisible))
        {
            svg.AppendLine(renderer.RenderNode(node));
        }
        
        svg.AppendLine("</svg>");
        
        return svg.ToString();
    }
    
    /// <summary>
    /// Downloads the diagram as a PNG file
    /// </summary>
    public async Task DownloadAsPngAsync(DiagramModel model, RenderContext context, string fileName = "diagram.png")
    {
        var dataUrl = await ExportToPngAsync(model, context);
        
        await _jsRuntime.InvokeVoidAsync(
            "BlazorDiagrams.downloadFile",
            dataUrl,
            fileName
        );
    }
    
    /// <summary>
    /// Exports a specific area of the diagram
    /// </summary>
    public async Task<string> ExportAreaToPngAsync(DiagramModel model, RenderContext context, double x, double y, double width, double height)
    {
        // Create a modified context with the specific viewport
        var areaContext = new RenderContext
        {
            Model = model,
            Viewport = new Geometry.Rect(x, y, width, height),
            Zoom = context.Zoom,
            PanOffset = context.PanOffset,
            ShowGrid = context.ShowGrid,
            GridSize = context.GridSize,
            BackgroundColor = context.BackgroundColor
        };
        
        return await ExportToPngAsync(model, areaContext, (int)width, (int)height);
    }
}

