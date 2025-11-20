using BlazorDiagrams.Core.Geometry;
using BlazorDiagrams.Core.Models;

namespace BlazorDiagrams.Core.Layouts;

/// <summary>
/// Grid layout that arranges nodes in a grid pattern
/// </summary>
public class GridLayout : ILayout
{
    /// <summary>
    /// Number of columns (0 = auto-calculate)
    /// </summary>
    public int Columns { get; set; } = 0;
    
    /// <summary>
    /// Number of rows (0 = auto-calculate)
    /// </summary>
    public int Rows { get; set; } = 0;
    
    /// <summary>
    /// Horizontal spacing between nodes
    /// </summary>
    public double HorizontalSpacing { get; set; } = 50;
    
    /// <summary>
    /// Vertical spacing between nodes
    /// </summary>
    public double VerticalSpacing { get; set; } = 50;
    
    /// <summary>
    /// Alignment of nodes within grid cells
    /// </summary>
    public GridAlignment Alignment { get; set; } = GridAlignment.Center;
    
    /// <summary>
    /// Whether to sort nodes before arranging
    /// </summary>
    public bool SortNodes { get; set; } = false;
    
    /// <summary>
    /// Custom comparer for sorting nodes
    /// </summary>
    public Comparison<Node>? NodeComparer { get; set; }
    
    /// <inheritdoc />
    public void Apply(DiagramModel model)
    {
        if (model.Nodes.Count == 0)
            return;
        
        var nodes = model.Nodes.ToList();
        
        // Sort if requested
        if (SortNodes && NodeComparer != null)
        {
            nodes.Sort(NodeComparer);
        }
        
        // Calculate grid dimensions
        int cols, rows;
        CalculateGridDimensions(nodes.Count, out cols, out rows);
        
        // Calculate cell sizes (use max node size in each dimension)
        var maxWidth = nodes.Max(n => n.Size.Width);
        var maxHeight = nodes.Max(n => n.Size.Height);
        var cellWidth = maxWidth + HorizontalSpacing;
        var cellHeight = maxHeight + VerticalSpacing;
        
        // Arrange nodes
        for (int i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];
            var row = i / cols;
            var col = i % cols;
            
            var cellX = col * cellWidth;
            var cellY = row * cellHeight;
            
            // Apply alignment within cell
            var nodeX = Alignment switch
            {
                GridAlignment.TopLeft or GridAlignment.LeftCenter or GridAlignment.BottomLeft => cellX,
                GridAlignment.TopCenter or GridAlignment.Center or GridAlignment.BottomCenter => cellX + (cellWidth - node.Size.Width) / 2,
                GridAlignment.TopRight or GridAlignment.RightCenter or GridAlignment.BottomRight => cellX + cellWidth - node.Size.Width,
                _ => cellX
            };
            
            var nodeY = Alignment switch
            {
                GridAlignment.TopLeft or GridAlignment.TopCenter or GridAlignment.TopRight => cellY,
                GridAlignment.LeftCenter or GridAlignment.Center or GridAlignment.RightCenter => cellY + (cellHeight - node.Size.Height) / 2,
                GridAlignment.BottomLeft or GridAlignment.BottomCenter or GridAlignment.BottomRight => cellY + cellHeight - node.Size.Height,
                _ => cellY
            };
            
            node.Position = new Point(nodeX, nodeY);
        }
        
        // Center the entire grid
        CenterDiagram(model);
    }
    
    /// <inheritdoc />
    public void ApplyToGroup(Group group)
    {
        // Similar logic for groups
    }
    
    private void CalculateGridDimensions(int nodeCount, out int cols, out int rows)
    {
        if (Columns > 0 && Rows > 0)
        {
            cols = Columns;
            rows = Rows;
        }
        else if (Columns > 0)
        {
            cols = Columns;
            rows = (int)Math.Ceiling((double)nodeCount / cols);
        }
        else if (Rows > 0)
        {
            rows = Rows;
            cols = (int)Math.Ceiling((double)nodeCount / rows);
        }
        else
        {
            // Auto-calculate to make roughly square
            cols = (int)Math.Ceiling(Math.Sqrt(nodeCount));
            rows = (int)Math.Ceiling((double)nodeCount / cols);
        }
    }
    
    private void CenterDiagram(DiagramModel model)
    {
        if (model.Nodes.Count == 0)
            return;
        
        var bounds = model.GetBoundingBox();
        var center = bounds.Center;
        
        foreach (var node in model.Nodes)
        {
            node.Position = new Point(
                node.Position.X - center.X,
                node.Position.Y - center.Y
            );
        }
    }
}

/// <summary>
/// Grid alignment options
/// </summary>
public enum GridAlignment
{
    /// <summary>
    /// Align top-left
    /// </summary>
    TopLeft,
    
    /// <summary>
    /// Align top-center
    /// </summary>
    TopCenter,
    
    /// <summary>
    /// Align top-right
    /// </summary>
    TopRight,
    
    /// <summary>
    /// Align left-center
    /// </summary>
    LeftCenter,
    
    /// <summary>
    /// Align center
    /// </summary>
    Center,
    
    /// <summary>
    /// Align right-center
    /// </summary>
    RightCenter,
    
    /// <summary>
    /// Align bottom-left
    /// </summary>
    BottomLeft,
    
    /// <summary>
    /// Align bottom-center
    /// </summary>
    BottomCenter,
    
    /// <summary>
    /// Align bottom-right
    /// </summary>
    BottomRight
}

