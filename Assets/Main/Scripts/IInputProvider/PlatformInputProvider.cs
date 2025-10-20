using UnityEngine;

public class PlatformInputProvider
{
    public IInputProvider GetPlatformInputProvider()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                return new TouchInputProvider();

            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.LinuxPlayer:
            case RuntimePlatform.WebGLPlayer:
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.OSXEditor:
                return new MouseInputProvider();

            default:
                return new MouseInputProvider(); // fallback
        }
    }
}