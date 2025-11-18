using BlazorDiagrams.Core.Geometry;
using Xunit;

namespace BlazorDiagrams.Core.Tests.Geometry;

public class PointTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        // Arrange & Act
        var point = new Point(10, 20);
        
        // Assert
        Assert.Equal(10, point.X);
        Assert.Equal(20, point.Y);
    }
    
    [Fact]
    public void Zero_ReturnsOrigin()
    {
        // Act
        var zero = Point.Zero;
        
        // Assert
        Assert.Equal(0, zero.X);
        Assert.Equal(0, zero.Y);
    }
    
    [Fact]
    public void Add_CombinesTwoPoints()
    {
        // Arrange
        var p1 = new Point(10, 20);
        var p2 = new Point(5, 15);
        
        // Act
        var result = p1 + p2;
        
        // Assert
        Assert.Equal(15, result.X);
        Assert.Equal(35, result.Y);
    }
    
    [Fact]
    public void Subtract_SubtractsTwoPoints()
    {
        // Arrange
        var p1 = new Point(10, 20);
        var p2 = new Point(5, 15);
        
        // Act
        var result = p1 - p2;
        
        // Assert
        Assert.Equal(5, result.X);
        Assert.Equal(5, result.Y);
    }
    
    [Fact]
    public void Multiply_ScalesPoint()
    {
        // Arrange
        var point = new Point(10, 20);
        
        // Act
        var result = point * 2;
        
        // Assert
        Assert.Equal(20, result.X);
        Assert.Equal(40, result.Y);
    }
    
    [Fact]
    public void Divide_ScalesPointDown()
    {
        // Arrange
        var point = new Point(10, 20);
        
        // Act
        var result = point / 2;
        
        // Assert
        Assert.Equal(5, result.X);
        Assert.Equal(10, result.Y);
    }
    
    [Fact]
    public void DistanceTo_CalculatesCorrectDistance()
    {
        // Arrange
        var p1 = new Point(0, 0);
        var p2 = new Point(3, 4);
        
        // Act
        var distance = p1.DistanceTo(p2);
        
        // Assert
        Assert.Equal(5, distance);
    }
    
    [Fact]
    public void Equals_ComparesCorrectly()
    {
        // Arrange
        var p1 = new Point(10, 20);
        var p2 = new Point(10, 20);
        var p3 = new Point(15, 25);
        
        // Act & Assert
        Assert.True(p1.Equals(p2));
        Assert.False(p1.Equals(p3));
    }
    
    [Fact]
    public void GetHashCode_ReturnsSameForEqualPoints()
    {
        // Arrange
        var p1 = new Point(10, 20);
        var p2 = new Point(10, 20);
        
        // Act & Assert
        Assert.Equal(p1.GetHashCode(), p2.GetHashCode());
    }
}

