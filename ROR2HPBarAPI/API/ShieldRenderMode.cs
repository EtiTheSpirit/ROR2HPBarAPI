using System;
using System.Collections.Generic;
using System.Text;

namespace ROR2HPBarAPI.API {
	/// <summary>
	/// Describes the manner in which this shield renderer should apply.
	/// </summary>
	public enum ShieldRenderMode {
		/// <summary>
		/// Use vanilla behavior for figuring out whether or not to render.
		/// </summary>
		RenderWhenVanillaRenders,

		/// <summary>
		/// Figure out whether or not vanilla wants to render, and then proceed to do the exact opposite.
		/// Render when vanilla doesn't want to. Don't render when vanilla does want to.
		/// </summary>
		RenderOppositeOfVanilla,

		/// <summary>
		/// Always render, no questions asked. The only exception to this rule is if
		/// the player currently has the maximum amount of overlays (limit does not apply to barrier, only shield).
		/// </summary>
		AlwaysRender,

		/// <summary>
		/// Never render, no questions asked.
		/// </summary>
		NeverRender
	}
}
