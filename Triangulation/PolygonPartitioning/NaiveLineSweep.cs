using System;
using System.Collections.Generic;

namespace Triangulation.PolygonPartitioning;

// let's assume in this class that all edges are with vertex structures
using Edge = Edge<VertexStructure>;

public class NaiveLineSweep : ILineSweep
{
    private readonly List<Edge> _edges;

    /// <summary>
    /// Maintain a parallel list of supporting vertices for each edge.
    /// This is used to keep track of what the best diagnoal to add is when we need it.
    /// </summary>
    private readonly List<VertexStructure> _supports;
    private float _sweepY;

    public NaiveLineSweep()
    {
        _edges = [];
        _supports = [];
        _sweepY = 0;
    }

    public void Sweep(float sweepY)
    {
        _sweepY = sweepY;
    }

    public List<Edge> Edges()
    {
        return _edges;
    }

    // get the two neighbouring edges of the vertex ordered in the x direction
    private static Tuple<Edge, Edge> GetLeftRightEdges(VertexStructure vertex)
    {
        var edgeA = new Edge(vertex.Previous, vertex);
        var edgeB = new Edge(vertex, vertex.Next);

        if (vertex.Previous.Position.X < vertex.Next.Position.X)
        {
            return new(edgeA, edgeB);
        }
        else
        {
            return new(edgeB, edgeA);
        }
    }

    public VertexStructure? AddEdges(VertexStructure vertex)
    {
        Console.WriteLine("Current edges:");
        foreach (var edge in _edges)
        {
            Console.WriteLine(Shapes.DescribeEdge(edge));
        }

        var (leftEdge, rightEdge) = GetLeftRightEdges(vertex);

        Console.WriteLine(
            $"Adding edges {Shapes.DescribeEdge(leftEdge)} and {Shapes.DescribeEdge(rightEdge)}"
        );
        var i = _edges.FindLastIndex(e =>
        {
            var intersection = EdgeIntersection.EdgeHorizontalIntersection(e, _sweepY);
            return intersection < vertex.Position.X;
        });

        if (i == -1)
        {
            i = _edges.Count;
        }

        var support = i < _supports.Count ? _supports[i] : null;

        _edges.InsertRange(i, [leftEdge, rightEdge]);

        // for new edges the supporting vertex is the vertex itself
        _supports.InsertRange(i, [vertex, vertex]);

        return support;
    }

    // remove the two neighbouring edges for a given vertex from the line sweep
    public VertexStructure? RemoveEdges(VertexStructure vertex)
    {
        Console.WriteLine("Current edges:");
        foreach (var edge in _edges)
        {
            Console.WriteLine(Shapes.DescribeEdge(edge));
        }

        // get the two neighbouring edges for the vertex
        var (leftEdge, rightEdge) = GetLeftRightEdges(vertex);
        Console.WriteLine(
            $"Removing edges {Shapes.DescribeEdge(leftEdge)} and {Shapes.DescribeEdge(rightEdge)}"
        );

        // find where the edge is in the list of current edges for the line sweep
        // specifically what is the first edge in our list that intersects with our current sweep height
        // at the position of our current vertex
        var i = _edges.FindIndex(e =>
        {
            // the x value of where the edge meets our sweep y
            var intersection = EdgeIntersection.EdgeHorizontalIntersection(e, _sweepY);
            return intersection == vertex.Position.X;
        });

        if (i == -1)
        {
            throw new Exception(
                $"Edge {Shapes.DescribeEdge(leftEdge)} or {Shapes.DescribeEdge(rightEdge)} could not be removed because was not present"
            );
        }

        var support = _supports[i];

        _edges.RemoveRange(i, 2);
        // TODO what are we supposed to do with the supporting vertices?
        _supports.RemoveRange(i, 2);

        return support;
    }

    public VertexStructure? ReplaceEdge(VertexStructure vertex)
    {
        var previousEdge = new Edge(vertex.Previous, vertex);
        var nextEdge = new Edge(vertex, vertex.Next);

        if (vertex.Previous.Position.Y < vertex.Position.Y)
        {
            // we are on the outside edge ascending
            return ReplaceEdge(nextEdge, previousEdge);
        }
        else
        {
            // we are on the outside edge descending
            return ReplaceEdge(previousEdge, nextEdge);
        }
    }

    private VertexStructure? ReplaceEdge(Edge removedEdge, Edge addedEdge)
    {
        Console.WriteLine(
            $"Replacing edge {Shapes.DescribeEdge(removedEdge)} with {Shapes.DescribeEdge(addedEdge)}"
        );
        var i = _edges.IndexOf(removedEdge);
        if (i == -1)
        {
            throw new Exception(
                $"Edge {Shapes.DescribeEdge(removedEdge)} could not be replaced because was not present"
            );
        }

        var support = _supports[i];

        _edges[i] = addedEdge;
        // _supports do not change when replacing edges

        return support;
    }
}
