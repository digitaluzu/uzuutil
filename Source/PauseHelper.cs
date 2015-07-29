using UnityEngine;

namespace Uzu
{
	/// <summary>
	/// Allows generic pausing through bit flags.
	/// By using flags, it allows us to have certain functionality
	/// paused, while other functionality continues to update.
	/// 
	/// Example:
	///   Uzu.PauseHelper pauseObject = new Uzu.PauseHelper();
	/// 
	///   // Define:
	///   const int BACKGROUND_LAYER = 1 << 0;
	/// 
	///	  // From GUI button:
	///   pauseObject.Pause(BACKGROUND_LAYER);
	/// 
	///   // ...
	/// 
	///   // From background layer (Update()):
	///   if (pauseObject.IsPaused(BACKGROUND_LAYER)) {
	///        return;
	///   }
	///   
	/// </summary>
	public class PauseHelper
	{
		/// <summary>
		/// Pause the specified flag.
		/// </summary>
		public void Pause (int flag)
		{
			_flags |= flag;
		}
		
		/// <summary>
		/// Unpause the specified flag.
		/// </summary>
		public void Unpause (int flag)
		{
			_flags &= ~flag;
		}

		/// <summary>
		/// Unpause all flags.
		/// </summary>
		public void UnpauseAll ()
		{
			_flags = 0;
		}
		
		/// <summary>
		/// Determines whether the specified flag is paused.
		/// </summary>
		public bool IsPaused (int flag)
		{
			return (_flags & flag) != 0;
		}
		
		#region Implementation.
		private int _flags = 0;
		#endregion
	}
}