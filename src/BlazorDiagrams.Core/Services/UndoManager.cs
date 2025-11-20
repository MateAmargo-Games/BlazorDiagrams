using BlazorDiagrams.Core.Services.Commands;

namespace BlazorDiagrams.Core.Services;

/// <summary>
/// Manages undo/redo functionality for diagram operations
/// </summary>
public class UndoManager
{
    private readonly Stack<ICommand> _undoStack = new();
    private readonly Stack<ICommand> _redoStack = new();
    private int _maxHistorySize = 100;
    private bool _isExecuting;
    
    /// <summary>
    /// Maximum number of commands to keep in history
    /// </summary>
    public int MaxHistorySize
    {
        get => _maxHistorySize;
        set => _maxHistorySize = Math.Max(1, value);
    }
    
    /// <summary>
    /// Whether there are commands that can be undone
    /// </summary>
    public bool CanUndo => _undoStack.Count > 0;
    
    /// <summary>
    /// Whether there are commands that can be redone
    /// </summary>
    public bool CanRedo => _redoStack.Count > 0;
    
    /// <summary>
    /// Number of commands in undo history
    /// </summary>
    public int UndoCount => _undoStack.Count;
    
    /// <summary>
    /// Number of commands in redo history
    /// </summary>
    public int RedoCount => _redoStack.Count;
    
    /// <summary>
    /// Event raised when the undo/redo state changes
    /// </summary>
    public event EventHandler? StateChanged;
    
    /// <summary>
    /// Executes a command and adds it to the undo stack
    /// </summary>
    public void ExecuteCommand(ICommand command)
    {
        if (_isExecuting)
            return;
        
        _isExecuting = true;
        
        try
        {
            // Try to merge with the last command
            if (_undoStack.Count > 0)
            {
                var lastCommand = _undoStack.Peek();
                if (lastCommand.CanMergeWith(command))
                {
                    lastCommand.MergeWith(command);
                    command.Execute();
                    OnStateChanged();
                    return;
                }
            }
            
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear(); // Clear redo stack when new command is executed
            
            // Limit history size
            if (_undoStack.Count > MaxHistorySize)
            {
                var list = _undoStack.ToList();
                list.RemoveAt(list.Count - 1);
                _undoStack.Clear();
                foreach (var cmd in Enumerable.Reverse(list))
                {
                    _undoStack.Push(cmd);
                }
            }
            
            OnStateChanged();
        }
        finally
        {
            _isExecuting = false;
        }
    }
    
    /// <summary>
    /// Undoes the last command
    /// </summary>
    public void Undo()
    {
        if (!CanUndo || _isExecuting)
            return;
        
        _isExecuting = true;
        
        try
        {
            var command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);
            OnStateChanged();
        }
        finally
        {
            _isExecuting = false;
        }
    }
    
    /// <summary>
    /// Redoes the last undone command
    /// </summary>
    public void Redo()
    {
        if (!CanRedo || _isExecuting)
            return;
        
        _isExecuting = true;
        
        try
        {
            var command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);
            OnStateChanged();
        }
        finally
        {
            _isExecuting = false;
        }
    }
    
    /// <summary>
    /// Clears all undo/redo history
    /// </summary>
    public void Clear()
    {
        _undoStack.Clear();
        _redoStack.Clear();
        OnStateChanged();
    }
    
    /// <summary>
    /// Gets the description of the next undo command
    /// </summary>
    public string? GetUndoDescription()
    {
        return CanUndo ? _undoStack.Peek().Description : null;
    }
    
    /// <summary>
    /// Gets the description of the next redo command
    /// </summary>
    public string? GetRedoDescription()
    {
        return CanRedo ? _redoStack.Peek().Description : null;
    }
    
    /// <summary>
    /// Begins a transaction for grouping multiple commands
    /// </summary>
    public UndoTransaction BeginTransaction(string description)
    {
        return new UndoTransaction(this, description);
    }
    
    /// <summary>
    /// Raises the <see cref="StateChanged"/> event
    /// </summary>
    protected virtual void OnStateChanged()
    {
        StateChanged?.Invoke(this, EventArgs.Empty);
    }
}

/// <summary>
/// Transaction for grouping multiple commands into one undoable operation
/// </summary>
public class UndoTransaction : IDisposable
{
    private readonly UndoManager _manager;
    private readonly CompoundCommand _command;
    private bool _committed;
    
    internal UndoTransaction(UndoManager manager, string description)
    {
        _manager = manager;
        _command = new CompoundCommand(description);
    }
    
    /// <summary>
    /// Adds a command to this transaction
    /// </summary>
    public void AddCommand(ICommand command)
    {
        if (_committed)
            throw new InvalidOperationException("Cannot add commands to a committed transaction");
        
        _command.AddCommand(command);
        command.Execute();
    }
    
    /// <summary>
    /// Commits the transaction
    /// </summary>
    public void Commit()
    {
        if (!_committed)
        {
            _committed = true;
            if (_command != null)
            {
                // Note: Don't execute here, commands were already executed in AddCommand
                // Just add to undo stack
                _manager.ExecuteCommand(_command);
            }
        }
    }
    
    /// <summary>
    /// Disposes the transaction, committing it if not already committed
    /// </summary>
    public void Dispose()
    {
        if (!_committed)
        {
            // Auto-commit on dispose
            Commit();
        }
    }
}

