using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace ROR2HPBarAPI.Helper {
	internal static class Extensions {

		/// <summary>
		/// Returns whether or not the given <see cref="CharacterBody"/> has a plasma shrimp
		/// </summary>
		/// <param name="body"></param>
		/// <returns></returns>
		public static bool HasPlasmaShrimp(CharacterBody body) {
			Inventory inventory = body.inventory;
			bool isVoid = false;
			if (inventory != null) isVoid = inventory.GetItemCount(DLC1Content.Items.MissileVoid) > 0;
			return isVoid;
		}

	}
}
