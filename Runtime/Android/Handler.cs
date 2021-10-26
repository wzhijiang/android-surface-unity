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

        public Handler(AndroidJavaObject jLooper)
        {
            Init(jLooper);
        }

        private void Init(AndroidJavaObject jLooper)
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
#if UNITY_EDITOR
                Execute();
#else
                m_Host.Post(Execute);
#endif
            }

            private void Execute()
            {
                try
                {
                    m_Action.Invoke();
                }
                catch (Exception e)
                {
                    this.Exception = e;
                }

                m_IsDone = true;
            }

            public override bool keepWaiting
            {
                get
                {
                    if (this.Exception != null)
                    {
                        if (m_ThrowIfExceptional)
                        {
                            throw this.Exception;
                        }

                        return false;
                    }

                    return !m_IsDone;
                }
            }
        }
    }
}