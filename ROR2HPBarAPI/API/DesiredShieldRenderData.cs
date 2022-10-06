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
		/// <returns>The material to render with. This is identical to referencing <see cref="HPBarAPIShieldDataStorage.EffectiveMaterial"/>.</returns>
		private HPBarAPIShieldDataStorage CreateAndUpdateShieldStorage(CharacterBody forBody) {
			if (_shieldMtlTemplate == null || _voidShieldMtlTemplate) {
				_shieldMtlTemplate = LegacyResourcesAPI.Load<Material>("Materials/matEnergyShield");
			}

			HPBarAPIShieldDataStorage storage = forBody.gameObject.GetComponent<HPBarAPIShieldDataStorage>();
			if (storage == null) {
				storage = forBody.gameObject.AddComponent<HPBarAPIShieldDataStorage>();
			}
			if (_currentGlobalMaterial == null) {
				_currentGlobalMaterial = new Material(_shieldMtlTemplate);
				storage.globalMaterialForBodyType = _currentGlobalMaterial;
			}
			if (ShieldColorIsDynamic) {
				if (storage.instanceMaterialForThisBody == null) {
					storage.instanceMaterialForThisBody = new Material(_shieldMtlTemplate);
				}
			}
			storage.useDynamicRendering = ShieldColorIsDynamic;
			storage.renderMode = ShieldRenderMode;

			return storage;
		}
		/// <summary>This is the material for the entire provider (across the entire classification of body). Used when the body does not have custom colors.</summary>
		private Material _currentGlobalMaterial = null;
		private protected static Material _shieldMtlTemplate = null;
		private protected static Material _voidShieldMtlTemplate = null;
		#endregion

		internal void ApplyTo(CharacterModel mdl) {
			HPBarAPIShieldDataStorage storage = CreateAndUpdateShieldStorage(mdl.body);
			if (storage.renderMode == ShieldRenderMode.Vanilla) return;

			Material result = storage.EffectiveMaterial;
			bool hasPlasma = Extensions.HasPlasmaShrimp(mdl.body);
			result.SetColor("_TintColor", DefaultHealthAndShieldData.GetShieldColor(OverrideBodyShieldColor, hasPlasma));
			result.SetFloat("_Boost", DefaultHealthAndShieldData.GetShieldBoost(OverrideBoost, hasPlasma));

			Material[] currentOverlays = mdl.currentOverlays;
			for (int index = 0; index < mdl.activeOverlayCount; index++) {
				Material current = currentOverlays[index];
				if (current == CharacterModel.energyShieldMaterial || current == CharacterModel.voidShieldMaterial) {
					if (storage.renderMode == ShieldRenderMode.ReplaceVanilla || storage.renderMode == ShieldRenderMode.AlwaysRender) {
						currentOverlays[index] = result;
						return;
					} else if (storage.renderMode == ShieldRenderMode.NeverRender) {
						currentOverlays[index] = null;
						return;
					}
				}
			}
			if (storage.renderMode == ShieldRenderMode.AlwaysRender && mdl.activeOverlayCount < CharacterModel.maxOverlays) {
				currentOverlays[mdl.activeOverlayCount] = result;
				mdl.activeOverlayCount++;
			}
		}

		/// <summary>
		/// How to render this shield on the player character. See the enum itself for more documentation.
		/// </summary>
		public ShieldRenderMode ShieldRenderMode { get; set; } = ShieldRenderMode.Vanilla;

		/// <summary>
		/// Separate from the actual shield on the healthbar, this is the color of the overlay on the actual character mesh.<para/>
		/// Setting this to <see langword="null"/> will use vanilla colors.
		/// </summary>
		public Color? OverrideBodyShieldColor { get; set; }

		/// <summary>
		/// The brightness/opacity boost of the shield. Lower values make it fainter, while higher values make it more pronounced.
		/// </summary>
		public float? OverrideBoost { get; set; }

		/// <summary>
		/// If true, the shield color that you return will be expected to be <strong>dynamic</strong> (or animated) by the system. 
		/// This means that instead of sharing the shield material across the entire <see cref="AbstractColorProvider"/>, 
		/// it creates a new one for each body that it renders on, allowing each body to manage its own color rather than having that color
		/// shared between all of them.
		/// </summary>
		/// <remarks>
		/// Due to the implementation of this feature, this is not computationally cheap when used in excess (a custom color costs one draw call), so please
		/// practice restraint when using this, especially if the intent is to be used on many characters at once (10+).
		/// </remarks>
		public bool ShieldColorIsDynamic { get; set; }

	}
}
