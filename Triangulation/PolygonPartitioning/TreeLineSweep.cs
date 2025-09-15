using System;
using System.Collections.Generic;

namespace Triangulation.PolygonPartitioning;

using Edge = Tuple<VertexStructure, VertexStructure>;

// class TreeLineSweep : ILineSweep
// {
//     /// <summary>
//     /// The edges that are currently pierced by the sweep line
//     ///
//     /// SortedList uses an efficient tree algorithm to maintain the order of the edges
//     /// and so we can use it to efficiently find the next edge to process in O(log n)
//     /// </summary>
//     private readonly SortedList<Edge, Edge> _edges;
//     private readonly LineSweepEdgeComparer _edgeComparer;

//     public TreeLineSweep()
//     {
//         _edgeComparer = new LineSweepEdgeComparer();
//         _edges = new SortedList<Edge, Edge>(_edgeComparer);
//     }

//     /// <summary>
//     /// Updates the Y-coordinate for all subsequent comparisons.
//     /// MUST be called before any other operation at a new event point.
//     /// </summary>
//     /// <param name="y">The new y-coordinate of the sweep line.</param>
//     public void Sweep(float sweepY)
//     {
//         _edgeComparer.SweepY = sweepY;
//     }

//     public void Add(Edge edge)
//     {
//         _edges.Add(edge, edge);
//     }


//     public void Remove(Edge edge)
//     {
//         _edges.Remove(edge);
//     }
// }

// class LineSweepEdgeComparer : IComparer<Edge>
// {
//     public float SweepY;

//     public int Compare(Edge? e1, Edge? e2)
//     {
//         if (e1 == null || e2 == null)
//         {
//             throw new Exception("Unexpected null edge");
//         }

//         if (e1 == e2)
//         {
//             return 0;
//         }

//         var x1 = EdgeIntersection.EdgeHorizontalIntersection(e1, SweepY);
//         var x2 = EdgeIntersection.EdgeHorizontalIntersection(e2, SweepY);

//         // is problematic if the edges share a vertex, which is very likely
//         // TODO fix this
//         // in terms of the business logic we know when this will happen and how it should be handled
//         return x1.CompareTo(x2);
//     }
// }
