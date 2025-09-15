using Microsoft.Xna.Framework;
using Triangulation.PolygonPartitioning;

namespace Triangulation.Tests.PolygonPartitioning;

public class NaiveLineSweepTests
{
    [Fact]
    public void AddEdgesToEmptyList()
    {
        // Arrange
        NaiveLineSweep sweep = new();

        // Act
        sweep.Sweep(100);
        sweep.AddEdges(TestShapes.SimpleTriangleVertexA);
        var edges = sweep.Edges();

        // Assert
        Assert.Equal(
            new(TestShapes.SimpleTriangleVertexA, TestShapes.SimpleTriangleVertexB),
            edges[0]
        );
        Assert.Equal(
            new(TestShapes.SimpleTriangleVertexA, TestShapes.SimpleTriangleVertexC),
            edges[1]
        );
    }
}
