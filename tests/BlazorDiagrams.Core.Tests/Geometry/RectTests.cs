using BlazorDiagrams.Core.Geometry;
using Xunit;

namespace BlazorDiagrams.Core.Tests.Geometry;

public class RectTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        // Arrange & Act
        var rect = new Rect(10, 20, 100, 200);
        
        // Assert
        Assert.Equal(10, rect.X);
        Assert.Equal(20, rect.Y);
        Assert.Equal(100, rect.Width);
        Assert.Equal(200, rect.Height);
    }
    
    [Fact]
    public void Zero_ReturnsZeroRect()
    {
        // Act
        var zero = Rect.Zero;
        
        // Assert
        Assert.Equal(0, zero.X);
        Assert.Equal(0, zero.Y);
        Assert.Equal(0, zero.Width);
        Assert.Equal(0, zero.Height);
    }
    
    [Fact]
    public void Left_ReturnsXCoordinate()
    {
        // Arrange
        var rect = new Rect(10, 20, 100, 200);
        
        // Act & Assert
        Assert.Equal(10, rect.Left);
    }
    
    [Fact]
    public void Right_ReturnsRightEdge()
    {
        // Arrange
        var rect = new Rect(10, 20, 100, 200);
        
        // Act & Assert
        Assert.Equal(110, rect.Right);
    }
    
    [Fact]
    public void Top_ReturnsYCoordinate()
    {
        // Arrange
        var rect = new Rect(10, 20, 100, 200);
        
        // Act & Assert
        Assert.Equal(20, rect.Top);
    }
    
    [Fact]
    public void Bottom_ReturnsBottomEdge()
    {
        // Arrange
        var rect = new Rect(10, 20, 100, 200);
        
        // Act & Assert
        Assert.Equal(220, rect.Bottom);
    }
    
    [Fact]
    public void Contains_Point_ReturnsCorrectResult()
    {
        // Arrange
        var rect = new Rect(10, 20, 100, 200);
        var insidePoint = new Point(50, 100);
        var outsidePoint = new Point(5, 5);
        
        // Act & Assert
        Assert.True(rect.Contains(insidePoint));
        Assert.False(rect.Contains(outsidePoint));
    }
    
    [Fact]
    public void Contains_Coordinates_ReturnsCorrectResult()
    {
        // Arrange
        var rect = new Rect(10, 20, 100, 200);
        var inside = new Point(50, 100);
        var outside = new Point(5, 5);
        
        // Act & Assert
        Assert.True(rect.Contains(inside));
        Assert.False(rect.Contains(outside));
    }
    
    [Fact]
    public void Intersects_ReturnsCorrectResult()
    {
        // Arrange
        var rect1 = new Rect(10, 10, 100, 100);
        var rect2 = new Rect(50, 50, 100, 100); // Intersects
        var rect3 = new Rect(200, 200, 100, 100); // Does not intersect
        
        // Act & Assert
        Assert.True(rect1.Intersects(rect2));
        Assert.False(rect1.Intersects(rect3));
    }
    
    [Fact]
    public void Union_CombinesTwoRects()
    {
        // Arrange
        var rect1 = new Rect(10, 10, 100, 100);
        var rect2 = new Rect(50, 50, 100, 100);
        
        // Act
        var union = rect1.Union(rect2);
        
        // Assert
        Assert.Equal(10, union.X);
        Assert.Equal(10, union.Y);
        Assert.Equal(140, union.Width); // 50 + 100 - 10
        Assert.Equal(140, union.Height);
    }
    
    [Fact]
    public void Inflate_ExpandsRect()
    {
        // Arrange
        var rect = new Rect(10, 20, 100, 200);
        
        // Act
        var inflated = rect.Inflate(5);
        
        // Assert
        Assert.Equal(5, inflated.X);
        Assert.Equal(15, inflated.Y);
        Assert.Equal(110, inflated.Width);
        Assert.Equal(210, inflated.Height);
    }
    
    [Fact]
    public void Center_ReturnsCenter()
    {
        // Arrange
        var rect = new Rect(10, 20, 100, 200);
        
        // Act
        var center = rect.Center;
        
        // Assert
        Assert.Equal(60, center.X);
        Assert.Equal(120, center.Y);
    }
}

