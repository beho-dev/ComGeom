using System;
using System.Transactions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Triangulation;

public class TriangulationGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public TriangulationGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
            Exit();

        // TODO: Add your update logic here

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

        // // draw the diagonals of the polygon in green, if they are valid
        // // otherwise, draw them in red
        // polygon.EachVertex(
        //     (a) =>
        //     {
        //         polygon.EachVertex(
        //             (b) =>
        //             {
        //                 if (a == b || a.Next == b || a.Previous == b)
        //                 {
        //                     // a and b are on the same edge
        //                     // so it is not a valid diagonal
        //                     return;
        //                 }

        //                 Color color = Diagonal.IsDiagonal(a, b) ? Color.Green : Color.Red;
        //                 DrawEdge(lineTexture, a.Position, b.Position, color);
        //             }
        //         );
        //     }
        // );

        var triangles = EarTipRemoval.Triangulate(polygon);
        for (int i = 0; i < triangles.Count; i++)
        {
            var triangle = triangles[i];
            Color color = InterpolateColors(i, triangles.Count);
            FillTriangle(triangle.Item1, triangle.Item2, triangle.Item3, color);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
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
        effect.Projection = Matrix.CreateOrthographicOffCenter(
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

        _spriteBatch.Draw(
            lineTexture,
            a,
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
