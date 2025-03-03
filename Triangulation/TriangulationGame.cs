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

        var triangle = VertexStructure.FromList(
            [
                center + new Vector2(-100, -100),
                center + new Vector2(100, -100),
                center + new Vector2(150, 0),
                center + new Vector2(100, 100),
                center + new Vector2(0, 0),
                center + new Vector2(-150, 0),
            ]
        );
        DrawVertexStructure(triangle, lineTexture);

        // VertexStructure current = triangle;
        // do
        // {
        //     VertexStructure other = current.Next;
        //     while (other != current)
        //     {
        //         Vector2 edge = other.Position - current.Position;
        //         float length = edge.Length();
        //         float angle = MathF.Atan2(edge.Y, edge.X);
        //         Color color = VertexStructure.Diagonal(current, other) ? Color.Green : Color.Red;
        //         DrawLine(lineTexture, current, length, angle, color);

        //         other = other.Next;
        //     }
        //     current = current.Next;
        // } while (current != triangle);

        DrawDiagonal(lineTexture, triangle, triangle.Next.Next);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawDiagonal(Texture2D lineTexture, VertexStructure a, VertexStructure b)
    {
        DrawEdge(
            lineTexture,
            a.Position,
            b.Position,
            VertexStructure.Diagonal(a, b) ? Color.Green : Color.Red
        );
    }

    private Vector2 ViewportCenter()
    {
        return new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
    }

    private void DrawVertexStructure(VertexStructure head, Texture2D lineTexture)
    {
        VertexStructure current = head;

        do
        {
            DrawEdge(lineTexture, current.Position, current.Next.Position, Color.White);
            current = current.Next;
        } while (current != head);
    }

    private void DrawEdge(Texture2D lineTexture, Vector2 a, Vector2 b, Color color)
    {
        Vector2 edge = b - a;
        float length = edge.Length();
        float angle = MathF.Atan2(edge.Y, edge.X);

        DrawLine(lineTexture, a, length, angle, color);
    }

    private void DrawLine(
        Texture2D lineTexture,
        Vector2 start,
        float length,
        float angle,
        Color color
    )
    {
        _spriteBatch.Draw(
            lineTexture,
            start,
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
