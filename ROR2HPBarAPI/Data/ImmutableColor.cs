using UnityEngine;

namespace ROR2HPBarAPI.Data {

	/// <summary>
	/// A read-only <see cref="Color32"/> alternative, intended for use in constant data storage for this plugin to prevent unwanted outside tampering.
	/// </summary>
	internal readonly struct ImmutableColor {

		public readonly byte R;

		public readonly byte G;

		public readonly byte B;

		public readonly byte A;

		/// <summary>
		/// Construct a new <see cref="ImmutableColor"/> with the given RGB color
		/// </summary>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		public ImmutableColor(int r, int g, int b) {
			R = (byte)r;
			G = (byte)g;
			B = (byte)b;
			A = byte.MaxValue;
		}

		public ImmutableColor(int r, int g, int b, int a) {
			R = (byte)r;
			G = (byte)g;
			B = (byte)b;
			A = (byte)a;
		}

		/// <summary>
		/// Create a new color with the given ARGB value (in that order).
		/// </summary>
		/// <param name="RGB"></param>
		public ImmutableColor(uint ARGB) {
			A = (byte)((ARGB & 0xFF000000) >> 24);
			R = (byte)((ARGB & 0x00FF0000) >> 16);
			G = (byte)((ARGB & 0x0000FF00) >> 08);
			B = (byte)((ARGB & 0x000000FF) >> 00);
		}

		/// <summary>
		/// Create a new color with the given RGB value.<para/>
		/// <strong>This does not include alpha, making the value <c>..RRGGBB</c> (alpha is always fully opaque (255)). To include alpha, pass in the color integer as <see langword="uint"/>.</strong>
		/// </summary>
		/// <param name="RGB"></param>
		public ImmutableColor(int RGB) {
			R = (byte)((RGB & 0x00FF0000) >> 16);
			G = (byte)((RGB & 0x0000FF00) >> 08);
			B = (byte)((RGB & 0x000000FF) >> 00);
			A = byte.MaxValue;
		}
		
		private static int FloatToByte(float f) {
			return Mathf.RoundToInt(Mathf.Clamp01(f) * 255f);
		}

		public static implicit operator Color32(ImmutableColor @this) => new Color32(@this.R, @this.G, @this.B, @this.A);
		public static implicit operator Color(ImmutableColor @this) => new Color(@this.R / 255f, @this.G / 255f, @this.B / 255f, @this.A / 255f);
		public static implicit operator ImmutableColor(Color @this) => new ImmutableColor(FloatToByte(@this.r), FloatToByte(@this.g), FloatToByte(@this.b), FloatToByte(@this.a));

		public override string ToString() {
			return $"RGBA[{R}, {G}, {B}, {A}]";
		}
	}
}
