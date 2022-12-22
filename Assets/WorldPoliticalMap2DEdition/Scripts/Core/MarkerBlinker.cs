using UnityEngine;
using System.Collections;

namespace WPMF {

    public class MarkerBlinker : MonoBehaviour {

        public float duration = 4.0f;
        public float speed = 0.25f;
        Renderer[] rr;

        public static void AddTo(GameObject marker, float duration, float speed) {
            MarkerBlinker mb = marker.AddComponent<MarkerBlinker>();
            mb.duration = duration;
            mb.speed = speed;
        }

        float startTime, lapTime;
        bool phase;

        void Start() {
            startTime = Time.time;
            lapTime = startTime - speed;
            rr = GetComponentsInChildren<Renderer>();
        }

        void OnDisable() {
            UpdateRenderers(true);
        }


        // Update is called once per frame
        void Update() {
            float elapsed = Time.time - startTime;
            if (elapsed > duration) {
                UpdateRenderers(true);
                Destroy(this);
                return;
            }
            if (Time.time - lapTime > speed) {
                lapTime = Time.time;
                phase = !phase;
                UpdateRenderers(phase);
            }
        }

        void UpdateRenderers(bool visible) {
            for (int k = 0; k < rr.Length; k++) {
                Renderer r = rr[k];
                if (r != null) {
                    r.enabled = visible;
                }
            }
        }
    }
}