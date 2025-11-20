using BlazorDiagrams.Core.Geometry;
using BlazorDiagrams.Core.Models;

namespace BlazorDiagrams.Core.Layouts;

/// <summary>
/// Tree layout for hierarchical diagrams like org charts
/// </summary>
public class TreeLayout : ILayout
{
    /// <summary>
    /// Direction of the tree growth
    /// </summary>
    public double Angle { get; set; } = 90; // 0=right, 90=down, 180=left, 270=up
    
    /// <summary>
    /// Spacing between layers (levels)
    /// </summary>
    public double LayerSpacing { get; set; } = 50;
    
    /// <summary>
    /// Spacing between nodes in the same layer
    /// </summary>
    public double NodeSpacing { get; set; } = 30;
    
    /// <summary>
    /// Arrangement style for the tree
    /// </summary>
    public TreeArrangement Arrangement { get; set; } = TreeArrangement.Vertical;
    
    /// <summary>
    /// Alignment of nodes within their layer
    /// </summary>
    public TreeAlignment Alignment { get; set; } = TreeAlignment.CenterChildren;
    
    /// <summary>
    /// Whether to sort children nodes
    /// </summary>
    public bool SortChildren { get; set; } = false;
    
    /// <summary>
    /// Comparer for sorting children
    /// </summary>
    public Comparison<Node>? ChildComparer { get; set; }
    
    /// <inheritdoc />
    public void Apply(DiagramModel model)
    {
        if (model.Nodes.Count == 0)
            return;
        
        // Build tree structure
        var roots = FindRootNodes(model);
        if (roots.Count == 0)
            return;
        
        // Layout each tree
        var currentOffset = 0.0;
        foreach (var root in roots)
        {
            var treeInfo = BuildTreeInfo(root, model);
            LayoutTree(treeInfo, currentOffset);
            currentOffset += treeInfo.TotalWidth + NodeSpacing * 3;
        }
    }
    
    /// <inheritdoc />
    public void ApplyToGroup(Group group)
    {
        // Similar logic for group layouts
        // For now, delegate to main Apply
    }
    
    private List<Node> FindRootNodes(DiagramModel model)
    {
        // A root node is one with no incoming links
        var nodesWithIncoming = new HashSet<Node>();
        
        foreach (var link in model.Links)
        {
            if (link.ToNode != null)
            {
                nodesWithIncoming.Add(link.ToNode);
            }
        }
        
        var roots = model.Nodes.Where(n => !nodesWithIncoming.Contains(n)).ToList();
        
        // If no roots found (circular), use first node
        if (roots.Count == 0 && model.Nodes.Count > 0)
        {
            roots.Add(model.Nodes[0]);
        }
        
        return roots;
    }
    
    private TreeNodeInfo BuildTreeInfo(Node node, DiagramModel model)
    {
        var info = new TreeNodeInfo { Node = node };
        
        // Find children
        var outgoingLinks = model.Links.Where(l => l.FromNode == node).ToList();
        var children = outgoingLinks
            .Select(l => l.ToNode)
            .Where(n => n != null)
            .Cast<Node>()
            .ToList();
        
        if (SortChildren && children.Count > 0 && ChildComparer != null)
        {
            children.Sort(ChildComparer);
        }
        
        // Build tree recursively
        foreach (var child in children)
        {
            var childInfo = BuildTreeInfo(child, model);
            childInfo.Parent = info;
            info.Children.Add(childInfo);
        }
        
        // Calculate dimensions
        CalculateTreeDimensions(info);
        
        return info;
    }
    
    private void CalculateTreeDimensions(TreeNodeInfo info)
    {
        var node = info.Node;
        
        if (Arrangement == TreeArrangement.Vertical)
        {
            info.Width = node.Size.Width;
            info.Height = node.Size.Height;
        }
        else
        {
            info.Width = node.Size.Height;
            info.Height = node.Size.Width;
        }
        
        if (info.Children.Count == 0)
        {
            info.TotalWidth = info.Width;
            info.TotalHeight = info.Height;
        }
        else
        {
            // Sum children widths
            var childrenWidth = info.Children.Sum(c => c.TotalWidth) + 
                               (info.Children.Count - 1) * NodeSpacing;
            
            info.TotalWidth = Math.Max(info.Width, childrenWidth);
            
            var maxChildHeight = info.Children.Max(c => c.TotalHeight);
            info.TotalHeight = info.Height + LayerSpacing + maxChildHeight;
        }
    }
    
    private void LayoutTree(TreeNodeInfo info, double offsetX)
    {
        LayoutNode(info, offsetX, 0, info.TotalWidth);
    }
    
    private void LayoutNode(TreeNodeInfo info, double x, double y, double allocatedWidth)
    {
        var node = info.Node;
        
        // Position this node
        double nodeX, nodeY;
        
        if (Alignment == TreeAlignment.CenterChildren && info.Children.Count > 0)
        {
            // Center over children
            var childrenWidth = info.Children.Sum(c => c.TotalWidth) + 
                               (info.Children.Count - 1) * NodeSpacing;
            nodeX = x + (allocatedWidth - childrenWidth) / 2 + childrenWidth / 2 - info.Width / 2;
        }
        else if (Alignment == TreeAlignment.Start)
        {
            nodeX = x;
        }
        else if (Alignment == TreeAlignment.End)
        {
            nodeX = x + allocatedWidth - info.Width;
        }
        else // Center or CenterChildren with no children
        {
            nodeX = x + (allocatedWidth - info.Width) / 2;
        }
        
        nodeY = y;
        
        // Apply angle transformation
        var position = TransformPosition(nodeX, nodeY, info.Width, info.Height);
        node.Position = position;
        
        // Layout children
        if (info.Children.Count > 0)
        {
            var childY = y + info.Height + LayerSpacing;
            var childX = x;
            
            if (Alignment == TreeAlignment.CenterChildren)
            {
                var childrenWidth = info.Children.Sum(c => c.TotalWidth) + 
                                   (info.Children.Count - 1) * NodeSpacing;
                childX = x + (allocatedWidth - childrenWidth) / 2;
            }
            
            foreach (var child in info.Children)
            {
                LayoutNode(child, childX, childY, child.TotalWidth);
                childX += child.TotalWidth + NodeSpacing;
            }
        }
    }
    
    private Point TransformPosition(double x, double y, double width, double height)
    {
        // Transform based on angle
        var angleRad = Angle * Math.PI / 180.0;
        
        if (Arrangement == TreeArrangement.Horizontal)
        {
            // Swap x and y for horizontal layout
            return new Point(y, x);
        }
        
        if (Angle == 90) // Down (default)
        {
            return new Point(x, y);
        }
        else if (Angle == 270) // Up
        {
            return new Point(x, -y);
        }
        else if (Angle == 0) // Right
        {
            return new Point(y, x);
        }
        else if (Angle == 180) // Left
        {
            return new Point(-y, x);
        }
        
        // General rotation (rarely used)
        var cos = Math.Cos(angleRad);
        var sin = Math.Sin(angleRad);
        return new Point(x * cos - y * sin, x * sin + y * cos);
    }
    
    private class TreeNodeInfo
    {
        public Node Node { get; set; } = null!;
        public TreeNodeInfo? Parent { get; set; }
        public List<TreeNodeInfo> Children { get; } = new();
        public double Width { get; set; }
        public double Height { get; set; }
        public double TotalWidth { get; set; }
        public double TotalHeight { get; set; }
    }
}

/// <summary>
/// Tree arrangement styles
/// </summary>
public enum TreeArrangement
{
    /// <summary>
    /// Vertical arrangement (top-to-bottom or bottom-to-top)
    /// </summary>
    Vertical,
    
    /// <summary>
    /// Horizontal arrangement (left-to-right or right-to-left)
    /// </summary>
    Horizontal
}

/// <summary>
/// Node alignment within tree layers
/// </summary>
public enum TreeAlignment
{
    /// <summary>
    /// Align to start
    /// </summary>
    Start,
    
    /// <summary>
    /// Align to center
    /// </summary>
    Center,
    
    /// <summary>
    /// Align to end
    /// </summary>
    End,
    
    /// <summary>
    /// Center over children
    /// </summary>
    CenterChildren
}

