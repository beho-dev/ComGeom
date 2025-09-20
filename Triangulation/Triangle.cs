using System;
using Microsoft.Xna.Framework;

namespace Triangulation;

public record Triangle(Point A, Point B, Point C)
{
    public static Triangle Equilateral(Point center, int sideLength)
    {
        int height = (int)Math.Floor(sideLength * MathF.Sqrt(3) / 2);
        return new Triangle(
            center + new Point(0, -height * 2 / 3),
            center + new Point(sideLength / 2, height / 3),
            center + new Point(-sideLength / 2, height / 3)
        );
    }

    public Triangle Transform(Func<Point, Point> transform) =>
        new(transform(A), transform(B), transform(C));

    public (T, T, T) TransformTo<T>(Func<Point, T> transform) =>
        (transform(A), transform(B), transform(C));

    public T[] MapTo<T>(Func<Point, T> transform) => [transform(A), transform(B), transform(C)];

    /// <summary>
    /// Calculates twice the area of a triangle.
    ///
    /// This is done to avoid floating point precision issues.
    ///
    /// N.B. this is more a principal of the textbook I am
    /// following, the monogame framework works in floating point,
    /// precision, we may want to limit our game logic to integer
    /// coordinates for fun later.
    /// </summary>
    /// <param name="a">the first vertex of the triangle</param>
    /// <param name="b">the second vertex of the triangle</param>
    /// <param name="c">the third vertex of the triangle</param>
    /// <returns>twice the area of the triangle</returns>
    public int Area2()
    {
        return (B.X - A.X) * (C.Y - A.Y) - (B.Y - A.Y) * (C.X - A.X);
    }
}
