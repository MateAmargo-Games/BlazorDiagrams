using BlazorDiagrams.Core.Geometry;
using BlazorDiagrams.Core.Models;

namespace BlazorDiagrams.Core.Layouts;

/// <summary>
/// Circular layout that arranges nodes in a circle
/// </summary>
public class CircularLayout : ILayout
{
    /// <summary>
    /// Radius of the circle
    /// </summary>
    public double Radius { get; set; } = 200;
    
    /// <summary>
    /// Starting angle in degrees (0 = right, 90 = down)
    /// </summary>
    public double StartAngle { get; set; } = 0;
    
    /// <summary>
    /// Whether to automatically calculate radius based on number of nodes
    /// </summary>
    public bool AutoRadius { get; set; } = true;
    
    /// <summary>
    /// Spacing between nodes when using auto radius
    /// </summary>
    public double NodeSpacing { get; set; } = 50;
    
    /// <summary>
    /// Whether to sort nodes before arranging (can use custom comparer)
    /// </summary>
    public bool SortNodes { get; set; } = false;
    
    /// <summary>
    /// Custom comparer for sorting nodes
    /// </summary>
    public Comparison<Node>? NodeComparer { get; set; }
    
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
        
        // Calculate radius
        var radius = Radius;
        if (AutoRadius)
        {
            // Calculate radius so nodes are properly spaced
            var avgNodeSize = nodes.Average(n => Math.Max(n.Size.Width, n.Size.Height));
            var circumference = nodes.Count * (avgNodeSize + NodeSpacing);
            radius = circumference / (2 * Math.PI);
            radius = Math.Max(radius, 100); // Minimum radius
        }
        
        // Arrange nodes in circle
        var angleStep = 360.0 / nodes.Count;
        var startAngleRad = StartAngle * Math.PI / 180.0;
        
        for (int i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];
            var angle = startAngleRad + (i * angleStep * Math.PI / 180.0);
            
            var x = radius * Math.Cos(angle);
            var y = radius * Math.Sin(angle);
            
            // Center the node at the calculated position
            node.Position = new Point(x - node.Size.Width / 2, y - node.Size.Height / 2);
        }
        
        // Center the entire layout
        CenterDiagram(model);
    }
    
    public void ApplyToGroup(Group group)
    {
        // Similar logic for groups
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

