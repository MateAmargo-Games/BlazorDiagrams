namespace BlazorDiagrams.Core.Geometry;

/// <summary>
/// Represents a rectangle defined by position and size
/// </summary>
public readonly struct Rect : IEquatable<Rect>
{
    /// <summary>
    /// Creates a new instance of the <see cref="Rect"/> struct.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public Rect(double x, double y, double width, double height)
    {
        X = x;
        Y = y;
        Width = Math.Max(0, width);
        Height = Math.Max(0, height);
    }
    
    /// <summary>
    /// Creates a new instance of the <see cref="Rect"/> struct from a position and size.
    /// </summary>
    /// <param name="position">The position (top-left corner).</param>
    /// <param name="size">The size.</param>
    public Rect(Point position, Size size)
    {
        X = position.X;
        Y = position.Y;
        Width = size.Width;
        Height = size.Height;
    }
    
    /// <summary>
    /// Gets the X coordinate.
    /// </summary>
    public double X { get; init; }

    /// <summary>
    /// Gets the Y coordinate.
    /// </summary>
    public double Y { get; init; }

    /// <summary>
    /// Gets the width.
    /// </summary>
    public double Width { get; init; }

    /// <summary>
    /// Gets the height.
    /// </summary>
    public double Height { get; init; }
    
    /// <summary>
    /// Gets the position (top-left corner).
    /// </summary>
    public Point Position => new(X, Y);

    /// <summary>
    /// Gets the size.
    /// </summary>
    public Size Size => new(Width, Height);
    
    /// <summary>
    /// Gets the X coordinate of the left edge.
    /// </summary>
    public double Left => X;

    /// <summary>
    /// Gets the Y coordinate of the top edge.
    /// </summary>
    public double Top => Y;

    /// <summary>
    /// Gets the X coordinate of the right edge.
    /// </summary>
    public double Right => X + Width;

    /// <summary>
    /// Gets the Y coordinate of the bottom edge.
    /// </summary>
    public double Bottom => Y + Height;
    
    /// <summary>
    /// Gets the top-left corner point.
    /// </summary>
    public Point TopLeft => new(Left, Top);

    /// <summary>
    /// Gets the top-right corner point.
    /// </summary>
    public Point TopRight => new(Right, Top);

    /// <summary>
    /// Gets the bottom-left corner point.
    /// </summary>
    public Point BottomLeft => new(Left, Bottom);

    /// <summary>
    /// Gets the bottom-right corner point.
    /// </summary>
    public Point BottomRight => new(Right, Bottom);

    /// <summary>
    /// Gets the center point.
    /// </summary>
    public Point Center => new(X + Width / 2, Y + Height / 2);
    
    /// <summary>
    /// Gets a rectangle with all values set to zero.
    /// </summary>
    public static Rect Zero => new(0, 0, 0, 0);

    /// <summary>
    /// Gets an empty rectangle.
    /// </summary>
    public static Rect Empty => Zero;
    
    /// <summary>
    /// Gets a value indicating whether the rectangle is empty (width and height are 0).
    /// </summary>
    public bool IsEmpty => Width == 0 && Height == 0;
    
    /// <summary>
    /// Determines if the rectangle contains the specified point.
    /// </summary>
    public bool Contains(Point point) =>
        point.X >= Left && point.X <= Right &&
        point.Y >= Top && point.Y <= Bottom;
    
    /// <summary>
    /// Determines if the rectangle contains another rectangle.
    /// </summary>
    public bool Contains(Rect other) =>
        other.Left >= Left && other.Right <= Right &&
        other.Top >= Top && other.Bottom <= Bottom;
    
    /// <summary>
    /// Determines if the rectangle intersects with another rectangle.
    /// </summary>
    public bool Intersects(Rect other) =>
        Left < other.Right && Right > other.Left &&
        Top < other.Bottom && Bottom > other.Top;
    
    /// <summary>
    /// Calculates the intersection of this rectangle with another.
    /// </summary>
    public Rect? Intersection(Rect other)
    {
        if (!Intersects(other))
            return null;
            
        var left = Math.Max(Left, other.Left);
        var top = Math.Max(Top, other.Top);
        var right = Math.Min(Right, other.Right);
        var bottom = Math.Min(Bottom, other.Bottom);
        
        return new Rect(left, top, right - left, bottom - top);
    }
    
    /// <summary>
    /// Calculates the union of this rectangle with another.
    /// </summary>
    public Rect Union(Rect other)
    {
        var left = Math.Min(Left, other.Left);
        var top = Math.Min(Top, other.Top);
        var right = Math.Max(Right, other.Right);
        var bottom = Math.Max(Bottom, other.Bottom);
        
        return new Rect(left, top, right - left, bottom - top);
    }
    
    /// <summary>
    /// Inflates the rectangle by the specified amount in all directions.
    /// </summary>
    public Rect Inflate(double amount) =>
        new(X - amount, Y - amount, Width + 2 * amount, Height + 2 * amount);
    
    /// <summary>
    /// Inflates the rectangle by the specified horizontal and vertical amounts.
    /// </summary>
    public Rect Inflate(double horizontal, double vertical) =>
        new(X - horizontal, Y - vertical, Width + 2 * horizontal, Height + 2 * vertical);
    
    /// <summary>
    /// Offsets the rectangle by the specified amounts.
    /// </summary>
    public Rect Offset(double dx, double dy) =>
        new(X + dx, Y + dy, Width, Height);
    
    /// <summary>
    /// Offsets the rectangle by the specified point.
    /// </summary>
    public Rect Offset(Point offset) =>
        Offset(offset.X, offset.Y);
    
    /// <inheritdoc />
    public bool Equals(Rect other) =>
        X.Equals(other.X) && Y.Equals(other.Y) &&
        Width.Equals(other.Width) && Height.Equals(other.Height);
    
    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Rect other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

    /// <summary>
    /// Determines whether two specified rectangles have the same value.
    /// </summary>
    public static bool operator ==(Rect left, Rect right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified rectangles have different values.
    /// </summary>
    public static bool operator !=(Rect left, Rect right) => !left.Equals(right);
    
    /// <inheritdoc />
    public override string ToString() => $"[{X:F2}, {Y:F2}, {Width:F2}, {Height:F2}]";
    
    /// <summary>
    /// Deconstructs the rectangle into its components.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public void Deconstruct(out double x, out double y, out double width, out double height)
    {
        x = X;
        y = Y;
        width = Width;
        height = Height;
    }
}

