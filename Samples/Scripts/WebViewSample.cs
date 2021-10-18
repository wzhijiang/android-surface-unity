using Igw.Android;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Igw.Samples
{
#if UNITY_EDITOR
    using ExternalTexture = MockExternalTexture;
#endif

    public class WebViewSample : MonoBehaviour
    {
        [SerializeField]
        private Renderer m_Renderer;
        [SerializeField]
        private TouchDetector m_TouchDetector;
        
        private SimpleWebView m_WebView;

        private Surface m_Surface;
        private ExternalTexture m_ExternalTexture;

        private Texture2D m_Texture;

        private Handler m_Handler;

        IEnumerator Start()
        {
            m_Handler = new Handler(Activity.GetMainLooper());
            m_TouchDetector.onTouch += OnTouch;

            InitSurface(1920, 1080);

            yield return m_Handler.PostAsync(() =>
            {
                m_WebView = new SimpleWebView(Activity.CurrentActivity, Activity.UnityPlayer.JavaObject, 1920, 1080);
                m_WebView.SetSurface(m_Surface.JavaObject);
                m_WebView.LoadUrl("https://www.google.com");
            });
        }

        void Update()
        {
            m_ExternalTexture.UpdateTexture();
        }

        void InitSurface(int width, int height)
        {
            m_ExternalTexture = new ExternalTexture(m_Handler, width, height);
            m_Surface = new Surface(m_ExternalTexture.GetSurfaceTexture());

            int textureId = m_ExternalTexture.GetTextureId();
            Debug.LogFormat("Texture created: {0}", textureId);

            m_Texture = Texture2D.CreateExternalTexture(width, height, TextureFormat.RGBA32, false, true, (IntPtr)textureId);
            m_Renderer.sharedMaterial.mainTexture = m_Texture;
        }

        void OnTouch(Vector2 point, int action)
        {
            m_Handler.Post(() =>
            {
                m_WebView.DispatchTouchEvent((int)(m_WebView.GetWidth() * point.x), (int)(m_WebView.GetHeight() * point.y), action);
            });
        }
    }
}