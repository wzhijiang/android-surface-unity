using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Igw.Android
{
    public class UnityPlayer
    {
        private AndroidJavaObject m_JavaObject;
        public AndroidJavaObject JavaObject
        {
            get { return m_JavaObject; }
        }

        public UnityPlayer(AndroidJavaObject javaObject)
        {
            m_JavaObject = javaObject;
        }
    }
}
