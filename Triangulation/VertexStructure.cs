using System;
using Microsoft.Xna.Framework;

namespace Triangulation;

/// <summary>
/// A structure that represents a vertex in a polygon.
///
/// The polygon is represented as a circular linked list of vertices.
///
/// General polygon methods are abstracted in the <see cref="Polygon"/> class.
/// </summary>
public class VertexStructure
{
    public Vector2 Position;
    public VertexStructure Next;
    public VertexStructure Previous;

    public override string ToString()
    {
        return $"Vertex({Position})";
    }
}
