#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
using System;
using System.Runtime.InteropServices;
#endif

namespace Adverty.PlatformSpecific
{
    public class VideoPlayerBridge
    {
#if UNITY_ANDROID || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        protected const string LIBRARY_NAME = "AdvertyVideoPlayer";
#elif UNITY_IOS
        protected const string LIBRARY_NAME = "__Internal";
#endif

#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        [DllImport(LIBRARY_NAME, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetVideoPlayerLibraryVersion();
#endif

        public string GetVideoPlayerVersion()
        {
#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
            IntPtr versionStrPtr = GetVideoPlayerLibraryVersion();
            return Marshal.PtrToStringUni(versionStrPtr);
#else
            return string.Empty;
#endif
        }
    }
}
