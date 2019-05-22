//
//  TouchBar - Main
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
/// Main TouchBar - shown on all windows.
/// </summary>
[InitializeOnLoad]
public class MainTouchBar {

    public static TouchBar.Group mainGroup;
    public static TouchBar.Button playButton;

    static MainTouchBar () {
        TouchBar.Manager.OnReady += TouchBar_Manager_OnReady;
        TouchBar.Manager.OnUnityPlaymodeChanged += TouchBar_Manager_OnUnityPlaymodeChanged;
    }

    static void TouchBar_Manager_OnReady () {
        TouchBar.Manager.OnReady -= TouchBar_Manager_OnReady;
        mainGroup = new TouchBar.Group ("main", 0);
        playButton = mainGroup.AddImageButton ("play", "/Editor/UniTouchBar/Icons/play.png", "", () => {
            playButton.UpdateBackgroundColor (new Color (0.5F, 0.5F, 0.5F));
            EditorApplication.isPlaying = !EditorApplication.isPlaying;
        });

        mainGroup.ShowOnWindow (TouchBar.Windows.ALL);
        TouchBar.AddGroup (mainGroup);
    }

    static void TouchBar_Manager_OnUnityPlaymodeChanged (PlayModeStateChange change) {
        if (change == PlayModeStateChange.EnteredPlayMode) {
            playButton.UpdateImage ("/Editor/UniTouchBar/Icons/playing.png");
            playButton.UpdateBackgroundColor (new Color (0.26F, 0.26F, 0.26F));
        } else if (change == PlayModeStateChange.EnteredEditMode) {
            playButton.UpdateImage ("/Editor/UniTouchBar/Icons/play.png");
            playButton.UpdateBackgroundColor (new Color (0.26F, 0.26F, 0.26F));
        }
    }
}