using Microsoft.Xna.Framework;
using Triangulation.PolygonPartitioning;

namespace Triangulation.Tests.PolygonPartitioning;

public class TestShapes
{
    // a triangle shaped a bit like this
    // A
    // |\
    // | C
    // |/
    // B
    static public readonly Polygon SimpleTriangle = Polygon.FromList(
        // assumes anti-clockwise vertex order [A, B, C]
        [new(0, 100), new(0, -100), new(100, 0)]
    );

    public static readonly VertexStructure SimpleTriangleVertexA = SimpleTriangle.Vertices()[0];
    public static readonly VertexStructure SimpleTriangleVertexB = SimpleTriangle.Vertices()[1];
    public static readonly VertexStructure SimpleTriangleVertexC = SimpleTriangle.Vertices()[2];

    // a box with a downward interior cusp, shaped a bit like this
    //
    // A   D
    // |\ /|
    // | E |
    // B---C
    public static readonly Polygon BoxWithDownwardCusp = Polygon.FromList(
        // [A, B, C, D, E]
        [new(-100, 100), new(-100, -100), new(100, -100), new(100, 100), new(0, 0)]
    );

    public static readonly VertexStructure BoxWithDownwardCuspVertexE =
        BoxWithDownwardCusp.Vertices()[4];

    // a box with an upward interior cusp, shaped a bit like this
    //
    // A---E
    // | C |
    // |/ \|
    // B   D
    public static readonly Polygon BoxWithUpwardCusp = Polygon.FromList(
        // [A, B, C, D, E]
        [new(-100, 100), new(-100, -100), new(0, 0), new(100, -100), new(100, 100)]
    );

    public static readonly VertexStructure BoxWithUpwardCuspVertexC = BoxWithUpwardCusp.Vertices()[
        2
    ];
}
