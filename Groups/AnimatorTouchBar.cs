//
//  TouchBar - Animator
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
public class AnimatorTouchBar {

    public static TouchBar.Group group;
    public static TouchBar.Button recordButton;
    public static TouchBar.Segments segments;
    public static TouchBar.Slider slider;

    public static bool loaded;

    static AnimatorTouchBar () {
        TouchBar.Manager.OnReady += TouchBar_Manager_OnReady;

    }

    static void EditorApplication_Update () {

        if (group.showing && slider != null) {
            if (AnimatorEditor.window != null) {
                try {

                    float max = (float) System.Type.GetType ("UnityEditorInternal.AnimationWindowState, UnityEditor.dll").GetProperty ("maxTime").GetValue (AnimatorEditor.stateWindow, null);
                    float time = (float) System.Type.GetType ("UnityEditorInternal.AnimationWindowState, UnityEditor.dll").GetProperty ("currentTime").GetValue (AnimatorEditor.stateWindow, null);
                    slider.UpdateSlider (0, max, time);
                } catch (System.Exception ex) {
                    TouchBar.Log (ex);
                }
            }
        }
    }

    static void SegmentClick () {
        if (AnimatorEditor.GetFlag ("playing")) segments.Select (2);
        else segments.Select (-1);

    }

    static void TouchBar_Manager_OnReady () {
        TouchBar.Manager.OnReady -= TouchBar_Manager_OnReady;
        EditorApplication.update += EditorApplication_Update;
        group = new TouchBar.Group ("animation", 4);

        segments = group.AddSegments ("segments", 5);

        recordButton = group.AddImageButton ("record", "/Editor/UniTouchBar/Icons/record.png", "", () => {

            if (AnimatorEditor.GetFlag ("recording")) {
                //STOP
                System.Type.GetType ("UnityEditorInternal.AnimationWindowState, UnityEditor.dll").GetMethod ("StopRecording").Invoke (AnimatorEditor.stateWindow, null);
                recordButton.UpdateImage ("/Editor/UniTouchBar/Icons/record.png");
            } else {
                //START
                System.Type.GetType ("UnityEditorInternal.AnimationWindowState, UnityEditor.dll").GetMethod ("StartRecording").Invoke (AnimatorEditor.stateWindow, null);
                recordButton.UpdateImage ("/Editor/UniTouchBar/Icons/recording.png");
            }
            AnimatorEditor.Repaint ();
        });

        slider = group.AddSlider ("slider", 0, 100, 1, (double obj) => {

            try {

            object[] parameters = new object[] {
            (float) obj
                };
                System.Type.GetType ("UnityEditorInternal.IAnimationWindowControl, UnityEditor.dll").GetMethod ("GoToTime").Invoke (AnimatorEditor.controlInterface, parameters);
                AnimatorEditor.Repaint ();
            } catch (System.Exception ex) {
                TouchBar.Log (ex);
            }

            ///TouchBar.Log(System.Type.GetType("UnityEditorInternal.AnimationWindowState, UnityEditor.dll").GetProperty("maxTime").GetValue(AnimatorEditor.stateWindow, null));

        });
        TouchBar.AddGroup (group);
        segments.SetOptionWithImage (0, "/Editor/UniTouchBar/Icons/segment_rewind_back.png", () => {
            System.Type.GetType ("UnityEditorInternal.IAnimationWindowControl, UnityEditor.dll").GetMethod ("GoToFirstKeyframe").Invoke (AnimatorEditor.controlInterface, null);
            AnimatorEditor.Repaint ();
            AnimatorTouchBar.SegmentClick ();

        });
        segments.SetOptionWithImage (1, "/Editor/UniTouchBar/Icons/segment_step_back.png", () => {
            System.Type.GetType ("UnityEditorInternal.IAnimationWindowControl, UnityEditor.dll").GetMethod ("GoToPreviousKeyframe").Invoke (AnimatorEditor.controlInterface, null);
            AnimatorEditor.Repaint ();
            AnimatorTouchBar.SegmentClick ();
        });
        segments.SetOptionWithImage (2, "/Editor/UniTouchBar/Icons/segment_play.png", () => {
            if (AnimatorEditor.GetFlag ("playing")) {
                //STOP
                segments.Select (-1);
                System.Type.GetType ("UnityEditorInternal.AnimationWindowState, UnityEditor.dll").GetMethod ("StopPlayback").Invoke (AnimatorEditor.stateWindow, null);
            } else {
                //START
                segments.Select (2);
                System.Type.GetType ("UnityEditorInternal.AnimationWindowState, UnityEditor.dll").GetMethod ("StartPlayback").Invoke (AnimatorEditor.stateWindow, null);
            }
            AnimatorEditor.Repaint ();
        });
        segments.SetOptionWithImage (3, "/Editor/UniTouchBar/Icons/segment_step.png", () => {
            System.Type.GetType ("UnityEditorInternal.IAnimationWindowControl, UnityEditor.dll").GetMethod ("GoToNextKeyframe").Invoke (AnimatorEditor.controlInterface, null);
            AnimatorEditor.Repaint ();
            AnimatorTouchBar.SegmentClick ();
        });
        segments.SetOptionWithImage (4, "/Editor/UniTouchBar/Icons/segment_rewind.png", () => {
            System.Type.GetType ("UnityEditorInternal.IAnimationWindowControl, UnityEditor.dll").GetMethod ("GoToLastKeyframe").Invoke (AnimatorEditor.controlInterface, null);
            AnimatorEditor.Repaint ();
            AnimatorTouchBar.SegmentClick ();

        });

        group.ShowOnWindow (TouchBar.Windows.Animation);
        group.ShowOnWindow (TouchBar.Windows.Scene);
        group.OnShow += () => {
            AnimatorEditor.Clean ();
            //            TouchBar.Log("Hello");
            loaded = true;
            if (AnimatorEditor.window == null) {
                group.Hide ();
                loaded = false;
            }

            if ((EditorWindow.focusedWindow != null) && (EditorWindow.focusedWindow.titleContent.text == "Scene")) {

            }
        };
    }

    class AnimatorEditor {

        public static void Clean () {
            //TODO v1.03 Detect when AnimationWindow was closed and reload when needed.
            _window = null;
            _animEditor = null;
            _stateWindow = null;
            _controlInterface = null;
        }

        private static object _window;
        public static object window {
            get {
                if (_window == null) {

                    var animationWindow = System.Type.GetType ("UnityEditor.AnimationWindow, UnityEditor.dll");
                    var view = animationWindow.GetField ("s_AnimationWindows", BindingFlags.NonPublic | BindingFlags.Static);

                    IList list = (view.GetValue (null) as IList);

                    if (list.Count > 0) _window = list[0];
                }
                return _window;
            }
        }

        private static object _animEditor;
        public static object animEditor {
            get {
                if (_animEditor == null) {
                    var animationWindow = System.Type.GetType ("UnityEditor.AnimationWindow, UnityEditor.dll");
                    var editor = animationWindow.GetField ("m_AnimEditor", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (window != null) _animEditor = editor.GetValue (window);
                }
                return _animEditor;
            }
        }

        private static object _stateWindow;
        public static object stateWindow {
            get {
                if (_stateWindow == null) {
                    var state = System.Type.GetType ("UnityEditor.AnimEditor, UnityEditor.dll").GetField ("m_State", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (animEditor != null) _stateWindow = state.GetValue (animEditor);
                }
                return _stateWindow;
            }
        }

        private static object _controlInterface;
        public static object controlInterface {
            get {
                if (_controlInterface == null) {
                    var state = System.Type.GetType ("UnityEditor.AnimEditor, UnityEditor.dll").GetProperty ("controlInterface", BindingFlags.Public | BindingFlags.Instance);
                    if (animEditor != null) _controlInterface = state.GetValue (animEditor, null);
                }
                return _controlInterface;
            }
        }

        public static void Repaint () {
            if (window != null) System.Type.GetType ("UnityEditor.AnimationWindow, UnityEditor.dll").GetMethod ("Repaint").Invoke (window, null);
        }

        public static bool GetFlag (string name) {
            try {
                //var stateeditor = System.Type.GetType("UnityEditorInternal.AnimationWindowState, UnityEditor.dll");
                var flag = System.Type.GetType ("UnityEditorInternal.AnimationWindowState, UnityEditor.dll").GetProperty (name, BindingFlags.Public | BindingFlags.Instance);
                bool value = (bool) flag.GetValue (stateWindow, null);
                return value;
            } catch (System.Exception ex) {
                TouchBar.Log (ex);
                return false;
            }
        }
    }

}