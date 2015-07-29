using UnityEngine;
using System.Collections.Generic;

namespace Uzu
{
    /// <summary>
    /// Uzu user interface panel manager.
    ///
    /// Usage:
    ///  1) Add UiPanelMgr to the scene.
    ///  2) Add all panels as children.
    ///
    /// Note:
    ///  Multiple UiPanelMgrs can exist in the scene at once.
    /// </summary>
    public class UiPanelMgr : BaseBehaviour
    {
        public delegate void OnPanelChangeDelegate (PanelChangeContext context);

        /// <summary>
        /// Callback that is trigger when the current panel is changed.
        /// </summary>
        public event OnPanelChangeDelegate OnPanelChange;

        /// <summary>
        /// Changes the current panel to the specified panel id.
        /// </summary>
        public void ChangeCurrentPanel (string panelId)
        {
            UiPanelInterface panel;
            if (_uiPanelDataHolder.TryGetValue (panelId, out panel)) {
                UiPanelInterface prevPanel = _currentPanel;

                if (prevPanel != null) {
                    prevPanel.Deactivate ();
                }

                _currentPanel = panel;
                _currentPanelId = panelId;
                _currentPanel.Activate ();

                // Trigger callback.
                {
                    if (OnPanelChange != null) {
                        PanelChangeContext context = new PanelChangeContext();
                        context.PreviousPanel = prevPanel;
                        context.CurrentPanel = _currentPanel;
                        OnPanelChange(context);
                    }
                }
            } else {
                Debug.LogError ("Unable to activate a panel that is not registered: " + panelId);
            }
        }

        /// <summary>
        /// Gets the currently active panel id.
        /// </summary>
        public string CurrentPanelId {
            get { return _currentPanelId; }
        }

        /// <summary>
        /// Gets the currently active panel.
        /// </summary>
        public UiPanelInterface CurrentPanel {
            get { return _currentPanel; }
        }

        #region Implementation.
        private Dictionary<string, UiPanelInterface> _uiPanelDataHolder = new Dictionary<string, UiPanelInterface> ();
        private UiPanelInterface _currentPanel;
        private string _currentPanelId;

        private void RegisterPanel (string name, UiPanelInterface panel)
        {
    #if UNITY_EDITOR
            if (_uiPanelDataHolder.ContainsKey(name)) {
                Debug.LogWarning("Panel with name [" + name + "] already exists.");
            }
    #endif // UNITY_EDITOR

            _uiPanelDataHolder [name] = panel;

            // Initialize the panel.
            panel.Initialize (this);
        }

        protected override void Awake ()
        {
            base.Awake ();

            // Register all child panels.
            MonoBehaviour[] panels = this.gameObject.GetComponentsInChildren<MonoBehaviour> (true);
            for (int i = 0; i < panels.Length; i++) {
                UiPanelInterface panel = panels [i] as UiPanelInterface;
                if (panel != null) {
                    RegisterPanel (panel.GetName (), panel);
                }
            }
        }

        private void Update ()
        {
            // Update the current panel.
            if (_currentPanel != null) {
                _currentPanel.DoUpdate ();
            }
        }
        #endregion
    }
}
