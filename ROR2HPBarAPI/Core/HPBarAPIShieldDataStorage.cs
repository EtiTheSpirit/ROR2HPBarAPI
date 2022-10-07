using RoR2;
using ROR2HPBarAPI.API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ROR2HPBarAPI.Core {
	internal class HPBarAPIShieldDataStorage : MonoBehaviour {

		/// <summary>
		/// The material that should be used by shield renderers that employ this data. This selects between <see cref="instanceShieldForThisBody"/> vs. <see cref="globalShieldForBodyType"/>
		/// </summary>
		public Material EffectiveShieldMaterial => useDynamicShieldRendering ? instanceShieldForThisBody : globalShieldForBodyType;

		/// <summary>
		/// The material that should be used by renderers for barriers. This selects between <see cref="instanceBarrierForThisBody"/> and <see cref="globalBarrierForBodyType"/>.
		/// </summary>
		public Material EffectiveBarrierMaterial => useDynamicBarrierRendering ? instanceBarrierForThisBody : globalBarrierForBodyType;

		/// <summary>
		/// If true, the per-instance material should be used. If false, the global one should be used.
		/// </summary>
		public bool useDynamicShieldRendering;

		/// <summary>
		/// If true, the per-instance material should be used for barriers. If false, the global one should be used.
		/// </summary>
		public bool useDynamicBarrierRendering;

		/// <summary>
		/// How to go about rendering this shield.
		/// </summary>
		public ShieldRenderMode shieldRenderMode;

		/// <summary>
		/// How to go about rendering this barrier.
		/// </summary>
		public ShieldRenderMode barrierRenderMode;

		/// <summary>
		/// The global material that has been created as a uniform default for the entire type of body itself.
		/// </summary>
		public Material globalShieldForBodyType;

		/// <summary>
		/// A specifically cloned material for use in this shield instance, for use when the body declares it wants unique render data.
		/// This instance is destroyed when this component is destroyed.
		/// </summary>
		public Material instanceShieldForThisBody;

		/// <summary>
		/// For use with barriers, this is the replacement prefab for use on common barriers.
		/// </summary>
		public Material globalBarrierForBodyType;

		/// <summary>
		/// For use with barriers, this is the replacement prefab for use on instanced barriers.
		/// </summary>
		public Material instanceBarrierForThisBody;


		private void OnDestroy() {
			DestroyImmediate(instanceShieldForThisBody);
		}

	}
}
