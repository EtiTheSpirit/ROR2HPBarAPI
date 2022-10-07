#pragma warning disable Publicizer001
using BepInEx;
using RoR2;
using RoR2.UI;
using ROR2HPBarAPI.API;
using ROR2HPBarAPI.Data;
using System;
using UnityEngine;

namespace ROR2HPBarAPI.Core {
	internal static class Updater {

		public static void Init() {
			On.RoR2.CharacterModel.UpdateOverlays += OnOverlaysUpdating;
			On.RoR2.UI.HealthBar.UpdateBarInfos += OnHealthbarInfoUpdating;
			On.RoR2.CharacterBody.UpdateSingleTemporaryVisualEffect_refTemporaryVisualEffect_GameObject_float_bool_string += OnUpdatingTempEffect;
		}

		/// <summary>
		/// This handles the barrier effect. This is kept out of <see cref="DesiredShieldRenderData"/> because of how this operates (namely, it needs to 
		/// call the original method in the middle of the process rather than before or after).
		/// </summary>
		/// <param name="originalMethod"></param>
		/// <param name="this"></param>
		/// <param name="tempEffect"></param>
		/// <param name="tempEffectPrefab"></param>
		/// <param name="effectRadius"></param>
		/// <param name="active"></param>
		/// <param name="childLocatorOverride"></param>
		private static void OnUpdatingTempEffect(On.RoR2.CharacterBody.orig_UpdateSingleTemporaryVisualEffect_refTemporaryVisualEffect_GameObject_float_bool_string originalMethod, CharacterBody @this, ref TemporaryVisualEffect tempEffect, GameObject tempEffectPrefab, float effectRadius, bool active, string childLocatorOverride) {
			if (tempEffectPrefab == CharacterBody.AssetReferences.barrierTempEffectPrefab) {
				// This is the barrier effect. Override it!
				AbstractColorProvider provider = Registry.GetProvider(@this);
				if (provider != null) {
					HPBarAPIShieldDataStorage storage = provider.ShieldRenderData.CreateAndUpdateShieldStorage(@this);

					// Start by (im)politely coming in and quietly editing the results.
					provider.UpdateShieldOverrides(@this, provider.ShieldRenderData);
					if (provider.ShieldRenderData.BarrierRenderMode == ShieldRenderMode.AlwaysRender) {
						active = true;
					} else if (provider.ShieldRenderData.BarrierRenderMode == ShieldRenderMode.NeverRender) {
						active = false;
					} else if (provider.ShieldRenderData.BarrierRenderMode == ShieldRenderMode.RenderOppositeOfVanilla) {
						active = !active;
					}

					// Now that active has been changed, call the original method to apply it. The color is applied later.
					// The main reason for this is that tempEffect is assigned to (and so the working reference here changes to an instance).
					// From here, the material can be swapped.
					originalMethod(@this, ref tempEffect, tempEffectPrefab, effectRadius, active, childLocatorOverride);

					if (tempEffect != null) {
						// This will, however, be null if the effect is being removed.
						Renderer barrierRenderer = tempEffect.GetComponentInChildren<Renderer>();
						Material result = storage.EffectiveBarrierMaterial;
						result.SetColor("_TintColor", provider.ShieldRenderData.OverrideBarrierColor ?? DefaultHealthAndShieldData.NormalBarrierColor);
						result.SetFloat("_Boost", provider.ShieldRenderData.OverrideBarrierBoost ?? DefaultHealthAndShieldData.NormalBarrierBoost);
						barrierRenderer.material = result;
					}

					return; // This is important as to not call it again below.
				}
			}

			originalMethod(@this, ref tempEffect, tempEffectPrefab, effectRadius, active, childLocatorOverride);
		}

		private static void OnOverlaysUpdating(On.RoR2.CharacterModel.orig_UpdateOverlays originalMethod, CharacterModel @this) {
			// Call the original method...
			originalMethod(@this);

			if (@this.body != null) {
				AbstractColorProvider provider = Registry.GetProvider(@this.body);
				if (provider != null) {
					// ... then (im)politely come in and quietly edit the results.
					provider.UpdateShieldOverrides(@this.body, provider.ShieldRenderData);
					provider.ShieldRenderData.ApplyShieldTo(@this);
				}
			}
		}

		private static void OnHealthbarInfoUpdating(On.RoR2.UI.HealthBar.orig_UpdateBarInfos originalMethod, HealthBar @this) {
			originalMethod(@this);

			TryUpdate(@this);
		}

		internal static void TryUpdate(HealthBar bar) {
			HealthComponent src = bar.source;
			if (src == null) {
				// This is something that can happen.
				return;
			}
			if (src.body == null) {
				if (HPBarPlugin.IsDebugMode) Log.LogError("(Debug) Attempted to update a HealthBar whose source's body was null.");
				return;
			}
			AbstractColorProvider provider = Registry.GetProvider(src.body);
			if (provider != null) {
				try {
					provider.UpdateBarColors(src.body, provider.BarColorData);
					provider.BarColorData.ApplyTo(bar);
				} catch (Exception err) {
					BaseUnityPlugin registryAppender = Registry._registeredIndexTracker[src.body.bodyIndex];
					BepInPlugin meta = registryAppender.Info.Metadata;
					Log.LogWarning($"An error occurred when trying to render BodyIndex={src.body.bodyIndex} ({src.body.GetDisplayName()}) -- This renderer was registered by {meta.Name} ({meta.GUID} version {meta.Version}).");
					Log.LogError(err.Message);
					Log.LogError(err.StackTrace);
				}
			}
		}
	}
}
