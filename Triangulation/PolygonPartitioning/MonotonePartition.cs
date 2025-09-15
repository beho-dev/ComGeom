using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Triangulation.PolygonPartitioning;

namespace Triangulation;

// define a type alias for the edge type
using Edge = Tuple<VertexStructure, VertexStructure>;

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
    public static List<Tuple<Vector2, Vector2, Vector2>> Triangulate(Polygon polygon)
    {
        var diagonals = CalculateDiagonals(polygon);
        return [];
    }

    public static List<Edge> CalculateDiagonals(Polygon polygon)
    {
        Console.WriteLine("Trapezoidalizing");
        List<VertexStructure> vertices = PolygonVerticalSort.Sort(polygon);

        // keep track of a set of edges that are currently pierced by the line
        // the list should be kept in order from left to right
        // chose between algorithms by commenting out the line below

        // ILineSweep lineSweep = new TreeLineSweep();
        ILineSweep lineSweep = new NaiveLineSweep();

        List<Edge> diagonals = [];

        foreach (var vertex in vertices)
        {
            Console.WriteLine($"======= Processing vertex {vertex.Position}");

            lineSweep.Sweep(vertex.Position.Y);

            var vertexType = VertexTypeClassifier.ClassifyVertex(vertex);
            Console.WriteLine($"Vertex type: {vertexType}");

            VertexStructure? support;
            switch (vertexType)
            {
                case VertexType.Start:
                    lineSweep.AddEdges(vertex);
                    break;
                case VertexType.Split:
                    support = lineSweep.AddEdges(vertex);
                    if (support != null)
                    {
                        diagonals.Add(new(support, vertex));
                    }
                    break;
                case VertexType.Merge:
                    support = lineSweep.RemoveEdges(vertex);
                    if (support != null)
                    {
                        diagonals.Add(new(vertex, support));
                    }
                    break;
                case VertexType.End:
                    support = lineSweep.RemoveEdges(vertex);
                    if (support != null)
                    {
                        diagonals.Add(new(vertex, support));
                    }
                    break;
                case VertexType.Regular:
                    support = lineSweep.ReplaceEdge(vertex);
                    if (support != null)
                    {
                        diagonals.Add(new(support, vertex));
                    }
                    break;
            }
        }

        return diagonals;
    }
}
