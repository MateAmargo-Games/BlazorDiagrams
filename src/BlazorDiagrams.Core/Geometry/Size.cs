namespace BlazorDiagrams.Core.Geometry;

/// <summary>
/// Represents the size (width and height) of an object
/// </summary>
public readonly struct Size : IEquatable<Size>
{
    public Size(double width, double height)
    {
        Width = Math.Max(0, width);
        Height = Math.Max(0, height);
    }
    
    public double Width { get; init; }
    public double Height { get; init; }
    
    public static Size Zero => new(0, 0);
    public static Size Empty => Zero;
    
    public double Area => Width * Height;
    public double AspectRatio => Height > 0 ? Width / Height : 0;
    
    public bool IsEmpty => Width == 0 && Height == 0;
    
    public Size Scale(double factor) => new(Width * factor, Height * factor);
    public Size Scale(double widthFactor, double heightFactor) => new(Width * widthFactor, Height * heightFactor);
    
    public static Size operator +(Size a, Size b) => new(a.Width + b.Width, a.Height + b.Height);
    public static Size operator -(Size a, Size b) => new(a.Width - b.Width, a.Height - b.Height);
    public static Size operator *(Size s, double scalar) => s.Scale(scalar);
    public static Size operator *(double scalar, Size s) => s.Scale(scalar);
    public static Size operator /(Size s, double scalar) => new(s.Width / scalar, s.Height / scalar);
    
    public bool Equals(Size other) => Width.Equals(other.Width) && Height.Equals(other.Height);
    public override bool Equals(object? obj) => obj is Size other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Width, Height);
    public static bool operator ==(Size left, Size right) => left.Equals(right);
    public static bool operator !=(Size left, Size right) => !left.Equals(right);
    
    public override string ToString() => $"({Width:F2} × {Height:F2})";
    
    public void Deconstruct(out double width, out double height)
    {
        width = Width;
        height = Height;
    }
}

