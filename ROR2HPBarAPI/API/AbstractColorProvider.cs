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
		/// The implementor's copy of <see cref="DesiredBarColorData"/> that it should modify as needed.
		/// </summary>
		public DesiredBarColorData BarColorData { get; } = new DesiredBarColorData();

		/// <summary>
		/// The implementor's copy of <see cref="DesiredShieldRenderData"/> that it should modify as needed.
		/// </summary>
		public DesiredShieldRenderData ShieldRenderData { get; } = new DesiredShieldRenderData();

		/// <summary>
		/// When called, the implementor should edit <see cref="BarColorData"/> to reflect upon the implementor's wishes.
		/// </summary>
		/// <param name="sourceBody">The <see cref="CharacterBody"/> that this is rendering for.</param>
		public abstract void UpdateBarColors(CharacterBody sourceBody);

		/// <summary>
		/// When called, the implementor should edit <see cref="ShieldRenderData"/> to reflect upon the implementor's wishes.
		/// </summary>
		/// <param name="sourceBody">The <see cref="CharacterBody"/> that this is rendering for.</param>
		public abstract void UpdateShieldOverrides(CharacterBody sourceBody);

	}
}
