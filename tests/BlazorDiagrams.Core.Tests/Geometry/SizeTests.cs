using BlazorDiagrams.Core.Geometry;
using Xunit;

namespace BlazorDiagrams.Core.Tests.Geometry;

public class SizeTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        // Arrange & Act
        var size = new Size(100, 200);
        
        // Assert
        Assert.Equal(100, size.Width);
        Assert.Equal(200, size.Height);
    }
    
    [Fact]
    public void Zero_ReturnsZeroSize()
    {
        // Act
        var zero = Size.Zero;
        
        // Assert
        Assert.Equal(0, zero.Width);
        Assert.Equal(0, zero.Height);
    }
    
    [Fact]
    public void IsEmpty_ReturnsTrueForZeroSize()
    {
        // Arrange
        var empty = new Size(0, 0);
        var notEmpty = new Size(10, 20);
        
        // Act & Assert
        Assert.True(empty.IsEmpty);
        Assert.False(notEmpty.IsEmpty);
    }
    
    [Fact]
    public void Add_CombinesTwoSizes()
    {
        // Arrange
        var s1 = new Size(10, 20);
        var s2 = new Size(5, 15);
        
        // Act
        var result = s1 + s2;
        
        // Assert
        Assert.Equal(15, result.Width);
        Assert.Equal(35, result.Height);
    }
    
    [Fact]
    public void Multiply_ScalesSize()
    {
        // Arrange
        var size = new Size(10, 20);
        
        // Act
        var result = size * 2;
        
        // Assert
        Assert.Equal(20, result.Width);
        Assert.Equal(40, result.Height);
    }
    
    [Fact]
    public void Equals_ComparesCorrectly()
    {
        // Arrange
        var s1 = new Size(10, 20);
        var s2 = new Size(10, 20);
        var s3 = new Size(15, 25);
        
        // Act & Assert
        Assert.True(s1.Equals(s2));
        Assert.False(s1.Equals(s3));
    }
}

