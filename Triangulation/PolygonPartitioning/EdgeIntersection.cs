using System;

namespace Triangulation.PolygonPartitioning;

class EdgeIntersection
{
    // find the horizontal coordinate where the edge between two points a and b crosses a vertical height
    public static float EdgeHorizontalIntersection(Edge<VertexStructure> edge, float verticalHeight)
    {
        var a = edge.From.Position;
        var b = edge.To.Position;

        if (verticalHeight < Math.Min(a.Y, b.Y) || verticalHeight > Math.Max(a.Y, b.Y))
        {
            // The horizontal line at our desired vertical height does not pierce the edge
            throw new Exception("Unexpected check for unpierced edge");
        }

        // If the edge is horizontal or the points are the same, return the x-coordinate of the first point
        if (b.Y - a.Y == 0)
        {
            Console.WriteLine(
                $"The edge is horizontal - returning the x coordinate closest to the left"
            );
            return Math.Min(a.X, b.X);
        }

        // Use linear interpolation to find the x-coordinate
        // t = (y - y1) / (y2 - y1)
        // x = x1 + t(x2 - x1)
        float t = (verticalHeight - a.Y) / (b.Y - a.Y);
        var result = a.X + t * (b.X - a.X);
        return result;
    }
}
