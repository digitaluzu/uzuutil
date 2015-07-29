using UnityEngine;

namespace Uzu
{
	/// <summary>
	/// Base message class for all message types.
	/// </summary>
	public abstract class Message
	{
		/// <summary>
		/// If a message requires a receiver and no one receives
		/// the message, an error will be displayed.
		/// </summary>
		public bool DoesRequireReceiver {
			get { return _doesRequireReceiver; }
			set { _doesRequireReceiver = value; }
		}
	
		#region Implementation.
		private bool _doesRequireReceiver = true;
		#endregion
	}
	
	/// <summary>
	/// Message subscriber interface.
	/// Implement this interface to receive messages.
	/// </summary>
	public interface MessageSubscriberInterface
	{
		/// <summary>
		/// Receive a message and process it.
		/// </summary>
		void OnReceiveMessage (Message message);
	}
	
	/// <summary>
	/// Message dispatcher interface.
	/// Implement this interface to become a message hub, and process
	/// message dispatching.
	/// </summary>
	public interface MessageDispatcherInterface
	{
		/// <summary>
		/// Subscribe to messages of a certain type T.
		/// </summary>
		void AddSubscriber<T> (MessageSubscriberInterface subscriber);
	
		/// <summary>
		/// Send a message.
		/// </summary>
		void SendMessage (Message message);
	}
}