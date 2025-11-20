using BlazorDiagrams.Core.Geometry;
using BlazorDiagrams.Core.Models;

namespace BlazorDiagrams.Core.Layouts;

/// <summary>
/// Layered digraph layout using Sugiyama framework for hierarchical flowcharts
/// </summary>
public class LayeredDigraphLayout : ILayout
{
    /// <summary>
    /// Direction of the layout
    /// </summary>
    public LayoutDirection Direction { get; set; } = LayoutDirection.Down;
    
    /// <summary>
    /// Spacing between layers
    /// </summary>
    public double LayerSpacing { get; set; } = 80;
    
    /// <summary>
    /// Spacing between nodes in the same layer
    /// </summary>
    public double NodeSpacing { get; set; } = 40;
    
    /// <summary>
    /// Number of iterations for crossing reduction
    /// </summary>
    public int CrossingReductionIterations { get; set; } = 10;
    
    /// <inheritdoc />
    public void Apply(DiagramModel model)
    {
        if (model.Nodes.Count == 0)
            return;
        
        // Step 1: Cycle removal (make graph acyclic)
        var reversedLinks = RemoveCycles(model);
        
        // Step 2: Layer assignment
        var layers = AssignLayers(model);
        
        // Step 3: Add dummy nodes for long edges
        AddDummyNodes(model, layers);
        
        // Step 4: Crossing reduction
        ReduceCrossings(model, layers);
        
        // Step 5: Coordinate assignment
        AssignCoordinates(model, layers);
        
        // Step 6: Restore reversed links
        RestoreReversedLinks(reversedLinks);
    }
    
    /// <inheritdoc />
    public void ApplyToGroup(Group group)
    {
        // Similar logic for groups
    }
    
    private List<Link> RemoveCycles(DiagramModel model)
    {
        var reversedLinks = new List<Link>();
        var visited = new HashSet<Node>();
        var recursionStack = new HashSet<Node>();
        
        foreach (var node in model.Nodes)
        {
            if (!visited.Contains(node))
            {
                DetectAndBreakCycles(node, model, visited, recursionStack, reversedLinks);
            }
        }
        
        // Reverse the links
        foreach (var link in reversedLinks)
        {
            var temp = link.FromNode;
            link.FromNode = link.ToNode;
            link.ToNode = temp;
        }
        
        return reversedLinks;
    }
    
    private void DetectAndBreakCycles(Node node, DiagramModel model, HashSet<Node> visited, 
        HashSet<Node> recursionStack, List<Link> reversedLinks)
    {
        visited.Add(node);
        recursionStack.Add(node);
        
        var outgoingLinks = model.Links.Where(l => l.FromNode == node).ToList();
        
        foreach (var link in outgoingLinks)
        {
            var target = link.ToNode;
            if (target == null) continue;
            
            if (!visited.Contains(target))
            {
                DetectAndBreakCycles(target, model, visited, recursionStack, reversedLinks);
            }
            else if (recursionStack.Contains(target))
            {
                // Cycle detected, mark link for reversal
                reversedLinks.Add(link);
            }
        }
        
        recursionStack.Remove(node);
    }
    
    private List<List<Node>> AssignLayers(DiagramModel model)
    {
        var layers = new List<List<Node>>();
        var nodeLayer = new Dictionary<Node, int>();
        
        // Find root nodes (no incoming edges)
        var inDegree = new Dictionary<Node, int>();
        foreach (var node in model.Nodes)
        {
            inDegree[node] = 0;
        }
        
        foreach (var link in model.Links)
        {
            if (link.ToNode != null && inDegree.ContainsKey(link.ToNode))
            {
                inDegree[link.ToNode]++;
            }
        }
        
        var queue = new Queue<Node>();
        foreach (var node in model.Nodes)
        {
            if (inDegree[node] == 0)
            {
                queue.Enqueue(node);
                nodeLayer[node] = 0;
            }
        }
        
        // BFS-based layering
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            var layer = nodeLayer[node];
            
            // Ensure we have enough layers
            while (layers.Count <= layer)
            {
                layers.Add(new List<Node>());
            }
            layers[layer].Add(node);
            
            // Process successors
            var outgoingLinks = model.Links.Where(l => l.FromNode == node);
            foreach (var link in outgoingLinks)
            {
                var target = link.ToNode;
                if (target == null) continue;
                
                if (!nodeLayer.ContainsKey(target))
                {
                    nodeLayer[target] = layer + 1;
                    inDegree[target]--;
                    if (inDegree[target] == 0)
                    {
                        queue.Enqueue(target);
                    }
                }
                else
                {
                    // Ensure target is in a later layer
                    nodeLayer[target] = Math.Max(nodeLayer[target], layer + 1);
                }
            }
        }
        
        // Rebuild layers based on final assignments
        layers.Clear();
        foreach (var kvp in nodeLayer)
        {
            while (layers.Count <= kvp.Value)
            {
                layers.Add(new List<Node>());
            }
            layers[kvp.Value].Add(kvp.Key);
        }
        
        return layers;
    }
    
    private void AddDummyNodes(DiagramModel model, List<List<Node>> layers)
    {
        // For now, skip dummy nodes for simplicity
        // In a full implementation, we'd add dummy nodes for edges spanning multiple layers
    }
    
    private void ReduceCrossings(DiagramModel model, List<List<Node>> layers)
    {
        // Barycenter heuristic for crossing reduction
        for (int iter = 0; iter < CrossingReductionIterations; iter++)
        {
            // Forward pass
            for (int i = 1; i < layers.Count; i++)
            {
                OrderLayerByBarycenter(model, layers[i], layers[i - 1], true);
            }
            
            // Backward pass
            for (int i = layers.Count - 2; i >= 0; i--)
            {
                OrderLayerByBarycenter(model, layers[i], layers[i + 1], false);
            }
        }
    }
    
    private void OrderLayerByBarycenter(DiagramModel model, List<Node> currentLayer, 
        List<Node> referenceLayer, bool useIncoming)
    {
        var barycenters = new Dictionary<Node, double>();
        
        foreach (var node in currentLayer)
        {
            var connectedNodes = useIncoming
                ? model.Links.Where(l => l.ToNode == node && l.FromNode != null)
                           .Select(l => l.FromNode!)
                : model.Links.Where(l => l.FromNode == node && l.ToNode != null)
                           .Select(l => l.ToNode!);
            
            var indices = connectedNodes
                .Select(n => referenceLayer.IndexOf(n))
                .Where(idx => idx >= 0)
                .ToList();
            
            barycenters[node] = indices.Count > 0 ? indices.Average() : referenceLayer.Count / 2.0;
        }
        
        // Sort layer by barycenter
        currentLayer.Sort((a, b) => barycenters[a].CompareTo(barycenters[b]));
    }
    
    private void AssignCoordinates(DiagramModel model, List<List<Node>> layers)
    {
        double currentY = 0;
        
        for (int i = 0; i < layers.Count; i++)
        {
            var layer = layers[i];
            if (layer.Count == 0) continue;
            
            // Calculate total width of layer
            var totalWidth = layer.Sum(n => n.Size.Width) + (layer.Count - 1) * NodeSpacing;
            var currentX = -totalWidth / 2; // Center the layer
            
            // Position each node
            foreach (var node in layer)
            {
                var pos = Direction switch
                {
                    LayoutDirection.Down => new Point(currentX + node.Size.Width / 2, currentY),
                    LayoutDirection.Up => new Point(currentX + node.Size.Width / 2, -currentY),
                    LayoutDirection.Right => new Point(currentY, currentX + node.Size.Width / 2),
                    LayoutDirection.Left => new Point(-currentY, currentX + node.Size.Width / 2),
                    _ => new Point(currentX, currentY)
                };
                
                node.Position = pos;
                currentX += node.Size.Width + NodeSpacing;
            }
            
            // Move to next layer
            var maxHeight = layer.Max(n => n.Size.Height);
            currentY += maxHeight + LayerSpacing;
        }
        
        // Center the entire diagram
        if (layers.Count > 0 && layers[0].Count > 0)
        {
            var bounds = model.GetBoundingBox();
            var offset = new Point(-bounds.Left, -bounds.Top);
            
            foreach (var node in model.Nodes)
            {
                node.Position = node.Position + offset;
            }
        }
    }
    
    private void RestoreReversedLinks(List<Link> reversedLinks)
    {
        foreach (var link in reversedLinks)
        {
            var temp = link.FromNode;
            link.FromNode = link.ToNode;
            link.ToNode = temp;
        }
    }
}

/// <summary>
/// Layout direction options
/// </summary>
public enum LayoutDirection
{
    /// <summary>
    /// Top to bottom
    /// </summary>
    Down,
    
    /// <summary>
    /// Bottom to top
    /// </summary>
    Up,
    
    /// <summary>
    /// Left to right
    /// </summary>
    Right,
    
    /// <summary>
    /// Right to left
    /// </summary>
    Left
}

