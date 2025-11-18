namespace BlazorDiagrams.Core.Geometry;

/// <summary>
/// Represents a rectangle defined by position and size
/// </summary>
public readonly struct Rect : IEquatable<Rect>
{
    public Rect(double x, double y, double width, double height)
    {
        X = x;
        Y = y;
        Width = Math.Max(0, width);
        Height = Math.Max(0, height);
    }
    
    public Rect(Point position, Size size)
    {
        X = position.X;
        Y = position.Y;
        Width = size.Width;
        Height = size.Height;
    }
    
    public double X { get; init; }
    public double Y { get; init; }
    public double Width { get; init; }
    public double Height { get; init; }
    
    public Point Position => new(X, Y);
    public Size Size => new(Width, Height);
    
    public double Left => X;
    public double Top => Y;
    public double Right => X + Width;
    public double Bottom => Y + Height;
    
    public Point TopLeft => new(Left, Top);
    public Point TopRight => new(Right, Top);
    public Point BottomLeft => new(Left, Bottom);
    public Point BottomRight => new(Right, Bottom);
    public Point Center => new(X + Width / 2, Y + Height / 2);
    
    public static Rect Zero => new(0, 0, 0, 0);
    public static Rect Empty => Zero;
    
    public bool IsEmpty => Width == 0 && Height == 0;
    
    public bool Contains(Point point) =>
        point.X >= Left && point.X <= Right &&
        point.Y >= Top && point.Y <= Bottom;
    
    public bool Contains(Rect other) =>
        other.Left >= Left && other.Right <= Right &&
        other.Top >= Top && other.Bottom <= Bottom;
    
    public bool Intersects(Rect other) =>
        Left < other.Right && Right > other.Left &&
        Top < other.Bottom && Bottom > other.Top;
    
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
    
    public Rect Union(Rect other)
    {
        var left = Math.Min(Left, other.Left);
        var top = Math.Min(Top, other.Top);
        var right = Math.Max(Right, other.Right);
        var bottom = Math.Max(Bottom, other.Bottom);
        
        return new Rect(left, top, right - left, bottom - top);
    }
    
    public Rect Inflate(double amount) =>
        new(X - amount, Y - amount, Width + 2 * amount, Height + 2 * amount);
    
    public Rect Inflate(double horizontal, double vertical) =>
        new(X - horizontal, Y - vertical, Width + 2 * horizontal, Height + 2 * vertical);
    
    public Rect Offset(double dx, double dy) =>
        new(X + dx, Y + dy, Width, Height);
    
    public Rect Offset(Point offset) =>
        Offset(offset.X, offset.Y);
    
    public bool Equals(Rect other) =>
        X.Equals(other.X) && Y.Equals(other.Y) &&
        Width.Equals(other.Width) && Height.Equals(other.Height);
    
    public override bool Equals(object? obj) => obj is Rect other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(X, Y, Width, Height);
    public static bool operator ==(Rect left, Rect right) => left.Equals(right);
    public static bool operator !=(Rect left, Rect right) => !left.Equals(right);
    
    public override string ToString() => $"[{X:F2}, {Y:F2}, {Width:F2}, {Height:F2}]";
    
    public void Deconstruct(out double x, out double y, out double width, out double height)
    {
        x = X;
        y = Y;
        width = Width;
        height = Height;
    }
}

