using Igw.Android;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Igw.Samples
{
    public class WebViewSample : MonoBehaviour
    {
        [SerializeField]
        private Renderer m_Renderer;
        
        private SimpleWebView m_WebView;

        private Surface m_Surface;
        private ExternalTexture m_ExternalTexture;

        void Start()
        {
            InitSurface(1920, 1080);

            m_WebView = new SimpleWebView(Activity.CurrentActivity, 1920, 1080);
            m_WebView.SetSurface(m_Surface.JavaObject);
            m_WebView.LoadUrl("https://www.google.com");
        }

        void Update()
        {
            m_WebView.DrawSurface();
            m_ExternalTexture.UpdateTexture();
        }

        void InitSurface(int width, int height)
        {
            var handler = new Handler(Activity.GetMainLooper());
            
            m_ExternalTexture = new ExternalTexture(handler, width, height);
            m_Surface = new Surface(m_ExternalTexture.GetSurfaceTexture());

            int textureId = m_ExternalTexture.GetTextureId();
            Debug.LogFormat("Texture created: {0}", textureId);

            var texture = Texture2D.CreateExternalTexture(width, height, TextureFormat.RGBA32, false, true, (IntPtr)textureId);
            m_Renderer.sharedMaterial.mainTexture = texture;
        }
    }
}