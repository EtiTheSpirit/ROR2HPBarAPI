using BepInEx;
using BepInEx.Configuration;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.UI;
using ROR2HPBarAPI.Core;
using ROR2HPBarAPI.Data;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ROR2HPBarAPI {

	[BepInDependency(R2API.R2API.PluginGUID)]
	[R2APISubmoduleDependency(nameof(ItemAPI), nameof(LanguageAPI))]
	[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
	internal class HPBarPlugin : BaseUnityPlugin {
		public const string PLUGIN_GUID = PLUGIN_AUTHOR + "." + PLUGIN_NAME;
		public const string PLUGIN_AUTHOR = "Xan";
		public const string PLUGIN_NAME = "HPBarAPI";
		public const string PLUGIN_VERSION = "1.0.0";

		public static bool IsDebugMode => _debugMode.Value;
		private static ConfigEntry<bool> _debugMode;

		public void Awake() {
			Log.Init(Logger);
			Log.LogMessage("Initializing health bar API.");
			_debugMode = Config.Bind("HP Bar API", "Debug Mode", false, "If true, detailed messages are given for all hitches or errors that occur, even if they are not breaking. This can be used to nail down why your provider is seemingly doing nothing.");
			DefaultHealthAndShieldData.Init();
			Updater.Init();
			Log.LogMessage("Initialization complete.");

			// TestImplementation.Init(this);
		}
	}
}
