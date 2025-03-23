using Microsoft.Xna.Framework;

namespace Triangulation.Tests;

public class TriangleTests
{
    [Fact]
    public void EquilateralTriangle_CreatesCorrectVertices()
    {
        // Arrange
        Vector2 center = new(0, 0);
        float sideLength = 2f;
        float expectedHeight = sideLength * MathF.Sqrt(3) / 2f;

        // Act
        var triangle = Triangle.Equilateral(center, sideLength);

        // Assert
        Assert.Equal(new Vector2(0, -expectedHeight * 2 / 3f), triangle.A);
        Assert.Equal(new Vector2(sideLength / 2f, expectedHeight / 3f), triangle.B);
        Assert.Equal(new Vector2(-sideLength / 2f, expectedHeight / 3f), triangle.C);

        Assert.Equal(triangle.Area2(), sideLength * sideLength * MathF.Sqrt(3) / 2f);
    }
}
