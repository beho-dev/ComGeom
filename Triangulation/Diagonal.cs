namespace Triangulation;

public class Diagonal
{
    /// <summary>
    /// Checks if the edge between a and b crosses any edge in the polygon.
    /// </summary>
    /// <param name="a">the start of the edge</param>
    /// <param name="b">the end of the edge</param>
    /// <returns>true if the edge between a and b crosses any edge in the polygon, false otherwise</returns>
    public static bool DiagonalDoesNotCrossAnyEdge(VertexStructure a, VertexStructure b)
    {
        // considering the edge between a and b
        // we need to check if any of the other edges intersect with it

        // iterate through the edges in the vertex structure
        // the edge is current -> current.Next
        VertexStructure current = a;
        do
        {
            if (current == a || current == b || current.Next == a || current.Next == b)
            {
                // the current edge has either a or b as a vertex
                // so it is not relevant to the diagonal check
                // skip this edge
                current = current.Next;
                continue;
            }

            // check if the current edge intersects with the edge between a and b
            if (Shapes.Intersects(a.Position, b.Position, current.Position, current.Next.Position))
            {
                return false;
            }
            current = current.Next;
        } while (current != a);

        return true;
    }

    /// <summary>
    /// Checks if the edge between a and b falls within the "cone" of the vertex a.
    ///
    /// The cone is defined as the area between the vectors a.Previous - a and a.Next - a.
    /// </summary>
    /// <param name="a">the vertex to check the cone of</param>
    /// <param name="b">the vertex to check if it falls within the cone of</param>
    /// <returns>true if b falls within the cone of a, false otherwise</returns>
    public static bool InCone(VertexStructure a, VertexStructure b)
    {
        return Shapes.InAngle(a.Previous.Position, a.Position, a.Next.Position, b.Position);
    }

    /// <summary>
    /// Checks if the edge between a and b is a diagonal of the polygon.
    /// </summary>
    /// <param name="a">the start of the edge</param>
    /// <param name="b">the end of the edge</param>
    /// <returns>true if the edge between a and b is a diagonal of the polygon, false otherwise</returns>
    public static bool IsDiagonal(VertexStructure a, VertexStructure b)
    {
        return InCone(a, b) && InCone(b, a) && DiagonalDoesNotCrossAnyEdge(a, b);
    }
}
