using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Igw.Android
{
    public class ExternalTexture
    {
        private AndroidJavaObject m_JavaObject;

        public ExternalTexture(Handler handler, int width, int height)
        {
            m_JavaObject = new AndroidJavaObject("io.github.wzhijiang.android.surface.ExternalTexture", handler.JavaObject, width, height);
        }

        public AndroidJavaObject GetSurfaceTexture()
        {
            return m_JavaObject.Call<AndroidJavaObject>("getSurfaceTexture");
        }

        public int GetTextureId()
        {
            return m_JavaObject.Call<int>("getTextureId");
        }

        public bool UpdateTexture()
        {
            return m_JavaObject.Call<bool>("updateTexture");
        }
    }
}