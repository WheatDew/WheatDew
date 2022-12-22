using UnityEngine;
using WPMF;

namespace WPMF.Demos {

    public delegate void OnMarkerPointerClickEvent(int buttonIndex);
    public delegate void OnMarkerEvent();

    public class ViewportClickHandler : MonoBehaviour {

        public bool respectOtherUI = true;
        public OnMarkerPointerClickEvent OnMarkerMouseDown;
        public OnMarkerPointerClickEvent OnMarkerMouseUp;
        public OnMarkerEvent OnMarkerMouseEnter;
        public OnMarkerEvent OnMarkerMouseExit;
        public WorldMap2D map;
        bool wasInside;
        Vector3 lastMousePos;

        void Start() {
            // Get a reference to the World Map API:
            if (map == null) {
                map = WorldMap2D.instance;
            }
            wasInside = SpriteRectContainsPointer();
        }


        void LateUpdate() {

            if (respectOtherUI && map != null && map.IsPointerOverUI()) {
                return;
            }

            bool leftButtonPressed = Input.GetMouseButtonDown(0);
            bool rightButtonPressed = Input.GetMouseButtonDown(1);
            bool leftButtonReleased = Input.GetMouseButtonUp(0);
            bool rightButtonReleased = Input.GetMouseButtonUp(1);
            if (lastMousePos != Input.mousePosition || leftButtonPressed || rightButtonPressed || leftButtonReleased || rightButtonReleased) {
                lastMousePos = Input.mousePosition;
                // Check if cursor location is inside marker rect
                bool inside = SpriteRectContainsPointer();
                if (inside) {
                    if (leftButtonPressed) {
                        if (OnMarkerMouseDown != null) {
                            OnMarkerMouseDown(0);
                        }
                        SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
                    }
                    if (rightButtonPressed) {
                        if (OnMarkerMouseDown != null) {
                            OnMarkerMouseDown(1);
                        }
                    }
                    if (leftButtonReleased) {
                        if (OnMarkerMouseUp != null) {
                            OnMarkerMouseUp(0);
                        }
                        SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
                    }
                    if (rightButtonReleased) {
                        if (OnMarkerMouseUp != null) {
                            OnMarkerMouseUp(1);
                        }
                    }
                    if (!wasInside) {
                        if (OnMarkerMouseEnter != null) {
                            OnMarkerMouseEnter();
                        }
                        SendMessage("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
                    }
                } else {
                    if (wasInside) {
                        if (OnMarkerMouseExit != null) {
                            OnMarkerMouseExit();
                        }
                        SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
                    }
                }
                wasInside = inside;
            }
        }

        bool SpriteRectContainsPointer() {
            // Check if cursor location is inside marker rect
            if (map == null)
                return false;
            Vector2 cursorLocation = map.cursorLocation;
            Rect rect = new Rect(transform.localPosition - transform.localScale * 0.5f, transform.localScale);
            return rect.Contains(cursorLocation);
        }
    }

}


