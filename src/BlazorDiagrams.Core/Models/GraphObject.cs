using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BlazorDiagrams.Core.Models;

/// <summary>
/// Base class for all visual elements in the diagram (nodes, links, ports, groups, etc.)
/// </summary>
public abstract class GraphObject : INotifyPropertyChanged
{
    private string _id = Guid.NewGuid().ToString();
    private bool _isSelected;
    private bool _isVisible = true;
    private bool _isLocked;
    private string? _category;
    private object? _data;
    private double _opacity = 1.0;
    
    /// <summary>
    /// Unique identifier for this graph object
    /// </summary>
    public string Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }
    
    /// <summary>
    /// Whether this object is currently selected
    /// </summary>
    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }
    
    /// <summary>
    /// Whether this object is visible in the diagram
    /// </summary>
    public bool IsVisible
    {
        get => _isVisible;
        set => SetProperty(ref _isVisible, value);
    }
    
    /// <summary>
    /// Whether this object is locked (cannot be moved or edited)
    /// </summary>
    public bool IsLocked
    {
        get => _isLocked;
        set => SetProperty(ref _isLocked, value);
    }
    
    /// <summary>
    /// Optional category for grouping and styling
    /// </summary>
    public string? Category
    {
        get => _category;
        set => SetProperty(ref _category, value);
    }
    
    /// <summary>
    /// Custom data associated with this object
    /// </summary>
    public object? Data
    {
        get => _data;
        set => SetProperty(ref _data, value);
    }
    
    /// <summary>
    /// Opacity of the object (0.0 to 1.0)
    /// </summary>
    public double Opacity
    {
        get => _opacity;
        set => SetProperty(ref _opacity, Math.Clamp(value, 0.0, 1.0));
    }
    
    /// <summary>
    /// Custom properties for extensibility
    /// </summary>
    public Dictionary<string, object?> Properties { get; } = new();
    
    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;
    
    /// <summary>
    /// Raises the PropertyChanged event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    /// <summary>
    /// Sets a property value and raises the PropertyChanged event if the value has changed.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    /// <param name="field">The backing field.</param>
    /// <param name="value">The new value.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <returns>True if the value changed, false otherwise.</returns>
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;
            
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}

