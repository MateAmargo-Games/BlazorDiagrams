using Microsoft.AspNetCore.Components;

namespace BlazorDiagrams.Core.Models;

/// <summary>
/// Represents an interactive action that can be displayed on a node
/// </summary>
public class NodeAction
{
    /// <summary>
    /// Unique identifier for this action
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    /// <summary>
    /// Display text for the action
    /// </summary>
    public string? Label { get; set; }
    
    /// <summary>
    /// Icon to display (CSS class, emoji, or SVG path)
    /// </summary>
    public string? Icon { get; set; }
    
    /// <summary>
    /// Icon type (css, emoji, svg)
    /// </summary>
    public NodeActionIconType IconType { get; set; } = NodeActionIconType.Emoji;
    
    /// <summary>
    /// Tooltip text to show on hover
    /// </summary>
    public string? Tooltip { get; set; }
    
    /// <summary>
    /// Position of the action relative to the node
    /// </summary>
    public NodeActionPosition Position { get; set; } = NodeActionPosition.TopRight;
    
    /// <summary>
    /// Visibility mode for the action
    /// </summary>
    public NodeActionVisibility Visibility { get; set; } = NodeActionVisibility.OnHover;
    
    /// <summary>
    /// Whether the action is enabled
    /// </summary>
    public bool IsEnabled { get; set; } = true;
    
    /// <summary>
    /// Background color of the action button
    /// </summary>
    public string? BackgroundColor { get; set; }
    
    /// <summary>
    /// Foreground color of the action button
    /// </summary>
    public string? ForegroundColor { get; set; }
    
    /// <summary>
    /// Custom CSS class for styling
    /// </summary>
    public string? CssClass { get; set; }
    
    /// <summary>
    /// Custom render fragment for complete customization
    /// </summary>
    public RenderFragment<NodeAction>? CustomContent { get; set; }
    
    /// <summary>
    /// Callback invoked when the action is clicked
    /// </summary>
    public Action<Node>? OnClick { get; set; }
    
    /// <summary>
    /// Function to determine if the action should be visible
    /// </summary>
    public Func<Node, bool>? ShouldDisplay { get; set; }
    
    /// <summary>
    /// Order for rendering multiple actions (lower values render first)
    /// </summary>
    public int Order { get; set; } = 0;
}

/// <summary>
/// Position of the action relative to the node
/// </summary>
public enum NodeActionPosition
{
    /// <summary>
    /// Top left corner
    /// </summary>
    TopLeft,
    
    /// <summary>
    /// Top center
    /// </summary>
    TopCenter,
    
    /// <summary>
    /// Top right corner
    /// </summary>
    TopRight,
    
    /// <summary>
    /// Middle left
    /// </summary>
    MiddleLeft,
    
    /// <summary>
    /// Center
    /// </summary>
    Center,
    
    /// <summary>
    /// Middle right
    /// </summary>
    MiddleRight,
    
    /// <summary>
    /// Bottom left corner
    /// </summary>
    BottomLeft,
    
    /// <summary>
    /// Bottom center
    /// </summary>
    BottomCenter,
    
    /// <summary>
    /// Bottom right corner
    /// </summary>
    BottomRight,
    
    /// <summary>
    /// Custom position
    /// </summary>
    Custom
}

/// <summary>
/// Visibility mode for node actions
/// </summary>
public enum NodeActionVisibility
{
    /// <summary>
    /// Always visible
    /// </summary>
    Always,
    
    /// <summary>
    /// Visible only when node is hovered
    /// </summary>
    OnHover,
    
    /// <summary>
    /// Visible only when node is selected
    /// </summary>
    OnSelected,
    
    /// <summary>
    /// Visible when node is hovered or selected
    /// </summary>
    OnHoverOrSelected
}

/// <summary>
/// Type of icon for the action
/// </summary>
public enum NodeActionIconType
{
    /// <summary>
    /// CSS class (e.g., "fas fa-edit")
    /// </summary>
    Css,
    
    /// <summary>
    /// Emoji character (e.g., "✏️")
    /// </summary>
    Emoji,
    
    /// <summary>
    /// SVG path or icon
    /// </summary>
    Svg
}

