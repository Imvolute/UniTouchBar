//
//  TouchBar - GameObject
//
//  Created by Bartosz Swiety
//  Copyright © 2018 Imvolute. All rights reserved.
//
//  but you can edit ;)
//

using System.Collections;
using System.Collections.Generic;
using Uni;
using UnityEditor;
using UnityEngine;

/// <summary>
/// GameObject selection TouchBar.
/// </summary>
[InitializeOnLoad]
public class GameObjectTouchBar {

	public static TouchBar.Group gameobjectGroup;
	public static TouchBar.Button enableButton;

	static GameObjectTouchBar () {
		TouchBar.Manager.OnReady += TouchBar_Manager_OnReady;

	}

	public static bool ready;
	public static bool tick;
	static void TouchBar_Manager_OnReady () {
		TouchBar.Manager.OnReady -= TouchBar_Manager_OnReady;
		EditorApplication.update += EditorApplication_Update;
		gameobjectGroup = new TouchBar.Group ("gameobject", 4);
		enableButton = gameobjectGroup.AddImageButton ("enabled", "/Editor/UniTouchBar/Icons/enabled.png", "", () => {
			if (Selection.activeGameObject != null) {
				Selection.activeGameObject.SetActive (!Selection.activeGameObject.activeSelf);
			}

		});
		gameobjectGroup.ShowOnWindow (TouchBar.Windows.None);
		ready = true;
		TouchBar.AddGroup (gameobjectGroup);
	}

	static void EditorApplication_Update () {
		if (ready == true) {
			if (Selection.activeGameObject != null) {
				gameobjectGroup.Show ();

				if ((Selection.activeGameObject.GetComponent<Animator> () != null) || (Selection.activeGameObject.GetComponent<Animation> () != null)) {
					//                    TouchBar.Log("ej");
					if (AnimatorTouchBar.loaded) {
						if (AnimatorTouchBar.group.windowMask == (AnimatorTouchBar.group.windowMask | (1 << (int) TouchBar.Windows.Scene)))
							AnimatorTouchBar.group.Show ();
					}
				} else {
					AnimatorTouchBar.group.Hide ();
				}

				if (Selection.activeGameObject.activeSelf) {
					if (!tick) {
						enableButton.UpdateImage ("/Editor/UniTouchBar/Icons/enabled.png");
						tick = true;
					}
				} else {
					if (tick) {
						enableButton.UpdateImage ("/Editor/UniTouchBar/Icons/disabled.png");
						tick = false;
					}
				}
			} else {
				if (AnimatorTouchBar.loaded)
					AnimatorTouchBar.group.Hide ();

				gameobjectGroup.Hide ();
			}

		}
	}
}