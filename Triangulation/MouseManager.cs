using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Triangulation;

public class MouseManager
{
    private MouseState _previousState;
    public event Action<int> OnScroll;

    public MouseManager()
    {
        _previousState = Mouse.GetState();
        OnScroll = delta => { };
    }

    public void Update()
    {
        var currentState = Mouse.GetState();

        var scrollWheelDelta = currentState.ScrollWheelValue - _previousState.ScrollWheelValue;
        if (Math.Abs(scrollWheelDelta) > 0)
        {
            OnScroll.Invoke(scrollWheelDelta);
        }

        _previousState = currentState;
    }

    public Vector2 Position()
    {
        return new Vector2(_previousState.X, _previousState.Y);
    }
}
