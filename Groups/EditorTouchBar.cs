//
//  TouchBar - Editor
//
//  Created by Bartosz Swiety
//  Copyright © 2018 Imvolute. All rights reserved.
//
//  but you can edit ;)
//

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Uni;
using UnityEditor;
using UnityEngine;
[InitializeOnLoad]
public class EditorTouchBar {

    public static TouchBar.Group editorGroup;
    public static TouchBar.Segments cameras;
    public static TouchBar.Button dbutton;
    public static TouchBar.Button addbutton;
    static EditorTouchBar () {
        TouchBar.Manager.OnReady += TouchBar_Manager_OnReady;

    }

    static void TouchBar_Manager_OnReady () {
        TouchBar.Manager.OnReady -= TouchBar_Manager_OnReady;
        editorGroup = new TouchBar.Group ("editor", 1, () => {
            dbutton.Highlight (SceneView.GetFlag ("in2DMode"));

        });
        cameras = editorGroup.AddSegments ("cameramodes", 6);
        dbutton = editorGroup.AddTextButton ("2d", "2D", () => {
            bool valu = !SceneView.GetFlag ("in2DMode");
            SceneView.SetFlag ("in2DMode", valu);
            dbutton.Highlight (valu);

        });

        addbutton = editorGroup.AddImageButton ("addgameobject", "/Editor/UniTouchBar/Icons/go.png", "", () => {

            GameObject gm = new GameObject ("GameObject");
            Selection.activeGameObject = gm;

        });

        TouchBar.AddGroup (editorGroup);

        editorGroup.ShowOnWindow (TouchBar.Windows.Scene);
        editorGroup.ShowOnWindow (TouchBar.Windows.Hierarchy);

        cameras.SetOptionWithImage (0, "/Editor/UniTouchBar/Icons/hand.png", () => {
                TouchBar.Log ("Option");

                Tools.current = Tool.View;
            },
            35);
        cameras.SetOptionWithImage (1, "/Editor/UniTouchBar/Icons/move.png", () => {
                Tools.current = Tool.Move;
            },
            35);
        cameras.SetOptionWithImage (2, "/Editor/UniTouchBar/Icons/rotate.png", () => {
                Tools.current = Tool.Rotate;
            },
            35);
        cameras.SetOptionWithImage (3, "/Editor/UniTouchBar/Icons/resize.png", () => {
                Tools.current = Tool.Scale;

            },
            35);

        cameras.SetOptionWithImage (4, "/Editor/UniTouchBar/Icons/transform.png", () => {
                Tools.current = Tool.Transform;

            },
            35);

        cameras.SetOptionWithImage (5, "/Editor/UniTouchBar/Icons/rect.png", () => {
                Tools.current = Tool.Rect;

            },
            35);
    }

    public class SceneView {
        public static bool GetFlag (string flag) {
            try {
                var gameview = System.Type.GetType ("UnityEditor.SceneView, UnityEditor.dll");
                if (gameview != null) {
                    var view = gameview.GetField ("s_LastActiveSceneView", BindingFlags.NonPublic | BindingFlags.Static);
                    var stats = gameview.GetProperty (flag, BindingFlags.Instance | BindingFlags.Public);

                    if (stats != null) {
                        if ((view != null) && (view.GetValue (null) != null)) return ((bool) stats.GetValue (view.GetValue (null), null));
                    } else return false;
                }
                return false;
            } catch (System.Exception ex) {
                TouchBar.Log (ex);
                return false;
            }
        }

        public static void SetFlag (string flag, bool value) {
            try {
                var gameview = System.Type.GetType ("UnityEditor.SceneView, UnityEditor.dll");
                var view = gameview.GetField ("s_LastActiveSceneView", BindingFlags.NonPublic | BindingFlags.Static);

                if (view != null) {
                    var stats = gameview.GetProperty (flag, BindingFlags.Instance | BindingFlags.Public);

                    if (stats != null) {
                        stats.SetValue (view.GetValue (null), value, null);
                    }
                }
            } catch (System.Exception ex) {
                TouchBar.Log (ex);
            }
        }
    }

}