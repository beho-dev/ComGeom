using System;
using Microsoft.Xna.Framework;

namespace Triangulation;

public record Triangle(Vector2 A, Vector2 B, Vector2 C)
{
    public static Triangle Equilateral(Vector2 center, float sideLength)
    {
        float height = sideLength * MathF.Sqrt(3) / 2f;
        return new Triangle(
            center + new Vector2(0, -height * 2 / 3f),
            center + new Vector2(sideLength / 2f, height / 3f),
            center + new Vector2(-sideLength / 2f, height / 3f)
        );
    }

    public Triangle Transform(Func<Vector2, Vector2> transform) =>
        new(transform(A), transform(B), transform(C));

    public (T, T, T) TransformTo<T>(Func<Vector2, T> transform) =>
        (transform(A), transform(B), transform(C));

    public T[] MapTo<T>(Func<Vector2, T> transform) => [transform(A), transform(B), transform(C)];

    public Triangle Rotate(float rotation, Vector2 position)
    {
        var rotationMatrix = Matrix.CreateRotationZ(rotation);
        return Transform(p => Vector2.Transform(p, rotationMatrix) + position);
    }

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
    public float Area2()
    {
        return (B.X - A.X) * (C.Y - A.Y) - (B.Y - A.Y) * (C.X - A.X);
    }
}
