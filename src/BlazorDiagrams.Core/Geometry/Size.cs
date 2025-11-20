namespace BlazorDiagrams.Core.Geometry;

/// <summary>
/// Represents the size (width and height) of an object
/// </summary>
public readonly struct Size : IEquatable<Size>
{
    /// <summary>
    /// Creates a new instance of the <see cref="Size"/> struct.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public Size(double width, double height)
    {
        Width = Math.Max(0, width);
        Height = Math.Max(0, height);
    }
    
    /// <summary>
    /// Gets the width.
    /// </summary>
    public double Width { get; init; }

    /// <summary>
    /// Gets the height.
    /// </summary>
    public double Height { get; init; }
    
    /// <summary>
    /// Gets a size with width and height set to zero.
    /// </summary>
    public static Size Zero => new(0, 0);

    /// <summary>
    /// Gets an empty size.
    /// </summary>
    public static Size Empty => Zero;
    
    /// <summary>
    /// Gets the area (Width * Height).
    /// </summary>
    public double Area => Width * Height;

    /// <summary>
    /// Gets the aspect ratio (Width / Height). Returns 0 if Height is 0.
    /// </summary>
    public double AspectRatio => Height > 0 ? Width / Height : 0;
    
    /// <summary>
    /// Gets a value indicating whether the size is empty (width and height are 0).
    /// </summary>
    public bool IsEmpty => Width == 0 && Height == 0;
    
    /// <summary>
    /// Scales the size by a factor.
    /// </summary>
    public Size Scale(double factor) => new(Width * factor, Height * factor);

    /// <summary>
    /// Scales the size by separate width and height factors.
    /// </summary>
    public Size Scale(double widthFactor, double heightFactor) => new(Width * widthFactor, Height * heightFactor);
    
    /// <summary>
    /// Adds two sizes.
    /// </summary>
    public static Size operator +(Size a, Size b) => new(a.Width + b.Width, a.Height + b.Height);

    /// <summary>
    /// Subtracts one size from another.
    /// </summary>
    public static Size operator -(Size a, Size b) => new(a.Width - b.Width, a.Height - b.Height);

    /// <summary>
    /// Multiplies a size by a scalar.
    /// </summary>
    public static Size operator *(Size s, double scalar) => s.Scale(scalar);

    /// <summary>
    /// Multiplies a scalar by a size.
    /// </summary>
    public static Size operator *(double scalar, Size s) => s.Scale(scalar);

    /// <summary>
    /// Divides a size by a scalar.
    /// </summary>
    public static Size operator /(Size s, double scalar) => new(s.Width / scalar, s.Height / scalar);
    
    /// <inheritdoc />
    public bool Equals(Size other) => Width.Equals(other.Width) && Height.Equals(other.Height);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Size other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Width, Height);

    /// <summary>
    /// Determines whether two specified sizes have the same value.
    /// </summary>
    public static bool operator ==(Size left, Size right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified sizes have different values.
    /// </summary>
    public static bool operator !=(Size left, Size right) => !left.Equals(right);
    
    /// <inheritdoc />
    public override string ToString() => $"({Width:F2} × {Height:F2})";
    
    /// <summary>
    /// Deconstructs the size into its width and height.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public void Deconstruct(out double width, out double height)
    {
        width = Width;
        height = Height;
    }
}

