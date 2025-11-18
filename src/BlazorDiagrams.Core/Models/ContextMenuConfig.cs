using Microsoft.AspNetCore.Components;
using BlazorDiagrams.Core.Interaction;

namespace BlazorDiagrams.Core.Models;

/// <summary>
/// Configuration for context menu appearance and behavior
/// </summary>
public class ContextMenuConfig
{
    /// <summary>
    /// Theme for the context menu (light, dark, auto)
    /// </summary>
    public ContextMenuTheme Theme { get; set; } = ContextMenuTheme.Light;
    
    /// <summary>
    /// Width of the context menu (null for auto)
    /// </summary>
    public int? Width { get; set; }
    
    /// <summary>
    /// Maximum height of the context menu before scrolling
    /// </summary>
    public int MaxHeight { get; set; } = 400;
    
    /// <summary>
    /// Custom CSS class for the menu
    /// </summary>
    public string? CssClass { get; set; }
    
    /// <summary>
    /// Whether to close the menu when clicking outside
    /// </summary>
    public bool CloseOnClickOutside { get; set; } = true;
    
    /// <summary>
    /// Whether to close the menu after clicking an item
    /// </summary>
    public bool CloseOnItemClick { get; set; } = true;
    
    /// <summary>
    /// Animation duration in milliseconds
    /// </summary>
    public int AnimationDuration { get; set; } = 150;
    
    /// <summary>
    /// Menu items provider for nodes
    /// </summary>
    public Func<Node, List<ContextMenuItem>>? NodeMenuItems { get; set; }
    
    /// <summary>
    /// Menu items provider for links
    /// </summary>
    public Func<Link, List<ContextMenuItem>>? LinkMenuItems { get; set; }
    
    /// <summary>
    /// Menu items provider for canvas (empty space)
    /// </summary>
    public Func<DiagramModel, List<ContextMenuItem>>? CanvasMenuItems { get; set; }
    
    /// <summary>
    /// Custom render fragment for menu items
    /// </summary>
    public RenderFragment<ContextMenuItem>? CustomItemTemplate { get; set; }
}

/// <summary>
/// Theme for context menu
/// </summary>
public enum ContextMenuTheme
{
    Light,
    Dark,
    Auto
}

/// <summary>
/// Context menu target type
/// </summary>
public enum ContextMenuTarget
{
    Node,
    Link,
    Canvas,
    Group
}

