using System;
using Microsoft.Xna.Framework;

namespace comgeom;

public class Shapes
{
    public static VertexStructure EquilateralTriangle(Vector2 center, float sideLength)
    {
        float height = sideLength * MathF.Sqrt(3) / 2f;
        return VertexStructure.FromList(
            [
                center + new Vector2(0, -height * 2 / 3f),
                center + new Vector2(-sideLength / 2f, height / 3f),
                center + new Vector2(sideLength / 2f, height / 3f),
            ]
        );
    }

    public static float TriangleArea(Vector2 a, Vector2 b, Vector2 c)
    {
        return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
    }

    public static Boolean isLeft(Vector2 a, Vector2 b, Vector2 c)
    {
        return TriangleArea(a, b, c) > 0;
    }

    public static Boolean isLeftOrOn(Vector2 a, Vector2 b, Vector2 c)
    {
        return TriangleArea(a, b, c) <= 0;
    }

    public static Boolean Collinear(Vector2 a, Vector2 b, Vector2 c)
    {
        return TriangleArea(a, b, c) == 0;
    }

    public static Boolean IntersectsProperly(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        if (Collinear(a, b, c) || Collinear(a, b, d) || Collinear(c, d, a) || Collinear(c, d, b))
        {
            return false;
        }

        return isLeft(a, b, c) != isLeft(a, b, d) && isLeft(c, d, a) != isLeft(c, d, b);
    }

    public static Boolean Between(Vector2 a, Vector2 b, Vector2 c)
    {
        if (!Collinear(a, b, c))
        {
            return false;
        }

        if (a.X != b.X)
        {
            return ((a.X <= c.X) && (c.X <= b.X)) || ((a.X >= c.X) && (c.X >= b.X));
        }
        else
        {
            return ((a.Y <= c.Y) && (c.Y <= b.Y)) || ((a.Y >= c.Y) && (c.Y >= b.Y));
        }
    }

    public static Boolean Intersects(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        if (IntersectsProperly(a, b, c, d))
        {
            return true;
        }

        if (Between(a, b, c) || Between(a, b, d) || Between(c, d, a) || Between(c, d, b))
        {
            return true;
        }

        return false;
    }

    internal static object EquilateralTriangle(object value, float v)
    {
        throw new NotImplementedException();
    }
}
