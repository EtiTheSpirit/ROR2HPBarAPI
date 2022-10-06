using BepInEx;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace ROR2HPBarAPI.API {

	/// <summary>
	/// Register your <see cref="AbstractColorProvider"/> here.
	/// </summary>
	public static class Registry {

		internal static readonly Dictionary<BodyIndex, BaseUnityPlugin> _registeredIndexTracker = new Dictionary<BodyIndex, BaseUnityPlugin>();
		internal static readonly Dictionary<BodyIndex, AbstractColorProvider> _registry = new Dictionary<BodyIndex, AbstractColorProvider>();

		private static string PluginToString(BaseUnityPlugin src) {
			return $"{src.Info.Metadata.Name} ({src.Info.Metadata.GUID} ver. {src.Info.Metadata.Version})";
		}

		/// <summary>
		/// Attempts to get ahold of a provider for colors, returning null if one could not be acquired.
		/// </summary>
		/// <param name="body"></param>
		/// <returns></returns>
		internal static AbstractColorProvider GetProvider(CharacterBody body) {
			if (_registry.TryGetValue(body.bodyIndex, out AbstractColorProvider provider) && provider != null) {
				BaseUnityPlugin registryAppender = _registeredIndexTracker[body.bodyIndex];
				if (registryAppender == null) {
					Log.LogError($"Some knucklehead tried to hide their plugin, or something horribly wrong has happened in the render code for custom healthbars - there is no plugin associated with this body index! This occurred when attempting to render BodyIndex={body.bodyIndex} ({body.GetDisplayName()}). I do hope *you're* not responsible.");
					return null;
				}
				return provider;
			}
			return null;
		}

		/// <summary>
		/// Registers the given <see cref="BodyIndex"/> (which can be acquired from your <see cref="CharacterBody"/>) such that players who select this survivor
		/// will have their healthbars render with influence from the given <see cref="AbstractColorProvider"/>. This also theoretically works for NPCs, but this
		/// has not actually been tested.
		/// </summary>
		/// <param name="source">The plugin that registered the survivor or body index. This is used to pin the blame on someone to help with debugging, especially when it's registered by a mod of a mod.</param>
		/// <param name="bodyIndex">The ID of the body to affect.</param>
		/// <param name="provider">The system to provide colors to the color manager.</param>
		public static void RegisterColorProvider(BaseUnityPlugin source, BodyIndex bodyIndex, AbstractColorProvider provider) {
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (bodyIndex == BodyIndex.None) throw new ArgumentException($"Cannot register {nameof(BodyIndex.None)} to a provider.", nameof(bodyIndex));
			if (provider == null) throw new ArgumentNullException(nameof(provider));
			if (_registry.ContainsKey(bodyIndex)) throw new ArgumentException($"BodyIndex {bodyIndex} has already been registered to a provider.", nameof(bodyIndex));

			Log.LogInfo($"{PluginToString(source)} registered character {bodyIndex} ({BodyCatalog.GetBodyName(bodyIndex)}) to be overridden by {provider.GetType().FullName}");
			_registry[bodyIndex] = provider;
			_registeredIndexTracker[bodyIndex] = source;
		}

	}
}
