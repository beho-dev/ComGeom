using Microsoft.Xna.Framework;

namespace Triangulation;

public class MonoGameUtil
{
    public static Vector2 Vector2FromPoint(Point p)
    {
        return new Vector2(p.X, p.Y);
    }
}
