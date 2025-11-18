using BlazorDiagrams.Core.Models;

namespace BlazorDiagrams.Core.Services.Commands;

/// <summary>
/// Command for adding a node to the diagram
/// </summary>
public class AddNodeCommand : ICommand
{
    private readonly DiagramModel _model;
    private readonly Node _node;
    
    public AddNodeCommand(DiagramModel model, Node node)
    {
        _model = model;
        _node = node;
    }
    
    public string Description => $"Add node {_node.Id}";
    
    public void Execute()
    {
        _model.AddNode(_node);
    }
    
    public void Undo()
    {
        _model.RemoveNode(_node);
    }
    
    public bool CanMergeWith(ICommand other) => false;
    
    public void MergeWith(ICommand other)
    {
        // No merging for add commands
    }
}

