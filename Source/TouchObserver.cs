using UnityEngine;
using System.Collections;

namespace Uzu
{
	/// <summary>
	/// Monitors 'touch' input events.
	/// Many of the touch event phases supplied by Unity are
	/// not reliable, as they may or may not be fired depending on the
	/// frame rate.
	/// </summary>
	public class TouchObserver
	{
		/// <summary>
		/// Wrapper class for encapsulating touch data whether it
		/// comes from an actual touch or a mouse or some other input.
		/// Allows the application to handle the input without knowing
		/// from what source it came from.
		/// </summary>
		public struct TouchWrapper
		{
			public int touchId;
			public Vector2 position;
			public float deltaTime;
		}

		public const int LMB_TOUCH_ID = -1;
		public const int RMB_TOUCH_ID = -2;

		public delegate void OnTouchBeginDelegate (TouchWrapper touch);
	
		public delegate void OnTouchUpdateDelegate (TouchWrapper touch);
	
		public delegate void OnTouchEndDelegate (int touchId, Vector2 lastPosition);
	
		public OnTouchBeginDelegate OnTouchBegin { get; set; }
	
		public OnTouchUpdateDelegate OnTouchUpdate { get; set; }
	
		public OnTouchEndDelegate OnTouchEnd { get; set; }
		
		/// <summary>
		/// Creates a new TouchObserver.
		/// maxTouchCount # of touches will be monitored.
		/// Any touches over this number will be ignored.
		/// </summary>
		public TouchObserver (int maxTouchCount)
		{
			MAX_TOUCH_TRACKER_COUNT = maxTouchCount;
			
			// Allocate.
			{
				_trackers = new TouchTracker[MAX_TOUCH_TRACKER_COUNT];
				for (int i = 0; i < MAX_TOUCH_TRACKER_COUNT; i++) {
					_trackers [i] = new TouchTracker ();
				}
			}
		}
		
		/// <summary>
		/// Manually clear all currently active touches.
		/// </summary>
		public void ClearTouches ()
		{
			for (int i = 0; i < _trackers.Length; i++) {
				TouchTracker tracker = _trackers [i];
				if (tracker.IsActive) {
					DoTrackingEnd (tracker);
				}
			}
		}
		
		/// <summary>
		/// Updates the states of all touches.
		/// Call once per frame.
		/// </summary>
		public void Update ()
		{
			// Clear all active trackers dirty state.
			for (int i = 0; i < _trackers.Length; i++) {
				TouchTracker tracker = _trackers [i];
				if (tracker.IsActive) {
					tracker.IsDirty = false;
				}
			}
			
			// Process all touches...
			int touchCount = Input.touches.Length;
			for (int i = 0; i < touchCount; i++) {
				Touch touch = Input.touches [i];
				TouchWrapper touchWrapper = TouchToTouchWrapper (touch);
				
				// Are we already tracking this finger?
				TouchTracker tracker = GetExistingTracker (touch.fingerId);
				
				if (touch.phase == TouchPhase.Began) {
					// If the tracker for this finger is already existing,
					// but the touch phase is just beginning, then this is
					// a different touch event. End the previous event, and detect
					// a new event.
					if (tracker != null) {
						DoTrackingEnd (tracker);
						tracker = null;
					}
					
					// New finger detected - start tracking if possible.
					if (tracker == null) {				
						tracker = GetNewTracker ();
						if (tracker != null) {
							DoTrackingBegin (tracker, touchWrapper);
						}
					}
				}
				else {
					// Update the tracker.
					if (tracker != null) {
						DoTrackingUpdate (tracker, touchWrapper);
					}
				}
			}

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
			// Mouse.
			{
				DoMouseProcessing (0, LMB_TOUCH_ID, ref _LMBDownStartTime, LMBToTouchWrapper);
				DoMouseProcessing (1, RMB_TOUCH_ID, ref _RMBDownStartTime, RMBToTouchWrapper);
			}
#endif
			
			// Deactivate all active trackers that weren't updated this frame so they can be re-used.
			// Ideally, TouchPhase.Ended could be used to do this, but it is not reliable.
			for (int i = 0; i < _trackers.Length; i++) {
				TouchTracker tracker = _trackers [i];
				if (tracker.IsActive && !tracker.IsDirty) {
					DoTrackingEnd (tracker);
				}
			}
		}

		private void DoMouseProcessing (int buttonIndex, int touchId, ref float downStartTime, System.Func <TouchWrapper> touchWrapperFunc)
		{
			if (Input.GetMouseButtonDown (buttonIndex)) {
				TouchTracker tracker = GetExistingTracker (touchId);

				if (tracker != null) {
					Debug.LogWarning ("Mouse button [" + buttonIndex + "] pressed down twice without an up!?");
					DoTrackingEnd (tracker);
				}

				tracker = GetNewTracker ();
				if (tracker != null) {
					downStartTime = Time.time;
					TouchWrapper touch = touchWrapperFunc ();

					DoTrackingBegin (tracker, touch);
				}
			}
			else if (Input.GetMouseButtonUp (buttonIndex)) {
				TouchTracker tracker = GetExistingTracker (touchId);
				if (tracker != null) {
					DoTrackingEnd (tracker);
				}
			}
			else if (Input.GetMouseButton (buttonIndex)) {
				TouchTracker tracker = GetExistingTracker (touchId);
				if (tracker != null) {
					TouchWrapper touch = touchWrapperFunc ();
					DoTrackingUpdate (tracker, touch);
				}
			}
		}
		
		#region Implementation.
		private readonly int MAX_TOUCH_TRACKER_COUNT;
		private TouchTracker[] _trackers;
		
		/// <summary>
		/// Tracks a single touch.
		/// </summary>
		private class TouchTracker
		{	
			private Vector2 _lastPosition;
			private int _fingerId;
			private bool _isActive = false;
			private bool _isDirty = false;
			
			public int FingerId {
				get { return _fingerId; }
			}
			
			public bool IsActive {
				get { return _isActive; }
			}
			
			public bool IsDirty {
				get { return _isDirty; }
				set { _isDirty = value; }
			}
			
			public Vector2 LastPosition {
				get { return _lastPosition; }
			}
			
			public void BeginTracking (TouchWrapper touch)
			{
				_isActive = true;
				_isDirty = true;
				_fingerId = touch.touchId;
				_lastPosition = touch.position;
			}
			
			public void Update (TouchWrapper touch)
			{
				_isDirty = true;
				_lastPosition = touch.position;
			}
			
			public void EndTracking ()
			{
				_isActive = false;
			}
		}
		
		private static TouchWrapper TouchToTouchWrapper (Touch t)
		{
			TouchWrapper touch = new TouchWrapper ();
			touch.position = t.position;
			touch.touchId = t.fingerId;
			touch.deltaTime = t.deltaTime;
			return touch;
		}

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
		private static float _LMBDownStartTime;
		private static float _RMBDownStartTime;

		private static TouchWrapper LMBToTouchWrapper ()
		{
			TouchWrapper touch = new TouchWrapper ();
			touch.position = Input.mousePosition;
			touch.touchId = LMB_TOUCH_ID;
			touch.deltaTime = Time.time - _LMBDownStartTime;
			return touch;
		}

		private static TouchWrapper RMBToTouchWrapper ()
		{
			TouchWrapper touch = new TouchWrapper ();
			touch.position = Input.mousePosition;
			touch.touchId = RMB_TOUCH_ID;
			touch.deltaTime = Time.time - _RMBDownStartTime;
			return touch;
		}
#endif
	
		private void DoTrackingBegin (TouchTracker tracker, TouchWrapper touch)
		{
			tracker.BeginTracking (touch);
			if (OnTouchBegin != null) {
				OnTouchBegin (touch);
			}
		}
		
		private void DoTrackingUpdate (TouchTracker tracker, TouchWrapper touch)
		{
			tracker.Update (touch);
			if (OnTouchUpdate != null) {
				OnTouchUpdate (touch);
			}
		}
		
		private void DoTrackingEnd (TouchTracker tracker)
		{
			if (OnTouchEnd != null) {
				OnTouchEnd (tracker.FingerId, tracker.LastPosition);
			}
			tracker.EndTracking ();
		}
		
		private TouchTracker GetExistingTracker (int fingerId)
		{
			for (int i = 0; i < _trackers.Length; i++) {
				TouchTracker tracker = _trackers [i];
				if (tracker.IsActive && tracker.FingerId == fingerId) {
					return tracker;
				}
			}
			
			return null;
		}
		
		private TouchTracker GetNewTracker ()
		{
			for (int i = 0; i < _trackers.Length; i++) {
				TouchTracker tracker = _trackers [i];
				if (!tracker.IsActive) {
					return tracker;
				}
			}
			
			return null;
		}
		#endregion
	}
}