using BlazorDiagrams.Core.Geometry;
using BlazorDiagrams.Core.Styling;

namespace BlazorDiagrams.Core.Models;

/// <summary>
/// Represents a connection between two nodes or ports
/// </summary>
public class Link : GraphObject
{
    private Node? _fromNode;
    private Node? _toNode;
    private Port? _fromPort;
    private Port? _toPort;
    private LinkRouting _routing = LinkRouting.Straight;
    private string _stroke = "#000000";
    private double _strokeWidth = 2;
    private string _strokeDashArray = "";
    private bool _showArrowhead = true;
    private ArrowheadStyle _arrowheadStyle = ArrowheadStyle.Standard;
    private List<Point> _routePoints = new();
    private string? _label;
    private LinkStyleConfig? _styleConfig;
    
    /// <summary>
    /// Source node of the link
    /// </summary>
    public Node? FromNode
    {
        get => _fromNode;
        set => SetProperty(ref _fromNode, value);
    }
    
    /// <summary>
    /// Target node of the link
    /// </summary>
    public Node? ToNode
    {
        get => _toNode;
        set => SetProperty(ref _toNode, value);
    }
    
    /// <summary>
    /// Source port of the link (optional, more specific than FromNode)
    /// </summary>
    public Port? FromPort
    {
        get => _fromPort;
        set
        {
            if (_fromPort != null)
                _fromPort.Links.Remove(this);
            
            if (SetProperty(ref _fromPort, value) && value != null)
            {
                value.Links.Add(this);
                FromNode = value.ParentNode;
            }
        }
    }
    
    /// <summary>
    /// Target port of the link (optional, more specific than ToNode)
    /// </summary>
    public Port? ToPort
    {
        get => _toPort;
        set
        {
            if (_toPort != null)
                _toPort.Links.Remove(this);
            
            if (SetProperty(ref _toPort, value) && value != null)
            {
                value.Links.Add(this);
                ToNode = value.ParentNode;
            }
        }
    }
    
    /// <summary>
    /// Routing style for the link
    /// </summary>
    public LinkRouting Routing
    {
        get => _routing;
        set => SetProperty(ref _routing, value);
    }
    
    /// <summary>
    /// Stroke color of the link
    /// </summary>
    public string Stroke
    {
        get => _stroke;
        set => SetProperty(ref _stroke, value);
    }
    
    /// <summary>
    /// Stroke width of the link
    /// </summary>
    public double StrokeWidth
    {
        get => _strokeWidth;
        set => SetProperty(ref _strokeWidth, value);
    }
    
    /// <summary>
    /// SVG stroke-dasharray for dashed lines
    /// </summary>
    public string StrokeDashArray
    {
        get => _strokeDashArray;
        set => SetProperty(ref _strokeDashArray, value);
    }
    
    /// <summary>
    /// Whether to show an arrowhead at the end of the link
    /// </summary>
    public bool ShowArrowhead
    {
        get => _showArrowhead;
        set => SetProperty(ref _showArrowhead, value);
    }
    
    /// <summary>
    /// Style of the arrowhead
    /// </summary>
    public ArrowheadStyle ArrowheadStyle
    {
        get => _arrowheadStyle;
        set => SetProperty(ref _arrowheadStyle, value);
    }
    
    /// <summary>
    /// Intermediate points for the link route (used by routing algorithms)
    /// </summary>
    public List<Point> RoutePoints
    {
        get => _routePoints;
        set => SetProperty(ref _routePoints, value);
    }
    
    /// <summary>
    /// Optional label for the link
    /// </summary>
    public string? Label
    {
        get => _label;
        set => SetProperty(ref _label, value);
    }
    
    /// <summary>
    /// Custom style configuration for this link (overrides theme defaults)
    /// </summary>
    public LinkStyleConfig? StyleConfig
    {
        get => _styleConfig;
        set => SetProperty(ref _styleConfig, value);
    }
    
    /// <summary>
    /// Gets the start point of the link
    /// </summary>
    public Point GetStartPoint()
    {
        if (FromPort != null)
            return FromPort.GetAbsolutePosition();
        
        if (FromNode != null && ToNode != null)
            return FromNode.GetConnectionPoint(ToNode.Center);
        
        if (FromNode != null)
            return FromNode.Center;
        
        return Point.Zero;
    }
    
    /// <summary>
    /// Gets the end point of the link
    /// </summary>
    public Point GetEndPoint()
    {
        if (ToPort != null)
            return ToPort.GetAbsolutePosition();
        
        if (ToNode != null && FromNode != null)
            return ToNode.GetConnectionPoint(FromNode.Center);
        
        if (ToNode != null)
            return ToNode.Center;
        
        return Point.Zero;
    }
    
    /// <summary>
    /// Gets all points that define the link path (start, intermediate, end)
    /// </summary>
    public List<Point> GetAllPoints()
    {
        var points = new List<Point> { GetStartPoint() };
        points.AddRange(RoutePoints);
        points.Add(GetEndPoint());
        return points;
    }
    
    /// <summary>
    /// Generates an SVG path string for this link
    /// </summary>
    public string GetSvgPath()
    {
        var points = GetAllPoints();
        if (points.Count < 2)
            return "";
        
        return Routing switch
        {
            LinkRouting.Straight => GetStraightPath(points),
            LinkRouting.Orthogonal => GetOrthogonalPath(points),
            LinkRouting.Bezier => GetBezierPath(points),
            _ => GetStraightPath(points)
        };
    }
    
    private string GetStraightPath(List<Point> points)
    {
        if (points.Count < 2)
            return "";
        
        var path = $"M {points[0].X:F2} {points[0].Y:F2}";
        for (int i = 1; i < points.Count; i++)
        {
            path += $" L {points[i].X:F2} {points[i].Y:F2}";
        }
        return path;
    }
    
    private string GetOrthogonalPath(List<Point> points)
    {
        // Simple orthogonal routing - can be enhanced
        return GetStraightPath(points);
    }
    
    private string GetBezierPath(List<Point> points)
    {
        if (points.Count < 2)
            return "";
        
        if (points.Count == 2)
        {
            // Simple curve with control points
            var start = points[0];
            var end = points[1];
            var distance = start.DistanceTo(end);
            var offset = distance * 0.3;
            
            var cp1 = new Point(start.X + offset, start.Y);
            var cp2 = new Point(end.X - offset, end.Y);
            
            return $"M {start.X:F2} {start.Y:F2} C {cp1.X:F2} {cp1.Y:F2}, {cp2.X:F2} {cp2.Y:F2}, {end.X:F2} {end.Y:F2}";
        }
        
        // For multiple points, use smooth curves
        return GetStraightPath(points);
    }
}

/// <summary>
/// Link routing styles
/// </summary>
public enum LinkRouting
{
    Straight,
    Orthogonal,
    Bezier
}

/// <summary>
/// Arrowhead styles for links
/// </summary>
public enum ArrowheadStyle
{
    None,
    Standard,
    Open,
    Diamond,
    Circle
}

