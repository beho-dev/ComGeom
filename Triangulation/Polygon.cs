using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Triangulation;

/// <summary>
/// A class that represents a polygon.
///
/// The polygon is represented as a circular linked list of vertices.
///
/// Each vertex is modelled by the <see cref="VertexStructure"/> class.
/// </summary>
public class Polygon
{
    public VertexStructure Head;

    public Polygon(Vector2 position)
    {
        this.Head = new VertexStructure { Position = position };
        this.Head.Next = this.Head;
        this.Head.Previous = this.Head;
    }

    public void Add(Vector2 position)
    {
        var newVertex = new VertexStructure
        {
            Position = position,
            Next = this.Head,
            Previous = this.Head.Previous,
        };

        this.Head.Previous = newVertex;
        newVertex.Previous.Next = newVertex;
    }

    public static Polygon FromList(List<Vector2> points)
    {
        var polygon = new Polygon(points[0]);

        for (int i = 1; i < points.Count; i++)
        {
            polygon.Add(points[i]);
        }

        return polygon;
    }

    public void EachVertex(System.Action<VertexStructure> action)
    {
        VertexStructure current = this.Head;

        do
        {
            action(current);
            current = current.Next;
        } while (current != this.Head);
    }

    public List<Vector2> Points()
    {
        var vertices = new List<Vector2>();
        this.EachVertex((vertex) => vertices.Add(vertex.Position));
        return vertices;
    }

    public List<Tuple<Vector2, Vector2>> Edges()
    {
        var edges = new List<Tuple<Vector2, Vector2>>();
        this.EachVertex((vertex) => edges.Add(new(vertex.Position, vertex.Next.Position)));
        return edges;
    }

    /// <summary>
    /// Calculates the area of the polygon using the shoelace formula.
    ///
    /// It produces an area that is double the actual area to allow
    /// for calculations to be done in integer precision.
    ///
    /// I believe this method will only work for convex polygons.
    /// </summary>
    /// <returns>The area of the polygon.</returns>
    public float Area2()
    {
        float area = 0;
        this.EachVertex(
            (vertex) =>
            {
                if (vertex == this.Head || vertex.Next == this.Head)
                {
                    // skip the first and last vertex to avoid double counting
                    return;
                }

                area += Shapes.TriangleArea2(
                    this.Head.Position,
                    vertex.Position,
                    vertex.Next.Position
                );
            }
        );

        return area;
    }

    internal Polygon Clone()
    {
        VertexStructure current = this.Head;
        Polygon clone = new(this.Head.Position);
        do
        {
            clone.Add(current.Position);
            current = current.Next;
        } while (current != this.Head);

        return clone;
    }
}
