// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("oDtB23f7qaa8tlGDF7Z/a1rKazkrD6C/32ooKDMByIGLhbwTJ/pBIJA5deZIb0ulZajWRdn02rFfBAuNFDxlRcUWLA+JB+QrCaSu4JNMBjyF8l0nGlF3uwTKnXQO1ocPCkQkd80gEKnXp3C2RVIZjIj2oa5jS5XEf/zy/c1//Pf/f/z8/VSvmbfb7fzl3xk/ZMdC2mnoWgSJJPAu6nmiP/wnEAF19+aq4uH71o2GHKXrR0D4zX/8383w+/TXe7V7CvD8/Pz4/f6gCy0GCUv1OPzPx4EyYhXIXg71dI2Owj94b1r86fDEyKXhGIhHbsNXqr495HJXl2x7PAfvA3sAnDbi5BbYj548DrGUCjreF4qPRztQctNusOMZBJkZcKg1UP/+/P38");
        private static int[] order = new int[] { 5,4,9,13,9,7,9,10,13,10,12,12,12,13,14 };
        private static int key = 253;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
