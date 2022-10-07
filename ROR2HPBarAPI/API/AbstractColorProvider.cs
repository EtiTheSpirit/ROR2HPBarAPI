using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ROR2HPBarAPI.API {

	/// <summary>
	/// Provides a means of supplying colors for various conditions when rendering the healthbar. Extend this to implement it.
	/// </summary>
	/// <remarks>
	/// Haha Abstract...Base go brrrrrrr
	/// </remarks>
	public abstract class AbstractColorProvider {

		/// <summary>
		/// This is the current <see cref="CharacterBody"/>'s health bar coloration data.
		/// </summary>
		internal DesiredBarColorData BarColorData { get; } = new DesiredBarColorData();

		/// <summary>
		/// This is the current <see cref="CharacterBody"/>'s shield coloration data.
		/// </summary>
		internal DesiredShieldRenderData ShieldRenderData { get; } = new DesiredShieldRenderData();

		/// <summary>
		/// When called, the implementor should edit <see cref="BarColorData"/> to reflect upon the implementor's wishes.
		/// </summary>
		/// <param name="sourceBody">The <see cref="CharacterBody"/> that this is rendering for.</param>
		/// <param name="barColorData">This is the color data associated with <paramref name="sourceBody"/>.</param>
		public abstract void UpdateBarColors(CharacterBody sourceBody, DesiredBarColorData barColorData);

		/// <summary>
		/// When called, the implementor should edit <see cref="ShieldRenderData"/> to reflect upon the implementor's wishes.
		/// </summary>
		/// <param name="sourceBody">The <see cref="CharacterBody"/> that this is rendering for.</param>
		/// <param name="shieldRenderData">This is the color data associated with <paramref name="sourceBody"/>.</param>
		public abstract void UpdateShieldOverrides(CharacterBody sourceBody, DesiredShieldRenderData shieldRenderData);

	}
}
