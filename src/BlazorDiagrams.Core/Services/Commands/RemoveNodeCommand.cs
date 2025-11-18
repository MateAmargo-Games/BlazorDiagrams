using BlazorDiagrams.Core.Models;

namespace BlazorDiagrams.Core.Services.Commands;

/// <summary>
/// Command for removing a node from the diagram
/// </summary>
public class RemoveNodeCommand : ICommand
{
    private readonly DiagramModel _model;
    private readonly Node _node;
    private readonly List<Link> _removedLinks = new();
    
    public RemoveNodeCommand(DiagramModel model, Node node)
    {
        _model = model;
        _node = node;
        
        // Store links that will be removed
        _removedLinks = model.Links
            .Where(l => l.FromNode == node || l.ToNode == node)
            .ToList();
    }
    
    public string Description => $"Remove node {_node.Id}";
    
    public void Execute()
    {
        _model.RemoveNode(_node);
    }
    
    public void Undo()
    {
        _model.AddNode(_node);
        
        // Restore removed links
        foreach (var link in _removedLinks)
        {
            _model.AddLink(link);
        }
    }
    
    public bool CanMergeWith(ICommand other) => false;
    
    public void MergeWith(ICommand other)
    {
        // No merging for remove commands
    }
}

