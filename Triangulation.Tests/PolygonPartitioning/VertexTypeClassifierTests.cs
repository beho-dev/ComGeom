using Microsoft.Xna.Framework;
using Triangulation.PolygonPartitioning;

namespace Triangulation.Tests.PolygonPartitioning;

public class VertexTypeClassifierTests
{
    [Fact]
    public void ClassifyStartVertex()
    {
        // Arrange
        VertexStructure vertex = TestShapes.SimpleTriangleVertexA;

        // Act
        var type = VertexTypeClassifier.ClassifyVertex(vertex);

        // Assert
        Assert.Equal(VertexType.Start, type);
    }

    [Fact]
    public void ClassifyRegularVertex()
    {
        // Arrange
        VertexStructure vertex = TestShapes.SimpleTriangleVertexC;

        // Act
        var type = VertexTypeClassifier.ClassifyVertex(vertex);

        // Assert
        Assert.Equal(VertexType.Regular, type);
    }

    [Fact]
    public void ClassifyEndVertex()
    {
        // Arrange
        VertexStructure vertex = TestShapes.SimpleTriangleVertexB;

        // Act
        var type = VertexTypeClassifier.ClassifyVertex(vertex);

        // Assert
        Assert.Equal(VertexType.End, type);
    }

    [Fact]
    public void ClassifyMergeVertex()
    {
        // Arrange
        VertexStructure vertex = TestShapes.BoxWithDownwardCuspVertexE;

        // Act
        var type = VertexTypeClassifier.ClassifyVertex(vertex);

        // Assert
        Assert.Equal(VertexType.Merge, type);
    }

    [Fact]
    public void ClassifySplitVertex()
    {
        // Arrange
        VertexStructure vertex = TestShapes.BoxWithUpwardCuspVertexC;

        // Act
        var type = VertexTypeClassifier.ClassifyVertex(vertex);

        // Assert
        Assert.Equal(VertexType.Split, type);
    }
}
