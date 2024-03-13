using UnityEditor;
using UnityEngine;
//using UnityEngine.iOS;

namespace EPRA.Utilities
{
    public static class Vibrator
    {
#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService","vibrator");
#else
        public static AndroidJavaClass unityPlayer;
        public static AndroidJavaObject currentActivity;
        public static AndroidJavaObject vibrator;
#endif

        private static bool _canVibrate;

        public static bool CanVibrate { get => _canVibrate; set => _canVibrate = value; }

        public static void Vibrate(long milliseconds = 250)
        {
            if (!CanVibrate) return;

            if (IsAndroid())
            {
                vibrator.Call("vibrate", milliseconds);
            }
            else
            {
                Handheld.Vibrate();
            }
        }

        public static void Cancel()
        {
            if (IsAndroid())
            {
                vibrator.Call("cancel");
            }
        }

        public static bool IsAndroid()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return true;
#else
            return false;
#endif
        }
    }
}