using Microsoft.Xna.Framework;

namespace Triangulation;

public class Camera
{
    private float _zoom = 1.0f;

    public float Zoom
    {
        get => _zoom;
        set => _zoom = MathHelper.Clamp(value, 0.1f, 5f);
    }
}
