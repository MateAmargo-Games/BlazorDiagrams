using BlazorDiagrams.Core.Models;

namespace BlazorDiagrams.Core.Services.Commands;

/// <summary>
/// Command for adding a node to the diagram
/// </summary>
public class AddNodeCommand : ICommand
{
    private readonly DiagramModel _model;
    private readonly Node _node;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AddNodeCommand"/> class.
    /// </summary>
    public AddNodeCommand(DiagramModel model, Node node)
    {
        _model = model;
        _node = node;
    }
    
    /// <summary>
    /// Gets the description of the command
    /// </summary>
    public string Description => $"Add node {_node.Id}";
    
    /// <summary>
    /// Executes the command
    /// </summary>
    public void Execute()
    {
        _model.AddNode(_node);
    }
    
    /// <summary>
    /// Undoes the command
    /// </summary>
    public void Undo()
    {
        _model.RemoveNode(_node);
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
        // No merging for add commands
    }
}

