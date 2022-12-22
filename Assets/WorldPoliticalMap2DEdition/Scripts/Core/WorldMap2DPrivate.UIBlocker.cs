// World Political Map - 2D Edition for Unity - Main Script
// Copyright 2015-2020 Kronnect
// Don't modify this script - changes could be lost if you upgrade to a more recent version of WPM

//#define TRACE_CTL

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Globalization;


namespace WPMF {

    public partial class WorldMap2D : MonoBehaviour {

        bool mouseIsOverUIElement;
        private readonly HashSet<int> currentIgnoredFingerIDs = new HashSet<int>();

        public bool IsPointerOverUI() {
            CheckPointerOverUI();
            return mouseIsOverUIElement;
        }

        void CheckPointerOverUI() {
            // Check whether the points is on an UI element
            if (UnityEngine.EventSystems.EventSystem.current != null) {
                if (Input.touchSupported && Input.touchCount > 0) {
                    for (int i = 0; i < Input.touchCount; i++) {
                        Touch currTouch = Input.GetTouch(i);
                        if (currTouch.phase == TouchPhase.Began && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(currTouch.fingerId)) {
                            mouseIsOverUIElement = true;
                            currentIgnoredFingerIDs.Add(currTouch.fingerId);
                        } else {
                            mouseIsOverUIElement = currentIgnoredFingerIDs.Contains(currTouch.fingerId);
                            if (currTouch.phase == TouchPhase.Ended || currTouch.phase == TouchPhase.Canceled) {
                                currentIgnoredFingerIDs.Remove(currTouch.fingerId);
                            }
                        }
                    }
                    return;
                } else if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1)) {
                    mouseIsOverUIElement = true;
                    return;
                }
            }
            mouseIsOverUIElement = false;
        }

    }

}