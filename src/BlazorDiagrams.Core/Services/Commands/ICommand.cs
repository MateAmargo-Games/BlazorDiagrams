namespace BlazorDiagrams.Core.Services.Commands;

/// <summary>
/// Interface for undoable/redoable commands
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Executes the command
    /// </summary>
    void Execute();
    
    /// <summary>
    /// Undoes the command
    /// </summary>
    void Undo();
    
    /// <summary>
    /// Description of the command for display purposes
    /// </summary>
    string Description { get; }
    
    /// <summary>
    /// Whether this command can be merged with another command
    /// </summary>
    bool CanMergeWith(ICommand other);
    
    /// <summary>
    /// Merges this command with another command
    /// </summary>
    void MergeWith(ICommand other);
}

