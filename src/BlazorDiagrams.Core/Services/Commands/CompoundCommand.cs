namespace BlazorDiagrams.Core.Services.Commands;

/// <summary>
/// Command that contains multiple sub-commands
/// </summary>
public class CompoundCommand : ICommand
{
    private readonly List<ICommand> _commands = new();
    private readonly string _description;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CompoundCommand"/> class with a list of commands.
    /// </summary>
    public CompoundCommand(string description, IEnumerable<ICommand> commands)
    {
        _description = description;
        _commands.AddRange(commands);
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CompoundCommand"/> class.
    /// </summary>
    public CompoundCommand(string description)
    {
        _description = description;
    }
    
    /// <summary>
    /// Gets the description of the command
    /// </summary>
    public string Description => _description;
    
    /// <summary>
    /// Adds a command to the compound command
    /// </summary>
    public void AddCommand(ICommand command)
    {
        _commands.Add(command);
    }
    
    /// <summary>
    /// Executes all sub-commands
    /// </summary>
    public void Execute()
    {
        foreach (var command in _commands)
        {
            command.Execute();
        }
    }
    
    /// <summary>
    /// Undoes all sub-commands in reverse order
    /// </summary>
    public void Undo()
    {
        // Undo in reverse order
        for (int i = _commands.Count - 1; i >= 0; i--)
        {
            _commands[i].Undo();
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
        // No merging for compound commands
    }
}

