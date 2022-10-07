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
			public override void UpdateBarColors(CharacterBody sourceBody, DesiredBarColorData barColorData) {
				barColorData.OverrideBarrierColor = new Color(1, 0, 0);
				barColorData.OverrideCullBar = new Color(0, 1, 0);
				barColorData.OverridePainColor = new Color(1, 0, 1);
				barColorData.OverrideHealingColor = new Color(0, 1, 1);
				barColorData.OverrideHealthColor = new Color(0, 1, 0);
				barColorData.OverrideLowHealthBacking = new Color(0.5f, 0.5f, 0.5f);
				barColorData.OverrideLowHealthFlashColor1 = new Color(0, 0, 0);
				barColorData.OverrideLowHealthFlashColor2 = new Color(255, 0, 0);
				barColorData.OverrideOSPBar = new Color(1, 0, 0, 0.5f);
				barColorData.OverrideShieldColor = new Color(0, 0, 1);
			}

			public override void UpdateShieldOverrides(CharacterBody sourceBody, DesiredShieldRenderData shieldRenderData) {
				shieldRenderData.BarrierRenderMode = ShieldRenderMode.AlwaysRender;
				shieldRenderData.OverrideBarrierColor = new Color(1, 0, 0);
			}
		}

	}
}
