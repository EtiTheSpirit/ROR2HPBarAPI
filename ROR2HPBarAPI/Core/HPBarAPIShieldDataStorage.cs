using ROR2HPBarAPI.API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ROR2HPBarAPI.Core {
	internal class HPBarAPIShieldDataStorage : MonoBehaviour {

		/// <summary>
		/// The material that should be used by renderers that employ this data.
		/// </summary>
		public Material EffectiveMaterial => useDynamicRendering ? instanceMaterialForThisBody : globalMaterialForBodyType;

		/// <summary>
		/// If true, the per-instance material should be used. If false, the global one should be used.
		/// </summary>
		public bool useDynamicRendering;

		/// <summary>
		/// How to go about rendering this shield.
		/// </summary>
		public ShieldRenderMode renderMode;
		
		/// <summary>
		/// The global material that has been created as a uniform default for the entire type of body itself.
		/// </summary>
		public Material globalMaterialForBodyType;

		/// <summary>
		/// A specifically cloned material for use in this shield instance, for use when the body declares it wants unique render data.
		/// This instance is destroyed when this component is destroyed.
		/// </summary>
		public Material instanceMaterialForThisBody;


		private void OnDestroy() {
			DestroyImmediate(instanceMaterialForThisBody);
		}

	}
}
