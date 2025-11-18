using BlazorDiagrams.Core.Geometry;
using BlazorDiagrams.Core.Models;

namespace BlazorDiagrams.Core.Services.Commands;

/// <summary>
/// Command for moving a node
/// </summary>
public class MoveNodeCommand : ICommand
{
    private readonly Node _node;
    private readonly Point _oldPosition;
    private readonly Point _newPosition;
    
    public MoveNodeCommand(Node node, Point oldPosition, Point newPosition)
    {
        _node = node;
        _oldPosition = oldPosition;
        _newPosition = newPosition;
    }
    
    public string Description => $"Move {_node.Id}";
    
    public void Execute()
    {
        _node.Position = _newPosition;
    }
    
    public void Undo()
    {
        _node.Position = _oldPosition;
    }
    
    public bool CanMergeWith(ICommand other)
    {
        return other is MoveNodeCommand moveCmd && moveCmd._node == _node;
    }
    
    public void MergeWith(ICommand other)
    {
        // The new position is already stored in this command
        // We keep the old position from the first command
    }
}

