#pragma warning disable Publicizer001
using RoR2;
using RoR2.UI;
using ROR2HPBarAPI.Data;
using ROR2HPBarAPI.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ROR2HPBarAPI.API {

	/// <summary>
	/// Contains the result of a query for HP bar color. This is identical to ROR2's BarInfoCollection struct, with some notable differences.
	/// </summary>
	/// <remarks>
	/// For performance reasons, it is recommended you cache this class and then modify it before returning it as a result.
	/// </remarks>
	public class DesiredBarColorData {

		/// <summary>
		/// Make the ctor internal so implementors cannot create it.
		/// </summary>
		internal DesiredBarColorData() { }

		#region Helpers

		/// <summary>
		/// Given the current time right now, this returns the correct color for the flashing low health bar.
		/// </summary>
		/// <returns></returns>
		private Color GetCriticallyHurtColorNow() {
			if (Mathf.FloorToInt(Time.fixedTime * 10f) % 2 != 0) {
				return OverrideLowHealthFlashColor1 ?? DefaultHealthAndShieldData.LowHealthFlashColor1;
			}
			return OverrideLowHealthFlashColor2 ?? DefaultHealthAndShieldData.LowHealthFlashColor2;
		}

		/// <summary>
		/// Depending on the items the character has in their inventory as well as their current condition, this returns the appropriate color.
		/// </summary>
		/// <param name="bar"></param>
		/// <param name="component"></param>
		/// <returns></returns>
		private Color GetAppropriateHPBarColor(HealthBar bar) {
			HealthComponent.HealthBarValues values = bar.source.GetHealthBarValues();
			if (values.isBoss || values.hasInfusion) {
				return DefaultHealthAndShieldData.InfusionHealth;
			}
			if (values.isVoid) {
				return DefaultHealthAndShieldData.VoidHealth;
			}
			if (bar.healthCritical && bar.style.flashOnHealthCritical) {
				return GetCriticallyHurtColorNow();
			}
			return DefaultHealthAndShieldData.NormalHealth;
		}

		/// <summary>
		/// Given the items the player currently has in their inventory, this returns either the stock shield color, or the void shield color.
		/// </summary>
		/// <param name="component"></param>
		/// <returns></returns>
		private Color GetAppropriateShieldColor(HealthComponent component) {
			bool isVoid = Extensions.HasPlasmaShrimp(component.body);
			return isVoid ? DefaultHealthAndShieldData.VoidShield : DefaultHealthAndShieldData.NormalShield;
		}

		#endregion

		/// <summary>
		/// Apply all colors to the given health component
		/// </summary>
		/// <param name="component"></param>
		/// <param name="bar"></param>
		internal void ApplyTo(HealthBar bar) {
			ref HealthBar.BarInfoCollection collection = ref bar.barInfoCollection;
			collection.trailingOverHealthbarInfo.color = OverrideHealthColor ?? GetAppropriateHPBarColor(bar);
			if (OverridePainColor != null) {
				collection.trailingUnderHealthbarInfo.sprite = Images.ColorableHurtBar;
				collection.trailingUnderHealthbarInfo.color = OverridePainColor.Value;
			} else {
				collection.trailingUnderHealthbarInfo.sprite = Images.NormalHurtBar;
				collection.trailingUnderHealthbarInfo.color = DefaultHealthAndShieldData.Pain;
			}

			collection.instantHealthbarInfo.color = OverrideHealingColor ?? DefaultHealthAndShieldData.Healing;
			collection.shieldBarInfo.color = OverrideShieldColor ?? GetAppropriateShieldColor(bar.source);
			if (OverrideBarrierColor != null) {
				collection.barrierBarInfo.color = OverrideBarrierColor.Value;
				collection.barrierBarInfo.sprite = Images.ColorableBarrier;
			} else {
				collection.barrierBarInfo.color = Color.white;
				collection.barrierBarInfo.sprite = Images.NormalBarrier;
			}

			collection.cullBarInfo.color = OverrideCullBar ?? Color.white;
			collection.ospBarInfo.color = OverrideOSPBar ?? DefaultHealthAndShieldData.OSP;
			collection.lowHealthOverBarInfo.color = OverrideLowHealthOverlay ?? DefaultHealthAndShieldData.LowHealthOverlayColor;
			collection.lowHealthUnderBarInfo.color = OverrideLowHealthBacking ?? DefaultHealthAndShieldData.LowHealthBackingColor;
			collection.curseBarInfo.color = OverrideCurseBar ?? DefaultHealthAndShieldData.CurseBar;
		}

		/// <summary>
		/// Overrides the color of the default health bar.
		/// </summary>
		public Color? OverrideHealthColor { get; set; }

		/// <summary>
		/// Overrides the color of pain. In the vanilla game, this is the red bar that appears when you take damage.
		/// </summary>
		public Color? OverridePainColor { get; set; }
		
		/// <summary>
		/// Overrides the color of pain. In the vanilla game, this is the yellow bar that appears at the top of your health when you heal.
		/// </summary>
		public Color? OverrideHealingColor { get; set; }

		/// <summary>
		/// Overrides the color of the shield component of the health bar when it is the normal shield.
		/// </summary>
		public Color? OverrideShieldColor { get; set; }

		/// <summary>
		/// Overrides the color of the barrier component of the health bar.
		/// </summary>
		public Color? OverrideBarrierColor { get; set; }

		/// <summary>
		/// Overrides the color of the culling bar, which is used when guillotines are equipped (making it visible on enemy healthbars), as well as when shaped glass is equipped (obscuring the upper half of the healthbar).
		/// </summary>
		public Color? OverrideCullBar { get; set; }

		/// <summary>
		/// Overrides the color of the one-shot protection part of the healthbar.
		/// </summary>
		public Color? OverrideOSPBar { get; set; }

		/// <summary>
		/// I am not sure where this color applies.
		/// </summary>
		public Color? OverrideCurseBar { get; set; }

		/// <summary>
		/// Overrides the first of the two colors that the healthbar alternates between when the character has very low health.
		/// </summary>
		public Color? OverrideLowHealthFlashColor1 { get; set; }

		/// <summary>
		/// Overrides the second of the two colors that the healthbar alternates between when the character has very low health.
		/// </summary>
		public Color? OverrideLowHealthFlashColor2 { get; set; }

		/// <summary>
		/// Overrides the color of the bar when the user's health is very low.
		/// </summary>
		public Color? OverrideLowHealthOverlay { get; set; }

		/// <summary>
		/// Overrides the background color of the bar when the user's health is very low.
		/// </summary>
		public Color? OverrideLowHealthBacking { get; set; }



	}
}
