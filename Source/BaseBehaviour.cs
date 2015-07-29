using UnityEngine;

namespace Uzu
{
	/// <summary>
	/// Base "MonoBehaviour" class to be used by all Uzu systems.
	/// Provides basic functionality for MonoBehaviours.
	/// </summary>
	public abstract class BaseBehaviour : MonoBehaviour
	{
		private Transform _cachedXform;
		
		/// <summary>
		/// Cached Transform of this GameObject.
		/// Avoids overhead of GetComponent() call.
		/// </summary>
		public Transform CachedXform {
			get { return _cachedXform; }
			private set { _cachedXform = value; }
		}
		
		protected virtual void Awake ()
		{
			CachedXform = transform;
		}
	}
}