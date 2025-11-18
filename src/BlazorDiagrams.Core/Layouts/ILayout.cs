using BlazorDiagrams.Core.Models;

namespace BlazorDiagrams.Core.Layouts;

/// <summary>
/// Interface for layout algorithms that position nodes in the diagram
/// </summary>
public interface ILayout
{
    /// <summary>
    /// Applies the layout algorithm to the diagram
    /// </summary>
    /// <param name="model">The diagram model to layout</param>
    void Apply(DiagramModel model);
    
    /// <summary>
    /// Applies the layout algorithm to a specific group
    /// </summary>
    /// <param name="group">The group to layout</param>
    void ApplyToGroup(Group group);
}

