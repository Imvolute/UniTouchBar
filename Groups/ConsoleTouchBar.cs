//
//  TouchBar - Console
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
public class ConsoleTouchBar {

    public static TouchBar.Group consoleGroup;
    public static TouchBar.Button clearButton;
    public static TouchBar.Button collapse;
    public static TouchBar.Button clearOnPlay;
    public static TouchBar.Button errorPause;

    static ConsoleTouchBar () {
        TouchBar.Manager.OnReady += TouchBar_Manager_OnReady;
        //TouchBar.Manager.OnUnityWindowChanged += TouchBar_Manager_OnUnityWindowChanged;
    }

    static void TouchBar_Manager_OnReady () {
        TouchBar.Manager.OnReady -= TouchBar_Manager_OnReady;
        consoleGroup = new TouchBar.Group ("console", 1);

        clearButton = consoleGroup.AddTextButton ("clear", "Clear", () => {
            EditConsole.Clear ();

        });

        collapse = consoleGroup.AddTextButton ("collapse", "Collapse", () => {

            if (EditConsole.GetFlag (EditConsole.ConsoleFlags.Collapse)) {
                collapse.UpdateBackgroundColor (new Color (0.26F, 0.26F, 0.26F));
                EditConsole.SetFlag (EditConsole.ConsoleFlags.Collapse, false);
            } else {
                collapse.UpdateBackgroundColor (new Color (0.5F, 0.5F, 0.5F));

                EditConsole.SetFlag (EditConsole.ConsoleFlags.Collapse, true);
            }

        });
        clearOnPlay = consoleGroup.AddTextButton ("clearplay", "ClearOnPlay", () => {

            if (EditConsole.GetFlag (EditConsole.ConsoleFlags.ClearOnPlay)) {
                clearOnPlay.UpdateBackgroundColor (new Color (0.26F, 0.26F, 0.26F));
                EditConsole.SetFlag (EditConsole.ConsoleFlags.ClearOnPlay, false);
            } else {
                clearOnPlay.UpdateBackgroundColor (new Color (0.5F, 0.5F, 0.5F));

                EditConsole.SetFlag (EditConsole.ConsoleFlags.ClearOnPlay, true);
            }

        });
        errorPause = consoleGroup.AddTextButton ("errorpause", "ErrorPause", () => {

            if (EditConsole.GetFlag (EditConsole.ConsoleFlags.ErrorPause)) {
                errorPause.UpdateBackgroundColor (new Color (0.26F, 0.26F, 0.26F));
                EditConsole.SetFlag (EditConsole.ConsoleFlags.ErrorPause, false);
            } else {
                errorPause.UpdateBackgroundColor (new Color (0.5F, 0.5F, 0.5F));

                EditConsole.SetFlag (EditConsole.ConsoleFlags.ErrorPause, true);
            }

        });

        TouchBar.AddGroup (consoleGroup);
        consoleGroup.ShowOnWindow (TouchBar.Windows.Console);
        consoleGroup.OnShow += ConsoleGroup_OnShow;
        //cameras.SetOptionWithText(0, "elo", () => { });

    }

    static void ConsoleGroup_OnShow () {
        if (EditConsole.GetFlag (EditConsole.ConsoleFlags.ClearOnPlay)) clearOnPlay.UpdateBackgroundColor (new Color (0.5F, 0.5F, 0.5F));
        else clearOnPlay.UpdateBackgroundColor (new Color (0.26F, 0.26F, 0.26F));
    }

    static void TouchBar_Manager_OnUnityWindowChanged (EditorWindow window) {
        if ((window.titleContent.text == "Console")) {
            consoleGroup.Show ();

            if (EditConsole.GetFlag (EditConsole.ConsoleFlags.ClearOnPlay)) clearOnPlay.UpdateBackgroundColor (new Color (0.5F, 0.5F, 0.5F));
            else clearOnPlay.UpdateBackgroundColor (new Color (0.26F, 0.26F, 0.26F));
        } else {
            consoleGroup.Hide ();
        }
    }

    public static class EditConsole {

        public enum ConsoleFlags {
            Collapse = 1,
            ClearOnPlay = 2,
            ErrorPause = 4,
            Verbose = 8,
            StopForAssert = 16,
            StopForError = 32,
            Autoscroll = 64,
            LogLevelLog = 128,
            LogLevelWarning = 256,
            LogLevelError = 512,
        }

        public static void Clear () {

            var logEntries = System.Type.GetType ("UnityEditor.LogEntries, UnityEditor.dll");
            var clearMethod = logEntries.GetMethod ("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            clearMethod.Invoke (null, null);
        }

        public static void SetFlag (ConsoleFlags flag, bool value) {
            try {
                var logEntries = System.Type.GetType ("UnityEditor.LogEntries, UnityEditor.dll");
                object[] parametersArray = new object[] {
                    (int) flag,
                    value
                };
                var clearMethod = logEntries.GetMethod ("SetConsoleFlag", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                if (clearMethod != null) {
                    clearMethod.Invoke (null, parametersArray);
                }
            } catch (System.Exception ex) {
                TouchBar.Log (ex);
            }
        }

        public static bool GetFlag (ConsoleFlags flag) {
            try {
                var logEntries = System.Type.GetType ("UnityEditor.LogEntries, UnityEditor.dll");
                //object[] parametersArray = new object[] { (int) flag,
                //          true
                //     };
                //var clearMethod = logEntries.GetMethod("SetConsoleFlag", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                var clearMethod = logEntries.GetProperty ("consoleFlags");
                if (clearMethod != null) {
                    int value = (int) clearMethod.GetValue (null, null);
                    ConsoleFlags flags = (ConsoleFlags) value;
                    bool show = ((flags & flag) != 0);

                    return show;
                }
            } catch (System.Exception ex) {
                TouchBar.Log (ex);
                return false;
            }
            return false;
        }

    }

}