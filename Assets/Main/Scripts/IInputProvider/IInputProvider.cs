using UnityEngine;

public interface IInputProvider
{
    bool IsPressed();
    bool IsReleased();
    Vector2 GetInputPosition();
}