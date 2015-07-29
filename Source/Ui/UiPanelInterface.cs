using UnityEngine;

namespace Uzu
{
	/// <summary>
	/// Base interface for all panel types.
	/// </summary>
	public interface UiPanelInterface
	{
		/// <summary>
		/// Gets the name of this panel.
		/// </summary>
		string GetName ();
		
		/// <summary>
		/// Initialize the panel.
		/// </summary>
		/// <param name='ownerManager'>
		/// Owner manager.
		/// </param>
		void Initialize (UiPanelMgr ownerManager);
		
		/// <summary>
		/// Activate this instance.
		/// </summary>
		void Activate ();
		
		/// <summary>
		/// Deactivate this instance.
		/// </summary>
		void Deactivate ();
		
		/// <summary>
		/// Called every frame while this panel is active.
		/// </summary>
		void DoUpdate ();
	}
}