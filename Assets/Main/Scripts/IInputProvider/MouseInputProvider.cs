using UnityEngine;

public class MouseInputProvider : IInputProvider
{
    public bool IsPressed() => Input.GetMouseButtonDown(0);
    public bool IsReleased() => Input.GetMouseButtonUp(0);
    public Vector2 GetInputPosition() => Input.mousePosition;
}