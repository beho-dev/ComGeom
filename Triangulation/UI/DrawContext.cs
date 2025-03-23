using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Triangulation.UI;

public class DrawContext(Camera Camera, GraphicsDevice GraphicsDevice)
{
    public void FillTriangle(Triangle triangle, Color color)
    {
        DrawTriangles(triangle.MapTo(p => new VertexPositionColor(new Vector3(p, 0), color)));
    }

    public Vector2 ViewportCenter()
    {
        return new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
    }

    private void DrawTriangles(VertexPositionColor[] vertices)
    {
        // Create or reuse the basic effect
        BasicEffect effect = new(GraphicsDevice) { VertexColorEnabled = true };

        // Apply zoom to the projection matrix
        Vector2 center = ViewportCenter();
        Matrix zoom = Matrix.CreateScale(Camera.Zoom);
        Matrix translation =
            Matrix.CreateTranslation(new Vector3(-center, 0))
            * zoom
            * Matrix.CreateTranslation(new Vector3(center, 0));

        effect.Projection =
            translation
            * Matrix.CreateOrthographicOffCenter(
                0,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                0,
                0,
                1
            );

        // Draw the triangle
        foreach (EffectPass pass in effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 1);
        }
    }
}
