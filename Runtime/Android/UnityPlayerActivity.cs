using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Igw.Android
{
    public class UnityPlayerActivity
    {
        public const string UNITY_ACTIVITY_CLASS = "com.unity3d.player.UnityPlayer";

        private static AndroidJavaObject m_CurrentActivity;
        public static AndroidJavaObject CurrentActivity
        {
            get
            {
                if (m_CurrentActivity == null)
                {
                    using (AndroidJavaClass jc = new AndroidJavaClass(UNITY_ACTIVITY_CLASS))
                    {
                        m_CurrentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
                    }
                }
                return m_CurrentActivity;
            }
        }

        public static AndroidJavaObject GetMainLooper()
        {
            return CurrentActivity.Call<AndroidJavaObject>("getMainLooper");
        }

        private static UnityPlayer s_UnityPlayer;
        public static UnityPlayer UnityPlayer
        {
            get
            { 
                if (s_UnityPlayer == null)
                {
                    s_UnityPlayer = new UnityPlayer(CurrentActivity.Get<AndroidJavaObject>("mUnityPlayer"));
                }
                return s_UnityPlayer;
            }
        }
    }
}