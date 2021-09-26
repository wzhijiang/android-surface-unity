using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Igw.Android
{
    public class Activity
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
    }
}