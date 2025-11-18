using BlazorDiagrams.Core.Geometry;
using BlazorDiagrams.Core.Models;

namespace BlazorDiagrams.Core.Interaction;

/// <summary>
/// Service for managing context menus and tooltips
/// </summary>
public class ContextMenuService
{
    public event EventHandler<ContextMenuEventArgs>? ContextMenuRequested;
    public event EventHandler? ContextMenuClosed;
    
    /// <summary>
    /// Shows a context menu at the specified position
    /// </summary>
    public void ShowContextMenu(Point position, object? target = null)
    {
        ContextMenuRequested?.Invoke(this, new ContextMenuEventArgs
        {
            Position = position,
            Target = target
        });
    }
    
    /// <summary>
    /// Closes the current context menu
    /// </summary>
    public void CloseContextMenu()
    {
        ContextMenuClosed?.Invoke(this, EventArgs.Empty);
    }
}

/// <summary>
/// Event args for context menu requests
/// </summary>
public class ContextMenuEventArgs : EventArgs
{
    /// <summary>
    /// Position where the context menu should appear
    /// </summary>
    public Point Position { get; set; }
    
    /// <summary>
    /// Target object (Node, Link, etc.) if any
    /// </summary>
    public object? Target { get; set; }
}

/// <summary>
/// Represents a context menu item
/// </summary>
public class ContextMenuItem
{
    /// <summary>
    /// Label of the menu item
    /// </summary>
    public string Label { get; set; } = "";
    
    /// <summary>
    /// Action to execute when clicked
    /// </summary>
    public Action<object?>? Action { get; set; }
    
    /// <summary>
    /// Whether this item is enabled
    /// </summary>
    public bool IsEnabled { get; set; } = true;
    
    /// <summary>
    /// Whether this is a separator
    /// </summary>
    public bool IsSeparator { get; set; }
    
    /// <summary>
    /// Icon CSS class or emoji
    /// </summary>
    public string? Icon { get; set; }
    
    /// <summary>
    /// Submenu items
    /// </summary>
    public List<ContextMenuItem>? SubItems { get; set; }
}

/// <summary>
/// Service for managing tooltips
/// </summary>
public class TooltipService
{
    public event EventHandler<TooltipEventArgs>? TooltipRequested;
    public event EventHandler? TooltipClosed;
    
    private System.Timers.Timer? _showTimer;
    private System.Timers.Timer? _hideTimer;
    
    /// <summary>
    /// Delay before showing tooltip (milliseconds)
    /// </summary>
    public int ShowDelay { get; set; } = 500;
    
    /// <summary>
    /// Duration to show tooltip (milliseconds, 0 = indefinite)
    /// </summary>
    public int HideDuration { get; set; } = 0;
    
    /// <summary>
    /// Shows a tooltip at the specified position
    /// </summary>
    public void ShowTooltip(Point position, string content, object? target = null)
    {
        _showTimer?.Stop();
        _hideTimer?.Stop();
        
        _showTimer = new System.Timers.Timer(ShowDelay);
        _showTimer.Elapsed += (s, e) =>
        {
            _showTimer?.Stop();
            TooltipRequested?.Invoke(this, new TooltipEventArgs
            {
                Position = position,
                Content = content,
                Target = target
            });
            
            if (HideDuration > 0)
            {
                _hideTimer = new System.Timers.Timer(HideDuration);
                _hideTimer.Elapsed += (s2, e2) =>
                {
                    _hideTimer?.Stop();
                    CloseTooltip();
                };
                _hideTimer.Start();
            }
        };
        _showTimer.Start();
    }
    
    /// <summary>
    /// Closes the current tooltip
    /// </summary>
    public void CloseTooltip()
    {
        _showTimer?.Stop();
        _hideTimer?.Stop();
        TooltipClosed?.Invoke(this, EventArgs.Empty);
    }
}

/// <summary>
/// Event args for tooltip requests
/// </summary>
public class TooltipEventArgs : EventArgs
{
    /// <summary>
    /// Position where the tooltip should appear
    /// </summary>
    public Point Position { get; set; }
    
    /// <summary>
    /// Content to display in the tooltip
    /// </summary>
    public string Content { get; set; } = "";
    
    /// <summary>
    /// Target object if any
    /// </summary>
    public object? Target { get; set; }
}

