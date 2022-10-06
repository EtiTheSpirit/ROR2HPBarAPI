using System;
using System.Collections.Generic;
using System.Text;

namespace ROR2HPBarAPI.API {
	/// <summary>
	/// Describes the manner in which this shield renderer should apply.
	/// </summary>
	public enum ShieldRenderMode {
		/// <summary>
		/// Do not override vanilla behavior. Do whatever vanilla wants.
		/// </summary>
		Vanilla,

		/// <summary>
		/// Render the custom shield iff the vanilla game wants to render one.
		/// </summary>
		ReplaceVanilla,

		/// <summary>
		/// Even if the player has no shield, render it on their body anyway. The only exception to this rule is if
		/// the player currently has the maximum amount of overlays.
		/// </summary>
		AlwaysRender,

		/// <summary>
		/// Even if the player has a shield, never render it on their body.
		/// </summary>
		NeverRender
	}
}
