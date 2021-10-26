using AOT;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Igw.Android
{
    public class ExternalTexture
    {
        private ExternalTextureInternal m_Internal;
        private int m_ID = GenerateID();

        private Handler m_Handler;

        public ExternalTexture(int width, int height)
        {
            Handler handler = new Handler(Looper.MyLooper());
            Init(handler, width, height);
        }

        public ExternalTexture(Handler handler, int width, int height)
        {
            Init(handler, width, height);
        }

        private void Init(Handler handler, int width, int height)
        {
            PostToRenderThread(() =>
            {
                AndroidJNI.AttachCurrentThread();
                
                m_Internal = new ExternalTextureInternal(handler, width, height);
                m_ID = GenerateID();

                s_ExternalTextureInternalMap.TryAdd(m_ID, m_Internal);
            });
        }

        public WaitUntil WaitForInitialized()
        {
            return new WaitUntil(() => m_ID > 0);
        }

        public AndroidJavaObject GetSurfaceTexture()
        {
            return m_Internal.GetSurfaceTexture();
        }

        public int GetTextureId()
        {
            return m_Internal.GetTextureId();
        }

        public void UpdateTexture()
        {
            if (m_ID > 0)
            {
                GL.IssuePluginEvent(s_RenderThreadHandlePtr, ConstructEventID(OP_EXTRENAL_TEXTURE_UPDATE, m_ID));
            }
        }

        public void PostToRenderThread(Action a)
        {
            s_ExecutionQueue.Enqueue(a);
            GL.IssuePluginEvent(s_RenderThreadHandlePtr, OP_EXECUTE_ON_RENDER_THREAD);
        }

        public void Release()
        {
            GL.IssuePluginEvent(s_RenderThreadHandlePtr, ConstructEventID(OP_EXTERNAL_TEXTURE_RELEASE, m_ID));
        }

        private delegate void RenderEventDelegate(int eventID);
        private static RenderEventDelegate s_RenderThreadHandle = new RenderEventDelegate(RunOnRenderThread);
        private static IntPtr s_RenderThreadHandlePtr = Marshal.GetFunctionPointerForDelegate(s_RenderThreadHandle);

        public const int OP_EXECUTE_ON_RENDER_THREAD = 2;
        public const int OP_EXTRENAL_TEXTURE_UPDATE = 3;
        public const int OP_EXTERNAL_TEXTURE_RELEASE = 4;

        private static ConcurrentQueue<Action> s_ExecutionQueue = new ConcurrentQueue<Action>();
        private static ConcurrentDictionary<int, ExternalTextureInternal> s_ExternalTextureInternalMap = new ConcurrentDictionary<int, ExternalTextureInternal>();

        [MonoPInvokeCallback(typeof(RenderEventDelegate))]
        private static void RunOnRenderThread(int eventID)
        {
            int op = GetOperationFromEventID(eventID);
            int externalTextureID = GetExternalTextureIDFromEventID(eventID);

            switch (op)
            {
                case OP_EXECUTE_ON_RENDER_THREAD:
                    {
                        Action a = null;
                        if (s_ExecutionQueue.TryDequeue(out a))
                        {
                            a.Invoke();
                        }
                    }
                    break;
                case OP_EXTRENAL_TEXTURE_UPDATE:
                    {
                        ExternalTextureInternal i;
                        if (s_ExternalTextureInternalMap.TryGetValue(externalTextureID, out i))
                        {
                            i.UpdateTexture();
                        }
                    }
                    break;
                case OP_EXTERNAL_TEXTURE_RELEASE:
                    {
                        ExternalTextureInternal i;
                        if (s_ExternalTextureInternalMap.TryRemove(externalTextureID, out i))
                        {
                            i.Release();
                        }
                    }
                    break;
            }
        }

        private static int GetOperationFromEventID(int eventID)
        {
            return eventID % 100;
        }

        private static int GetExternalTextureIDFromEventID(int eventID)
        {
            return eventID / 100;
        }

        private static int ConstructEventID(int op, int externalTextureID)
        {
            return externalTextureID * 100 + op;
        }

        private static int s_Seq = 0;
        public static int GenerateID()
        {
            s_Seq += 1;
            return s_Seq;
        }
    }
}