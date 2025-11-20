using BlazorDiagrams.Core.Models;

namespace BlazorDiagrams.Core.Services.Commands;

/// <summary>
/// Command for adding a link to the diagram
/// </summary>
public class AddLinkCommand : ICommand
{
    private readonly DiagramModel _model;
    private readonly Link _link;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AddLinkCommand"/> class.
    /// </summary>
    public AddLinkCommand(DiagramModel model, Link link)
    {
        _model = model;
        _link = link;
    }
    
    /// <summary>
    /// Gets the description of the command
    /// </summary>
    public string Description => $"Add link {_link.Id}";
    
    /// <summary>
    /// Executes the command
    /// </summary>
    public void Execute()
    {
        _model.AddLink(_link);
    }
    
    /// <summary>
    /// Undoes the command
    /// </summary>
    public void Undo()
    {
        _model.RemoveLink(_link);
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

