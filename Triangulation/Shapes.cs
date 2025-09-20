using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Triangulation;

public class Shapes
{
    // TODO move references to triangle class
    public static float TriangleArea2(Point a, Point b, Point c)
    {
        return new Triangle(a, b, c).Area2();
    }

    /// <summary>
    /// Checks if the point c is on the left side of the line segment between a and b.
    /// </summary>
    /// <param name="a">the start of the line segment</param>
    /// <param name="b">the end of the line segment</param>
    /// <param name="c">the point to check if it is on the left side of the line segment</param>
    /// <returns>true if c is on the left side of the line segment, false otherwise</returns>
    public static bool IsLeft(Point a, Point b, Point c)
    {
        return TriangleArea2(a, b, c) > 0;
    }

    /// <summary>
    /// Checks if the point c is on the left side of the line segment between a and b.
    /// </summary>
    /// <param name="a">the start of the line segment</param>
    /// <param name="b">the end of the line segment</param>
    /// <param name="c">the point to check if it is on the left side of the line segment</param>
    /// <returns>true if c is on the left side of the line segment, false otherwise</returns>
    public static bool IsLeftOrOn(Point a, Point b, Point c)
    {
        return TriangleArea2(a, b, c) >= 0;
    }

    public static bool Collinear(Point a, Point b, Point c)
    {
        return TriangleArea2(a, b, c) == 0;
    }

    /// <summary>
    /// Checks if the point c is between the points a and b.
    /// </summary>
    /// <param name="a">the start of the line segment</param>
    /// <param name="b">the end of the line segment</param>
    /// <param name="c">the point to check if it is between a and b</param>
    /// <returns>true if c is between a and b, false otherwise</returns>
    public static bool Between(Point a, Point b, Point c)
    {
        if (!Collinear(a, b, c))
        {
            return false;
        }

        // if a and b are not vertical compare the
        if (a.X != b.X)
        {
            return a.X <= c.X && c.X <= b.X || a.X >= c.X && c.X >= b.X;
        }

        // a and b are vertical so check if c is between a and b
        return a.Y <= c.Y && c.Y <= b.Y || a.Y >= c.Y && c.Y >= b.Y;
    }

    /// <summary>
    /// Checks if the line segment between a and b intersects with the line segment between c and d.
    /// </summary>
    /// <param name="a">the start of the first line segment</param>
    /// <param name="b">the end of the first line segment</param>
    /// <param name="c">the start of the second line segment</param>
    /// <param name="d">the end of the second line segment</param>
    /// <returns>true if the line segments intersect, false otherwise</returns>
    public static bool Intersects(Point a, Point b, Point c, Point d)
    {
        if (IntersectsProperly(a, b, c, d))
        {
            return true;
        }

        // if c or d lies on the line segment between a and b, then the line segments intersect
        // if a or b lies on the line segment between c and d, then the line segments intersect
        return Between(a, b, c) || Between(a, b, d) || Between(c, d, a) || Between(c, d, b);
    }

    /// <summary>
    /// Checks if the line segment between a and b intersects with the line segment between c and d.
    /// </summary>
    /// <param name="a">the start of the first line segment</param>
    /// <param name="b">the end of the first line segment</param>
    /// <param name="c">the start of the second line segment</param>
    /// <param name="d">the end of the second line segment</param>
    /// <returns>true if the line segments intersect, false otherwise</returns>
    public static bool IntersectsProperly(Point a, Point b, Point c, Point d)
    {
        if (Collinear(a, b, c) || Collinear(a, b, d) || Collinear(c, d, a) || Collinear(c, d, b))
        {
            return false;
        }

        return IsLeft(a, b, c) != IsLeft(a, b, d) && IsLeft(c, d, a) != IsLeft(c, d, b);
    }

    public static bool InAngle(Point a, Point b, Point c, Point x)
    {
        if (AngleIsConvex(a, b, c))
        {
            return IsLeft(b, x, a) && IsLeft(x, b, c);
        }

        return !(IsLeftOrOn(b, x, c) && IsLeftOrOn(x, b, a));
    }

    public static bool AngleIsConvex(Point a, Point b, Point c)
    {
        return IsLeftOrOn(a, b, c);
    }

    public static double VectorLength(Point p)
    {
        return Math.Sqrt(Math.Pow(p.X, 2) + Math.Pow(p.Y, 2));
    }

    public static Point? FindClosestVertexWithin(
        List<Point> vertices,
        Point position,
        float maxDistance
    )
    {
        if (vertices.Count == 0)
        {
            return null;
        }

        Point closest = vertices[0];
        double closestDistance = VectorLength(closest - position);
        vertices.ForEach(vertex =>
        {
            var distance = VectorLength(vertex - position);
            if (distance < closestDistance)
            {
                closest = vertex;
                closestDistance = distance;
            }
        });

        return closestDistance <= maxDistance ? closest : null;
    }

    public static string DescribeEdge(Edge<VertexStructure>? edge)
    {
        if (edge == null)
        {
            return "null";
        }

        return $"{edge.From.Position} -> {edge.To.Position}";
    }
}
