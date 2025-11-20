# Data Binding en BlazorDiagrams

BlazorDiagrams ofrece dos modos de operación para adaptarse a diferentes necesidades y niveles de complejidad.

## Modos de Operación

### 1. Modo Manual (Core)
Este es el modo tradicional y más flexible. Usted es responsable de instanciar el `DiagramModel`, crear nodos y enlaces, y gestionar los eventos.

**Recomendado para:**
- Aplicaciones complejas con lógica de negocio personalizada.
- Diagramas que no se mapean directamente a una lista simple de datos.
- Usuarios que necesitan control total sobre el rendimiento y el comportamiento.

### 2. Modo Data Binding (`DiagramData<T>`)
Este modo simplifica la creación de diagramas al permitir enlazar directamente una colección de datos (`IEnumerable<T>`), similar a como funcionan componentes como `MudTable` o `MudChart`.

**Recomendado para:**
- Visualización rápida de listas de objetos.
- Aplicaciones CRUD simples.
- Usuarios que prefieren una API declarativa en Razor.

## Uso de `DiagramData<T>`

El componente `DiagramData<T>` envuelve al diagrama y sincroniza automáticamente los nodos con su fuente de datos.

### Ejemplo Básico

```razor
<DiagramData Items="@Users" 
             KeySelector="@(u => u.Id)" 
             PositionSelector="@(u => u.Location)">
    
    <NodeTemplate Context="user">
        <div class="card" style="width: 150px; padding: 10px; border: 1px solid #ccc; background: white;">
            <h4>@user.Name</h4>
            <p>@user.Role</p>
        </div>
    </NodeTemplate>

</DiagramData>

@code {
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public Point Location { get; set; }
    }

    private List<User> Users = new()
    {
        new User { Id = "1", Name = "Alice", Role = "Admin", Location = new Point(100, 100) },
        new User { Id = "2", Name = "Bob", Role = "User", Location = new Point(300, 100) }
    };
}
```

### Parámetros

| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `Items` | `IEnumerable<T>` | La colección de datos a visualizar. |
| `KeySelector` | `Func<T, string>` | Función para obtener un ID único para cada elemento (Requerido). |
| `PositionSelector` | `Func<T, Point>` | Función para determinar la posición inicial del nodo (Opcional). |
| `NodeSize` | `Size` | Tamaño de cada nodo (Opcional, por defecto 200x100). |
| `NodeTemplate` | `RenderFragment<T>` | Plantilla Razor para renderizar el contenido de cada nodo. |
| `OnChange` | `EventCallback<T>` | Evento que se dispara cuando un nodo cambia (ej. posición). |

### Sincronización
- **Agregar**: Si agrega un elemento a la lista `Items` y llama a `StateHasChanged()` (o si la colección es `ObservableCollection`), el nodo aparecerá automáticamente.
- **Eliminar**: Si elimina un elemento de `Items`, el nodo correspondiente desaparecerá.
- **Actualizar**: El contenido del nodo se actualizará si las propiedades del objeto cambian y se renderiza de nuevo.
