using RoR2;
using ROR2HPBarAPI.API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ROR2HPBarAPI {
	internal class TestImplementation {

		public static void Init(HPBarPlugin plugin) {
			BodyCatalog.availability.CallWhenAvailable(() => {
				Registry.RegisterColorProvider(plugin, (BodyIndex)32, new Colors());
			});
		}

		public class Colors : AbstractColorProvider {
			public override void UpdateBarColors(CharacterBody sourceBody) {
				BarColorData.OverrideBarrierColor = new Color(1, 0, 0);
				BarColorData.OverrideCullBar = new Color(0, 1, 0);
				BarColorData.OverridePainColor = new Color(1, 0, 1);
				BarColorData.OverrideHealingColor = new Color(0, 1, 1);
				BarColorData.OverrideHealthColor = new Color(0, 1, 0);
				BarColorData.OverrideLowHealthBacking = new Color(0.5f, 0.5f, 0.5f);
				BarColorData.OverrideLowHealthFlashColor1 = new Color(0, 0, 0);
				BarColorData.OverrideLowHealthFlashColor2 = new Color(255, 0, 0);
				BarColorData.OverrideOSPBar = new Color(1, 0, 0, 0.5f);
				BarColorData.OverrideShieldColor = new Color(0, 0, 1);
			}

			public override void UpdateShieldOverrides(CharacterBody sourceBody) {
				
			}
		}

	}
}
