using UnityEngine;

namespace Uzu
{
    /// <summary>
    /// The application main entry point.
    /// This object is guaranteed to exist at all times.
    /// </summary>
    public class Main : BaseBehaviour
    {
        /// <summary>
        /// Override this to load application resources.
        /// </summary>
        protected virtual void OnMainBegin ()
        {
        }
        /// <summary>
        /// Override this to provide any application-specific startup logic.
        /// </summary>
        protected virtual void OnMainBegin2 ()
        {
        }
        /// <summary>
        /// Override this to provide any application-specific cleanup logic.
        /// </summary>
        protected virtual void OnMainEnd ()
        {
        }

        #region Implementation.
        #region Singleton implementation.
        private static Main _instance;

        protected static Main Instance {
            get { return _instance; }
            private set { Uzu.Util.SingletonSet<Main> (ref _instance, value); }
        }
        #endregion

        protected override void Awake ()
        {
            base.Awake ();

            // Only allow single instance to exist.
            if (_instance != null && _instance != this) {
                Destroy (gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad (gameObject);

            // Invoke main.
            OnMainBegin ();
        }

        private void Start ()
        {
            // Invoke main.
            OnMainBegin2 ();
        }

        protected void OnDestroy ()
        {
            if (_instance != this) {
                return;
            }

            // Cleanup.
            OnMainEnd ();

            Instance = null;
        }
        #endregion
    }
}
