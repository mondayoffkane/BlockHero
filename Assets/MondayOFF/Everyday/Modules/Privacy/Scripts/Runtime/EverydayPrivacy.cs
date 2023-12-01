
using UnityEngine;
using System.Runtime.InteropServices;

namespace MondayOFF
{
    public static class Privacy
    {
        internal static int IS_GDPR_APPLICABLE => GetGDPRApplicable();
        internal static string GDPR_STRING => GetTCF2String();
        internal static string CCPA_STRING = "";
        internal static bool HAS_ATT_CONSENT = true;

        private static AttAuthorizationStatus _attAuthorizationStatus = AttAuthorizationStatus.NotDetermined;
        private static System.Action<AttAuthorizationStatus> _onAppTrackingAllow = default;

        private static void Initialize(AttAuthorizationStatus authorizationStatus)
        {
            EverydayLogger.Info($"Initialize {authorizationStatus}");

            _attAuthorizationStatus = authorizationStatus;
            HAS_ATT_CONSENT = _attAuthorizationStatus == AttAuthorizationStatus.Authorized;

            // CMP first
            _onAppTrackingAllow?.Invoke(_attAuthorizationStatus);
        }

        private static string GetTCF2String()
        {
#if UNITY_EDITOR
            return null;
#endif
            string tcfString = null;
#if UNITY_ANDROID 
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass preferenceManagerClass = new AndroidJavaClass("android.preference.PreferenceManager");
            AndroidJavaObject sharedPreferences = preferenceManagerClass.CallStatic<AndroidJavaObject>("getDefaultSharedPreferences", currentActivity);
            tcfString = sharedPreferences.Call<string>("getString", "IABTCF_TCString", "");
#elif UNITY_IOS 
            tcfString = PlayerPrefs.GetString("IABTCF_TCString", null);
#endif
            return tcfString;
        }

        private static int GetGDPRApplicable()
        {
#if UNITY_EDITOR
            return 0;
#endif

            int isGdprApplicable = 0;

#if UNITY_ANDROID
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass preferenceManagerClass = new AndroidJavaClass("android.preference.PreferenceManager");
            AndroidJavaObject sharedPreferences = preferenceManagerClass.CallStatic<AndroidJavaObject>("getDefaultSharedPreferences", currentActivity);
            isGdprApplicable = sharedPreferences.Call<int>("getInt", "IABTCF_gdprApplies", 0);
#elif UNITY_IOS
            isGdprApplicable = PlayerPrefs.GetInt("IABTCF_gdprApplies", 0);
#endif
            return isGdprApplicable;
        }


#if UNITY_IOS && !UNITY_EDITOR
        /// <summary>Requests App Tracking Authorization to a user.</summary>
        /// <param name="onAllowCallback">Delegate to be called on authorization. True only if the user allows app tracking.</param>
        internal static void RequestTrackingAuthorization(System.Action<AttAuthorizationStatus> onAllowCallback) {
            _onAppTrackingAllow = onAllowCallback;

            _RequestTrackingAuthorization(OnCompleteCallback);
        }
        [DllImport("__Internal")]
        private static extern void _RequestTrackingAuthorization(System.Action<AttAuthorizationStatus> onAllowCallback);

        public static void OpenAppSettings() {
            _OpenAppSettings();
        }
        [DllImport("__Internal")]
        private static extern void _OpenAppSettings();

        [AOT.MonoPInvokeCallback(typeof(System.Action<int>))]
        private static void OnCompleteCallback(AttAuthorizationStatus status) {
            Initialize(status);
        }

#else
        public static void OpenAppSettings()
        {
            // TODO: Implement for Android Sandbox if needed
        }
        internal static void RequestTrackingAuthorization(System.Action<AttAuthorizationStatus> onAllowCallback)
        {
            _onAppTrackingAllow = onAllowCallback;

            // No action required for Android
            Initialize(AttAuthorizationStatus.Authorized);
        }
#endif
    }
}
