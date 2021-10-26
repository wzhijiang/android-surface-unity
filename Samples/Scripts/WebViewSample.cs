using Igw.Android;
using System;
using System.Collections;
using UnityEngine;

namespace Igw.Samples
{
    public class WebViewSample : MonoBehaviour
    {
        public const int SURFACE_WIDTH = 1920;
        public const int SURFACE_HEIGHT = 1080;
        
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
            m_TouchDetector.onTouch += OnTouch;

            m_ExternalTexture = new ExternalTexture(SURFACE_WIDTH, SURFACE_HEIGHT);
            yield return m_ExternalTexture.WaitForInitialized();

            InitSurface(SURFACE_WIDTH, SURFACE_HEIGHT);

            m_Handler = new Handler(Looper.GetMainLooper());
            yield return m_Handler.PostAsync(() =>
            {
                m_WebView = new SimpleWebView(UnityPlayerActivity.CurrentActivity, UnityPlayerActivity.UnityPlayer.JavaObject, SURFACE_WIDTH, SURFACE_HEIGHT);
                m_WebView.SetSurface(m_Surface.JavaObject);
                m_WebView.LoadUrl("https://www.google.com");
            });
        }

        void Update()
        {
            if (m_ExternalTexture != null)
            {
                m_ExternalTexture.UpdateTexture();
            }
        }

        void InitSurface(int width, int height)
        {
            m_Surface = new Surface(m_ExternalTexture.GetSurfaceTexture());

            int textureId = m_ExternalTexture.GetTextureId();
            if (textureId == 0)
            {
                Debug.LogErrorFormat("Texture create failed: {0}", textureId);
                return;
            }

            Debug.LogFormat("Texture created: {0}", textureId);

            m_Texture = Texture2D.CreateExternalTexture(width, height, TextureFormat.RGBA32, false, true, (IntPtr)textureId);
            m_Renderer.sharedMaterial.mainTexture = m_Texture;
        }

        void OnTouch(Vector2 point, int action)
        {
            if (m_Handler != null)
            {
                m_Handler.Post(() =>
                {
                    m_WebView.DispatchTouchEvent((int)(m_WebView.GetWidth() * point.x), (int)(m_WebView.GetHeight() * point.y), action);
                });
            }
        }
    }
}