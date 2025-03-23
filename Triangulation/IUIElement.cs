using Microsoft.Xna.Framework;

namespace Triangulation.UI;

public interface IUIElement
{
    void Draw(GameTime gameTime, DrawContext drawContext);
    void Update(GameTime gameTime);
}
