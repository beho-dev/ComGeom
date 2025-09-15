using System;
using System.Collections.Generic;

namespace Triangulation.PolygonPartitioning;

class PolygonVerticalSort
{
    /// <summary>
    /// A linear time sort of the vertices of a polygon by their vertical position.
    /// </summary>
    /// <param name="polygon">The polygon to sort.</param>
    /// <returns>A list of vertices sorted by their vertical position.</returns>
    public static List<VertexStructure> Sort(Polygon polygon)
    {
        var vertices = polygon.Vertices();
        Console.WriteLine("Sorting vertices by vertical position");
        vertices.Sort((a, b) => (int)(b.Position.Y - a.Position.Y));

        // TODO implement linear sort
        // polygon.EachVertex((vertex) => Console.WriteLine($"Vertex: {vertex.Position}"));

        return vertices;
    }
}
