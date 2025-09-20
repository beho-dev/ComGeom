using Microsoft.Xna.Framework;

namespace Triangulation.Tests;

public class DiagonalTests
{
    [Fact]
    public void DiagonalDoesNotCrossAnyEdge()
    {
        // Arrange
        var polygon = Polygon.FromList(
            [new Point(0, 0), new Point(1, 0), new Point(1, 1), new Point(0, 1)]
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
            [new Point(0, 0), new Point(4, 0), new Point(4, 4), new Point(2, 1), new Point(0, 4)]
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
                new Point(0, 0),
                new Point(4, 0),
                new Point(6, 2),
                new Point(4, 4),
                new Point(2, 1),
                new Point(0, 4),
                new Point(-2, 2),
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
            [new Point(0, 0), new Point(1, 0), new Point(1, 1), new Point(0, 1)]
        );

        // Act
        var diagonal = Diagonal.IsDiagonal(polygon.Head, polygon.Head.Next);

        // Assert
        Assert.False(diagonal);
    }
}
