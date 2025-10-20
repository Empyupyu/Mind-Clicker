using UnityEngine;

public class TouchInputProvider : IInputProvider
{
    public bool IsPressed() => Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    public bool IsReleased() => Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended;
    public Vector2 GetInputPosition() => Input.GetTouch(0).position;
}