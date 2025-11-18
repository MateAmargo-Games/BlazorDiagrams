using BlazorDiagrams.Core.Geometry;
using BlazorDiagrams.Core.Layouts;

namespace BlazorDiagrams.Core.Models;

/// <summary>
/// Represents a group that can contain nodes and other groups
/// </summary>
public class Group : GraphObject
{
    private Point _position = Point.Zero;
    private Size _size = new(200, 150);
    private bool _isExpanded = true;
    private bool _isCollapsible = true;
    private string _fill = "#f0f0f0";
    private string _stroke = "#cccccc";
    private double _strokeWidth = 1;
    private double _padding = 10;
    private string? _title;
    private ILayout? _layout;
    
    /// <summary>
    /// Position of the group in diagram coordinates
    /// </summary>
    public Point Position
    {
        get => _position;
        set => SetProperty(ref _position, value);
    }
    
    /// <summary>
    /// Size of the group
    /// </summary>
    public Size Size
    {
        get => _size;
        set => SetProperty(ref _size, value);
    }
    
    /// <summary>
    /// Whether the group is expanded (showing its contents)
    /// </summary>
    public bool IsExpanded
    {
        get => _isExpanded;
        set => SetProperty(ref _isExpanded, value);
    }
    
    /// <summary>
    /// Whether the group can be collapsed
    /// </summary>
    public bool IsCollapsible
    {
        get => _isCollapsible;
        set => SetProperty(ref _isCollapsible, value);
    }
    
    /// <summary>
    /// Fill color of the group background
    /// </summary>
    public string Fill
    {
        get => _fill;
        set => SetProperty(ref _fill, value);
    }
    
    /// <summary>
    /// Stroke color of the group border
    /// </summary>
    public string Stroke
    {
        get => _stroke;
        set => SetProperty(ref _stroke, value);
    }
    
    /// <summary>
    /// Stroke width of the group border
    /// </summary>
    public double StrokeWidth
    {
        get => _strokeWidth;
        set => SetProperty(ref _strokeWidth, value);
    }
    
    /// <summary>
    /// Padding inside the group
    /// </summary>
    public double Padding
    {
        get => _padding;
        set => SetProperty(ref _padding, value);
    }
    
    /// <summary>
    /// Optional title for the group
    /// </summary>
    public string? Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }
    
    /// <summary>
    /// Layout to apply to nodes within this group
    /// </summary>
    public ILayout? Layout
    {
        get => _layout;
        set => SetProperty(ref _layout, value);
    }
    
    /// <summary>
    /// Nodes that belong to this group
    /// </summary>
    public List<Node> Nodes { get; } = new();
    
    /// <summary>
    /// Subgroups within this group
    /// </summary>
    public List<Group> SubGroups { get; } = new();
    
    /// <summary>
    /// Gets the bounding rectangle of this group
    /// </summary>
    public Rect Bounds => new(Position, Size);
    
    /// <summary>
    /// Adds a node to this group
    /// </summary>
    public void AddNode(Node node)
    {
        if (!Nodes.Contains(node))
        {
            Nodes.Add(node);
            node.Group = this;
        }
    }
    
    /// <summary>
    /// Removes a node from this group
    /// </summary>
    public bool RemoveNode(Node node)
    {
        if (Nodes.Remove(node))
        {
            node.Group = null;
            return true;
        }
        return false;
    }
    
    /// <summary>
    /// Adds a subgroup to this group
    /// </summary>
    public void AddSubGroup(Group group)
    {
        if (!SubGroups.Contains(group))
        {
            SubGroups.Add(group);
        }
    }
    
    /// <summary>
    /// Removes a subgroup from this group
    /// </summary>
    public bool RemoveSubGroup(Group group)
    {
        return SubGroups.Remove(group);
    }
    
    /// <summary>
    /// Calculates the size needed to contain all nodes and subgroups
    /// </summary>
    public Size CalculateRequiredSize()
    {
        if (!IsExpanded || (Nodes.Count == 0 && SubGroups.Count == 0))
        {
            return new Size(Size.Width, 40); // Collapsed size
        }
        
        var allBounds = new List<Rect>();
        
        foreach (var node in Nodes)
        {
            allBounds.Add(node.Bounds);
        }
        
        foreach (var subGroup in SubGroups)
        {
            allBounds.Add(subGroup.Bounds);
        }
        
        if (allBounds.Count == 0)
            return Size;
        
        var minX = allBounds.Min(r => r.Left);
        var minY = allBounds.Min(r => r.Top);
        var maxX = allBounds.Max(r => r.Right);
        var maxY = allBounds.Max(r => r.Bottom);
        
        var contentWidth = maxX - minX;
        var contentHeight = maxY - minY;
        
        return new Size(
            contentWidth + 2 * Padding,
            contentHeight + 2 * Padding + 30 // 30 for title bar
        );
    }
    
    /// <summary>
    /// Updates the group size to fit its contents
    /// </summary>
    public void FitToContents()
    {
        Size = CalculateRequiredSize();
    }
    
    /// <summary>
    /// Toggles the expanded/collapsed state
    /// </summary>
    public void Toggle()
    {
        if (IsCollapsible)
        {
            IsExpanded = !IsExpanded;
        }
    }
}

