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
    
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveNodeCommand"/> class.
    /// </summary>
    public RemoveNodeCommand(DiagramModel model, Node node)
    {
        _model = model;
        _node = node;
        
        // Store links that will be removed
        _removedLinks = model.Links
            .Where(l => l.FromNode == node || l.ToNode == node)
            .ToList();
    }
    
    /// <summary>
    /// Gets the description of the command
    /// </summary>
    public string Description => $"Remove node {_node.Id}";
    
    /// <summary>
    /// Executes the command
    /// </summary>
    public void Execute()
    {
        _model.RemoveNode(_node);
    }
    
    /// <summary>
    /// Undoes the command
    /// </summary>
    public void Undo()
    {
        _model.AddNode(_node);
        
        // Restore removed links
        foreach (var link in _removedLinks)
        {
            _model.AddLink(link);
        }
    }
    
    /// <summary>
    /// Checks if this command can be merged with another command
    /// </summary>
    public bool CanMergeWith(ICommand other) => false;
    
    /// <summary>
    /// Merges this command with another command
    /// </summary>
    public void MergeWith(ICommand other)
    {
        // No merging for remove commands
    }
}

