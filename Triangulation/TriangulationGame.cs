using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Triangulation.UI;

namespace Triangulation;

public class TriangulationGame : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Camera _camera;
    private DrawContext _drawContext;

    private MouseManager _mouseManager;
    private KeyboardManager _keyboardManager;
    private bool _showDiagonals = false;
    private Polygon _polygon;
    private List<Tuple<Point, Point, Point>> _triangulation;
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
        _drawContext = new DrawContext(_camera, _graphics.GraphicsDevice);

        _mouseManager = new MouseManager();
        _mouseManager.OnScroll += (delta) => _camera.Zoom += delta * 0.001f;

        _keyboardManager = new KeyboardManager();
        _keyboardManager.On(Keys.Escape, Exit);
        _keyboardManager.On(Keys.Space, () => _showDiagonals = !_showDiagonals);

        var center = _drawContext.ViewportCenter();
        // TODO replace this with something that is applied at draw time
        var offset = new Point((int)center.X, (int)center.Y);
        // _polygon = Polygon.FromList(
        //     [
        //         center + new Point(-100, -100),
        //         center + new Point(100, -100),
        //         center + new Point(175, 0),
        //         center + new Point(150, 180),
        //         center + new Point(75, 65),
        //         center + new Point(-135, 145),
        //         center + new Point(-50, 30),
        //         center + new Point(-150, -85),
        //         center + new Point(-220, -120),
        //     ]
        // );
        _polygon = Polygon.FromList(
            [
                // bottom left
                offset + new Point(-100, -100),
                //centre
                offset + new Point(0, 0),
                // bottom right
                offset + new Point(100, -100),
                // top right
                offset + new Point(100, 100),
                // top left
                offset + new Point(-100, 120),
            ]
        );

        _triangulation = MonotonePartition.Triangulate(_polygon);
        // _triangulation = EarTipRemoval.Triangulate(_polygon);
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
        Nullable<Point> closest = Shapes.FindClosestVertexWithin(
            _polygon.Points(),
            mousePosition,
            10
        );

        if (!closest.HasValue)
        {
            _highlight.IsActive = false;
            Mouse.SetCursor(MouseCursor.Arrow); // Default cursor
            return;
        }

        _highlight.IsActive = true;
        _highlight.Position = closest.Value;
        Mouse.SetCursor(MouseCursor.Hand); // Hand cursor when over a vertex
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

        _highlight.Draw(gameTime, _drawContext);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void RenderTriangulation()
    {
        for (int i = 0; i < _triangulation.Count; i++)
        {
            var triangle = _triangulation[i];
            Color color = InterpolateColors(i, _triangulation.Count);
            _drawContext.FillTriangle(
                new Triangle(triangle.Item1, triangle.Item2, triangle.Item3),
                color
            );
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

    private void DrawEdge(Texture2D lineTexture, Edge<Point> edge, Color color)
    {
        DrawEdge(lineTexture, edge.From, edge.To, color);
    }

    private void DrawEdge(Texture2D lineTexture, Point pointA, Point pointB, Color color)
    {
        Vector2 a = MonoGameUtil.Vector2FromPoint(pointA);
        Vector2 b = MonoGameUtil.Vector2FromPoint(pointB);
        Vector2 edge = b - a;
        float angle = MathF.Atan2(edge.Y, edge.X);

        Vector2 center = _drawContext.ViewportCenter();
        Vector2 zoomedA = center + (a - center) * _camera.Zoom;
        Vector2 zoomedB = center + (b - center) * _camera.Zoom;
        edge = zoomedB - zoomedA;

        _spriteBatch.Draw(
            lineTexture,
            zoomedA,
            null,
            color,
            angle,
            Vector2.Zero,
            new Vector2(edge.Length(), 1),
            SpriteEffects.None,
            0
        );
    }
}
