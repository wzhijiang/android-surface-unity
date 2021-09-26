using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Igw.Android
{
    public class Surface
    {
        private AndroidJavaObject m_JavaObject;
        public AndroidJavaObject JavaObject
        {
            get { return m_JavaObject; }
        }

        public Surface(AndroidJavaObject surfaceTexture)
        {
            m_JavaObject = new AndroidJavaObject("android.view.Surface", surfaceTexture);
        }
    }
}