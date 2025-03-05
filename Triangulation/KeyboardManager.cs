using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Triangulation;

public class KeyboardManager
{
    private KeyboardState _previousState;
    private readonly Dictionary<Keys, Action> _listeningKeys = [];

    public KeyboardManager() { }

    internal void Update()
    {
        var currentState = Keyboard.GetState();

        // one is referring to keyboard keys the other is referring to map keys
        foreach (var key in _listeningKeys.Keys)
        {
            if (currentState.IsKeyDown(key) && !_previousState.IsKeyDown(key))
            {
                _listeningKeys[key].Invoke();
            }
        }

        _previousState = currentState;
    }

    public void On(Keys key, Action callback)
    {
        _listeningKeys.Add(key, callback);
    }
}
