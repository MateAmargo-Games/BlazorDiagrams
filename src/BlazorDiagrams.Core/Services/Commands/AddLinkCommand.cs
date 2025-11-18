using BlazorDiagrams.Core.Models;

namespace BlazorDiagrams.Core.Services.Commands;

/// <summary>
/// Command for adding a link to the diagram
/// </summary>
public class AddLinkCommand : ICommand
{
    private readonly DiagramModel _model;
    private readonly Link _link;
    
    public AddLinkCommand(DiagramModel model, Link link)
    {
        _model = model;
        _link = link;
    }
    
    public string Description => $"Add link {_link.Id}";
    
    public void Execute()
    {
        _model.AddLink(_link);
    }
    
    public void Undo()
    {
        _model.RemoveLink(_link);
    }
    
    public bool CanMergeWith(ICommand other) => false;
    
    public void MergeWith(ICommand other)
    {
        // No merging for add commands
    }
}

