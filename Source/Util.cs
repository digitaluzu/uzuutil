using UnityEngine;

namespace Uzu
{
    /// <summary>
    /// Miscellaneous services.
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Sets the "instance" variable of a singleton object to a new value.
        /// Performs error checking to make sure the instance is not already set.
        /// </summary>
        public static void SingletonSet<T> (ref T instance, T newValue)
        {
            if (instance != null && newValue != null) {
                Debug.LogError ("Singleton instance is already set! [" + typeof(T).ToString () + "].");
                return;
            }
            instance = newValue;
        }

        /// <summary>
        /// Finds the specified component on the game object or one of its parents.
        /// </summary>
        public static T FindInParents<T> (GameObject go) where T : Component
        {
            if (go == null) {
                return null;
            }

            object comp = go.GetComponent<T>();

            if (comp == null) {
                Transform t = go.transform.parent;

                while (t != null && comp == null) {
                    comp = t.gameObject.GetComponent<T>();
                    t = t.parent;
                }
            }

            return (T)comp;
        }

        /// <summary>
        /// Search through all children of the game object (and children's children)
        /// until an object with the given name is found.
        ///
        /// Performance can be quite slow, so it's not advised to call this
        /// in performance critical regions.
        /// </summary>
        public static GameObject FindInChildren (GameObject go, string name, bool searchInactiveChildren = false)
        {
            Transform xform = go.transform;
            Transform resultXform = FindInChildren (xform, name, searchInactiveChildren);
            if (resultXform != null) {
                return resultXform.gameObject;
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// Search through all children of the transform (and children's children)
        /// until an object with the given name is found.
        ///
        /// Performance can be quite slow, so it's not advised to call this
        /// in performance critical regions.
        /// </summary>
        public static Transform FindInChildren (Transform xform, string name, bool searchInactiveChildren = false)
        {
            // Found it.
            if (xform.name == name) {
                return xform;
            }

            // Search children.
            for (int i = 0; i < xform.childCount; i++) {
                Transform childXform = xform.GetChild (i);

                // Don't traverse inactive objects.
                if (!searchInactiveChildren &&
                    !childXform.gameObject.activeSelf) {
                    continue;
                }

                Transform resultXform = FindInChildren (childXform, name);
                if (resultXform != null) {
                    return resultXform;
                }
            }

            // Not found.
            return null;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Saves a screenshot to the specified path, or a default path
        /// under $ProjectBaseDir/Screenshots/
        /// </summary>
        public static void CaptureScreenshot (string fileName = null)
        {
            if (string.IsNullOrEmpty (fileName)) {
                fileName = GetDefaultScreenshotPath ();
            }

            Debug.Log("Saving screenshot to: " + fileName);
            Application.CaptureScreenshot(fileName);
        }

        private static string GetDefaultScreenshotPath ()
        {
            string fileName;
            int count = 0;

            // Create base directory if necessary.
            const string basePath = "Screenshots/";
            if (!System.IO.Directory.Exists(basePath)) {
                System.IO.Directory.CreateDirectory(basePath);
            }

            // Use the time as the file name.
            System.DateTime time = System.DateTime.Now;
            string timeStr = time.ToString("yyyy-MM-dd-HH-mm-ss-");

            do {
                fileName = basePath + timeStr + count + ".png";
                count++;
            } while (System.IO.File.Exists(fileName));

            return fileName;
        }
#endif // UNITY_EDITOR

        /// <summary>
        /// Gets the URL to rate the app.
        /// </summary>
        public static string GetRateAppURL (string appId)
        {
#if UNITY_IPHONE
            if (GetMajorVersion_iOS () >= 7) {
                // From iOS 7, 'viewContentsUserReviews' is no longer valid, so you cannot redirect the user directly to the ratings page.
                return "itms-apps://itunes.apple.com/app/id" + appId + "?at=10l6dK";
            }
            else {
                return GetAppURL (appId);
            }
#elif UNITY_ANDROID
            return GetAppURL (appId);
#else
            return string.Empty;
#endif
        }

        /// <summary>
        /// Gets the app store URL.
        /// </summary>
        public static string GetAppURL (string appId)
        {
#if UNITY_IPHONE
            // References:
            //  - http://stackoverflow.com/questions/18905686/itunes-review-url-and-ios-7-ask-user-to-rate-our-app-appstore-show-a-blank-pag
            //  - http://stackoverflow.com/questions/433907/how-to-link-to-apps-on-the-app-store
            return "itms-apps://itunes.apple.com/app/id" + appId + "?at=10l6dK";
#elif UNITY_ANDROID
            return "http://play.google.com/store/apps/details?id=" + appId;
#else
            return string.Empty;
#endif
        }

#if UNITY_IPHONE
        /// <summary>
        /// Gets the iOS major version #.
        /// Example: iOS 7.0.1 returns 7.
        /// </summary>
        public static int GetMajorVersion_iOS ()
        {
            // String in the form of: iPhone OS 6.1
            string osString = SystemInfo.operatingSystem;

            string versionString = osString.Replace("iPhone OS ", "");
            string[] tmpStrings = versionString.Split ('.');

            if (tmpStrings.Length == 0) {
                Debug.LogWarning ("Invalid version string.");
                return -1;
            }

            string majorVersionString = tmpStrings [0];

            return System.Convert.ToInt32 (majorVersionString);
        }
#endif // UNITY_IPHONE
    }
}
