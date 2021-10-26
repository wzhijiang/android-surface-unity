using AOT;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Igw.Android
{
    public class ExternalTextureInternal
    {
        private AndroidJavaObject m_JavaObject;

        public ExternalTextureInternal(Handler handler, int width, int height)
        {
            AndroidJavaObject jHandler = handler != null ? handler.JavaObject : null;
            m_JavaObject = new AndroidJavaObject("io.github.wzhijiang.android.surface.ExternalTexture", jHandler, width, height);
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

        public void Release()
        {
            m_JavaObject.Call("release");
        }
    }
}