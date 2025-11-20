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
    
    /// <summary>
    /// Initializes a new instance of the <see cref="MoveNodeCommand"/> class.
    /// </summary>
    public MoveNodeCommand(Node node, Point oldPosition, Point newPosition)
    {
        _node = node;
        _oldPosition = oldPosition;
        _newPosition = newPosition;
    }
    
    /// <summary>
    /// Gets the description of the command
    /// </summary>
    public string Description => $"Move {_node.Id}";
    
    /// <summary>
    /// Executes the command
    /// </summary>
    public void Execute()
    {
        _node.Position = _newPosition;
    }
    
    /// <summary>
    /// Undoes the command
    /// </summary>
    public void Undo()
    {
        _node.Position = _oldPosition;
    }
    
    /// <summary>
    /// Checks if this command can be merged with another command
    /// </summary>
    public bool CanMergeWith(ICommand other)
    {
        return other is MoveNodeCommand moveCmd && moveCmd._node == _node;
    }
    
    /// <summary>
    /// Merges this command with another command
    /// </summary>
    public void MergeWith(ICommand other)
    {
        // The new position is already stored in this command
        // We keep the old position from the first command
    }
}

