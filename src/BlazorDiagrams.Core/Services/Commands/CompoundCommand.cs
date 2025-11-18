namespace BlazorDiagrams.Core.Services.Commands;

/// <summary>
/// Command that contains multiple sub-commands
/// </summary>
public class CompoundCommand : ICommand
{
    private readonly List<ICommand> _commands = new();
    private readonly string _description;
    
    public CompoundCommand(string description, IEnumerable<ICommand> commands)
    {
        _description = description;
        _commands.AddRange(commands);
    }
    
    public CompoundCommand(string description)
    {
        _description = description;
    }
    
    public string Description => _description;
    
    public void AddCommand(ICommand command)
    {
        _commands.Add(command);
    }
    
    public void Execute()
    {
        foreach (var command in _commands)
        {
            command.Execute();
        }
    }
    
    public void Undo()
    {
        // Undo in reverse order
        for (int i = _commands.Count - 1; i >= 0; i--)
        {
            _commands[i].Undo();
        }
    }
    
    public bool CanMergeWith(ICommand other) => false;
    
    public void MergeWith(ICommand other)
    {
        // No merging for compound commands
    }
}

