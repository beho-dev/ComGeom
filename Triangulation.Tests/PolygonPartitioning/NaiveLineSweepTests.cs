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
        sweep.Sweep(TestShapes.SimpleTriangleVertexA.Position.Y);
        sweep.AddEdges(TestShapes.SimpleTriangleVertexA);

        // Assert
        var edges = sweep.Edges();
        Assert.Equal(2, edges.Count);
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
