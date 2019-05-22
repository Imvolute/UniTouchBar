/*
 * 
 * 
 * UniTouchBar  - SettingsWindow  + Test Script
 * 
 * 
 * API:
 * http://imvolute.com/unitouchbar/api
 * 
 * GIT:
 * http://github.com/Imvolute/UniTouchBar
 * 
 * CONTACT:
 * unitouchbar@imvolute.com
 * 
 * 
 * 
 * */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Required for using Touchbar
using Uni;



//Put this Script into Editor->UniTouchbar->Groups

[InitializeOnLoad]
public class Test:MonoBehaviour {

    static Test () {
        //Manager will tell us when TouchBar is ready for us.
        TouchBar.Manager.OnReady += Manager_OnReady;
    }

    //We have to store our elements somewhere - the best would be here - in static(not needed).
    private static TouchBar.Group settingsGroup;
    private static TouchBar.Button websiteButton;
    private static TouchBar.Button faqButton;


    static void Manager_OnReady () {
        //Ready? We don't need it anymore. - keep it fast!
        TouchBar.Manager.OnReady -= Manager_OnReady;

        //--------------------1.GROUP----------------------------
        //--Everything is allways stored in groups.
        settingsGroup = new TouchBar.Group ("settingsGroup", 5, () => {
            TouchBar.Log ("Settings Touchbar Loaded - Welcome in debug ;)");
        });

        //--------------------2.Items-----------------------------
        //Ok now buttons - all Items are created inside the group ok?
        websiteButton = settingsGroup.AddTextButton ("websiteButton", "Website", () => {
            //Button cliked. What now?
            Application.OpenURL ("http://imvolute.com/unitouchbar/");
        });

        //--FAQ BUTTON
        faqButton = settingsGroup.AddTextButton ("faqButton", "FAQ", () => {
            Application.OpenURL ("http://imvolute.com/unitouchbar/faq");
        });


        //--Ok we have buttons. Let's show the group only on Settings Window ;)

        //--------------------------3.SHOW-------------------------
        TouchBar.AddGroup (settingsGroup);
        //--Let's show the touchbar only on window called "Uni.Settings" (basing on what EditorWindow..titleContent.text says)
        settingsGroup.ShowOnWindow ("Uni.Settings");

        //--Do you want to keep this group shown all time?
        //mainGroup.ShowOnWindow (TouchBar.Windows.ALL);

        

        //--------------------That's it!----------------------------
        //LET'S ENCHANCE IT!
        
           
        //But we still can make our touchbar more complex.
        //Eq.Set Colors
        websiteButton.UpdateBackgroundColor (new Color (0.352941176F, 0, 1));
        faqButton.UpdateBackgroundColor (new Color (0.352941176F, 0, 1));

        //But nothig happens if you just add dynamic buttons (less efficient)
        TouchBar.Button moreButton = settingsGroup.AddTextButton ("moreButton", "more...", () => {
            //Maybe we will link some group to the button?
            DynamicGroup ();

        });

        //We can log to check if everything works.
        TouchBar.Log (moreButton);

        //But dynamic items need reloading :( //After adding all items.
        TouchBar.ReloadGroup (settingsGroup);

    }

    //EXAMPLE

    private static TouchBar.Group moreGroup;
    private static TouchBar.Button coolButton;
    //private static TouchBar.Button closeButton;
    static void DynamicGroup () {

        //No need to init it double times
        if (moreGroup == null) {

            moreGroup = new TouchBar.Group ("moreGroup", 6);

            coolButton = moreGroup.AddTextButton ("cool", "made by IMVOLUTE \ud83d\ude0e");

            //You can allways add action to already created items.
            coolButton.onClick = () => {
                Application.OpenURL ("https://imvolute.com");
            };

            //Making image button - is also easy ;)
            moreGroup.AddImageButton ("close", "/Editor/UniTouchBar/Icons/close.png", "", () => {
                if (moreGroup != null) //Shit happens - shouldn't but in case of!.
                    moreGroup.Hide ();
            });

            //Let's add and show!
            TouchBar.AddGroup (moreGroup);
            moreGroup.Show ();

            //Wow wow - what if we hide settings? This group has to hide too!
            settingsGroup.OnHidden += () => {
                if (moreGroup != null)
                    moreGroup.Hide ();
            };

        } else {
            //No need to recreate - just show!
            moreGroup.Show ();
        }

        //Cool idea! What if we chnage more... to more?

    }

}