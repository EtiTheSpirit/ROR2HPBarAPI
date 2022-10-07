using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ROR2HPBarAPI.Helper {
	internal static class Images {

		public static Sprite NormalBarrier {
			get {
				if (_normalBarrier == null) {
					_normalBarrier = ImageHelper.CreateSprite("normalBarrier.png", 100, new Vector4(8, 8, 8, 8));
					_normalBarrier.pivot.Set(32, 32);
					_normalBarrier.rect.Set(0, 0, 64, 64);
					_normalBarrier.textureRect.Set(0, 0, 64, 64);
					_normalBarrier.textureRectOffset.Set(0, 0);
					_normalBarrier.bounds.SetMinMax(new Vector3(-0.15f, -0.15f, -0.05f), new Vector3(0.15f, 0.15f, 0.05f));
				}
				return _normalBarrier;
			}
		}
		private static Sprite _normalBarrier = null;

		public static Sprite ColorableBarrier {
			get {
				if (_colorableBarrier == null) {
					_colorableBarrier = ImageHelper.CreateSprite("colorlessBarrier.png", 100, new Vector4(8, 8, 8, 8));
					_colorableBarrier.pivot.Set(32, 32);
					_colorableBarrier.rect.Set(0, 0, 64, 64);
					_colorableBarrier.textureRect.Set(0, 0, 64, 64);
					_colorableBarrier.textureRectOffset.Set(0, 0);
					_colorableBarrier.bounds.SetMinMax(new Vector3(-0.15f, -0.15f, -0.05f), new Vector3(0.15f, 0.15f, 0.05f));
				} 
				return _colorableBarrier;
			}
		}
		private static Sprite _colorableBarrier = null;

		public static Sprite NormalHurtBar {
			get {
				if (_normalHurtBar == null) {
					_normalHurtBar = ImageHelper.CreateSprite("normalHurtBar.png");
				}
				return _normalHurtBar;
			}
		}
		private static Sprite _normalHurtBar = null;

		public static Sprite ColorableHurtBar {
			get {
				if (_colorableHurtBar == null) {
					_colorableHurtBar = ImageHelper.CreateSprite("colorlessHurtBar.png");
				}
				return _colorableHurtBar;
			}
		}
		private static Sprite _colorableHurtBar = null;

	}
}
