using UnityEngine;
using System.Text;
using System.Collections.Generic;

namespace Uzu
{
	/// <summary>
	/// Class for pooling GameObjects.
	/// </summary>
	public class GameObjectPool : BaseBehaviour
	{
		/// <summary>
		/// Can we spawn any objects w/o having to re-allocate?
		/// </summary>
		public bool HasAvailableObject {
			get { return AvailableObjectCount != 0; }
		}

		/// <summary>
		/// The total # of objects that are available to use w/o having to re-allocate.
		/// </summary>
		public int AvailableObjectCount {
			get { return _availableObjects.Count; }
		}

		/// <summary>
		/// The total # of objects that are currently being utilized.
		/// </summary>
		public int ActiveObjectCount {
			get { return _allObjects.Count - _availableObjects.Count; }
		}

		/// <summary>
		/// The total # of objects that this pool can currently accomodate without growing.
		/// </summary>
		public int Capacity {
			get { return _allObjects.Count; }
		}

		/// <summary>
		/// Is the pool allowed to grow if it needs to spawn new objects?
		/// </summary>
		public bool DoesGrow {
			get { return _doesGrow; }
		}
		
		public List<GameObject> ActiveObjects {
			get {
				List<GameObject> activeObjects = new List<GameObject> (ActiveObjectCount);
				for (int i = 0; i < _allObjects.Count; i++) {
					PooledBehaviour obj = _allObjects [i];
					if (!_availableObjects.Contains (obj)) {
						activeObjects.Add (obj.gameObject);
					}
				}
				return activeObjects;
			}
		}
			
		/// <summary>
		/// Spawn a new GameObject with the given position.
		/// Re-uses an object in the pool if available.
		/// </summary>
		public GameObject Spawn (Vector3 position)
		{
			return Spawn (position, _prefabTransform.localRotation);
		}
		
		/// <summary>
		/// Spawn a new GameObject with the given position/rotation.
		/// Re-uses an object in the pool if available.
		/// </summary>
		public GameObject Spawn (Vector3 position, Quaternion rotation)
		{
			GameObject resultGO;
			PooledBehaviour resultComponent;
			
			if (_availableObjects.Count == 0) {
				// Should we automatically grow?
				if (!_doesGrow) {
					Debug.LogWarning ("Pool capacity [" + _initialCount + "] reached.");
					return null;
				}
				
				resultGO = CreateObject (position, rotation);
				resultComponent = resultGO.GetComponent<PooledBehaviour> ();
			} else {
				resultComponent = _availableObjects.Pop ();
				resultGO = resultComponent.gameObject;
				
				Transform resultTransform = resultComponent.CachedXform;
				resultTransform.localPosition = position;
				resultTransform.localRotation = rotation;
			}
			
			// Activate.
			resultGO.SetActive (true);
			resultComponent.OnSpawn ();
			
			return resultGO;
		}
		
		/// <summary>
		/// Unspawns a given GameObject and adds it back to the available
		/// resource pool.
		/// </summary>
		public void Unspawn (GameObject targetGO)
		{
			PooledBehaviour targetComponent = targetGO.GetComponent<PooledBehaviour> ();
			
	#if UNITY_EDITOR
			if (targetComponent == null) {
				Debug.LogError("Attempting to Unspawn an object not belonging to this pool!");
				return;
			}
	#endif // UNITY_EDITOR

			// If the object has been manually assigned to this pool,
			// add it to our master list.
			if (!_allObjects.Contains(targetComponent)) {
				_allObjects.Add (targetComponent);
			}

			// Reset parent in case the object was moved by the user.
			targetComponent.CachedXform.parent = _poolParentTransform;
			
			if (!_availableObjects.Contains (targetComponent)) {
				targetComponent.OnUnspawn ();
				targetGO.SetActive (false);
				_availableObjects.Push (targetComponent);
			}
		}
		
		/// <summary>
		/// Unspawns all GameObjects and adds them all back to the available
		/// resource pool.
		/// </summary>
		public void UnspawnAll ()
		{
			for (int i = 0; i < _allObjects.Count; i++) {
				Unspawn (_allObjects [i].gameObject);
			}
		}
		
		/// <summary>
		/// Destroys all GameObjects in the pool.
		/// </summary>
		public void DestroyAll ()
		{
			for (int i = 0; i < _allObjects.Count; i++) {
				GameObject.Destroy (_allObjects [i].gameObject);
			}
			
			_availableObjects.Clear ();
			_allObjects.Clear ();
		}
		
		#region Implementation.
		[SerializeField]
		private int _initialCount;
		[SerializeField]
		private GameObject _prefab;
		[SerializeField]
		private bool _doesGrow = true;
		private Transform _poolParentTransform;
		private Transform _prefabTransform;
		private Stack<PooledBehaviour> _availableObjects;
		private List<PooledBehaviour> _allObjects;
		
		private GameObject CreateObject (Vector3 position, Quaternion rotation)
		{
			GameObject resultGO = GameObject.Instantiate (_prefab, position, rotation) as GameObject;
			PooledBehaviour resultComponent = resultGO.GetComponent<PooledBehaviour> ();
			
			if (resultComponent == null) {
				Debug.LogError ("Pooled object must contain a Uzu.PooledBehaviour component.");
				GameObject.Destroy (resultGO);
				return null;
			}
			
			Transform resultTransform = resultComponent.CachedXform;
			resultTransform.parent = _poolParentTransform;
			resultTransform.localPosition = position;
			resultTransform.localRotation = rotation;
			resultTransform.localScale = _prefabTransform.localScale;
			
			_allObjects.Add (resultComponent);
			resultComponent.AddToPool (this);
			
			return resultGO;
		}
		
		protected override void Awake ()
		{
			base.Awake ();
			
			// Create a parent for grouping all objects together.
			{
				StringBuilder stringBuilder = new StringBuilder ("UzuPool - ");
				stringBuilder.Append (_prefab.name);
				GameObject poolParent = new GameObject (stringBuilder.ToString ());
				_poolParentTransform = poolParent.transform;

				_poolParentTransform.parent = CachedXform;
				_poolParentTransform.localPosition = Vector3.zero;
				_poolParentTransform.localScale = Vector3.one;
				_poolParentTransform.localRotation = Quaternion.identity;
			}
			
			_prefabTransform = _prefab.transform;			
			_availableObjects = new Stack<PooledBehaviour> (_initialCount);
			_allObjects = new List<PooledBehaviour> (_initialCount);
			
			// Pre-allocate our pool.
			{
				// Allocate objects.
				for (int i = 0; i < _initialCount; ++i) {
					CreateObject (Vector3.zero, Quaternion.identity);
				}
				
				// Add to pool.
				for (int i = 0; i < _allObjects.Count; i++) {
					GameObject targetGO = _allObjects [i].gameObject;
					targetGO.SetActive (false);
					
					PooledBehaviour targetComponent = targetGO.GetComponent<PooledBehaviour> ();
					_availableObjects.Push (targetComponent);
				}
			}
		}
		#endregion
	}
}