using Microsoft.Xna.Framework;
using Triangulation.PolygonPartitioning;

namespace Triangulation.Tests.PolygonPartitioning;

public class NaiveLineSweepTests
{
    // a triangle shaped a bit like this
    // A
    // |\
    // | B
    // |/
    // C
    static readonly Polygon simpleTriangle = Polygon.FromList(
        [new(0, 100), new(100, 0), new(0, -100)]
    );
    static readonly VertexStructure A = simpleTriangle.Vertices()[0];
    static readonly VertexStructure B = simpleTriangle.Vertices()[1];
    static readonly VertexStructure C = simpleTriangle.Vertices()[2];

    [Fact]
    public void AddEdgesToEmptyList()
    {
        // Arrange
        NaiveLineSweep sweep = new();

        // Act
        sweep.Sweep(100);
        sweep.AddEdges(simpleTriangle.Vertices().First());
        var edges = sweep.Edges();

        // Assert
        Assert.Equal(edges[0], new(A, C));
        Assert.Equal(edges[1], new(A, B));
    }
}
