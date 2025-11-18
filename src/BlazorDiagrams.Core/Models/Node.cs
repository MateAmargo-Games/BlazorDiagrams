using BlazorDiagrams.Core.Geometry;
using BlazorDiagrams.Core.Styling;

namespace BlazorDiagrams.Core.Models;

/// <summary>
/// Represents a node in the diagram
/// </summary>
public class Node : GraphObject
{
    private Point _position = Point.Zero;
    private Size _size = new(100, 50);
    private Size? _minSize;
    private Size? _maxSize;
    private bool _isResizable = true;
    private bool _isDraggable = true;
    private bool _isCollapsed;
    private Group? _group;
    private string _fill = "#ffffff";
    private string _stroke = "#000000";
    private double _strokeWidth = 1;
    private double _cornerRadius;
    private NodeStyleConfig? _styleConfig;
    
    /// <summary>
    /// Position of the node in diagram coordinates
    /// </summary>
    public Point Position
    {
        get => _position;
        set
        {
            if (SetProperty(ref _position, value))
            {
                UpdatePortPositions();
            }
        }
    }
    
    /// <summary>
    /// Size of the node
    /// </summary>
    public Size Size
    {
        get => _size;
        set
        {
            var newSize = value;
            
            if (MinSize.HasValue)
            {
                newSize = new Size(
                    Math.Max(newSize.Width, MinSize.Value.Width),
                    Math.Max(newSize.Height, MinSize.Value.Height)
                );
            }
            
            if (MaxSize.HasValue)
            {
                newSize = new Size(
                    Math.Min(newSize.Width, MaxSize.Value.Width),
                    Math.Min(newSize.Height, MaxSize.Value.Height)
                );
            }
            
            if (SetProperty(ref _size, newSize))
            {
                UpdatePortPositions();
            }
        }
    }
    
    /// <summary>
    /// Minimum size of the node
    /// </summary>
    public Size? MinSize
    {
        get => _minSize;
        set => SetProperty(ref _minSize, value);
    }
    
    /// <summary>
    /// Maximum size of the node
    /// </summary>
    public Size? MaxSize
    {
        get => _maxSize;
        set => SetProperty(ref _maxSize, value);
    }
    
    /// <summary>
    /// Whether the node can be resized
    /// </summary>
    public bool IsResizable
    {
        get => _isResizable;
        set => SetProperty(ref _isResizable, value);
    }
    
    /// <summary>
    /// Whether the node can be dragged
    /// </summary>
    public bool IsDraggable
    {
        get => _isDraggable;
        set => SetProperty(ref _isDraggable, value);
    }
    
    /// <summary>
    /// Whether the node is collapsed (for tree layouts)
    /// </summary>
    public bool IsCollapsed
    {
        get => _isCollapsed;
        set => SetProperty(ref _isCollapsed, value);
    }
    
    /// <summary>
    /// The group this node belongs to (if any)
    /// </summary>
    public Group? Group
    {
        get => _group;
        set => SetProperty(ref _group, value);
    }
    
    /// <summary>
    /// Fill color of the node
    /// </summary>
    public string Fill
    {
        get => _fill;
        set => SetProperty(ref _fill, value);
    }
    
    /// <summary>
    /// Stroke color of the node
    /// </summary>
    public string Stroke
    {
        get => _stroke;
        set => SetProperty(ref _stroke, value);
    }
    
    /// <summary>
    /// Stroke width of the node
    /// </summary>
    public double StrokeWidth
    {
        get => _strokeWidth;
        set => SetProperty(ref _strokeWidth, value);
    }
    
    /// <summary>
    /// Corner radius for rounded rectangles
    /// </summary>
    public double CornerRadius
    {
        get => _cornerRadius;
        set => SetProperty(ref _cornerRadius, value);
    }
    
    /// <summary>
    /// Ports on this node
    /// </summary>
    public List<Port> Ports { get; } = new();
    
    /// <summary>
    /// Links connected to this node
    /// </summary>
    public List<Link> Links { get; } = new();
    
    /// <summary>
    /// Interactive actions that can be displayed on this node
    /// </summary>
    public List<NodeAction> Actions { get; } = new();
    
    /// <summary>
    /// Custom style configuration for this node (overrides theme defaults)
    /// </summary>
    public NodeStyleConfig? StyleConfig
    {
        get => _styleConfig;
        set => SetProperty(ref _styleConfig, value);
    }
    
    /// <summary>
    /// Gets the bounding rectangle of this node
    /// </summary>
    public Rect Bounds => new(Position, Size);
    
    /// <summary>
    /// Gets the center point of this node
    /// </summary>
    public Point Center => new(Position.X + Size.Width / 2, Position.Y + Size.Height / 2);
    
    /// <summary>
    /// Adds a port to this node
    /// </summary>
    public Port AddPort(string id, PortAlignment alignment = PortAlignment.Center)
    {
        var port = new Port
        {
            Id = id,
            ParentNode = this,
            Alignment = alignment
        };
        
        Ports.Add(port);
        return port;
    }
    
    /// <summary>
    /// Removes a port from this node
    /// </summary>
    public bool RemovePort(Port port)
    {
        if (Ports.Remove(port))
        {
            port.ParentNode = null;
            return true;
        }
        return false;
    }
    
    /// <summary>
    /// Gets a port by its ID
    /// </summary>
    public Port? GetPort(string id) => Ports.FirstOrDefault(p => p.Id == id);
    
    /// <summary>
    /// Updates the positions of all ports on this node
    /// </summary>
    private void UpdatePortPositions()
    {
        foreach (var port in Ports)
        {
            OnPropertyChanged(nameof(port.Position));
        }
    }
    
    /// <summary>
    /// Checks if this node contains a point
    /// </summary>
    public bool ContainsPoint(Point point) => Bounds.Contains(point);
    
    /// <summary>
    /// Checks if this node intersects with another node
    /// </summary>
    public bool IntersectsWith(Node other) => Bounds.Intersects(other.Bounds);
    
    /// <summary>
    /// Gets the connection point on the node border for a given external point
    /// </summary>
    public Point GetConnectionPoint(Point externalPoint)
    {
        var center = Center;
        
        // If the point is at the center, return center
        if (externalPoint == center)
            return center;
        
        // Calculate the angle from center to external point
        var angle = Math.Atan2(externalPoint.Y - center.Y, externalPoint.X - center.X);
        
        // Calculate intersection with rectangle border
        var hw = Size.Width / 2;
        var hh = Size.Height / 2;
        
        var dx = Math.Cos(angle);
        var dy = Math.Sin(angle);
        
        // Find intersection with rectangle
        var t = Math.Min(
            dx != 0 ? Math.Abs(hw / dx) : double.MaxValue,
            dy != 0 ? Math.Abs(hh / dy) : double.MaxValue
        );
        
        return new Point(center.X + dx * t, center.Y + dy * t);
    }
    
    /// <summary>
    /// Adds an action to this node
    /// </summary>
    public NodeAction AddAction(NodeAction action)
    {
        Actions.Add(action);
        return action;
    }
    
    /// <summary>
    /// Adds an action to this node with simplified configuration
    /// </summary>
    public NodeAction AddAction(string icon, Action<Node> onClick, string? tooltip = null, 
        NodeActionPosition position = NodeActionPosition.TopRight)
    {
        var action = new NodeAction
        {
            Icon = icon,
            OnClick = onClick,
            Tooltip = tooltip,
            Position = position
        };
        Actions.Add(action);
        return action;
    }
    
    /// <summary>
    /// Removes an action from this node
    /// </summary>
    public bool RemoveAction(NodeAction action)
    {
        return Actions.Remove(action);
    }
    
    /// <summary>
    /// Removes an action by its ID
    /// </summary>
    public bool RemoveAction(string actionId)
    {
        var action = Actions.FirstOrDefault(a => a.Id == actionId);
        return action != null && Actions.Remove(action);
    }
    
    /// <summary>
    /// Gets an action by its ID
    /// </summary>
    public NodeAction? GetAction(string actionId) => Actions.FirstOrDefault(a => a.Id == actionId);
    
    /// <summary>
    /// Clears all actions from this node
    /// </summary>
    public void ClearActions()
    {
        Actions.Clear();
    }
}

