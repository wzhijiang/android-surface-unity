using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Igw.Android
{
    public class SimpleWebView
    {
        private static AndroidJavaClass s_JavaClass;
        public static AndroidJavaClass JavaClass
        {
            get
            {
                if (s_JavaClass == null)
                {
                    s_JavaClass = new AndroidJavaClass("io.github.wzhijiang.android.surface.SimpleWebView");
                }
                return s_JavaClass;
            }
        }

        private AndroidJavaObject m_JavaObject;
        public AndroidJavaObject JavaObject
        {
            get { return m_JavaObject; }
        }

        private int m_Width;
        private int m_Height;

        public SimpleWebView(AndroidJavaObject jContext, AndroidJavaObject parent, int width, int height)
        {
            m_Width = width;
            m_Height = height;

            m_JavaObject = JavaClass.CallStatic<AndroidJavaObject>("create", jContext, parent, width, height, 0);
        }

        public void LoadUrl(string url)
        {
            m_JavaObject.Call("loadUrl", url);
        }

        public void SetSurface(AndroidJavaObject jSurface)
        {
            m_JavaObject.Call("setSurface", jSurface);
        }

        public bool DispatchTouchEvent(int x, int y, int action)
        {
            return m_JavaObject.Call<bool>("dispatchTouchEvent", x, y, action);
        }

        public int GetWidth()
        {
            return m_Width;
        }

        public int GetHeight()
        {
            return m_Height;
        }
    }
}
