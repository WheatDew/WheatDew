using UnityEngine;
using System.Collections;

namespace WPMF {
	public class CityBlinker : MonoBehaviour {

		public float duration;
		public Color color1, color2;
		public float speed;
		public Material blinkMaterial;
		Material oldMaterial;
		float startTime, lapTime;
		bool whichColor;

		void Start () {
			oldMaterial = GetComponent<Renderer> ().sharedMaterial;
			GenerateMaterial ();
			startTime = Time.time;
			lapTime = startTime - speed;
		}

        private void OnDestroy() {
			if (blinkMaterial != null) DestroyImmediate(blinkMaterial);
		}

        // Update is called once per frame
        void Update () {
			float elapsed = Time.time - startTime;
			if (elapsed > duration) {
				GetComponent<Renderer> ().sharedMaterial = oldMaterial;
				Destroy (this);
				return;
			}
			if (Time.time - lapTime > speed) {
				lapTime = Time.time;
				Material mat = GetComponent<Renderer> ().sharedMaterial;
				if (mat != blinkMaterial)
					GenerateMaterial ();
				whichColor = !whichColor;
				if (whichColor) {
					blinkMaterial.color = color1;
				} else {
					blinkMaterial.color = color2;
				}
			}
		}

		void GenerateMaterial () {
			blinkMaterial = Instantiate (blinkMaterial);
			GetComponent<Renderer> ().sharedMaterial = blinkMaterial;
		}
	}

}