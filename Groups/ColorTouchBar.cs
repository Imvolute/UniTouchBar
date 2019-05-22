//
//  TouchBar - Color
//
//  Created by Bartosz Swiety
//  Copyright © 2018 Imvolute. All rights reserved.
//
//  but you can edit ;)
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Uni;
using UnityEditor;
using UnityEngine;
[InitializeOnLoad]
public class ColorTouchBar {

	public static TouchBar.Group colorGroup;
	public static TouchBar.Button[] buttons;

	static ColorTouchBar () {
		TouchBar.Manager.OnReady += TouchBar_Manager_OnReady;
	}

	static void TouchBar_Manager_OnReady () {
		TouchBar.Manager.OnReady -= TouchBar_Manager_OnReady;
		colorGroup = new TouchBar.Group ("color", 1, () => {

			Color[] colors = Colors.GetColors ();
			if (colors != null) {

				foreach (Color item in colors) {
					if (colorGroup.items.Find ((obj) => obj.identifier == item.ToString ()) == null) {
						TouchBar.Button button = colorGroup.AddTextButton (item.ToString (), " ", () => {
							//							  TouchBar.Log(item.ToString());
							Colors.SetColor (item);
						});
						button.Add (colorGroup.identifier);
						button.UpdateBackgroundColor (item);
					}
				}
			}

		});
		TouchBar.AddGroup (colorGroup);
		colorGroup.ShowOnWindow (TouchBar.Windows.Color);
	}

	public class Colors : EditorWindow {

		public static void PickColor () {
			//SOON
		}

		static object ColorWindow () {
			try {
				var gameview = System.Type.GetType ("UnityEditor.ColorPicker, UnityEditor.dll");

				var view = gameview.GetProperty (TouchBar.isUnity2018 ? "instance" : "get", BindingFlags.Public | BindingFlags.Static);

				return view.GetValue (null, null);
			} catch (System.Exception ex) {
				TouchBar.Log (ex);
				return false;
			}
		}

		public static Color[] GetColors () {
			try {
				object picker = ColorWindow ();
				var library = System.Type.GetType ("UnityEditor.ColorPicker, UnityEditor.dll").GetField ("m_ColorLibraryEditor", BindingFlags.NonPublic | BindingFlags.Instance).GetValue (picker);
				if (library != null) {
					var presets = library.GetType ().GetMethod ("GetCurrentLib", BindingFlags.Public | BindingFlags.Instance).Invoke (library, null);

					int count = (int) presets.GetType ().GetMethod ("Count", BindingFlags.Public | BindingFlags.Instance).Invoke (presets, null);
					var getColor = presets.GetType ().GetMethod ("GetPreset", BindingFlags.Public | BindingFlags.Instance);

					Color[] output = new Color[count];

					for (int i = 0; i < count; i++) {
						output[i] = (Color) getColor.Invoke (presets, new object[] {
							i
						});
					}

					return output;
				}
				return null;
			} catch (System.Exception ex) {
				TouchBar.Log (ex);
				return null;
			}
		}

		public static void SetColor (Color color) {
			try {
				object picker = ColorWindow ();
				var gameview = System.Type.GetType ("UnityEditor.ColorPicker, UnityEditor.dll");
				var view = gameview.GetMethod ("SetColor", BindingFlags.NonPublic | BindingFlags.Instance);
				object[] parameters = new object[] {
					color
				};
				view.Invoke (picker, parameters);
				gameview.GetProperty ("colorChanged", BindingFlags.NonPublic | BindingFlags.Instance).SetValue (picker, true, null);
			} catch (System.Exception ex) {
				TouchBar.Log (ex);

			}
		}

	}
}