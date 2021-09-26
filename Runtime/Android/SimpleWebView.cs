using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Igw.Android
{
    public class SimpleWebView
    {
        private AndroidJavaObject m_JavaObject;
        public AndroidJavaObject JavaObject
        {
            get { return m_JavaObject; }
        }

        public SimpleWebView(AndroidJavaObject jContext, int width, int height)
        {
            m_JavaObject = new AndroidJavaObject("io.github.wzhijiang.android.surface.SimpleWebView", jContext);
            m_JavaObject.Call("layout", 0, 0, width, height);
            m_JavaObject.Call("initSettings");
        }

        public void LoadUrl(string url)
        {
            m_JavaObject.Call("loadUrl", url);
        }

        public void SetSurface(AndroidJavaObject jSurface)
        {
            m_JavaObject.Call("setSurface", jSurface);
        }

        public void DrawSurface()
        {
            m_JavaObject.Call("drawSurface");
        }
    }
}
