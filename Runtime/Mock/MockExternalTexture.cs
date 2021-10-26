using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Igw.Android
{
    public class MockExternalTexture
    {
        public MockExternalTexture(Handler handler, int width, int height)
        {

        }

        public AndroidJavaObject GetSurfaceTexture()
        {
            return null;
        }

        public int GetTextureId()
        {
            return 0;
        }

        public bool UpdateTexture()
        {
            return false;
        }
    }
}