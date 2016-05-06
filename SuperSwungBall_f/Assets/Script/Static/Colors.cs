using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class Colors {

	public static class Normal {
		public static Color Green = new Color (44f / 255f, 208f / 255f, 76f / 255f);	// #2CD04C
		public static Color Red = new Color (229f / 255f, 76f / 255f, 76f / 255f); 		// #E54C4C
		public static Color Blue = new Color (45F / 255f, 125f / 255f, 204f / 255f);	// #2D7DCC
		public static Color Orange = new Color (247f / 255f, 179f / 255f, 43f / 255f);	// #F7B32B
		public static Color Brown = new Color (221f / 255f, 203f / 255f, 168f / 255f);	// #DDCBA8
	}

	public static class Highlighted {
		public static Color Green = new Color (27f / 255f, 176f / 255f, 56f / 255f);	// #1BB038
		public static Color Red = new Color (210f / 255f, 60f / 255f, 60f / 255f);		// #D23C3C
		public static Color Blue = new Color (14f / 255f, 103f / 255f, 175f / 255f);	// #0E67AF
		public static Color Orange = new Color (229f / 255f, 161f / 255f, 25f / 255f);	// #E4A019
	}

	public static class Pressed {
		public static Color Green = new Color (19f / 255f, 146f / 255f, 44f / 255f);	// #13922C
		public static Color Red = new Color (190f / 255f, 50f / 255f, 50f / 255f);		// #BE3232
		public static Color Blue = new Color (10f / 255f, 90f / 255f, 160f / 255f);		// #0A5AA0
		public static Color Orange = new Color (212f / 255f, 147f / 255f, 17f / 255f);	// #D39211
	}

	public static class Disable {
		public static Color Grey = new Color (200f / 255f, 200f / 255f, 200f / 255f, 128f / 255f); // #C8C8C880
	}


	public static class Block {
		public static ColorBlock Green {
			get {
				ColorBlock block = new ColorBlock();
				block.colorMultiplier = 1f;
				block.normalColor = Normal.Green;
				block.highlightedColor = Highlighted.Green;
				block.pressedColor = Pressed.Green;
				block.disabledColor = Disable.Grey;
				return block;
			}
		}
		public static ColorBlock Blue {
			get {
				ColorBlock block = new ColorBlock();
				block.colorMultiplier = 1f;
				block.normalColor = Normal.Blue;
				block.highlightedColor = Highlighted.Blue;
				block.pressedColor = Pressed.Blue;
				block.disabledColor = Disable.Grey;
				return block;
			}
		}
		public static ColorBlock Red {
			get {
				ColorBlock block = new ColorBlock();
				block.colorMultiplier = 1f;
				block.normalColor = Normal.Red;
				block.highlightedColor = Highlighted.Red;
				block.pressedColor = Pressed.Red;
				block.disabledColor = Disable.Grey;
				return block;
			}
		}
		public static ColorBlock Orange {
			get {
				ColorBlock block = new ColorBlock();
				block.colorMultiplier = 1f;
				block.normalColor = Normal.Orange;
				block.highlightedColor = Highlighted.Orange;
				block.pressedColor = Pressed.Orange;
				block.disabledColor = Disable.Grey;
				return block;
			}
		}
	}





}
