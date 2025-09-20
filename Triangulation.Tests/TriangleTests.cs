using Microsoft.Xna.Framework;

namespace Triangulation.Tests;

public class TriangleTests
{
    [Fact]
    public void EquilateralTriangle_CreatesCorrectVertices()
    {
        // Arrange
        Point center = new(0, 0);
        int sideLength = 20;

        // Act
        var triangle = Triangle.Equilateral(center, sideLength);

        // Assert
        int expectedHeight = (int)Math.Floor(sideLength * MathF.Sqrt(3) / 2);
        int expectedArea = 320;
        Assert.Equal(new Point(0, -11), triangle.A);
        Assert.Equal(new Point(10, 5), triangle.B);
        Assert.Equal(new Point(-10, 5), triangle.C);

        Assert.Equal(expectedArea, triangle.Area2());
    }
}
