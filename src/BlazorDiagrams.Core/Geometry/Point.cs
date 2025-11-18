namespace BlazorDiagrams.Core.Geometry;

/// <summary>
/// Represents a point in 2D space
/// </summary>
public readonly struct Point : IEquatable<Point>
{
    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }
    
    public double X { get; init; }
    public double Y { get; init; }
    
    public static Point Zero => new(0, 0);
    
    public Point Add(Point other) => new(X + other.X, Y + other.Y);
    public Point Subtract(Point other) => new(X - other.X, Y - other.Y);
    public Point Multiply(double scalar) => new(X * scalar, Y * scalar);
    public Point Divide(double scalar) => new(X / scalar, Y / scalar);
    
    public double DistanceTo(Point other)
    {
        var dx = X - other.X;
        var dy = Y - other.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
    
    public double Magnitude() => Math.Sqrt(X * X + Y * Y);
    
    public Point Normalize()
    {
        var mag = Magnitude();
        return mag > 0 ? new Point(X / mag, Y / mag) : Zero;
    }
    
    public double DotProduct(Point other) => X * other.X + Y * other.Y;
    
    public static Point operator +(Point a, Point b) => a.Add(b);
    public static Point operator -(Point a, Point b) => a.Subtract(b);
    public static Point operator *(Point p, double scalar) => p.Multiply(scalar);
    public static Point operator *(double scalar, Point p) => p.Multiply(scalar);
    public static Point operator /(Point p, double scalar) => p.Divide(scalar);
    
    public bool Equals(Point other) => X.Equals(other.X) && Y.Equals(other.Y);
    public override bool Equals(object? obj) => obj is Point other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(X, Y);
    public static bool operator ==(Point left, Point right) => left.Equals(right);
    public static bool operator !=(Point left, Point right) => !left.Equals(right);
    
    public override string ToString() => $"({X:F2}, {Y:F2})";
    
    public void Deconstruct(out double x, out double y)
    {
        x = X;
        y = Y;
    }
}

