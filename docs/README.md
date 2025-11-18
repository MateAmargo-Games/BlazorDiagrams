# BlazorDiagrams

A powerful and fully native Blazor .NET 8 library for creating interactive diagrams, org charts, flowcharts, and hierarchical visualizations using SVG rendering without JavaScript dependencies.

## Features

### Core Functionality
- **100% Native Blazor**: No JavaScript dependencies, pure C# and Blazor
- **SVG Rendering**: High-quality vector graphics with native browser support
- **Interactive**: Full support for drag & drop, zoom, pan, and selection
- **Layouts**: Multiple automatic layout algorithms
  - TreeLayout: For organizational charts and hierarchies
  - LayeredDigraphLayout: Sugiyama algorithm for flowcharts
  - ForceDirectedLayout: Physics-based layout for general graphs
  - CircularLayout: Circular arrangement
  - GridLayout: Simple grid-based layout
- **Data Binding**: Bidirectional data binding with INotifyPropertyChanged
- **Undo/Redo**: Full command pattern implementation
- **Templates**: Customizable node and link templates with RenderFragment
- **Ports**: Connection points for precise link placement
- **Groups**: Collapsible node grouping with nested layouts
- **Export**: SVG and JSON export capabilities

### Interaction Features
- **Drag & Drop**: Move nodes with mouse or touch (improved accuracy with JS interop)
- **Zoom & Pan**: Navigate large diagrams easily
- **Selection**: Single and multi-select with Ctrl/Shift
- **Node Actions**: Interactive buttons within nodes (edit, delete, custom actions)
- **Context Menus**: Right-click menus for nodes, links, and canvas
- **Keyboard**: Standard keyboard shortcuts
- **Touch Support**: Works on mobile and tablet devices

## Installation

```bash
dotnet add package BlazorDiagrams
```

## Quick Start

### 1. Add Using Directives

```csharp
@using BlazorDiagrams.Core.Models
@using BlazorDiagrams.Core.Layouts
@using BlazorDiagrams.Core.Components
@using BlazorDiagrams.Core.Geometry
```

### 2. Create a Simple Org Chart

```razor
@page "/orgchart"

<Diagram Model="@_model" 
         Width="100%" 
         Height="600px"
         ShowGrid="true"
         ShowZoomControls="true">
</Diagram>

@code {
    private DiagramModel _model = new();

    protected override void OnInitialized()
    {
        // Create nodes
        var ceo = _model.AddNode("1", new { Title = "CEO", Name = "John Doe" });
        ceo.Size = new Size(150, 60);
        
        var cto = _model.AddNode("2", new { Title = "CTO", Name = "Jane Smith" });
        cto.Size = new Size(150, 60);
        
        // Create link
        _model.AddLink(ceo, cto);
        
        // Apply tree layout
        _model.Layout = new TreeLayout
        {
            Angle = 90,
            LayerSpacing = 80,
            NodeSpacing = 40
        };
        
        _model.ApplyLayout();
    }
}
```

### 3. Custom Node Template

```razor
<Diagram Model="@_model">
    <NodeTemplate Context="node">
        <div style="padding: 10px; background: #4a90e2; color: white; border-radius: 5px;">
            @if (node.Data is EmployeeData emp)
            {
                <div><strong>@emp.Name</strong></div>
                <div>@emp.Title</div>
            }
        </div>
    </NodeTemplate>
</Diagram>
```

## Advanced Usage

### Using Ports

```csharp
var node = _model.AddNode("node1", new Point(100, 100));
var leftPort = node.AddPort("left", PortAlignment.LeftCenter);
var rightPort = node.AddPort("right", PortAlignment.RightCenter);

var node2 = _model.AddNode("node2", new Point(300, 100));
var node2Left = node2.AddPort("left", PortAlignment.LeftCenter);

// Connect ports
_model.AddLink(rightPort, node2Left);
```

### Undo/Redo

```csharp
var service = new DiagramService();
var undoManager = service.UndoManager;

// Execute command
undoManager.ExecuteCommand(new AddNodeCommand(_model, newNode));

// Undo
if (undoManager.CanUndo)
    undoManager.Undo();

// Redo
if (undoManager.CanRedo)
    undoManager.Redo();
```

### Groups

```csharp
var group = _model.AddGroup("group1", "Department");
group.Position = new Point(0, 0);
group.Size = new Size(400, 300);

// Add nodes to group
var node = _model.AddNode("node1", new Point(50, 50));
group.AddNode(node);
```

### Export

```csharp
var service = new DiagramService();

// Export to JSON
var json = service.SerializeToJson(_model);

// Export to SVG
var svg = service.ExportToSvg(_model);
```

## Layouts

### TreeLayout
Best for organizational charts and hierarchical data.

```csharp
_model.Layout = new TreeLayout
{
    Angle = 90,              // 0=right, 90=down, 180=left, 270=up
    LayerSpacing = 80,       // Distance between levels
    NodeSpacing = 40,        // Distance between siblings
    Arrangement = TreeArrangement.Vertical,
    Alignment = TreeAlignment.CenterChildren
};
```

### LayeredDigraphLayout
Best for flowcharts using the Sugiyama algorithm.

```csharp
_model.Layout = new LayeredDigraphLayout
{
    Direction = LayoutDirection.Down,
    LayerSpacing = 80,
    NodeSpacing = 40,
    CrossingReductionIterations = 10
};
```

### ForceDirectedLayout
Best for general graphs with spring-electrical simulation.

```csharp
_model.Layout = new ForceDirectedLayout
{
    Iterations = 100,
    SpringConstant = 0.5,
    SpringLength = 100,
    RepulsionConstant = 5000
};
```

### CircularLayout
Arranges nodes in a circle.

```csharp
_model.Layout = new CircularLayout
{
    Radius = 200,
    AutoRadius = true,
    StartAngle = 0
};
```

### GridLayout
Simple grid arrangement.

```csharp
_model.Layout = new GridLayout
{
    Columns = 3,
    HorizontalSpacing = 50,
    VerticalSpacing = 50
};
```

## API Reference

### DiagramModel
Main model containing all diagram elements.

**Properties:**
- `ObservableCollection<Node> Nodes` - All nodes in the diagram
- `ObservableCollection<Link> Links` - All links in the diagram
- `ObservableCollection<Group> Groups` - All groups in the diagram
- `ILayout? Layout` - Layout algorithm to apply
- `double Zoom` - Current zoom level (0.1 to 10.0)
- `Point PanOffset` - Pan offset for the view
- `bool IsReadOnly` - Whether diagram is read-only

**Methods:**
- `Node AddNode(string id, object? data = null)` - Add a new node
- `Link AddLink(Node from, Node to)` - Add a link between nodes
- `void ApplyLayout()` - Apply the configured layout
- `void ClearSelection()` - Clear all selections
- `Rect GetBoundingBox()` - Get bounding box of all nodes

### Node
Represents a node in the diagram.

**Properties:**
- `Point Position` - Position in diagram coordinates
- `Size Size` - Size of the node
- `bool IsSelected` - Whether node is selected
- `bool IsDraggable` - Whether node can be dragged
- `string Fill` - Fill color
- `string Stroke` - Stroke color
- `object? Data` - Custom data attached to node
- `List<Port> Ports` - Connection ports
- `List<Link> Links` - Connected links

### Link
Represents a connection between nodes.

**Properties:**
- `Node? FromNode` - Source node
- `Node? ToNode` - Target node
- `Port? FromPort` - Source port (optional)
- `Port? ToPort` - Target port (optional)
- `LinkRouting Routing` - Routing style (Straight, Orthogonal, Bezier)
- `string Stroke` - Link color
- `double StrokeWidth` - Link width
- `bool ShowArrowhead` - Whether to show arrowhead
- `string? Label` - Optional label text

## Advanced Features

### Node Actions

Node actions are interactive buttons that can be displayed on nodes. They support:

- Multiple positioning options (TopLeft, TopRight, BottomCenter, etc.)
- Visibility modes (Always, OnHover, OnSelected, OnHoverOrSelected)
- Custom icons (emoji, CSS classes, SVG paths)
- Tooltips
- Enable/disable states
- Custom styling

**Example:**

```csharp
// Add actions to a node
node.AddAction("✏️", n => EditNode(n), "Edit", NodeActionPosition.TopRight);
node.AddAction("🗑️", n => DeleteNode(n), "Delete", NodeActionPosition.TopLeft);

// Configure action visibility
var action = new NodeAction
{
    Icon = "ℹ️",
    Tooltip = "Information",
    Visibility = NodeActionVisibility.OnHover,
    Position = NodeActionPosition.BottomCenter,
    OnClick = node => ShowInfo(node)
};
node.AddAction(action);
```

### Context Menus

Fully customizable context menus for nodes, links, and canvas:

**Features:**
- Hierarchical submenus
- Icons and shortcuts
- Separators
- Conditional visibility
- Custom styling and themes
- Enable/disable states

**Example:**

```csharp
var config = new ContextMenuConfig
{
    Theme = ContextMenuTheme.Dark,
    NodeMenuItems = node => new List<ContextMenuItem>
    {
        new ContextMenuItem
        {
            Label = "Edit",
            Icon = "✏️",
            Shortcut = "E",
            Action = _ => EditNode(node)
        },
        new ContextMenuItem
        {
            Label = "Style",
            Icon = "🎨",
            SubItems = new List<ContextMenuItem>
            {
                new ContextMenuItem { Label = "Red", Action = _ => ChangeColor(node, "red") },
                new ContextMenuItem { Label = "Blue", Action = _ => ChangeColor(node, "blue") }
            }
        },
        new ContextMenuItem { IsSeparator = true },
        new ContextMenuItem
        {
            Label = "Delete",
            Icon = "🗑️",
            Action = _ => model.RemoveNode(node)
        }
    }
};
```

### Theming System

BlazorDiagrams includes a comprehensive theming system:

**Built-in Themes:**
- **Light**: Clean white background with subtle grid
- **Dark**: Dark background for reduced eye strain
- **Blueprint**: Technical blueprint style

**Customization Options:**
- Background and grid colors
- Default node and link styles
- Selected/hovered states
- Font families and sizes
- Shadows, gradients, and effects

**Example:**

```csharp
// Use built-in theme
model.Theme = DiagramTheme.Dark;

// Create custom theme
model.Theme = new DiagramTheme
{
    Name = "Ocean",
    BackgroundColor = "#e3f2fd",
    GridColor = "#90caf9",
    GridStyle = GridStyle.Dots,
    DefaultNodeStyle = new NodeStyleConfig
    {
        FillColor = "#ffffff",
        StrokeColor = "#1976d2",
        StrokeWidth = 2,
        Shape = NodeShape.RoundedRectangle,
        CornerRadius = 8,
        ShadowColor = "rgba(0,0,0,0.1)",
        ShadowBlur = 4
    },
    DefaultLinkStyle = new LinkStyleConfig
    {
        StrokeColor = "#1976d2",
        StrokeWidth = 2,
        ShowArrowhead = true
    }
};
```

### Style Configuration

Nodes and links support extensive style customization:

**Node Styles:**
- Shapes: Rectangle, RoundedRectangle, Circle, Ellipse, Diamond, Triangle, Hexagon
- Fill patterns: Solid, Gradient, Striped, Dotted
- Borders: Color, width, dash patterns
- Shadows: Color, blur, offset
- Typography: Font family, size, weight, color

**Link Styles:**
- Line styles: Solid, dashed, dotted
- Arrowhead styles: Filled, Open, Diamond, Circle, Square
- Line caps and joins
- Shadows and effects
- Label styling

**Example:**

```csharp
// Apply custom style to a node
node.StyleConfig = new NodeStyleConfig
{
    Shape = NodeShape.RoundedRectangle,
    FillPattern = FillPattern.Gradient,
    GradientStartColor = "#667eea",
    GradientEndColor = "#764ba2",
    CornerRadius = 12,
    ShadowColor = "rgba(0,0,0,0.3)",
    ShadowBlur = 10
};

// Apply custom style to a link
link.StyleConfig = new LinkStyleConfig
{
    StrokeColor = "#e91e63",
    StrokeWidth = 3,
    StrokeDashArray = "5,5",
    ArrowheadStyle = ArrowheadStyle.Filled
};
```

## Browser Support

- Chrome/Edge 90+
- Firefox 88+
- Safari 14+

## License

MIT License - feel free to use in commercial and open-source projects.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Support

For issues and feature requests, please use the GitHub issue tracker.

