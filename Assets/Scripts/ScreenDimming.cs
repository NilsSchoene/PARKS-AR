using UnityEngine;

public class ScreenDimming : MonoBehaviour
{
    /// <summary>
    /// Stops the automatic dimming on the device of the user.
    /// </summary>
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
