//
//  TouchBar - Project
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
using UnityEditor.SceneManagement;
using UnityEngine;
[InitializeOnLoad]
public class ProjectTouchBar {

    static ProjectTouchBar () {
        TouchBar.Manager.OnReady += TouchBar_Manager_OnReady;

    }

    static TouchBar.Group projectGroup;

    static void TouchBar_Manager_OnReady () {
        TouchBar.Manager.OnReady -= TouchBar_Manager_OnReady;
        projectGroup = new TouchBar.Group ("project", 2);

        foreach (var item in AssetDatabase.FindAssets ("t:scene")) {

            string path = AssetDatabase.GUIDToAssetPath (item);
            SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset> (path) as SceneAsset;
            if (scene != null) {
                projectGroup.AddImageButton ("scene" + scene.name, "/Editor/UniTouchBar/Icons/unity.png", scene.name, () => {
                    EditorSceneManager.OpenScene (path, OpenSceneMode.Single);

                });
            }
        }

        projectGroup.AddImageButton ("finder", "/Editor/UniTouchBar/Icons/finder.png", "", () => {
            OpenInMac (AssetDatabase.GetAssetPath (Selection.activeObject));
        });

        TouchBar.AddGroup (projectGroup);
        projectGroup.ShowOnWindow (TouchBar.Windows.Project);
    }

    public static void OpenInMac (string path) {
        bool openInsidesOfFolder = false;

        string macPath = path.Replace ("\\", "/");

        if (System.IO.Directory.Exists (macPath)) {
            openInsidesOfFolder = true;
        }

        if (!macPath.StartsWith ("\"")) {
            macPath = "\"" + macPath;
        }

        if (!macPath.EndsWith ("\"")) {
            macPath = macPath + "\"";
        }

        string arguments = (openInsidesOfFolder ? "" : "-R ") + macPath;

        try {
            System.Diagnostics.Process.Start ("open", arguments);
        } catch (System.ComponentModel.Win32Exception e) {

            e.HelpLink = "";
        }
    }
}