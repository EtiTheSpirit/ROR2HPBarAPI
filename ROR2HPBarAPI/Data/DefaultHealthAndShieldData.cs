#pragma warning disable Publicizer001
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ROR2HPBarAPI.Data {

	/// <summary>
	/// A lookup of the default colors Risk of Rain 2 uses for various parts of the healthbar.
	/// </summary>
	public static class DefaultHealthAndShieldData {

		internal static void Init() {
			Material stockShield = LegacyResourcesAPI.Load<Material>("Materials/matEnergyShield");
			Material voidShield = Addressables.LoadAssetAsync<Material>("RoR2/DLC1/MissileVoid/matEnergyShieldVoid.mat").WaitForCompletion();
			On.RoR2.CharacterBody.AssetReferences.Resolve += OnCBAssetReferencesResolved;
			
			_normalShieldColor = stockShield.GetColor("_TintColor");
			_voidShieldColor = voidShield.GetColor("_TintColor");
			_normalBoost = stockShield.GetFloat("_Boost");
			_voidBoost = voidShield.GetFloat("_Boost");
			_mtlShieldTemplate = stockShield;
			if (HPBarPlugin.IsDebugMode) {
				Log.LogDebug($"Got ahold of all the shield defaults. The normal shield color is {_normalShieldColor}, the void color is {_voidShieldColor}, normal brightness boost is {_normalBoost}, void brightness boost is {_voidBoost}.");
			}
		}

		private static void OnCBAssetReferencesResolved(On.RoR2.CharacterBody.AssetReferences.orig_Resolve originalMethod) {
			originalMethod();

			GameObject tempEffect = CharacterBody.AssetReferences.barrierTempEffectPrefab;
			Material mtl = tempEffect.GetComponentInChildren<Renderer>().material;
			_mtlBarrierTemplate = mtl;
			_defaultBarrierTint = mtl.GetColor("_TintColor");
			_defaultBarrierBoost = mtl.GetFloat("_Boost");
			if (HPBarPlugin.IsDebugMode) {
				Log.LogDebug($"Got ahold of the barrier defaults. Normal color is {_defaultBarrierTint} and boost is {_defaultBarrierBoost}");
			}
		}

		#region Colors

		/// <summary>
		/// The color of the ordinary green healthbar.
		/// </summary>
		public static Color NormalHealth => new ImmutableColor(125, 224, 66);

		/// <summary>
		/// The color of the healthbar when the character is a boss, or has one (or more) infusion(s)
		/// </summary>
		public static Color InfusionHealth => new ImmutableColor(231, 84, 58);

		/// <summary>
		/// The color of the healthbar when the character is voidtouched.
		/// </summary>
		public static Color VoidHealth => new ImmutableColor(217, 123, 255);

		/// <summary>
		/// The color of the default shield bar. This is for the actual healthbar component. If you want the color
		/// for the shield overlay, you should look at <see cref="NormalShieldOverlayColor"/>.
		/// </summary>
		public static Color NormalShield => new ImmutableColor(90, 124, 246);

		/// <summary>
		/// The color of the pink shield bar, as seen when the character has a Plasma Shrimp. 
		/// This is for the actual healthbar component. If you want the color
		/// for the shield overlay, you should look at <see cref="VoidShieldOverlayColor"/>.
		/// </summary>
		public static Color VoidShield => new ImmutableColor(255, 57, 199);

		/// <summary>
		/// The gray overlay that appears when losing health.
		/// </summary>
		public static Color Pain => new ImmutableColor(156, 156, 156);

		/// <summary>
		/// The yellow overlay that appearsn when gaining health.
		/// </summary>
		public static Color Healing => new ImmutableColor(244, 255, 148);

		/// <summary>
		/// I do not know where this applies.
		/// </summary>
		public static Color CurseBar => new ImmutableColor(215, 233, 255);

		/// <summary>
		/// When the character's health is critically low, it flashes between two colors. This is the first of the two colors.
		/// </summary>
		public static Color LowHealthFlashColor1 => new ImmutableColor(231, 84, 58);

		/// <summary>
		/// When the character's health is critically low, it flashes between two colors. This is the second of the two colors.
		/// </summary>
		public static Color LowHealthFlashColor2 => new ImmutableColor(255, 255, 255);

		/// <summary>
		/// Not certain where this is used.
		/// </summary>
		public static Color LowHealthOverlayColor => new ImmutableColor(0, 0, 0, 71);

		/// <summary>
		/// The background color of the healthbar when the character's health is critically low.
		/// </summary>
		public static Color LowHealthBackingColor => new ImmutableColor(41, 31, 31);

		/// <summary>
		/// The caution tape looking thing at the bottom of the healthbar, representing the one-shot protection zone. This should be transparent-ish.
		/// </summary>
		public static Color OSP => new ImmutableColor(0, 0, 0, 139);

		#endregion

		#region Shield stuff

		/// <summary>
		/// The normal shield's default color. This is for the overlay that renders on a character's body. 
		/// If you want the color of the health bar component, look at <see cref="NormalShield"/>.
		/// </summary>
		public static Color NormalShieldOverlayColor => _normalShieldColor;

		/// <summary>
		/// The void shield's default color. This is for the overlay that renders on a character's body.
		/// If you want the color of the health bar component, look at <see cref="VoidShield"/>.
		/// </summary>
		public static Color VoidShieldOverlayColor => _voidShieldColor;

		/// <summary>
		/// The shield has a "Boost" parameter for its brightness. This is the default value for that parameter on normal shields.
		/// </summary>
		public static float NormalShieldOverlayBoost => _normalBoost;

		/// <summary>
		/// The shield has a "Boost" parameter for its brightness. This is the default value for that parameter on void shields.
		/// </summary>
		public static float VoidShieldOverlayBoost => _voidBoost;

		/// <summary>
		/// The barrier from a Topaz Brooch or Aegis has a default color. This is the default color.
		/// </summary>
		public static Color NormalBarrierColor => _defaultBarrierTint;

		/// <summary>
		/// The barrier from a Topaz Brooch or Aegis has a default brightness. This is that value. This value affects the general opacity and glow strength of the effect.
		/// </summary>
		public static float NormalBarrierBoost => _defaultBarrierBoost;

		#region Helper Methods
		internal static Color GetShieldColor(Color? preferred, bool isVoid) {
			if (preferred != null) return preferred.Value;
			return isVoid ? VoidShieldOverlayColor : NormalShieldOverlayColor;
		}

		internal static float GetShieldBoost(float? preferred, bool isVoid) {
			if (preferred != null) return preferred.Value;
			return isVoid ? VoidShieldOverlayBoost : NormalShieldOverlayBoost;
		}
		#endregion

		#region Backing Fields
		private static ImmutableColor _normalShieldColor;
		private static ImmutableColor _voidShieldColor;
		private static float _normalBoost;
		private static float _voidBoost;

		/// <summary>
		/// This is a template, do not edit this object!
		/// </summary>
		internal static Material _mtlBarrierTemplate;

		/// <summary>
		/// This is a template, do not edit this object!
		/// </summary>
		internal static Material _mtlShieldTemplate;
		private static ImmutableColor _defaultBarrierTint;
		private static float _defaultBarrierBoost;
		#endregion

		#endregion

	}
}
