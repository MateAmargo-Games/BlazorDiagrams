# BlazorDiagrams

[![.NET](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![NuGet](https://img.shields.io/badge/nuget-v1.0.0-blue)](https://www.nuget.org/)

A powerful and fully native Blazor .NET 8 library for creating interactive diagrams, org charts, flowcharts, and hierarchical visualizations using SVG rendering **without JavaScript dependencies**.

![Blazor Diagrams Demo](https://via.placeholder.com/800x400?text=BlazorDiagrams+Demo)

## 🚀 Features

- **100% Native Blazor** - No JavaScript interop required
- **SVG Rendering** - High-quality vector graphics
- **Multiple Layouts** - Tree, Force-Directed, Circular, Grid, Layered Digraph
- **Interactive** - Drag & drop, zoom, pan, multi-select
- **Customizable** - Full template support with RenderFragment
- **Data Binding** - Bidirectional binding with INotifyPropertyChanged
- **Undo/Redo** - Complete command pattern implementation
- **Ports & Groups** - Advanced connection and grouping features
- **Export** - SVG and JSON export capabilities

## 📦 Installation

```bash
dotnet add package BlazorDiagrams
```

## 🎯 Quick Start

```razor
@page "/diagram"
@using BlazorDiagrams.Core.Models
@using BlazorDiagrams.Core.Layouts
@using BlazorDiagrams.Core.Components
@using BlazorDiagrams.Core.Geometry

<Diagram Model="@_model" Width="100%" Height="600px" ShowGrid="true">
    <NodeTemplate Context="node">
        <div style="padding: 10px; background: #4a90e2; color: white; border-radius: 5px;">
            @node.Data?.ToString()
        </div>
    </NodeTemplate>
</Diagram>

@code {
    private DiagramModel _model = new();

    protected override void OnInitialized()
    {
        var node1 = _model.AddNode("1", new { Title = "CEO" });
        var node2 = _model.AddNode("2", new { Title = "CTO" });
        
        node1.Size = new Size(150, 60);
        node2.Size = new Size(150, 60);
        
        _model.AddLink(node1, node2);
        
        _model.Layout = new TreeLayout { LayerSpacing = 80, NodeSpacing = 40 };
        _model.ApplyLayout();
    }
}
```

## 📚 Documentation

Full documentation is available in the [docs](./docs) folder:

- [Getting Started](./docs/README.md)
- [API Reference](./docs/API.md)
- [Examples](./src/BlazorDiagrams.Samples)

## 🏗️ Project Structure

```
BlazorDiagrams/
├── src/
│   ├── BlazorDiagrams.Core/      # Main library
│   └── BlazorDiagrams.Samples/   # Example applications
├── tests/
│   └── BlazorDiagrams.Core.Tests/ # Unit tests
└── docs/                          # Documentation
```

## 💡 Examples

### Organizational Chart
```csharp
_model.Layout = new TreeLayout
{
    Angle = 90,  // Vertical layout
    LayerSpacing = 80,
    NodeSpacing = 40,
    Alignment = TreeAlignment.CenterChildren
};
```

### Force-Directed Graph
```csharp
_model.Layout = new ForceDirectedLayout
{
    Iterations = 100,
    SpringConstant = 0.5,
    SpringLength = 100,
    RepulsionConstant = 5000
};
```

### Flowchart
```csharp
_model.Layout = new LayeredDigraphLayout
{
    Direction = LayoutDirection.Down,
    LayerSpacing = 80,
    NodeSpacing = 40
};
```

## 🎨 Customization

BlazorDiagrams supports full customization through:

- **Templates**: Use RenderFragment for custom node/link appearance
- **Styles**: CSS styling for all visual elements
- **Data Binding**: Attach any data model to nodes and links
- **Events**: Handle clicks, selections, and other interactions

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🌟 Inspired By

This library provides a Blazor-native alternative to [GoJS](https://gojs.net/), focusing on:
- Native .NET implementation
- No JavaScript dependencies
- Open-source MIT license
- Blazor Server and WASM compatibility

## 📧 Support

- 📫 Report issues on [GitHub Issues](https://github.com/yourusername/BlazorDiagrams/issues)
- 💬 Discussions on [GitHub Discussions](https://github.com/yourusername/BlazorDiagrams/discussions)

## 🙏 Acknowledgments

- Built with Blazor and .NET 8
- Inspired by GoJS
- SVG rendering techniques from various open-source projects

---

Made with ❤️ using Blazor .NET 8

