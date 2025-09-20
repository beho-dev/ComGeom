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

    public Polygon(Point position)
    {
        Head = new VertexStructure { Position = position };
        Head.Next = Head;
        Head.Previous = Head;
    }

    public void Add(Point position)
    {
        var newVertex = new VertexStructure
        {
            Position = position,
            Next = Head,
            Previous = Head.Previous,
        };

        Head.Previous = newVertex;
        newVertex.Previous.Next = newVertex;
    }

    public static Polygon FromList(List<Point> points)
    {
        var polygon = new Polygon(points[0]);

        for (int i = 1; i < points.Count; i++)
        {
            polygon.Add(points[i]);
        }

        return polygon;
    }

    public void EachVertex(Action<VertexStructure> action)
    {
        VertexStructure current = Head;

        do
        {
            action(current);
            current = current.Next;
        } while (current != Head);
    }

    public List<VertexStructure> Vertices()
    {
        var vertices = new List<VertexStructure>();
        EachVertex(vertices.Add);
        return vertices;
    }

    public List<Point> Points()
    {
        var vertices = new List<Point>();
        EachVertex((vertex) => vertices.Add(vertex.Position));
        return vertices;
    }

    public List<Edge<Point>> Edges()
    {
        var edges = new List<Edge<Point>>();
        EachVertex((vertex) => edges.Add(new(vertex.Position, vertex.Next.Position)));
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
        EachVertex(
            (vertex) =>
            {
                if (vertex == Head || vertex.Next == Head)
                {
                    // skip the first and last vertex to avoid double counting
                    return;
                }

                area += new Triangle(Head.Position, vertex.Position, vertex.Next.Position).Area2();
            }
        );

        return area;
    }

    internal Polygon Clone()
    {
        VertexStructure current = Head;
        Polygon clone = new(Head.Position);
        do
        {
            clone.Add(current.Position);
            current = current.Next;
        } while (current != Head);

        return clone;
    }
}
