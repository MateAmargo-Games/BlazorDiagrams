namespace BlazorDiagrams.Core.Geometry;

/// <summary>
/// Represents a point in 2D space
/// </summary>
public readonly struct Point : IEquatable<Point>
{
    /// <summary>
    /// Creates a new instance of the <see cref="Point"/> struct.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    public Point(double x, double y)
    {
        X = x;
        Y = y;
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
    /// Gets a point with coordinates (0, 0).
    /// </summary>
    public static Point Zero => new(0, 0);
    
    /// <summary>
    /// Adds two points.
    /// </summary>
    public Point Add(Point other) => new(X + other.X, Y + other.Y);

    /// <summary>
    /// Subtracts a point from this point.
    /// </summary>
    public Point Subtract(Point other) => new(X - other.X, Y - other.Y);

    /// <summary>
    /// Multiplies the point by a scalar.
    /// </summary>
    public Point Multiply(double scalar) => new(X * scalar, Y * scalar);

    /// <summary>
    /// Divides the point by a scalar.
    /// </summary>
    public Point Divide(double scalar) => new(X / scalar, Y / scalar);
    
    /// <summary>
    /// Calculates the distance to another point.
    /// </summary>
    public double DistanceTo(Point other)
    {
        var dx = X - other.X;
        var dy = Y - other.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
    
    /// <summary>
    /// Calculates the magnitude (length) of the vector represented by this point.
    /// </summary>
    public double Magnitude() => Math.Sqrt(X * X + Y * Y);
    
    /// <summary>
    /// Returns a normalized point (vector with magnitude 1).
    /// </summary>
    public Point Normalize()
    {
        var mag = Magnitude();
        return mag > 0 ? new Point(X / mag, Y / mag) : Zero;
    }
    
    /// <summary>
    /// Calculates the dot product with another point.
    /// </summary>
    public double DotProduct(Point other) => X * other.X + Y * other.Y;
    
    /// <summary>
    /// Adds two points.
    /// </summary>
    public static Point operator +(Point a, Point b) => a.Add(b);

    /// <summary>
    /// Subtracts one point from another.
    /// </summary>
    public static Point operator -(Point a, Point b) => a.Subtract(b);

    /// <summary>
    /// Multiplies a point by a scalar.
    /// </summary>
    public static Point operator *(Point p, double scalar) => p.Multiply(scalar);

    /// <summary>
    /// Multiplies a scalar by a point.
    /// </summary>
    public static Point operator *(double scalar, Point p) => p.Multiply(scalar);

    /// <summary>
    /// Divides a point by a scalar.
    /// </summary>
    public static Point operator /(Point p, double scalar) => p.Divide(scalar);
    
    /// <inheritdoc />
    public bool Equals(Point other) => X.Equals(other.X) && Y.Equals(other.Y);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Point other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(X, Y);

    /// <summary>
    /// Determines whether two specified points have the same value.
    /// </summary>
    public static bool operator ==(Point left, Point right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified points have different values.
    /// </summary>
    public static bool operator !=(Point left, Point right) => !left.Equals(right);
    
    /// <inheritdoc />
    public override string ToString() => $"({X:F2}, {Y:F2})";
    
    /// <summary>
    /// Deconstructs the point into its X and Y coordinates.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    public void Deconstruct(out double x, out double y)
    {
        x = X;
        y = Y;
    }
}

