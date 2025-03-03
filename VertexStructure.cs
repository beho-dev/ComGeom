using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace comgeom;

public class VertexStructure
{
    public Vector2 Position;
    public VertexStructure Next;
    public VertexStructure Previous;

    public static VertexStructure Create(Vector2 position)
    {
        var vertex = new VertexStructure { Position = position };

        vertex.Next = vertex;
        vertex.Previous = vertex;

        return vertex;
    }

    public static VertexStructure Add(VertexStructure head, Vector2 position)
    {
        var newVertex = new VertexStructure
        {
            Position = position,
            Next = head.Next,
            Previous = head,
        };

        head.Next.Previous = newVertex;
        head.Next = newVertex;

        return newVertex;
    }

    public static VertexStructure FromList(List<Vector2> points)
    {
        var head = Create(points[0]);

        for (int i = 1; i < points.Count; i++)
        {
            Add(head, points[i]);
        }

        return head;
    }

    public static float Area(VertexStructure head)
    {
        float area = 0;
        VertexStructure current = head;

        do
        {
            area += Shapes.TriangleArea(
                current.Position,
                current.Next.Position,
                current.Previous.Position
            );
            current = current.Next;
        } while (current != head);

        return area;
    }
}
