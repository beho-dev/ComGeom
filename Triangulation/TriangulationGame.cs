using System;
using System.Transactions;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Triangulation;

internal class Highlight
{
    public bool IsActive = false;
    public Vector2 Position = Vector2.Zero;
    public double Rotation = 0;

    public void Update(GameTime gameTime)
    {
        Rotation = gameTime.TotalGameTime.TotalSeconds * 2;
    }
}

public class TriangulationGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Camera _camera;
    private MouseManager _mouseManager;
    private KeyboardManager _keyboardManager;
    private bool _showDiagonals = false;
    private readonly Highlight _highlight = new() { IsActive = true };

    public TriangulationGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        _camera = new Camera();

        _mouseManager = new MouseManager();
        _mouseManager.OnScroll += (delta) => _camera.Zoom += delta * 0.001f;

        _keyboardManager = new KeyboardManager();
        _keyboardManager.On(Keys.Escape, () => Exit());
        _keyboardManager.On(Keys.Space, () => _showDiagonals = !_showDiagonals);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        _mouseManager.Update();
        _keyboardManager.Update();
        _highlight.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        Texture2D lineTexture = new(GraphicsDevice, 1, 1);
        lineTexture.SetData([Color.White]);

        var center = ViewportCenter();

        var polygon = Polygon.FromList(
            [
                center + new Vector2(-100, -100),
                center + new Vector2(100, -100),
                center + new Vector2(175, 0),
                center + new Vector2(150, 180),
                center + new Vector2(75, 65),
                center + new Vector2(-135, 145),
                center + new Vector2(-50, 30),
                center + new Vector2(-150, -85),
                center + new Vector2(-220, -120),
            ]
        );

        // draw the outline of the polygon in white
        polygon.EachVertex(
            (vertex) =>
            {
                DrawEdge(lineTexture, vertex.Position, vertex.Next.Position, Color.White);
            }
        );

        if (_showDiagonals)
        {
            RenderPolygonDiagonals(lineTexture, polygon);
        }
        else
        {
            RenderTriangulation(polygon);
        }

        if (_highlight.IsActive)
        {
            // Draw a semi-transparent green equilateral triangle
            var vertices = Shapes.EquilateralTriangle(Vector2.Zero, 20);

            var cycle = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 10);
            var opacity = cycle * 0.5f + 0.5f;
            var rotation = Matrix.CreateRotationZ((float)_highlight.Rotation);

            FillTriangle(
                Vector2.Transform(vertices[0], rotation) + ViewportCenter(),
                Vector2.Transform(vertices[1], rotation) + ViewportCenter(),
                Vector2.Transform(vertices[2], rotation) + ViewportCenter(),
                new Color(0.1f, 0.8f, 0.1f, opacity)
            );
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void RenderTriangulation(Polygon polygon)
    {
        var triangles = EarTipRemoval.Triangulate(polygon);
        for (int i = 0; i < triangles.Count; i++)
        {
            var triangle = triangles[i];
            Color color = InterpolateColors(i, triangles.Count);
            FillTriangle(triangle.Item1, triangle.Item2, triangle.Item3, color);
        }
    }

    private void RenderPolygonDiagonals(Texture2D lineTexture, Polygon polygon)
    {
        // draw the diagonals of the polygon in green, if they are valid
        // otherwise, draw them in red
        polygon.EachVertex(
            (a) =>
            {
                polygon.EachVertex(
                    (b) =>
                    {
                        if (a == b || a.Next == b || a.Previous == b)
                        {
                            // a and b are on the same edge
                            // so it is not a valid diagonal
                            return;
                        }

                        Color color = Diagonal.IsDiagonal(a, b) ? Color.Green : Color.Red;
                        DrawEdge(lineTexture, a.Position, b.Position, color);
                    }
                );
            }
        );
    }

    private static Color InterpolateColors(int index, int total)
    {
        var hue = index / (float)total;
        var color = new Color(
            r: (byte)(127 + 127 * MathF.Sin(hue * MathF.PI * 2)),
            g: (byte)(127 + 127 * MathF.Sin(hue * MathF.PI * 2 + MathF.PI * 2 / 3)),
            b: (byte)(127 + 127 * MathF.Sin(hue * MathF.PI * 2 + MathF.PI * 4 / 3))
        );
        return color;
    }

    // draw a filled triangle using the _spriteBatch

    private void FillTriangle(Vector2 a, Vector2 b, Vector2 c, Color color)
    {
        // Create vertices for the triangle
        VertexPositionColor[] vertices = new VertexPositionColor[3];
        vertices[0] = new VertexPositionColor(new Vector3(a, 0), color);
        vertices[1] = new VertexPositionColor(new Vector3(b, 0), color);
        vertices[2] = new VertexPositionColor(new Vector3(c, 0), color);

        // Create or reuse the basic effect
        BasicEffect effect = new(GraphicsDevice);
        effect.VertexColorEnabled = true;

        // Apply zoom to the projection matrix
        Vector2 center = ViewportCenter();
        Matrix zoom = Matrix.CreateScale(_camera.Zoom);
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

    private Vector2 ViewportCenter()
    {
        return new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
    }

    private void DrawEdge(Texture2D lineTexture, Vector2 a, Vector2 b, Color color)
    {
        Vector2 edge = b - a;
        float length = edge.Length();
        float angle = MathF.Atan2(edge.Y, edge.X);

        Vector2 center = ViewportCenter();
        Vector2 zoomedA = center + (a - center) * _camera.Zoom;
        Vector2 zoomedB = center + (b - center) * _camera.Zoom;
        edge = zoomedB - zoomedA;
        length = edge.Length();

        _spriteBatch.Draw(
            lineTexture,
            zoomedA,
            null,
            color,
            angle,
            Vector2.Zero,
            new Vector2(length, 1),
            SpriteEffects.None,
            0
        );
    }
}
