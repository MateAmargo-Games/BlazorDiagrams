using BlazorDiagrams.Core.Models;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorDiagrams.Core.Interaction;

/// <summary>
/// Handles keyboard events and shortcuts for the diagram
/// </summary>
public class KeyboardHandler
{
    private readonly DiagramModel _model;
    private readonly Dictionary<string, Action> _shortcuts = new();
    
    /// <summary>
    /// Initializes a new instance of the <see cref="KeyboardHandler"/> class.
    /// </summary>
    public KeyboardHandler(DiagramModel model)
    {
        _model = model;
        RegisterDefaultShortcuts();
    }
    
    /// <summary>
    /// Registers a keyboard shortcut
    /// </summary>
    public void RegisterShortcut(string key, bool ctrl, bool shift, bool alt, Action action)
    {
        var shortcutKey = GetShortcutKey(key, ctrl, shift, alt);
        _shortcuts[shortcutKey] = action;
    }
    
    /// <summary>
    /// Handles a keyboard event
    /// </summary>
    public bool HandleKeyDown(KeyboardEventArgs e)
    {
        var shortcutKey = GetShortcutKey(e.Key, e.CtrlKey, e.ShiftKey, e.AltKey);
        
        if (_shortcuts.TryGetValue(shortcutKey, out var action))
        {
            action.Invoke();
            return true; // Event handled
        }
        
        return false; // Event not handled
    }
    
    private void RegisterDefaultShortcuts()
    {
        // Select All (Ctrl+A)
        RegisterShortcut("a", true, false, false, () =>
        {
            foreach (var node in _model.Nodes)
            {
                node.IsSelected = true;
            }
        });
        
        // Delete (Delete key)
        RegisterShortcut("Delete", false, false, false, () =>
        {
            var selectedNodes = _model.GetSelectedNodes().ToList();
            foreach (var node in selectedNodes)
            {
                _model.RemoveNode(node);
            }
        });
        
        // Escape (clear selection)
        RegisterShortcut("Escape", false, false, false, () =>
        {
            _model.ClearSelection();
        });
        
        // Ctrl+Z (Undo) - placeholder, needs UndoManager integration
        RegisterShortcut("z", true, false, false, () =>
        {
            // Undo functionality would be integrated with UndoManager
        });
        
        // Ctrl+Y or Ctrl+Shift+Z (Redo)
        RegisterShortcut("y", true, false, false, () =>
        {
            // Redo functionality would be integrated with UndoManager
        });
        
        // Ctrl+C (Copy) - placeholder
        RegisterShortcut("c", true, false, false, () =>
        {
            // Copy functionality
        });
        
        // Ctrl+V (Paste) - placeholder
        RegisterShortcut("v", true, false, false, () =>
        {
            // Paste functionality
        });
        
        // Ctrl+X (Cut) - placeholder
        RegisterShortcut("x", true, false, false, () =>
        {
            // Cut functionality
        });
    }
    
    private string GetShortcutKey(string key, bool ctrl, bool shift, bool alt)
    {
        var modifiers = new List<string>();
        if (ctrl) modifiers.Add("Ctrl");
        if (shift) modifiers.Add("Shift");
        if (alt) modifiers.Add("Alt");
        
        modifiers.Add(key);
        
        return string.Join("+", modifiers);
    }
}

