using BlazorDiagrams.Core.Geometry;
using BlazorDiagrams.Core.Models;

namespace BlazorDiagrams.Core.Services;

/// <summary>
/// Service for virtualizing large diagrams by only rendering visible elements
/// </summary>
public class VirtualizationService
{
    private Rect _viewport = Rect.Zero;
    private double _buffer = 100; // Buffer zone around viewport
    
    /// <summary>
    /// Buffer size in pixels around the viewport to pre-render elements
    /// </summary>
    public double BufferSize
    {
        get => _buffer;
        set => _buffer = Math.Max(0, value);
    }
    
    /// <summary>
    /// Sets the current viewport
    /// </summary>
    public void SetViewport(Rect viewport)
    {
        _viewport = viewport;
    }
    
    /// <summary>
    /// Gets the extended viewport (viewport + buffer)
    /// </summary>
    public Rect GetExtendedViewport()
    {
        return _viewport.Inflate(BufferSize);
    }
    
    /// <summary>
    /// Checks if a node is visible in the current viewport
    /// </summary>
    public bool IsNodeVisible(Node node)
    {
        var extendedViewport = GetExtendedViewport();
        return extendedViewport.Intersects(node.Bounds);
    }
    
    /// <summary>
    /// Checks if a link is visible in the current viewport
    /// </summary>
    public bool IsLinkVisible(Link link)
    {
        var extendedViewport = GetExtendedViewport();
        
        // Check if any point of the link is within the viewport
        var points = link.GetAllPoints();
        if (points.Count < 2)
            return false;
        
        // Create a bounding box for the link
        var minX = points.Min(p => p.X);
        var minY = points.Min(p => p.Y);
        var maxX = points.Max(p => p.X);
        var maxY = points.Max(p => p.Y);
        
        var linkBounds = new Rect(minX, minY, maxX - minX, maxY - minY);
        
        return extendedViewport.Intersects(linkBounds);
    }
    
    /// <summary>
    /// Checks if a group is visible in the current viewport
    /// </summary>
    public bool IsGroupVisible(Group group)
    {
        var extendedViewport = GetExtendedViewport();
        return extendedViewport.Intersects(group.Bounds);
    }
    
    /// <summary>
    /// Gets all visible nodes in the current viewport
    /// </summary>
    public IEnumerable<Node> GetVisibleNodes(IEnumerable<Node> nodes)
    {
        var extendedViewport = GetExtendedViewport();
        return nodes.Where(n => n.IsVisible && extendedViewport.Intersects(n.Bounds));
    }
    
    /// <summary>
    /// Gets all visible links in the current viewport
    /// </summary>
    public IEnumerable<Link> GetVisibleLinks(IEnumerable<Link> links)
    {
        return links.Where(l => l.IsVisible && IsLinkVisible(l));
    }
    
    /// <summary>
    /// Gets all visible groups in the current viewport
    /// </summary>
    public IEnumerable<Group> GetVisibleGroups(IEnumerable<Group> groups)
    {
        var extendedViewport = GetExtendedViewport();
        return groups.Where(g => g.IsVisible && extendedViewport.Intersects(g.Bounds));
    }
    
    /// <summary>
    /// Calculates optimal buffer size based on viewport and node sizes
    /// </summary>
    public void AutoCalculateBufferSize(IEnumerable<Node> nodes, Rect viewport)
    {
        if (!nodes.Any())
        {
            BufferSize = 100;
            return;
        }
        
        var avgNodeSize = nodes.Average(n => Math.Max(n.Size.Width, n.Size.Height));
        var viewportSize = Math.Max(viewport.Width, viewport.Height);
        
        // Buffer should be about 10% of viewport or 2x average node size, whichever is larger
        BufferSize = Math.Max(viewportSize * 0.1, avgNodeSize * 2);
    }
}

/// <summary>
/// Cache service for expensive layout calculations
/// </summary>
public class DiagramCacheService
{
    private readonly Dictionary<string, object> _cache = new();
    private readonly Dictionary<string, DateTime> _cacheTimestamps = new();
    private TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);
    
    /// <summary>
    /// Cache expiration time
    /// </summary>
    public TimeSpan CacheExpiration
    {
        get => _cacheExpiration;
        set => _cacheExpiration = value;
    }
    
    /// <summary>
    /// Gets a value from cache
    /// </summary>
    public T? Get<T>(string key)
    {
        CleanExpiredEntries();
        
        if (_cache.TryGetValue(key, out var value))
        {
            return (T?)value;
        }
        
        return default;
    }
    
    /// <summary>
    /// Sets a value in cache
    /// </summary>
    public void Set<T>(string key, T value)
    {
        _cache[key] = value!;
        _cacheTimestamps[key] = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Checks if a key exists in cache and is not expired
    /// </summary>
    public bool Contains(string key)
    {
        CleanExpiredEntries();
        return _cache.ContainsKey(key);
    }
    
    /// <summary>
    /// Removes a key from cache
    /// </summary>
    public void Remove(string key)
    {
        _cache.Remove(key);
        _cacheTimestamps.Remove(key);
    }
    
    /// <summary>
    /// Clears all cache
    /// </summary>
    public void Clear()
    {
        _cache.Clear();
        _cacheTimestamps.Clear();
    }
    
    private void CleanExpiredEntries()
    {
        var now = DateTime.UtcNow;
        var expiredKeys = _cacheTimestamps
            .Where(kvp => now - kvp.Value > _cacheExpiration)
            .Select(kvp => kvp.Key)
            .ToList();
        
        foreach (var key in expiredKeys)
        {
            _cache.Remove(key);
            _cacheTimestamps.Remove(key);
        }
    }
}

