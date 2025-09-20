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

    [Fact]
    public void AddEdgesToRightOfSweep()
    {
        // Arrange
        NaiveLineSweep sweep = new();

        // Act
        sweep.AddEdges(TestShapes.BoxWithDownwardCuspVertexA);
        sweep.AddEdges(TestShapes.BoxWithDownwardCuspVertexD);

        // Assert
        var edges = sweep.Edges();
        Assert.Equal(4, edges.Count);
        Assert.Equal(
            new(TestShapes.BoxWithDownwardCuspVertexA, TestShapes.BoxWithDownwardCuspVertexB),
            edges[0]
        );
        Assert.Equal(
            new(TestShapes.BoxWithDownwardCuspVertexA, TestShapes.BoxWithDownwardCuspVertexE),
            edges[1]
        );
        // new edges are added here to the right
        Assert.Equal(
            new(TestShapes.BoxWithDownwardCuspVertexD, TestShapes.BoxWithDownwardCuspVertexE),
            edges[2]
        );
        Assert.Equal(
            new(TestShapes.BoxWithDownwardCuspVertexD, TestShapes.BoxWithDownwardCuspVertexC),
            edges[3]
        );
    }

    [Fact]
    public void AddEdgesToLeftOfSweep()
    {
        // Arrange
        NaiveLineSweep sweep = new();

        // Act
        sweep.AddEdges(TestShapes.BoxWithDownwardCuspVertexD);
        sweep.AddEdges(TestShapes.BoxWithDownwardCuspVertexA);

        // Assert
        var edges = sweep.Edges();
        Assert.Equal(4, edges.Count);
        Assert.Equal(
            new(TestShapes.BoxWithDownwardCuspVertexA, TestShapes.BoxWithDownwardCuspVertexB),
            edges[0]
        );
        Assert.Equal(
            new(TestShapes.BoxWithDownwardCuspVertexA, TestShapes.BoxWithDownwardCuspVertexE),
            edges[1]
        );
        // new edges are added here to the right
        Assert.Equal(
            new(TestShapes.BoxWithDownwardCuspVertexD, TestShapes.BoxWithDownwardCuspVertexE),
            edges[2]
        );
        Assert.Equal(
            new(TestShapes.BoxWithDownwardCuspVertexD, TestShapes.BoxWithDownwardCuspVertexC),
            edges[3]
        );
    }

    [Fact]
    public void AddEdgesToMiddleOfSweep()
    {
        // Arrange
        NaiveLineSweep sweep = new();

        // Act
        sweep.AddEdges(TestShapes.BoxWithUpwardCuspVertexA);
        sweep.AddEdges(TestShapes.BoxWithUpwardCuspVertexE);
        sweep.AddEdges(TestShapes.BoxWithUpwardCuspVertexC);

        // Assert
        var edges = sweep.Edges();
        Assert.Equal(4, edges.Count);
        Assert.Equal(
            new(TestShapes.BoxWithUpwardCuspVertexA, TestShapes.BoxWithUpwardCuspVertexB),
            edges[0]
        );
        Assert.Equal(
            new(TestShapes.BoxWithUpwardCuspVertexC, TestShapes.BoxWithUpwardCuspVertexB),
            edges[1]
        );
        // new edges are added here to the right
        Assert.Equal(
            new(TestShapes.BoxWithUpwardCuspVertexC, TestShapes.BoxWithUpwardCuspVertexD),
            edges[2]
        );
        Assert.Equal(
            new(TestShapes.BoxWithUpwardCuspVertexE, TestShapes.BoxWithUpwardCuspVertexD),
            edges[3]
        );
    }
}
