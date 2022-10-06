using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ROR2HPBarAPI.Helper {
	internal static class Images {

		public static Sprite NormalBarrier {
			get {
				if (_normalBarrier == null) {
					_normalBarrier = ImageHelper.CreateSprite("normalBarrier.png");
				}
				return _normalBarrier;
			}
		}
		private static Sprite _normalBarrier = null;

		public static Sprite ColorableBarrier {
			get {
				if (_colorableBarrier == null) {
					_colorableBarrier = ImageHelper.CreateSprite("colorlessBarrier.png");
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
