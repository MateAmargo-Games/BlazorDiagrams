using BlazorDiagrams.Core.Geometry;

namespace BlazorDiagrams.Core.Models;

/// <summary>
/// Represents a connection point on a node where links can attach
/// </summary>
public class Port : GraphObject
{
    private Node? _parentNode;
    private Point _position = Point.Zero;
    private PortAlignment _alignment = PortAlignment.Center;
    private double _offsetX;
    private double _offsetY;
    private int _maxLinks = int.MaxValue;
    private bool _canConnectToSelf = false;
    private HashSet<string>? _allowedCategories;
    
    /// <summary>
    /// The node that owns this port
    /// </summary>
    public Node? ParentNode
    {
        get => _parentNode;
        set => SetProperty(ref _parentNode, value);
    }
    
    /// <summary>
    /// Position relative to the parent node
    /// </summary>
    public Point Position
    {
        get => _position;
        set => SetProperty(ref _position, value);
    }
    
    /// <summary>
    /// Alignment of the port on the node
    /// </summary>
    public PortAlignment Alignment
    {
        get => _alignment;
        set => SetProperty(ref _alignment, value);
    }
    
    /// <summary>
    /// Horizontal offset from the alignment position
    /// </summary>
    public double OffsetX
    {
        get => _offsetX;
        set => SetProperty(ref _offsetX, value);
    }
    
    /// <summary>
    /// Vertical offset from the alignment position
    /// </summary>
    public double OffsetY
    {
        get => _offsetY;
        set => SetProperty(ref _offsetY, value);
    }
    
    /// <summary>
    /// Maximum number of links that can connect to this port
    /// </summary>
    public int MaxLinks
    {
        get => _maxLinks;
        set => SetProperty(ref _maxLinks, value);
    }
    
    /// <summary>
    /// Whether links can connect from this port back to the same node
    /// </summary>
    public bool CanConnectToSelf
    {
        get => _canConnectToSelf;
        set => SetProperty(ref _canConnectToSelf, value);
    }
    
    /// <summary>
    /// Categories of ports this port can connect to (null = all)
    /// </summary>
    public HashSet<string>? AllowedCategories
    {
        get => _allowedCategories;
        set => SetProperty(ref _allowedCategories, value);
    }
    
    /// <summary>
    /// Links connected to this port
    /// </summary>
    public List<Link> Links { get; } = new();
    
    /// <summary>
    /// Gets the absolute position of this port in diagram coordinates
    /// </summary>
    public Point GetAbsolutePosition()
    {
        if (ParentNode == null)
            return Position;
            
        var nodePos = ParentNode.Position;
        var nodeSize = ParentNode.Size;
        
        var alignmentPos = Alignment switch
        {
            PortAlignment.TopLeft => new Point(nodePos.X, nodePos.Y),
            PortAlignment.TopCenter => new Point(nodePos.X + nodeSize.Width / 2, nodePos.Y),
            PortAlignment.TopRight => new Point(nodePos.X + nodeSize.Width, nodePos.Y),
            PortAlignment.LeftCenter => new Point(nodePos.X, nodePos.Y + nodeSize.Height / 2),
            PortAlignment.Center => new Point(nodePos.X + nodeSize.Width / 2, nodePos.Y + nodeSize.Height / 2),
            PortAlignment.RightCenter => new Point(nodePos.X + nodeSize.Width, nodePos.Y + nodeSize.Height / 2),
            PortAlignment.BottomLeft => new Point(nodePos.X, nodePos.Y + nodeSize.Height),
            PortAlignment.BottomCenter => new Point(nodePos.X + nodeSize.Width / 2, nodePos.Y + nodeSize.Height),
            PortAlignment.BottomRight => new Point(nodePos.X + nodeSize.Width, nodePos.Y + nodeSize.Height),
            _ => nodePos
        };
        
        return new Point(alignmentPos.X + OffsetX + Position.X, alignmentPos.Y + OffsetY + Position.Y);
    }
    
    /// <summary>
    /// Checks if this port can accept more links
    /// </summary>
    public bool CanAddLink() => Links.Count < MaxLinks;
    
    /// <summary>
    /// Checks if this port can connect to another port
    /// </summary>
    public bool CanConnectTo(Port other)
    {
        if (other == this)
            return false;
            
        if (!CanConnectToSelf && other.ParentNode == ParentNode)
            return false;
            
        if (AllowedCategories != null && other.Category != null && !AllowedCategories.Contains(other.Category))
            return false;
            
        if (other.AllowedCategories != null && Category != null && !other.AllowedCategories.Contains(Category))
            return false;
            
        return CanAddLink() && other.CanAddLink();
    }
}

/// <summary>
/// Port alignment options relative to the parent node
/// </summary>
public enum PortAlignment
{
    /// <summary>
    /// Top left corner
    /// </summary>
    TopLeft,
    
    /// <summary>
    /// Top center
    /// </summary>
    TopCenter,
    
    /// <summary>
    /// Top right corner
    /// </summary>
    TopRight,
    
    /// <summary>
    /// Middle left
    /// </summary>
    LeftCenter,
    
    /// <summary>
    /// Center
    /// </summary>
    Center,
    
    /// <summary>
    /// Middle right
    /// </summary>
    RightCenter,
    
    /// <summary>
    /// Bottom left corner
    /// </summary>
    BottomLeft,
    
    /// <summary>
    /// Bottom center
    /// </summary>
    BottomCenter,
    
    /// <summary>
    /// Bottom right corner
    /// </summary>
    BottomRight
}

