using UnityEngine;
using System.Collections;

namespace Uzu {
	/// <summary>
	/// Uzu user interface panel.
	/// 
	/// How to:
	/// Override this class to create your custom panel Logic
	/// </summary>
	public class UiPanel : BaseBehaviour, UiPanelInterface
	{
		#region Implementation	
		/// <summary>
		/// The _owner manager of this panel.
		/// </summary>
		private UiPanelMgr _ownerManager;
		
		/// <summary>
		/// Gets or sets the owner manager.
		/// </summary>
		protected UiPanelMgr OwnerManager {
			get {
				return _ownerManager;
			}
			private set {
				_ownerManager = value;
			}
		}
		
		#region UiPanelInterface implementation.
		public string GetName ()
		{
			return gameObject.name;
		}
		
		/// <summary>
		/// Initialize this panel.
		/// Create the link between the panel manager and the 
		/// </summary>
		public void Initialize (UiPanelMgr ownerManager)
		{
			_ownerManager = ownerManager;	
			
			//Connect all the child widget to this panel
			Component[] widgets = this.gameObject.GetComponentsInChildren<UiWidget> (true);
			foreach (UiWidget widget in widgets) {
				widget.Initialize (this);
			}
			
			OnInitialize ();
			
			// Deactivate the object just in case it was active during edit mode.
			// We don't want to call Deactivate, since this triggers the callback.
			this.gameObject.SetActive (false);
		}
		
		/// <summary>
		/// Activate this instance.
		/// </summary>
		public void Activate ()
		{
			this.gameObject.SetActive (true);
			
			OnActivate ();
		}
		
		/// <summary>
		/// Deactivate this instance.
		/// </summary>
		public void Deactivate ()
		{
			this.gameObject.SetActive (false);
			
			OnDeactivate ();
		}
		
		public void DoUpdate ()
		{
			OnUpdate ();
		}
		#endregion
		#endregion
		
		#region Events.
		/// <summary>
		/// Called when the panel is first initialized.
		/// </summary>
		public virtual void OnInitialize ()
		{	
		}
		
		/// <summary>
		/// Called when the panel is activated.
		/// </summary>
		public virtual void OnActivate ()
		{	
		}
		
		/// <summary>
		/// Called when the panel is deactivated.
		/// </summary>
		public virtual void OnDeactivate ()
		{
		}
		
		/// <summary>
		/// Called every frame that this panel is active.
		/// </summary>
		public virtual void OnUpdate ()
		{		
		}
	
		public virtual void OnHover (UiWidget widget, bool isOver)
		{
		}
	
		public virtual void OnPress (UiWidget widget, bool isPressed)
		{
		}
	
		public virtual void OnClick (UiWidget widget)
		{
		}
	
		public virtual void OnDoubleClick (UiWidget widget)
		{
		}
	
		public virtual void OnSelect (UiWidget widget, bool selected)
		{
		}
	
		public virtual void OnDrag (UiWidget widget, Vector2 delta)
		{
		}
	
		public virtual void OnDrop (UiWidget widget, GameObject go)
		{
		}
	
		public virtual void OnInput (UiWidget widget, string text)
		{
		}
	
		public virtual void OnSubmit (UiWidget widget)
		{
		}
	
		public virtual void OnScroll (UiWidget widget, float delta)
		{
		}
		#endregion
	}
}