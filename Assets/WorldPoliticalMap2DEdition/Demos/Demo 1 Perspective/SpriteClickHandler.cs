using UnityEngine;

namespace WPMF.Demos {

    public class SpriteClickHandler : MonoBehaviour {

        public WorldMap2D map;

        void Start() {
            if (GetComponent<Collider2D>() == null) {
                gameObject.AddComponent<BoxCollider2D>();
            }

            if (map == null) {
                map = WorldMap2D.instance;
            }
            if (map.renderViewportEnabled && GetComponent<ViewportClickHandler>() == null) {
                gameObject.AddComponent<ViewportClickHandler>();
            }

        }

        void OnMouseDown() {
            Debug.Log("Mouse down on sprite!");
        }

        void OnMouseUp() {
            Debug.Log("Mouse up on sprite!");

            int countryIndex = map.countryLastClicked;
            if (countryIndex >= 0) {
                Debug.Log("Clicked on " + map.countries[countryIndex].name);
            }
        }

        void OnMouseEnter() {
            Debug.Log("Mouse over the sprite!");
        }

        void OnMouseExit() {
            Debug.Log("Mouse exited the sprite!");
        }

    }
}
