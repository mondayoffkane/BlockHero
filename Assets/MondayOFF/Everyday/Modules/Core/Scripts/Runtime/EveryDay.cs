using UnityEngine;

namespace MondayOFF
{
    public static partial class EveryDay
    {
        public const string Version = "3.0.34";

        public static event System.Action OnATTComplete = new System.Action(InitializeEveryday);
        public static bool isInitialized => initializationStatus == InitializationStatus.Initialized;

        internal static System.Action OnEverydayInitialized = default;
        private static InitializationStatus initializationStatus = InitializationStatus.NotInitialized;

        public static void Initialize()
        {
            if (EverydaySettings.Instance.initializeOnLaunch)
            {
                EverydayLogger.Warn("initializeOnLaunch is true. Please do not call Initialize() manually.");
                return;
            }

            InitializeImpl();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AfterSceneLoad()
        {
            initializationStatus = InitializationStatus.NotInitialized;
            if (EverydaySettings.Instance.initializeOnLaunch)
            {
                MainThreadDispatcher.Instance.EnqueueCoroutine(InitializeAfterDelay());
            }
        }

        private static System.Collections.IEnumerator InitializeAfterDelay()
        {
            yield return new WaitForSeconds(EverydaySettings.Instance.initializationDelay);
            InitializeImpl();
        }

        private static void InitializeImpl()
        {
            if (initializationStatus != InitializationStatus.NotInitialized)
            {
                EverydayLogger.Warn("Everyday is already initialized or initializing!");
                return;
            }

            initializationStatus = InitializationStatus.Initializing;

            Privacy.RequestTrackingAuthorization(OnTrackingAuthorized);
        }

        private static void InitializeEveryday()
        {
            var initMessage =
@$"
================== Everyday {Version} ==================
    Log Level: {EverydaySettings.Instance.logLevel}
    Test Mode: {EverydaySettings.Instance.isTestMode}
========================================================
";
            EverydayLogger.Info(initMessage);

            // Initialize Facebook SDK
            InitializeFacebook();

            // Initialize Singular SDK
            InitializeSingularSDK();

            // Initialize Max SDK
            InitializeMaxSdk();
        }

        private static void OnTrackingAuthorized(AttAuthorizationStatus consentStatus)
        {
            EverydayLogger.Debug($"Consent status: {consentStatus}");

            PrepareSettings(consentStatus);

            OnATTComplete?.Invoke();
        }

        private static void InitializeFacebook()
        {
            EverydayLogger.Info("Initializing Facebook SDK");
            FacebookInitializer.Initialize();
        }

        private static void InitializeSingularSDK()
        {
            // Initialize Singular
            EverydayLogger.Info("Initializing Singular SDK");
            if (SingularSDK.instance != null)
            {
                Object.Destroy(SingularSDK.instance);
                SingularSDK.instance = null;
            }
            var singularGO = new GameObject("SingularSDKObject", typeof(SingularSDK));
            SingularSDK.InitializeSingularSDK(Keys.EVERYDAY_SINGULAR_API_KEY, Keys.EVERYDAY_SINGULAR_SECRET_KEY);
#if !UNITY_EDITOR
            SingularSDK.SkanRegisterAppForAdNetworkAttribution();
#endif
        }

        private static void InitializeMaxSdk()
        {
            // MaxSDK
            EverydayLogger.Info("Initializing MaxSDK");
            MaxSdkCallbacks.OnSdkInitializedEvent -= OnMaxSdkInitialized;
            MaxSdkCallbacks.OnSdkInitializedEvent += OnMaxSdkInitialized;

            MaxSdk.SetSdkKey(Keys.EVERYDAY_MAX_KEY);
            MaxSdk.InitializeSdk();
        }

        private static void OnMaxSdkInitialized(MaxSdk.SdkConfiguration sdkConfiguration)
        {
            MainThreadDispatcher.Instance.Enqueue(() =>
            {
                // Send Max AdInfo to Singular
                MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent -= SingularAdDataSender.SendAdData;
                MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent -= SingularAdDataSender.SendAdData;
                MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent -= SingularAdDataSender.SendAdData;

                MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += SingularAdDataSender.SendAdData;
                MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += SingularAdDataSender.SendAdData;
                MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += SingularAdDataSender.SendAdData;

                // Initialize Ads Manager
                MainThreadDispatcher.Instance.Enqueue(InitializeAdsManager);

                initializationStatus = InitializationStatus.Initialized;
                OnEverydayInitialized?.Invoke();
                OnEverydayInitialized = null;
            });
        }

        private static void InitializeAdsManager()
        {
            // Initialize Ads Manager
            AdsManager.PrepareManager();
            if (EverydaySettings.AdSettings.initializeOnLoad)
            {
                AdsManager.Initialize();
            }
        }

#if UNITY_EDITOR
        static EveryDay()
        {
            Application.quitting -= OnEditorStop;
            Application.quitting += OnEditorStop;
        }

        private static void OnEditorStop()
        {
            initializationStatus = InitializationStatus.NotInitialized;
        }
#endif
    }

    internal enum InitializationStatus
    {
        NotInitialized,
        Initializing,
        Initialized
    }
}
