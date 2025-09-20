using System;
using Microsoft.Xna.Framework;

namespace Triangulation.UI;

public class Highlight : IUIElement
{
    public bool IsActive = false;
    public Point Position = Point.Zero;
    public double Rotation = 0;

    public void Update(GameTime gameTime)
    {
        if (!IsActive)
            return;
        Rotation = gameTime.TotalGameTime.TotalSeconds * 2;
    }

    public void Draw(GameTime gameTime, DrawContext drawContext)
    {
        if (!IsActive)
            return;

        // Draw a semi-transparent green equilateral triangle
        var triangle = Triangle.Equilateral(Point.Zero, 20);

        var cycle = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 10);
        var opacity = cycle * 0.5f + 0.5f;

        drawContext.FillTriangle(triangle, new Color(0.1f, 0.8f, 0.1f, opacity));
    }
}
