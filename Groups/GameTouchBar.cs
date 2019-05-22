//
//  TouchBar - Game
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
public class
GameTouchBar {

    public static TouchBar.Group gameGroup;
    public static TouchBar.Button muteButton;
    public static TouchBar.Button statsButton;

    public static TouchBar.Button pauseButton;
    public static TouchBar.Button maximizeButton;

    static GameTouchBar () {
        TouchBar.Manager.OnReady += TouchBar_Manager_OnReady;
        TouchBar.Manager.OnUnityPlaymodeChanged += TouchBar_Manager_OnUnityPlaymodeChanged;
    }

    static void CheckPause () {
        if (pauseButton != null) {
            if (EditorApplication.isPaused) {
                pauseButton.UpdateImage ("/Editor/UniTouchBar/Icons/paused.png");
            } else {
                pauseButton.UpdateImage ("/Editor/UniTouchBar/Icons/pause.png");
            }

        }
    }

    static void TouchBar_Manager_OnReady () {
        TouchBar.Manager.OnReady -= TouchBar_Manager_OnReady;
        gameGroup = new TouchBar.Group ("game", 3);

        pauseButton = gameGroup.AddImageButton ("pause", "/Editor/UniTouchBar/Icons/pause.png", "", () => {
            EditorApplication.isPaused = !EditorApplication.isPaused;

            CheckPause ();

        });

        TouchBar.Button space = gameGroup.AddTextButton ("space", "     ");

        muteButton = gameGroup.AddTextButton ("mute", "mute", () => {
            EditorUtility.audioMasterMute = !EditorUtility.audioMasterMute;
            muteButton.Highlight (EditorUtility.audioMasterMute);
        });

        statsButton = gameGroup.AddTextButton ("stats", "stats", () => {
            bool val = !EditGameView.GetFlag ("m_Stats");
            EditGameView.SetFlag ("m_Stats", val);
            statsButton.Highlight (val);
        });

        maximizeButton = gameGroup.AddImageButton ("maximize", "/Editor/UniTouchBar/Icons/resize.png", "", () => {

            bool val = !EditGameView.GetMaximized ();
            EditGameView.Maximize (val);
            maximizeButton.Highlight (val);

        });

        TouchBar.AddGroup (gameGroup);
        space.Hide ();

    }

    static void TouchBar_Manager_OnUnityPlaymodeChanged (PlayModeStateChange change) {
        if (change == PlayModeStateChange.EnteredPlayMode) {
            gameGroup.Show ();
            muteButton.Highlight (EditorUtility.audioMasterMute);
        } else if (change == PlayModeStateChange.EnteredEditMode) {
            gameGroup.Hide ();
        }

        CheckPause ();
    }
    public class EditGameView {

        public static bool GetMaximized () {
            try {
                var gameview = System.Type.GetType ("UnityEditor.GameView, UnityEditor.dll");
                var view = gameview.GetField ("s_LastFocusedGameView", BindingFlags.NonPublic | BindingFlags.Static);
                var stats = gameview.GetProperty ("maximized", BindingFlags.Public | BindingFlags.Instance);
                if (view != null) {
                    return (bool) stats.GetValue (view.GetValue (null), null);
                } else TouchBar.Log ("null");

                return false;
            } catch (System.Exception ex) {
                TouchBar.Log (ex);
                return false;
            }
        }

        public static void Maximize (bool value) {
            try {
                var gameview = System.Type.GetType ("UnityEditor.GameView, UnityEditor.dll");
                var view = gameview.GetField ("s_LastFocusedGameView", BindingFlags.NonPublic | BindingFlags.Static);
                var stats = gameview.GetProperty ("maximized", BindingFlags.Public | BindingFlags.Instance);
                if (view != null) {
                    stats.SetValue (view.GetValue (null), value, null);
                } else TouchBar.Log ("null");

            } catch (System.Exception ex) {
                TouchBar.Log (ex);

            }
        }

        public static bool GetFlag (string flag) {
            try {
                var gameview = System.Type.GetType ("UnityEditor.GameView, UnityEditor.dll");
                var view = gameview.GetField ("s_LastFocusedGameView", BindingFlags.NonPublic | BindingFlags.Static);
                var stats = gameview.GetField (flag, BindingFlags.NonPublic | BindingFlags.Instance);
                if (view != null) {
                    return (bool) stats.GetValue (view.GetValue (null));
                } else TouchBar.Log ("null");

                return false;
            } catch (System.Exception ex) {
                TouchBar.Log (ex);
                return false;
            }

        }

        public static void SetFlag (string flag, bool value) {
            try {
                TouchBar.Log ("Jazda");
                var gameview = System.Type.GetType ("UnityEditor.GameView, UnityEditor.dll");
                var view = gameview.GetField ("s_LastFocusedGameView", BindingFlags.NonPublic | BindingFlags.Static);
                var stats = gameview.GetField (flag, BindingFlags.NonPublic | BindingFlags.Instance);
                if (view != null) {
                    stats.SetValue (view.GetValue (null), value);
                } else TouchBar.Log ("null");

                //return false;
            } catch (System.Exception ex) {
                TouchBar.Log (ex);
                //return false;
            }
            //return false;
        }

    }

}