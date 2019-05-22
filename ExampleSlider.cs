/*
 *
 *
 * UniTouchBar  - Slider Example  + Test Script
 * Put me to Editor Directory
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

[InitializeOnLoad]
public class ExampleSliderTouchBar {

    static ExampleSliderTouchBar () {
        //Manager will tell us when TouchBar is ready for us.
        TouchBar.Manager.OnReady += Manager_OnReady;
    }

    private static TouchBar.Group sliderGroup;
    private static TouchBar.Button showSliderButton;

    static void Manager_OnReady () {
        //Ready? We don't need it anymore. - keep it fast!
        TouchBar.Manager.OnReady -= Manager_OnReady;

        //--------------------1.GROUP----------------------------
        //--Everything is allways stored in groups.
        sliderGroup = new TouchBar.Group ("sliderGroup", 2, () => {
            TouchBar.Log ("Sliders Touchbar Loaded - you can remove it)");
        });

        //--------------------2.Items-----------------------------
        showSliderButton = sliderGroup.AddTextButton ("slidersButton", "show", () => {
            //Show group of sliders.
            DynamicGroup ();
        });

        //--------------------------3.SHOW-------------------------
        TouchBar.AddGroup (sliderGroup);
        //--Let's show the touchbar on all windows! for specyfic check Uni.TouchBar.Windows or string (basing on what EditorWindow..titleContent.text says)
        sliderGroup.ShowOnWindow (Uni.TouchBar.Windows.ALL);
        //--------------------That's it!----------------------------

    }
    private static TouchBar.Group moreGroup;
    private static TouchBar.Slider slider;
    static void DynamicGroup () {

        //No need to init it double times
        if (moreGroup == null) {
            moreGroup = new TouchBar.Group ("slidersGroup", 6);

            slider = moreGroup.AddSlider ("slider", 0, 10, 3, (double value) => {
                Debug.Log ("On Slider change [value:" + value.ToString () + "]");
            });

            //Close group button
            moreGroup.AddImageButton ("close", "/Editor/UniTouchBar/Icons/close.png", "", () => {
                if (moreGroup != null) //Shit happens - shouldn't but in case of!.
                    moreGroup.Hide ();
            });

            //Let's add and show!
            TouchBar.AddGroup (moreGroup);
            //We can hide buttons
            moreGroup.OnHidden += () => {
                showSliderButton.Show ();
            };
            moreGroup.OnShow += () => {
                showSliderButton.Hide ();
            };
        }

        moreGroup.Show ();
    }

}