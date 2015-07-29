using UnityEngine;
using System;

namespace Uzu
{
	/// <summary>
	/// Timer class that utilizes the System.DataTime.
	/// Useful for profiling code and other tasks where "precise" time is necessary.
	/// </summary>
	public class SystemTimer
	{
		private long _startTicks;
		
		public SystemTimer ()
		{
			Restart ();
		}
		
		/// <summary>
		/// Restarts the timer.
		/// </summary>
		public void Restart ()
		{
			_startTicks = System.DateTime.Now.Ticks;
		}
		
		/// <summary>
		/// Get the amount of time elapsed in ticks.
		/// </summary>
		public long ElapsedTimeInTicks {
			get {
				return new System.TimeSpan (System.DateTime.Now.Ticks - _startTicks).Ticks;
			}
		}
		
		/// <summary>
		/// Get the amount of time elapsed in milliseconds.
		/// </summary>
		public double ElapsedTimeInMs {
			get {
				return new System.TimeSpan (System.DateTime.Now.Ticks - _startTicks).TotalMilliseconds;
			}
		}
		
		/// <summary>
		/// Get the amount of time elapsed in seconds.
		/// </summary>
		public double ElapsedTimeInSeconds {
			get {
				return new System.TimeSpan (System.DateTime.Now.Ticks - _startTicks).TotalSeconds;
			}
		}
	}
	
	/// <summary>
	/// Allows profiling of a block of code w/ timer result automatically called on exit of scope.
	/// 
	/// Usage:
	/// 	using (LGScopedSystemTimer timer = new LGScopedSystemTimer("Log message"))
	/// 	{
	/// 		// ~ code for profiling ~
	///		}
	/// </summary>
	public struct ScopedSystemTimer : IDisposable
	{
		private SystemTimer _timer;
		private string _logMessage;
		
		public ScopedSystemTimer (string logMessage)
		{
			_timer = new SystemTimer ();
			_logMessage = logMessage;
		}
		
		#region IDisposable interface.
		public void Dispose ()
		{
			Debug.Log (_logMessage + ": " + _timer.ElapsedTimeInMs + "(ms)");
		}
		#endregion
	}
}