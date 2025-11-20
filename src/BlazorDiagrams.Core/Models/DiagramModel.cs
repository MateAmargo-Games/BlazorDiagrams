using BlazorDiagrams.Core.Geometry;
using BlazorDiagrams.Core.Layouts;
using BlazorDiagrams.Core.Styling;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BlazorDiagrams.Core.Models;

/// <summary>
/// Main model that contains all diagram elements and configuration
/// </summary>
public class DiagramModel : INotifyPropertyChanged
{
    private ILayout? _layout;
    private double _zoom = 1.0;
    private Point _panOffset = Point.Zero;
    private bool _isReadOnly;
    private Rect _viewport = new(0, 0, 1000, 800);
    private DiagramTheme _theme = DiagramTheme.Light;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DiagramModel"/> class.
    /// </summary>
    public DiagramModel()
    {
        Nodes = new ObservableCollection<Node>();
        Links = new ObservableCollection<Link>();
        Groups = new ObservableCollection<Group>();
        
        Nodes.CollectionChanged += (s, e) => OnPropertyChanged(nameof(Nodes));
        Links.CollectionChanged += (s, e) => OnPropertyChanged(nameof(Links));
        Groups.CollectionChanged += (s, e) => OnPropertyChanged(nameof(Groups));
    }
    
    /// <summary>
    /// All nodes in the diagram
    /// </summary>
    public ObservableCollection<Node> Nodes { get; }
    
    /// <summary>
    /// All links in the diagram
    /// </summary>
    public ObservableCollection<Link> Links { get; }
    
    /// <summary>
    /// All groups in the diagram
    /// </summary>
    public ObservableCollection<Group> Groups { get; }
    
    /// <summary>
    /// Layout algorithm to apply to the diagram
    /// </summary>
    public ILayout? Layout
    {
        get => _layout;
        set
        {
            _layout = value;
            OnPropertyChanged(nameof(Layout));
        }
    }
    
    /// <summary>
    /// Current zoom level (1.0 = 100%)
    /// </summary>
    public double Zoom
    {
        get => _zoom;
        set
        {
            _zoom = Math.Clamp(value, 0.1, 10.0);
            OnPropertyChanged(nameof(Zoom));
        }
    }
    
    /// <summary>
    /// Pan offset for the diagram view
    /// </summary>
    public Point PanOffset
    {
        get => _panOffset;
        set
        {
            _panOffset = value;
            OnPropertyChanged(nameof(PanOffset));
        }
    }
    
    /// <summary>
    /// Whether the diagram is in read-only mode
    /// </summary>
    public bool IsReadOnly
    {
        get => _isReadOnly;
        set
        {
            _isReadOnly = value;
            OnPropertyChanged(nameof(IsReadOnly));
        }
    }
    
    /// <summary>
    /// Visible viewport area
    /// </summary>
    public Rect Viewport
    {
        get => _viewport;
        set
        {
            _viewport = value;
            OnPropertyChanged(nameof(Viewport));
        }
    }
    
    /// <summary>
    /// Theme configuration for the diagram
    /// </summary>
    public DiagramTheme Theme
    {
        get => _theme;
        set
        {
            _theme = value;
            OnPropertyChanged(nameof(Theme));
        }
    }
    
    /// <summary>
    /// Custom properties for extensibility
    /// </summary>
    public Dictionary<string, object?> Properties { get; } = new();
    
    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;
    
    /// <summary>
    /// Raises the PropertyChanged event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    #region Node Operations
    
    /// <summary>
    /// Adds a node to the diagram
    /// </summary>
    public Node AddNode(string id, object? data = null)
    {
        var node = new Node
        {
            Id = id,
            Data = data
        };
        Nodes.Add(node);
        return node;
    }
    
    /// <summary>
    /// Adds a node to the diagram at a specific position
    /// </summary>
    public Node AddNode(string id, Point position, object? data = null)
    {
        var node = new Node
        {
            Id = id,
            Position = position,
            Data = data
        };
        Nodes.Add(node);
        return node;
    }
    
    /// <summary>
    /// Adds an existing node to the diagram
    /// </summary>
    public void AddNode(Node node)
    {
        if (!Nodes.Contains(node))
        {
            Nodes.Add(node);
        }
    }
    
    /// <summary>
    /// Removes a node from the diagram and all its connected links
    /// </summary>
    public bool RemoveNode(Node node)
    {
        if (Nodes.Remove(node))
        {
            // Remove all links connected to this node
            var linksToRemove = Links.Where(l => l.FromNode == node || l.ToNode == node).ToList();
            foreach (var link in linksToRemove)
            {
                Links.Remove(link);
            }
            return true;
        }
        return false;
    }
    
    /// <summary>
    /// Removes a node by its ID
    /// </summary>
    public bool RemoveNode(string id)
    {
        var node = GetNode(id);
        return node != null && RemoveNode(node);
    }
    
    /// <summary>
    /// Gets a node by its ID
    /// </summary>
    public Node? GetNode(string id) => Nodes.FirstOrDefault(n => n.Id == id);
    
    #endregion
    
    #region Link Operations
    
    /// <summary>
    /// Adds a link between two nodes
    /// </summary>
    public Link AddLink(string fromNodeId, string toNodeId)
    {
        var fromNode = GetNode(fromNodeId);
        var toNode = GetNode(toNodeId);
        
        if (fromNode == null || toNode == null)
            throw new ArgumentException("One or both nodes not found");
        
        return AddLink(fromNode, toNode);
    }
    
    /// <summary>
    /// Adds a link between two nodes
    /// </summary>
    public Link AddLink(Node fromNode, Node toNode)
    {
        var link = new Link
        {
            Id = Guid.NewGuid().ToString(),
            FromNode = fromNode,
            ToNode = toNode
        };
        
        Links.Add(link);
        fromNode.Links.Add(link);
        toNode.Links.Add(link);
        
        return link;
    }
    
    /// <summary>
    /// Adds a link between two ports
    /// </summary>
    public Link AddLink(Port fromPort, Port toPort)
    {
        var link = new Link
        {
            Id = Guid.NewGuid().ToString(),
            FromPort = fromPort,
            ToPort = toPort
        };
        
        Links.Add(link);
        
        return link;
    }
    
    /// <summary>
    /// Adds an existing link to the diagram
    /// </summary>
    public void AddLink(Link link)
    {
        if (!Links.Contains(link))
        {
            Links.Add(link);
        }
    }
    
    /// <summary>
    /// Removes a link from the diagram
    /// </summary>
    public bool RemoveLink(Link link)
    {
        if (Links.Remove(link))
        {
            link.FromNode?.Links.Remove(link);
            link.ToNode?.Links.Remove(link);
            return true;
        }
        return false;
    }
    
    /// <summary>
    /// Gets a link by its ID
    /// </summary>
    public Link? GetLink(string id) => Links.FirstOrDefault(l => l.Id == id);
    
    #endregion
    
    #region Group Operations
    
    /// <summary>
    /// Adds a group to the diagram
    /// </summary>
    public Group AddGroup(string id, string? title = null)
    {
        var group = new Group
        {
            Id = id,
            Title = title
        };
        Groups.Add(group);
        return group;
    }
    
    /// <summary>
    /// Adds an existing group to the diagram
    /// </summary>
    public void AddGroup(Group group)
    {
        if (!Groups.Contains(group))
        {
            Groups.Add(group);
        }
    }
    
    /// <summary>
    /// Removes a group from the diagram
    /// </summary>
    public bool RemoveGroup(Group group)
    {
        if (Groups.Remove(group))
        {
            // Ungroup all nodes
            foreach (var node in group.Nodes.ToList())
            {
                group.RemoveNode(node);
            }
            return true;
        }
        return false;
    }
    
    /// <summary>
    /// Gets a group by its ID
    /// </summary>
    public Group? GetGroup(string id) => Groups.FirstOrDefault(g => g.Id == id);
    
    #endregion
    
    #region Selection
    
    /// <summary>
    /// Gets all selected nodes
    /// </summary>
    public IEnumerable<Node> GetSelectedNodes() => Nodes.Where(n => n.IsSelected);
    
    /// <summary>
    /// Gets all selected links
    /// </summary>
    public IEnumerable<Link> GetSelectedLinks() => Links.Where(l => l.IsSelected);
    
    /// <summary>
    /// Selects a node
    /// </summary>
    public void SelectNode(Node node, bool addToSelection = false)
    {
        if (!addToSelection)
        {
            ClearSelection();
        }
        node.IsSelected = true;
    }
    
    /// <summary>
    /// Deselects a node
    /// </summary>
    public void DeselectNode(Node node)
    {
        node.IsSelected = false;
    }
    
    /// <summary>
    /// Clears all selections
    /// </summary>
    public void ClearSelection()
    {
        foreach (var node in Nodes.Where(n => n.IsSelected))
        {
            node.IsSelected = false;
        }
        foreach (var link in Links.Where(l => l.IsSelected))
        {
            link.IsSelected = false;
        }
    }
    
    #endregion
    
    #region Layout
    
    /// <summary>
    /// Applies the configured layout to the diagram
    /// </summary>
    public void ApplyLayout()
    {
        if (Layout != null)
        {
            Layout.Apply(this);
        }
    }
    
    #endregion
    
    #region Utility
    
    /// <summary>
    /// Gets the bounding box of all nodes
    /// </summary>
    public Rect GetBoundingBox()
    {
        if (Nodes.Count == 0)
            return Rect.Zero;
        
        var bounds = Nodes.Select(n => n.Bounds);
        var minX = bounds.Min(b => b.Left);
        var minY = bounds.Min(b => b.Top);
        var maxX = bounds.Max(b => b.Right);
        var maxY = bounds.Max(b => b.Bottom);
        
        return new Rect(minX, minY, maxX - minX, maxY - minY);
    }
    
    /// <summary>
    /// Clears the entire diagram
    /// </summary>
    public void Clear()
    {
        Links.Clear();
        Nodes.Clear();
        Groups.Clear();
    }
    
    /// <summary>
    /// Gets all nodes at a specific point
    /// </summary>
    public IEnumerable<Node> GetNodesAtPoint(Point point)
    {
        return Nodes.Where(n => n.ContainsPoint(point));
    }
    
    #endregion
}

