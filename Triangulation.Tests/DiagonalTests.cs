using Microsoft.Xna.Framework;

namespace Triangulation.Tests;

public class DiagonalTests
{
    [Fact]
    public void DiagonalDoesNotCrossAnyEdge()
    {
        // Arrange
        var polygon = Polygon.FromList(
            [new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1)]
        );

        // Act
        var diagonal = Diagonal.IsDiagonal(polygon.Head, polygon.Head.Next.Next);

        // Assert
        Assert.True(diagonal);
    }

    [Fact]
    public void DiagonalNotInCone()
    {
        // Arrange
        var polygon = Polygon.FromList(
            [
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0.5f, 0.25f),
                new Vector2(0, 1),
            ]
        );

        // Act
        var diagonal = Diagonal.IsDiagonal(polygon.Head, polygon.Head.Next.Next);

        // Assert
        Assert.False(diagonal);
    }

    [Fact]
    public void DiagonalCrossesEdge()
    {
        // Arrange
        var polygon = Polygon.FromList(
            [
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1.5f, 0.5f),
                new Vector2(1, 1),
                new Vector2(0.5f, 0.25f),
                new Vector2(0, 1),
                new Vector2(-0.5f, 0.5f),
            ]
        );

        // Act
        var diagonal = Diagonal.IsDiagonal(polygon.Head.Previous, polygon.Head.Next.Next);

        // Assert
        Assert.False(diagonal);
    }

    [Fact]
    public void DiagonalIsAnExistingEdge()
    {
        // Arrange
        var polygon = Polygon.FromList(
            [
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1),
            ]
        );

        // Act
        var diagonal = Diagonal.IsDiagonal(polygon.Head, polygon.Head.Next);

        // Assert
        Assert.False(diagonal);
    }

    
}
