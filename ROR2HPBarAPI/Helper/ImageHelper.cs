using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ROR2HPBarAPI.Helper {
	internal static class ImageHelper {

		public static Stream GetResource(string name) {
			name = Assembly.GetExecutingAssembly().GetManifestResourceNames().FirstOrDefault(objName => objName.EndsWith(name));
			if (name == null) {
				throw new InvalidDataException($"Failed to find an embedded resource with the name \"{name}\". Did you forget to mark it as a resource in VS?");
			}
			return Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
		}

		public static Texture2D CreateTexture(Stream fileIn) {
			Texture2D tex = new Texture2D(1, 1);
			using (MemoryStream mStr = new MemoryStream()) {
				fileIn.CopyTo(mStr);
				if (tex.LoadImage(mStr.ToArray())) {
					tex.wrapMode = TextureWrapMode.Clamp;
					return tex;
				}
			}
			Log.LogError("Failed to load image from stream!");
			return null;
		}

		public static Sprite CreateSprite(Texture2D tex, int? overridePixelsPerUnit = null, Vector4? overrideBorder = null) {
			int resolution = tex.width;
			if (resolution != tex.height) {
				Log.LogError("The input texture is not square! It may not render correctly.");
			}
			return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2), overridePixelsPerUnit ?? resolution, 0, SpriteMeshType.FullRect, overrideBorder ?? default);
		}

		public static Sprite CreateSprite(Stream fileIn, int? overridePixelsPerUnit = null, Vector4? overrideBorder = null) => CreateSprite(CreateTexture(fileIn), overridePixelsPerUnit, overrideBorder);

		public static Sprite CreateSprite(string rsrcName, int? overridePixelsPerUnit = null, Vector4? overrideBorder = null) => CreateSprite(GetResource(rsrcName), overridePixelsPerUnit, overrideBorder);

	}
}
