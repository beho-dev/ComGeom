using System;

namespace Triangulation;

// the triangulation course assumes that everything is in int space to avoid
// issues with floating point accuracy
public struct Point(int X, int Y)
{
    public int X { get; private set; } = X;
    public int Y { get; private set; } = Y;

    public override string ToString()
    {
        return $"Point({X},{Y})";
    }

    // a bit weird to have vector arithmetic on points but I'll allow it for now
    public static Point operator +(Point value1, Point value2)
    {
        return new Point(value1.X + value2.X, value1.Y + value2.Y);
    }

    public static Point operator -(Point value1, Point value2)
    {
        return new Point(value1.X - value2.X, value1.Y - value2.Y);
    }

    public static readonly Point Zero = new(0, 0);
}
