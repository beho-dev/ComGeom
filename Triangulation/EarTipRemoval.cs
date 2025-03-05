using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Triangulation;

/// <summary>
/// Triangulates a polygon using the ear tip removal algorithm.
///
/// The algorithm works by repeatedly removing ears from the polygon until
/// only a triangle remains.
///
/// An ear is a vertex that is part of a triangle with two edges that are
/// diagonals of the polygon.
/// </summary>
public class EarTipRemoval
{
    /// <summary>
    /// Triangulates a polygon using the ear tip removal algorithm.
    /// </summary>
    /// <param name="polygon">The polygon to triangulate.</param>
    /// <returns>A list of triangles.</returns>
    public static List<System.Tuple<Vector2, Vector2, Vector2>> Triangulate(Polygon input)
    {
        Polygon polygon = input.Clone();

        // a set of vertices that have been identified as ears
        HashSet<VertexStructure> ears = [];

        // keep track of the diagonals that have been removed
        List<System.Tuple<Vector2, Vector2, Vector2>> removed = [];

        // the number of vertices in the polygon
        int n = 0;
        polygon.EachVertex(v =>
        {
            n++;
            if (Diagonal.IsDiagonal(v.Previous, v.Next))
            {
                ears.Add(v);
            }
        });

        // while there are more than 3 vertices in the polygon
        // i.e. while the polygon is not yet a triangle
        while (n > 3)
        {
            VertexStructure v = polygon.Head;

            do
            {
                if (!ears.Remove(v))
                {
                    v = v.Next;
                }

                // we have found an ear, remove it
                removed.Add(new(v.Previous.Position, v.Position, v.Next.Position));

                // join up the removed ends
                v.Previous.Next = v.Next;
                v.Next.Previous = v.Previous;

                // check if the previous and next vertices are now ears
                if (Diagonal.IsDiagonal(v.Previous.Previous, v.Previous.Next))
                {
                    ears.Add(v.Previous);
                }
                else
                {
                    ears.Remove(v.Previous);
                }

                if (Diagonal.IsDiagonal(v.Next.Previous, v.Next.Next))
                {
                    ears.Add(v.Next);
                }
                else
                {
                    ears.Remove(v.Next);
                }

                // update the number of vertices
                n--;

                // the polygon might have had its head removed
                if (v == polygon.Head)
                {
                    polygon.Head = v.Next;
                }

                // break out of the inner loop
                break;
            } while (v != polygon.Head);
        }

        removed.Add(
            new(polygon.Head.Position, polygon.Head.Next.Position, polygon.Head.Next.Next.Position)
        );

        return removed;
    }
}
