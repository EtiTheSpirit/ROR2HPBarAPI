using BepInEx;
using RoR2;
using RoR2.UI;
using ROR2HPBarAPI.API;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace ROR2HPBarAPI.Core {
	internal static class Updater {

		public static void Init() {
			On.RoR2.CharacterModel.UpdateOverlays += OnOverlaysUpdating;
			On.RoR2.UI.HealthBar.UpdateBarInfos += OnHealthbarInfoUpdating;
		}

		private static void OnOverlaysUpdating(On.RoR2.CharacterModel.orig_UpdateOverlays originalMethod, CharacterModel @this) {
			// Call the original method...
			originalMethod(@this);

			if (@this.body != null) {
				AbstractColorProvider provider = Registry.GetProvider(@this.body);
				if (provider != null) {
					// ... then (im)politely come in and quietly edit the results.
					provider.UpdateShieldColorOverride(@this.body);
					provider.ShieldRenderData.ApplyTo(@this);
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
					provider.UpdateBarColors(src.body);
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
