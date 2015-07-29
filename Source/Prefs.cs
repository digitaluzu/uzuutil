using UnityEngine;
using System.Collections.Generic;

namespace Uzu
{
	/// <summary>
	/// Wrapper for dealing with PlayerPrefs.
	/// Handles (de)-serialization of raw data.
	/// 
	/// See below for more advanced implementation if needed:
	///  - http://wiki.unity3d.com/index.php/ArrayPrefs2
	/// </summary>
	public class Prefs
	{
		public static float GetFloat (string key, float defaultValue)
		{
			return PlayerPrefs.GetFloat (key, defaultValue);
		}
		
		public static void SetFloat (string key, float val)
		{
			PlayerPrefs.SetFloat (key, val);
		}
		
		public static int GetInt (string key, int defaultValue)
		{
			return PlayerPrefs.GetInt (key, defaultValue);
		}
		
		public static void SetInt (string key, int val)
		{
			PlayerPrefs.SetInt (key, val);
		}
		
		public static string GetString (string key, string defaultValue)
		{
			return PlayerPrefs.GetString (key, defaultValue);
		}
	
		public static void SetString (string key, string val)
		{ 
			PlayerPrefs.SetString (key, val);
		}
		
		public static bool GetBool (string key, bool defaultValue)
		{
			return PlayerPrefs.GetInt (key, defaultValue ? 1 : 0) == 1;
		}
		
		public static void SetBool (string key, bool val)
		{
			PlayerPrefs.SetInt (key, val ? 1 : 0);
		}
		
		public static bool HasKey (string key)
		{
			return PlayerPrefs.HasKey (key);
		}
	
		public static void DeleteKey (string key)
		{
			PlayerPrefs.DeleteKey (key);
		}
		
		/// <summary>
		/// Save all preferences to disk.
		/// </summary>
		public static void Save ()
		{
			PlayerPrefs.Save ();
		}
	}
}
