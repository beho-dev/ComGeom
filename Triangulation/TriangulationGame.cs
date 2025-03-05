using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Triangulation;

public class DrawTriangleCommand(VertexPositionColor[] Vertices)
{
    public VertexPositionColor[] Vertices = Vertices;

    public static DrawTriangleCommand FromVertices(List<Vector2> vertices, Color color)
    {
        return new DrawTriangleCommand(
            [
                new VertexPositionColor(new Vector3(vertices[0], 0), color),
                new VertexPositionColor(new Vector3(vertices[1], 0), color),
                new VertexPositionColor(new Vector3(vertices[2], 0), color),
            ]
        );
    }
}

internal class Highlight
{
    public bool IsActive = false;
    public Vector2 Position = Vector2.Zero;
    public double Rotation = 0;

    public void Update(GameTime gameTime)
    {
        Rotation = gameTime.TotalGameTime.TotalSeconds * 2;
    }

    internal void Draw(GameTime gameTime, Camera camera, GraphicsDevice graphicsDevice)
    {
        if (!IsActive)
        {
            return;
        }
        // Draw a semi-transparent green equilateral triangle
        var vertices = Shapes.EquilateralTriangle(Vector2.Zero, 20);

        var cycle = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 10);
        var opacity = cycle * 0.5f + 0.5f;
        var rotation = Matrix.CreateRotationZ((float)Rotation);

        TriangulationGame.DrawTriangle(
            DrawTriangleCommand.FromVertices(
                [
                    Vector2.Transform(vertices[0], rotation) + Position,
                    Vector2.Transform(vertices[1], rotation) + Position,
                    Vector2.Transform(vertices[2], rotation) + Position,
                ],
                new Color(0.1f, 0.8f, 0.1f, opacity)
            ),
            camera,
            graphicsDevice
        );
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
    private Polygon _polygon;
    private List<Tuple<Vector2, Vector2, Vector2>> _triangulation;
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

        var center = ViewportCenter();
        _polygon = Polygon.FromList(
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

        _triangulation = EarTipRemoval.Triangulate(_polygon);
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

        var mousePosition = _mouseManager.Position();
        _highlight.Position = _polygon.Vertices().MinBy(v => (v - mousePosition).Length());

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        Texture2D lineTexture = new(GraphicsDevice, 1, 1);
        lineTexture.SetData([Color.White]);

        // draw the outline of the polygon in white
        _polygon.Edges().ForEach((edge) => DrawEdge(lineTexture, edge, Color.White));

        if (_showDiagonals)
        {
            RenderPolygonDiagonals(lineTexture);
        }
        else
        {
            RenderTriangulation();
        }

        _highlight.Draw(gameTime, _camera, GraphicsDevice);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void RenderTriangulation()
    {
        for (int i = 0; i < _triangulation.Count; i++)
        {
            var triangle = _triangulation[i];
            Color color = InterpolateColors(i, _triangulation.Count);
            FillTriangle(triangle.Item1, triangle.Item2, triangle.Item3, color);
        }
    }

    private void RenderPolygonDiagonals(Texture2D lineTexture)
    {
        // draw the diagonals of the polygon in green, if they are valid
        // otherwise, draw them in red
        _polygon.EachVertex(
            (a) =>
            {
                _polygon.EachVertex(
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

        DrawTriangle(new DrawTriangleCommand(vertices), _camera, GraphicsDevice);
    }

    public static void DrawTriangle(
        DrawTriangleCommand command,
        Camera camera,
        GraphicsDevice graphicsDevice
    )
    {
        // Create or reuse the basic effect
        BasicEffect effect = new(graphicsDevice) { VertexColorEnabled = true };

        // Apply zoom to the projection matrix
        Vector2 center = ViewportCenter(graphicsDevice);
        Matrix zoom = Matrix.CreateScale(camera.Zoom);
        Matrix translation =
            Matrix.CreateTranslation(new Vector3(-center, 0))
            * zoom
            * Matrix.CreateTranslation(new Vector3(center, 0));

        effect.Projection =
            translation
            * Matrix.CreateOrthographicOffCenter(
                0,
                graphicsDevice.Viewport.Width,
                graphicsDevice.Viewport.Height,
                0,
                0,
                1
            );

        // Draw the triangle
        foreach (EffectPass pass in effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, command.Vertices, 0, 1);
        }
    }

    private Vector2 ViewportCenter()
    {
        return ViewportCenter(GraphicsDevice);
    }

    public static Vector2 ViewportCenter(GraphicsDevice graphicsDevice)
    {
        return new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
    }

    private void DrawEdge(Texture2D lineTexture, Tuple<Vector2, Vector2> edge, Color color)
    {
        DrawEdge(lineTexture, edge.Item1, edge.Item2, color);
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
