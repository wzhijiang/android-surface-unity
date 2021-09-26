using System;
using UnityEngine;

namespace Igw.Android
{
    public class Handler
    {
        private AndroidJavaObject m_JavaObject;
        public AndroidJavaObject JavaObject
        {
            get { return m_JavaObject; }
        }

        public Handler()
        {
            m_JavaObject = new AndroidJavaObject("android.os.Handler");
        }

        public Handler(AndroidJavaObject jLooper)
        {
            m_JavaObject = new AndroidJavaObject("android.os.Handler", jLooper);
        }

        public void Post(Action action)
        {
            m_JavaObject.Call<bool>("post", new AndroidJavaRunnable(action));
        }
    }
}