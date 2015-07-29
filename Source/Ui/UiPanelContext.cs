using UnityEngine;
using System.Collections.Generic;

namespace Uzu
{
	/// <summary>
	/// Context for when the active panel changes.
	/// </summary>
	public struct PanelChangeContext
	{
		public UiPanelInterface PreviousPanel { get; set; }
		public UiPanelInterface CurrentPanel { get; set; }
	}
}