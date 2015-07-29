using UnityEngine;
using System.Collections.Generic;

namespace Uzu
{
	/// <summary>
	/// Debug related services.
	/// </summary>
	public static class Dbg
	{
		/// <summary>
		/// Assert that a given condition is true.
		/// </summary>
		[System.Diagnostics.Conditional("UNITY_EDITOR")]
		public static void Assert (bool condition)
		{
			Assert (condition, null, 2);
		}
	
		/// <summary>
		/// Assert that a given condition is true, with an optional message
		/// that is displayed if the condition is false.
		/// </summary>
		[System.Diagnostics.Conditional("UNITY_EDITOR")]
		public static void Assert (bool condition, string msg)
		{
			Assert (condition, msg, 2);
		}
		
		#region Debug drawing.
		/// <summary>
		/// Draws a debug sphere.
		/// </summary>
		[System.Diagnostics.Conditional("UNITY_EDITOR")]
		public static void DrawSphere (Vector3 pos, float radius, Color color)
		{
#if UNITY_EDITOR
			float radius2 = radius * 2.0f;
			GameObject dbgSphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			Transform xform = dbgSphere.transform;
			xform.position = pos;
			xform.localScale = new Vector3 (radius2, radius2, radius2);
			dbgSphere.renderer.material.color = color;

			// Disable physics.
			dbgSphere.GetComponent <SphereCollider> ().enabled = false;
			
			// Set parent so that debug objects don't clutter up scene.
			xform.parent = GetDebugParentGameObject ().transform;
#endif // UNITY_EDITOR
		}
		
		/// <summary>
		/// Draws 3D debug text.
		/// Must be called from either OnDrawGizmos or OnDrawGizmosSelected.
		/// See: http://docs.unity3d.com/Documentation/ScriptReference/Gizmos.html
		/// </summary>
		[System.Diagnostics.Conditional("UNITY_EDITOR")]
		public static void DrawText (Vector3 pos, string text)
		{
#if UNITY_EDITOR
			UnityEditor.Handles.Label (pos, text);
#endif // UNITY_EDITOR
		}
		#endregion
	
		#region Implementation.
		/// <summary>
		/// Asserts that a given condition is true.
		/// Displays stack trace information to console and pop-up dialog.
		/// Optionally opens up the text editor to the file/line where the assertion failed.
		/// </summary>
		[System.Diagnostics.Conditional("UNITY_EDITOR")]
		private static void Assert (bool condition, string msg, int stackTraceLevel)
		{
#if UNITY_EDITOR
			if (!condition) {
				// Halt.
				Debug.Break ();
				
				// Get stack information.
				System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace (true);
				// Requires file info.
				System.Diagnostics.StackFrame frame = trace.GetFrame (stackTraceLevel);
				
				#region Assert Info.
				string assertInfo = "Filename: " + frame.GetFileName () + "\n" + "Method: " + frame.GetMethod () + "\n" + "Line: " + frame.GetFileLineNumber ();
				#endregion
				
				#region Assert Dialog.
				string assertDialogMsg;
				
				if (msg != null) {
					assertDialogMsg = msg + "\n\n" + "Assert Info:\n" + assertInfo;
				} else {
					assertDialogMsg = "Assert Info:\n" + assertInfo;
				}
				#endregion .
				
				// Log to console.
				Debug.LogError (assertInfo);
				
				// Display dialog.
				if (UnityEditor.EditorUtility.DisplayDialog ("Assert", assertDialogMsg, "Go To File", "Cancel")) {
					// Open file in text editor:
					UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal (frame.GetFileName (), frame.GetFileLineNumber ());
				}
			}
#endif // UNITY_EDITOR
		}
		
		/// <summary>
		/// Creates a dummy game object to be used as the parent for all
		/// created debug objects.
		/// </summary>
		private static GameObject GetDebugParentGameObject ()
		{
			string goName = "UzuDbg";
			GameObject go = GameObject.Find (goName);
			
			// Create if necessary.
			if (go == null) {
				go = new GameObject (goName);
			}
			
			return go;
		}
		#endregion
	}
}