using System.Runtime.InteropServices;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
#if UNITY_WEBGL

    private static class WebGL
    {
        [DllImport("__Internal")]
        public static extern void Quit();

    }

    public void Quit()
    {
        WebGL.Quit();
    }

#else

    public void Quit()
    {
        Application.Quit();
    }

#endif
}
