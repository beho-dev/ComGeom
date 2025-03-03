using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace comgeom;

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

        Texture2D lineTexture = new Texture2D(GraphicsDevice, 1, 1);
        lineTexture.SetData(new[] { Color.White });

        var triangle = Shapes.EquilateralTriangle(ViewportCenter(), 100f);
        DrawVertexStructure(triangle, lineTexture);

        _spriteBatch.End();

        base.Draw(gameTime);
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
            Vector2 edge = current.Next.Position - current.Position;
            float length = edge.Length();
            float angle = MathF.Atan2(edge.Y, edge.X);

            _spriteBatch.Draw(
                lineTexture,
                current.Position,
                null,
                Color.White,
                angle,
                Vector2.Zero,
                new Vector2(length, 1),
                SpriteEffects.None,
                0
            );
            current = current.Next;
        } while (current != head);
    }
}
