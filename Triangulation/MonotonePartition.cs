using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using Microsoft.Xna.Framework;

namespace Triangulation;

/// <summary>
/// Triangulates a polygon using the monotone partition algorithm.
///
/// This is faster than the EarTipRemoval algorithm because it
/// works in n log(n) time.
/// </summary>
public class MonotonePartition
{
    /// <summary>
    /// Triangulates a polygon using the monotone partition algorithm.
    /// </summary>
    /// <param name="polygon">The polygon to triangulate.</param>
    /// <returns>A list of triangles.</returns>
    public static List<System.Tuple<Vector2, Vector2, Vector2>> Triangulate(Polygon polygon)
    {
        var trapezoids = Trapezoidalize(polygon);
        return [];
    }

    private static bool IsVertexConnectedToEdge(
        VertexStructure vertex,
        Tuple<VertexStructure, VertexStructure> edge
    )
    {
        // add null check
        return edge.Item1 == vertex || edge.Item2 == vertex;
    }

    private static float EdgeHorizontalIntersection(
        Tuple<VertexStructure, VertexStructure> edge,
        float verticalHeight
    )
    {
        return EdgeHorizontalIntersection(edge.Item1.Position, edge.Item2.Position, verticalHeight);
    }

    // find the horizontal coordinate where the edge between two points a and b crosses a vertical height
    private static float EdgeHorizontalIntersection(Vector2 a, Vector2 b, float verticalHeight)
    {
        if (verticalHeight < Math.Min(a.Y, b.Y) || verticalHeight > Math.Max(a.Y, b.Y))
        {
            // The horizontal line at our desired vertical height does not pierce the edge
            throw new Exception("Unexpected check for unpierced edge");
        }

        // If the edge is horizontal or the points are the same, return the x-coordinate of the first point
        if (b.Y - a.Y == 0)
        {
            return a.X;
        }

        // Use linear interpolation to find the x-coordinate
        // t = (y - y1) / (y2 - y1)
        // x = x1 + t(x2 - x1)
        float t = (verticalHeight - a.Y) / (b.Y - a.Y);
        return a.X + t * (b.X - a.X);
    }

    public static List<Tuple<Vector2, Vector2, Vector2, Vector2>> Trapezoidalize(Polygon input)
    {
        List<VertexStructure> vertices = [];
        input.EachVertex(v => vertices.Add(v));
        vertices.Sort((a, b) => (int)(b.Position.Y - a.Position.Y));

        // keep track of a set of edges that are currently pierced by the line
        // the list should be kept in order from left to right
        List<Tuple<VertexStructure, VertexStructure>> piercedEdges = [];
        List<Tuple<Vector2, Vector2, Vector2, Vector2>> trapezoids = [];

        foreach (var vertex in vertices)
        {
            // if piereced eges is empty then let's kick things off
            if (piercedEdges.Count == 0)
            {
                piercedEdges.AddRange([new(vertex.Previous, vertex), new(vertex, vertex.Next)]);
                continue;
            }

            // find the pierced edge to the left of our vertex
            var i = 0;
            while (
                i < piercedEdges.Count
                && EdgeHorizontalIntersection(piercedEdges[i], vertex.Position.Y)
                    < vertex.Position.X
            )
            {
                i++;
            }

            // TODO check that i - 1 is actually to the left
            var leftEdge = i > 0 ? piercedEdges[i - 1] : null;
            var rightEdge = i < piercedEdges.Count ? piercedEdges[i] : null;

            // Create trapezoids between the current vertex and the intersections
            if (leftEdge != null)
            {
                trapezoids.Add(
                    // TODO CORRECT THE DEFINITON OF THESE TRAPEZOIDS
                    new Tuple<Vector2, Vector2, Vector2, Vector2>(
                        leftEdge.Item1.Position,
                        leftEdge.Item2.Position,
                        new Vector2(
                            EdgeHorizontalIntersection(leftEdge, vertex.Position.Y),
                            vertex.Position.Y
                        ),
                        vertex.Position
                    )
                );
            }

            if (rightEdge != null)
            {
                trapezoids.Add(
                    new Tuple<Vector2, Vector2, Vector2, Vector2>(
                        rightEdge.Item1.Position,
                        rightEdge.Item2.Position,
                        vertex.Position,
                        new Vector2(
                            EdgeHorizontalIntersection(rightEdge, vertex.Position.Y),
                            vertex.Position.Y
                        )
                    )
                );
            }

            // Update piercedEdges
            if (
                IsVertexConnectedToEdge(vertex, leftEdge)
                && IsVertexConnectedToEdge(vertex, rightEdge)
            )
            {
                piercedEdges.RemoveRange(i - 1, 2);
            }
            else if (IsVertexConnectedToEdge(vertex, leftEdge))
            {
                piercedEdges[i - 1] = new(vertex, vertex.Next);
            }
            else if (IsVertexConnectedToEdge(vertex, rightEdge))
            {
                piercedEdges[i] = new(vertex, vertex.Next);
            }
            else
            {
                piercedEdges.InsertRange(
                    i,
                    [new(vertex.Previous, vertex), new(vertex, vertex.Next)]
                );
            }
        }

        return trapezoids;
    }
}
