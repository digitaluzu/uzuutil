using UnityEngine;

namespace Uzu
{
    /// <summary>
    /// Uzu user interface widget.
    /// </summary>
    [AddComponentMenu("Uzu/UI Widget")]
    public class UiWidget : BaseBehaviour
    {
        /// <summary>
        /// The _owner panel of this widget.
        /// </summary>
        private UiPanel _ownerPanel;

        private void Start ()
        {
            // Owner panel should have been added during Awake.
            // If owner is not set yet (this object was added dynamically during run-time), then
            // we need to find the owner manually.
            if (_ownerPanel == null) {
                _ownerPanel = Uzu.Util.FindInParents <UiPanel> (this.gameObject);
                if (_ownerPanel == null) {
                    Debug.LogWarning ("UiWidget [" + this.name + "] does not belong to a panel.");
                }
            }
        }

        /// <summary>
        /// Initialize this widget and set the connection to the owner panel.
        /// </summary>
        public void Initialize (UiPanel ownerPanel)
        {
            _ownerPanel = ownerPanel;
        }

        #region Events.
        private void OnHover (bool isOver)
        {
            _ownerPanel.OnHover (this, isOver);
        }

        private void OnPress (bool pressed)
        {
            _ownerPanel.OnPress (this, pressed);
        }

        private void OnClick ()
        {
            _ownerPanel.OnClick (this);
        }

        private void OnDoubleClick ()
        {
            _ownerPanel.OnDoubleClick (this);
        }

        private void OnSelect (bool selected)
        {
            _ownerPanel.OnSelect (this, selected);
        }

        private void OnDrag (Vector2 delta)
        {
            _ownerPanel.OnDrag (this, delta);
        }

        private void OnDrop (GameObject go)
        {
            _ownerPanel.OnDrop (this, go);
        }

        private void OnInput (string text)
        {
            _ownerPanel.OnInput (this, text);
        }

        private void OnSubmit ()
        {
            _ownerPanel.OnSubmit (this);
        }

        private void OnScroll (float delta)
        {
            _ownerPanel.OnScroll (this, delta);
        }
        #endregion
    }
}
