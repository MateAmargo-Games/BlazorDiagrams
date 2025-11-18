using BlazorDiagrams.Core.Geometry;
using BlazorDiagrams.Core.Models;

namespace BlazorDiagrams.Core.Layouts;

/// <summary>
/// Force-directed layout using spring-electrical model
/// </summary>
public class ForceDirectedLayout : ILayout
{
    /// <summary>
    /// Number of iterations to run the simulation
    /// </summary>
    public int Iterations { get; set; } = 100;
    
    /// <summary>
    /// Spring constant for attractive forces
    /// </summary>
    public double SpringConstant { get; set; } = 0.5;
    
    /// <summary>
    /// Ideal spring length (distance between connected nodes)
    /// </summary>
    public double SpringLength { get; set; } = 100;
    
    /// <summary>
    /// Repulsion constant for repulsive forces
    /// </summary>
    public double RepulsionConstant { get; set; } = 5000;
    
    /// <summary>
    /// Cooling factor (reduces movement over time)
    /// </summary>
    public double CoolingFactor { get; set; } = 0.95;
    
    /// <summary>
    /// Initial temperature (maximum displacement per iteration)
    /// </summary>
    public double InitialTemperature { get; set; } = 100;
    
    /// <summary>
    /// Whether to randomize initial positions
    /// </summary>
    public bool RandomizeInitialPositions { get; set; } = true;
    
    private Random _random = new Random();
    
    public void Apply(DiagramModel model)
    {
        if (model.Nodes.Count == 0)
            return;
        
        // Initialize positions if needed
        if (RandomizeInitialPositions)
        {
            InitializeRandomPositions(model);
        }
        
        // Run force-directed simulation
        var temperature = InitialTemperature;
        
        for (int iter = 0; iter < Iterations; iter++)
        {
            var forces = CalculateForces(model);
            ApplyForces(model, forces, temperature);
            temperature *= CoolingFactor;
        }
        
        // Center the diagram
        CenterDiagram(model);
    }
    
    public void ApplyToGroup(Group group)
    {
        // Similar logic for groups
    }
    
    private void InitializeRandomPositions(DiagramModel model)
    {
        var spread = 500.0;
        
        foreach (var node in model.Nodes)
        {
            node.Position = new Point(
                (_random.NextDouble() - 0.5) * spread,
                (_random.NextDouble() - 0.5) * spread
            );
        }
    }
    
    private Dictionary<Node, Point> CalculateForces(DiagramModel model)
    {
        var forces = new Dictionary<Node, Point>();
        
        // Initialize forces to zero
        foreach (var node in model.Nodes)
        {
            forces[node] = Point.Zero;
        }
        
        // Calculate repulsive forces between all pairs of nodes
        for (int i = 0; i < model.Nodes.Count; i++)
        {
            var node1 = model.Nodes[i];
            
            for (int j = i + 1; j < model.Nodes.Count; j++)
            {
                var node2 = model.Nodes[j];
                
                var delta = node2.Position - node1.Position;
                var distance = Math.Max(delta.Magnitude(), 0.01); // Avoid division by zero
                
                // Coulomb's law: F = k / d^2
                var repulsionForce = RepulsionConstant / (distance * distance);
                var forceVector = delta.Normalize() * repulsionForce;
                
                forces[node1] = forces[node1] - forceVector;
                forces[node2] = forces[node2] + forceVector;
            }
        }
        
        // Calculate attractive forces for connected nodes
        foreach (var link in model.Links)
        {
            if (link.FromNode == null || link.ToNode == null)
                continue;
            
            var node1 = link.FromNode;
            var node2 = link.ToNode;
            
            var delta = node2.Position - node1.Position;
            var distance = Math.Max(delta.Magnitude(), 0.01);
            
            // Hooke's law: F = k * (d - l0)
            var displacement = distance - SpringLength;
            var springForce = SpringConstant * displacement;
            var forceVector = delta.Normalize() * springForce;
            
            forces[node1] = forces[node1] + forceVector;
            forces[node2] = forces[node2] - forceVector;
        }
        
        return forces;
    }
    
    private void ApplyForces(DiagramModel model, Dictionary<Node, Point> forces, double temperature)
    {
        foreach (var node in model.Nodes)
        {
            if (node.IsLocked)
                continue;
            
            var force = forces[node];
            var displacement = force.Magnitude();
            
            // Limit displacement by temperature
            if (displacement > temperature)
            {
                force = force.Normalize() * temperature;
            }
            
            node.Position = node.Position + force;
        }
    }
    
    private void CenterDiagram(DiagramModel model)
    {
        if (model.Nodes.Count == 0)
            return;
        
        var bounds = model.GetBoundingBox();
        var center = bounds.Center;
        
        // Move all nodes so the center is at origin
        foreach (var node in model.Nodes)
        {
            node.Position = new Point(
                node.Position.X - center.X,
                node.Position.Y - center.Y
            );
        }
    }
}

