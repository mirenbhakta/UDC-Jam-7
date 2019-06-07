using System.Runtime.InteropServices;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
#if UNITY_WEBGL

    [DllImport("__Internal")]
    private static extern void QuitWebGL();

    public void Quit()
    {
        QuitWebGL();
    }

#else

    public void Quit()
    {
        Application.Quit();
    }

#endif
}
