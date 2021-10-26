using UnityEngine;

namespace Igw.Android
{
    public class Looper
    {
        private static AndroidJavaObject s_JavaClass;
        public static AndroidJavaObject JavaClass
        {
            get
            {
                if (s_JavaClass == null)
                {
                    s_JavaClass = new AndroidJavaClass("android.os.Looper");
                }
                return s_JavaClass;
            }
        }

        public static AndroidJavaObject MyLooper()
        {
            return JavaClass.CallStatic<AndroidJavaObject>("myLooper");
        }

        public static AndroidJavaObject GetMainLooper()
        {
            return JavaClass.CallStatic<AndroidJavaObject>("getMainLooper");
        }

        public static void Prepare()
        {
            JavaClass.CallStatic("prepare");
        }
    }
}
