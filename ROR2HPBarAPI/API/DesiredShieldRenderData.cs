#pragma warning disable Publicizer001
using RoR2;
using ROR2HPBarAPI.Core;
using ROR2HPBarAPI.Data;
using ROR2HPBarAPI.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ROR2HPBarAPI.API {

	/// <summary>
	/// Represents the desired shield rendering information, and is bound to an entire <see cref="CharacterBody"/>. Override fields are (potentially) written to then read per character
	/// but otherwise some data is stored for the entire body type.
	/// </summary>
	public class DesiredShieldRenderData {

		/// <summary>
		/// Make the ctor internal so implementors cannot create it.
		/// </summary>
		internal DesiredShieldRenderData() { }

		#region Helpers
		/// <summary>
		/// Get (and create, if needed) the material to use on this shield based on the current state of the data.<para/>
		/// Note that this method guarantees the existence of an instance of <see cref="HPBarAPIShieldDataStorage"/>. If this method has not been called, its
		/// existence is uncertain.
		/// </summary>
		/// <returns>The material to render with. This is identical to referencing <see cref="HPBarAPIShieldDataStorage.EffectiveShieldMaterial"/>.</returns>
		internal HPBarAPIShieldDataStorage CreateAndUpdateShieldStorage(CharacterBody forBody) {
			HPBarAPIShieldDataStorage storage = forBody.gameObject.GetComponent<HPBarAPIShieldDataStorage>();
			if (storage == null) {
				storage = forBody.gameObject.AddComponent<HPBarAPIShieldDataStorage>();
			}
			if (_currentGlobalShieldMaterial == null) {
				_currentGlobalShieldMaterial = new Material(DefaultHealthAndShieldData._mtlShieldTemplate);
			}
			if (storage.globalShieldForBodyType == null) {
				// Specifically do == null check here because it may have been deleted by the engine.
				storage.globalShieldForBodyType = _currentGlobalShieldMaterial;
			}

			if (_currentGlobalBarrierMaterial == null) {
				_currentGlobalBarrierMaterial = new Material(DefaultHealthAndShieldData._mtlBarrierTemplate);
			}
			if (storage.globalBarrierForBodyType == null) {
				// Specifically do == null check here because it may have been deleted by the engine.
				storage.globalBarrierForBodyType = _currentGlobalBarrierMaterial;
			}

			if (ShieldIsDynamic) {
				if (storage.instanceShieldForThisBody == null) {
					storage.instanceShieldForThisBody = new Material(DefaultHealthAndShieldData._mtlShieldTemplate);
				}
			}

			if (BarrierIsDynamic) {
				if (storage.instanceBarrierForThisBody == null) {
					storage.instanceBarrierForThisBody = new Material(DefaultHealthAndShieldData._mtlBarrierTemplate);
				}
			}

			storage.useDynamicShieldRendering = ShieldIsDynamic;
			storage.useDynamicBarrierRendering = BarrierIsDynamic;

			storage.shieldRenderMode = ShieldRenderMode;
			storage.barrierRenderMode = BarrierRenderMode;

			return storage;
		}
		/// <summary>This is the shield material for the entire provider (across the entire classification of body). Used when the body does not have custom colors or boosts.</summary>
		private Material _currentGlobalShieldMaterial = null;

		/// <summary>This is the shield material for the entire provider (across the entire classification of body). Used when the body does not have custom colors or boosts.</summary>
		private Material _currentGlobalBarrierMaterial = null;
		#endregion
		
		/// <summary>
		/// Applies the shield color and render data to the player. Note that this specifically handles the shield.
		/// Barrier is handled in the Updater.
		/// </summary>
		/// <param name="mdl"></param>
		internal void ApplyShieldTo(CharacterModel mdl) {
			HPBarAPIShieldDataStorage storage = CreateAndUpdateShieldStorage(mdl.body);

			Material result = storage.EffectiveShieldMaterial;
			bool hasPlasma = Extensions.HasPlasmaShrimp(mdl.body);
			result.SetColor("_TintColor", DefaultHealthAndShieldData.GetShieldColor(OverrideShieldOverlayColor, hasPlasma));
			result.SetFloat("_Boost", DefaultHealthAndShieldData.GetShieldBoost(OverrideShieldBoost, hasPlasma));

			if (storage.shieldRenderMode == ShieldRenderMode.RenderWhenVanillaRenders) return;
			// ^ Stop here when using vanilla rendering. Don't need to worry about the array.

			Material[] currentOverlays = mdl.currentOverlays;
			for (int index = 0; index < mdl.activeOverlayCount; index++) {
				Material current = currentOverlays[index];
				if (current == CharacterModel.energyShieldMaterial || current == CharacterModel.voidShieldMaterial) {
					if (storage.shieldRenderMode == ShieldRenderMode.AlwaysRender) {
						currentOverlays[index] = result;
						return;
					} else if (storage.shieldRenderMode == ShieldRenderMode.NeverRender || storage.shieldRenderMode == ShieldRenderMode.RenderOppositeOfVanilla) {
						currentOverlays[index] = null;
						return;
					}
				}
			}
			if ((storage.shieldRenderMode == ShieldRenderMode.AlwaysRender || storage.shieldRenderMode == ShieldRenderMode.RenderOppositeOfVanilla) && mdl.activeOverlayCount < CharacterModel.maxOverlays) {
				currentOverlays[mdl.activeOverlayCount] = result;
				mdl.activeOverlayCount++;
			}
		}

		/// <summary>
		/// How to render this shield on the player character. See the enum itself for more documentation.
		/// </summary>
		public ShieldRenderMode ShieldRenderMode { get; set; } = ShieldRenderMode.RenderWhenVanillaRenders;

		/// <summary>
		/// How to render the barrier on the player character. See the enum itself for more documentation.
		/// </summary>
		public ShieldRenderMode BarrierRenderMode { get; set; } = ShieldRenderMode.RenderWhenVanillaRenders;

		/// <summary>
		/// Separate from the actual shield on the healthbar, this is the color of the overlay on the actual character mesh.<para/>
		/// Setting this to <see langword="null"/> will use vanilla colors.
		/// </summary>
		public Color? OverrideShieldOverlayColor { get; set; }

		/// <summary>
		/// The brightness/opacity boost of the shield. Lower values make it fainter, while higher values make it more pronounced.
		/// </summary>
		public float? OverrideShieldBoost { get; set; }

		/// <summary>
		/// Separate from the actual barrier over the healthbar, this is the color of the barrier bubble on the actual character mesh.<para/>
		/// Setting this to <see langword="null"/> will use the vanilla color.
		/// </summary>
		public Color? OverrideBarrierColor { get; set; }

		/// <summary>
		/// The brightness/opacity boost of the barrier effect. Lower values make it fainter, while higher values make it more pronounced.
		/// </summary>
		public float? OverrideBarrierBoost { get; set; }

		/// <summary>
		/// If true, the shield color that you return will be expected to be <strong>dynamic</strong> (or, animated) by the system. 
		/// This means that instead of sharing the shield material across the entire <see cref="AbstractColorProvider"/>, 
		/// it creates a new one for each body that it renders on, allowing each body to manage its own color rather than having that color
		/// shared between all of them.
		/// </summary>
		/// <remarks>
		/// Due to the implementation of this feature, this is not computationally cheap when used in excess (a custom color costs one draw call), so please
		/// practice restraint when using this, especially if the intent is to be used on many characters at once (10+).
		/// </remarks>
		public bool ShieldIsDynamic { get; set; }

		/// <summary>
		/// If true, the barrier color (for a Topaz Brooch or Aegis) that you return will be expected to be <strong>dynamic</strong> (or, animated) by the system. 
		/// This means that instead of sharing the barrier material across the entire <see cref="AbstractColorProvider"/>, 
		/// it creates a new one for each body that it renders on, allowing each body to manage its own color rather than having that color
		/// shared between all of them.
		/// </summary>
		/// <remarks>
		/// Due to the implementation of this feature, this is not computationally cheap when used in excess (a custom color costs one draw call), so please
		/// practice restraint when using this, especially if the intent is to be used on many characters at once (10+).
		/// </remarks>
		public bool BarrierIsDynamic { get; set; }

	}
}
