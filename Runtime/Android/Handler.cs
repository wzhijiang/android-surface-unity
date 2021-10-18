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

        public WaitForPostExecution PostAsync(Action action, bool throwIfExceptional = true)
        {
            return new WaitForPostExecution(this, action, throwIfExceptional);
        }

        public class WaitForPostExecution : CustomYieldInstruction
        {
            private Action m_Action;
            private Handler m_Host;
            private bool m_ThrowIfExceptional;

            private volatile bool m_IsDone;
            public Exception Exception { get; private set; }

            public WaitForPostExecution(Handler host, Action action, bool throwIfExceptional)
            {
                m_Host = host;
                m_Action = action;
                m_ThrowIfExceptional = throwIfExceptional;

                Start();
            }

            private void Start()
            {
                try
                {
#if UNITY_EDITOR
                    Execute();
#else
                    Debug.Log("Handler PostAsync start");
                    m_Host.Post(Execute);
#endif
                }
                catch (Exception e)
                {
                    Debug.Log("Handler exception " + e.ToString());
                    if (m_ThrowIfExceptional)
                    {
                        throw e;
                    }

                    this.Exception = e;                    
                }
            }

            private void Execute()
            {
                m_Action.Invoke();
                m_IsDone = true;
            }

            public override bool keepWaiting
            {
                get
                {
                    if (this.Exception != null)
                    {
                        return false;
                    }

                    return !m_IsDone;
                }
            }
        }
    }
}