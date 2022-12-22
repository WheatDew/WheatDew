// World Political Map - 2D Edition for Unity - Main Script
// Copyright 2015-2020 Kronnect
// Don't modify this script - changes could be lost if you upgrade to a more recent version of WPM

//#define TRACE_CTL

// Comment the following define if you remove or add new countries
#define WPM_STANDARD_MAP
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace WPMF {
	public partial class WorldMap2D : MonoBehaviour {

		const string FRONTIERS_ROOT = "Frontiers";
		const string COUNTRY_OUTLINE_GAMEOBJECT_NAME = "countryOutline";

		/// <summary>
		/// Country look up dictionary. Used internally for fast searching of country names.
		/// </summary>
		Dictionary<string, int> _countryLookup;
		int lastCountryLookupCount = -1;

		Dictionary<string, int>countryLookup {
			get {
				if (_countryLookup == null || countries.Length != lastCountryLookupCount) {
					UpdateCountryLookup ();
				}
				return _countryLookup;
			}
		}

		List<int> _countriesOrderedBySize;


		// resources
		Material frontiersMat, hudMatCountry;

		// gameObjects
		GameObject countryRegionHighlightedObj;
		GameObject frontiersLayer;

		// caché and gameObject lifetime control
		Vector3[][] frontiers;
		int[][] frontiersIndices;

		// Stuff start
		void UpdateCountryLookup() {
			if (_countryLookup == null) {
				_countryLookup = new Dictionary<string,int> ();
			} else {
				_countryLookup.Clear ();
			}
			int countryCount = countries.Length;
			if (_countriesOrderedBySize == null) {
				_countriesOrderedBySize = new List<int> (countryCount);
			} else {
				_countriesOrderedBySize.Clear ();
			}
			for (int k = 0; k < countryCount; k++) {
				_countryLookup.Add (countries [k].name, k);
				_countriesOrderedBySize.Add (k);
			}
			// Sort countries based on size
			_countriesOrderedBySize.Sort ((int cIndex1, int cIndex2) => {
				Country c1 = countries [cIndex1];
				Region r1 = c1.regions [c1.mainRegionIndex];
				float r1Area = r1.rect2D.width * r1.rect2D.height;
				Country c2 = countries [cIndex2];
				Region r2 = c2.regions [c2.mainRegionIndex];
				float r2Area = r2.rect2D.width * r2.rect2D.height;
				return r1Area.CompareTo (r2Area);
			});
			lastCountryLookupCount = _countryLookup.Count;
		}




		void ReadCountriesPackedString () {
			string frontiersFileName = _frontiersDetail == FRONTIERS_DETAIL.Low ? "WPMF/Geodata/countries110" : "WPMF/Geodata/countries10";
			TextAsset ta = Resources.Load<TextAsset> (frontiersFileName);
			string s = ta.text;
			string[] countryList = s.Split (new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

			Resources.UnloadAsset(ta);

			int countryCount = countryList.Length;
			countries = new Country[countryCount];

			char[] separatorCountries = new char[] { '$' };
			char[] separatorRegions = new char[] { '*' };
			char[] separatorCoordinates = new char[] { ';' };

			Vector2 max, min;
			Vector2 minCountry, maxCountry;

			for (int k = 0; k < countryCount; k++) {
				string[] countryInfo = countryList [k].Split (separatorCountries, StringSplitOptions.RemoveEmptyEntries);
				string name = countryInfo [0];
				string continent = countryInfo [1];
				Country country = new Country (name, continent);
				string[] regions = countryInfo [2].Split (separatorRegions, StringSplitOptions.RemoveEmptyEntries);
				int regionCount = regions.Length;
				country.regions = new List<Region> ();
				float maxVol = 0;
				minCountry.x = minCountry.y = 10;
				maxCountry.x = maxCountry.y = -10;
				for (int r = 0; r < regionCount; r++) {
					string[] coordinates = regions [r].Split (separatorCoordinates, StringSplitOptions.RemoveEmptyEntries);
					int coorCount = coordinates.Length;
					if (coorCount < 3)
						continue;
					min.x = min.y = 10;
					max.x = max.y = -10;
					Region countryRegion = new Region (country, country.regions.Count);
					countryRegion.points = new Vector3[coorCount];
					for (int c = 0; c < coorCount; c++) {
						float x, y;
						GetPointFromPackedString (ref coordinates [c], out x, out y);
						if (x < min.x)
							min.x = x;
						if (x > max.x)
							max.x = x;
						if (y < min.y)
							min.y = y;
						if (y > max.y)
							max.y = y;
						countryRegion.points [c].x = x;
						countryRegion.points [c].y = y;
					}

					FastVector.Average (ref min, ref max, ref countryRegion.center);

					// Calculate country bounding rect
					if (min.x < minCountry.x)
						minCountry.x = min.x;
					if (min.y < minCountry.y)
						minCountry.y = min.y;
					if (max.x > maxCountry.x)
						maxCountry.x = max.x;
					if (max.y > maxCountry.y)
						maxCountry.y = max.y;
					countryRegion.rect2D = new Rect (min.x, min.y, Math.Abs (max.x - min.x), Mathf.Abs (max.y - min.y));
					float vol = (max - min).sqrMagnitude;
					if (vol > maxVol) {
						maxVol = vol;
						country.mainRegionIndex = country.regions.Count;
						country.center = countryRegion.center;
					}
					country.regions.Add (countryRegion);
				}
				// hidden
				if (countryInfo.Length >= 4) {
                    int hidden;
					if (int.TryParse (countryInfo [3], out hidden)) {
						country.hidden = hidden > 0;
					}
				}
				country.regionsRect2D = new Rect (minCountry.x, minCountry.y, Math.Abs (maxCountry.x - minCountry.x), Mathf.Abs (maxCountry.y - minCountry.y));
				countries [k] = country;
			}

			UpdateCountryLookup ();

			OptimizeFrontiers ();
		}

		/// <summary>
		/// Used internally by the Map Editor. It will recalculate de boundaries and optimize frontiers based on new data of countries array
		/// </summary>
		public void RefreshCountryDefinition (int countryIndex, List<Region>filterRegions) {
			if (countryIndex < 0 || countryIndex >= countries.Length)
				return;
			RefreshCountryGeometry (countryIndex);
			lastCountryLookupCount = -1;
			OptimizeFrontiers (filterRegions);
			DrawFrontiers ();
		}

		/// <summary>
		/// Used internally by the Map Editor. It will recalculate de boundaries and optimize frontiers based on new data of countries array
		/// </summary>
		public void RefreshCountryGeometry (int countryIndex) {
			if (countryIndex >= 0 && countryIndex < countries.Length) {
				float maxVol = 0;
				Country country = countries [countryIndex];
				int regionCount = country.regions.Count;
				Vector2 minCountry = Misc.Vector2one * 10;
				Vector2 maxCountry = -minCountry;
				for (int r = 0; r < regionCount; r++) {
					Region countryRegion = country.regions [r];
					countryRegion.entity = country;	// just in case one country has been deleted
					countryRegion.regionIndex = r;				// just in case a region has been deleted
					int coorCount = countryRegion.points.Length;
					Vector3 min = Misc.Vector3one * 10;
					Vector3 max = -min;
					for (int c = 0; c < coorCount; c++) {
						float x = countryRegion.points [c].x;
						float y = countryRegion.points [c].y;
						if (x < min.x)
							min.x = x;
						if (x > max.x)
							max.x = x;
						if (y < min.y)
							min.y = y;
						if (y > max.y)
							max.y = y;
					}
					Vector3 normRegionCenter = (min + max) * 0.5f;
					countryRegion.center = normRegionCenter; 
					
					// Calculate country bounding rect
					if (min.x < minCountry.x)
						minCountry.x = min.x;
					if (min.y < minCountry.y)
						minCountry.y = min.y;
					if (max.x > maxCountry.x)
						maxCountry.x = max.x;
					if (max.y > maxCountry.y)
						maxCountry.y = max.y;
					countryRegion.rect2D = new Rect (min.x, min.y, Math.Abs (max.x - min.x), Mathf.Abs (max.y - min.y));
					float vol = (max - min).sqrMagnitude;
					if (vol > maxVol) {
						maxVol = vol;
						country.mainRegionIndex = r;
						country.center = countryRegion.center;
					}
				}
				country.regionsRect2D = new Rect (minCountry.x, minCountry.y, Math.Abs (maxCountry.x - minCountry.x), Mathf.Abs (maxCountry.y - minCountry.y));
			}
		}

		/// <summary>
		/// Prepare and cache meshes for frontiers. Used internally by extra components (decorator). This is called just after loading data or when hidding a country.
		/// </summary>
		public void OptimizeFrontiers () {
			OptimizeFrontiers (null);
		}

		void TestNeighbourSegment (Region region, int i, int j) {
			double v = (region.points [i].x + region.points [j].x) + MAP_PRECISION * (region.points [i].y + region.points [j].y);
			Region neighbour;
			if (frontiersCacheHit.TryGetValue (v, out neighbour)) { // add neighbour references
				if (neighbour != region) {
					if (!region.neighbours.Contains (neighbour)) {
						region.neighbours.Add (neighbour);
						neighbour.neighbours.Add (region);
					}
				}
			} else {
				frontiersCacheHit [v] = region;
				frontiersPoints.Add (region.points [i]);
				frontiersPoints.Add (region.points [j]);
			}
		}

		void OptimizeFrontiers (List<Region>filterRegions) {
			if (frontiersPoints == null) {
				frontiersPoints = new List<Vector3> (290000); // needed for high-def resolution map
			} else {
				frontiersPoints.Clear ();
			}
			if (frontiersCacheHit == null) {
				frontiersCacheHit = new Dictionary<double, Region> (145000); // needed for high-resolution map
			} else {
				frontiersCacheHit.Clear ();
			}

			for (int k = 0; k < countries.Length; k++) {
				Country country = countries [k];
				int crCount = country.regions.Count;
				for (int r = 0; r < crCount; r++) {
					Region region = country.regions [r];
					if (filterRegions == null || filterRegions.Contains (region)) {
						region.entity = country;
						region.regionIndex = r;
						region.neighbours.Clear ();
					}
				}
			}

			for (int k = 0; k < countries.Length; k++) {
				Country country = countries [k];
				if (country.hidden)
					continue;
				int crCount = country.regions.Count;
				for (int r = 0; r < crCount; r++) {
					Region region = country.regions [r];
					if (filterRegions == null || filterRegions.Contains (region)) {
						int numPoints = region.points.Length - 1;
						for (int i = 0; i < numPoints; i++) {
							TestNeighbourSegment (region, i, i + 1);
						}
						// Close the polygon
						TestNeighbourSegment (region, numPoints, 0);
					}
				}
			}

			int meshGroups = (frontiersPoints.Count / 65000) + 1;
			int meshIndex = -1;
			frontiersIndices = new int[meshGroups][];
			frontiers = new Vector3[meshGroups][];
			int fpCount = frontiersPoints.Count;
			for (int k = 0; k < fpCount; k += 65000) {
				int max = Mathf.Min (fpCount - k, 65000); 
				frontiers [++meshIndex] = new Vector3[max];
				frontiersIndices [meshIndex] = new int[max];
				for (int j = k; j < k + max; j++) {
					frontiers [meshIndex] [j - k] = frontiersPoints [j];
					frontiersIndices [meshIndex] [j - k] = j - k;
				}
			}
		}

		#region Drawing stuff

		int GetCacheIndexForCountryRegion (int countryIndex, int regionIndex) {
			return countryIndex * 1000 + regionIndex;
		}

		void DrawFrontiers () {
			if (!gameObject.activeInHierarchy)
				return;
			if (!_showFrontiers)
				return;

			// Create frontiers layer
			Transform t = transform.Find(FRONTIERS_ROOT);
			do {
				if (t != null) {
					DestroyChildrenAndMeshes(t.gameObject);
				}
				t = transform.Find(FRONTIERS_ROOT);
			} while (t != null);
			frontiersLayer = new GameObject(FRONTIERS_ROOT);
			frontiersLayer.hideFlags = HideFlags.DontSaveInEditor;
			frontiersLayer.transform.SetParent (transform, false);
			frontiersLayer.transform.localPosition = Misc.Vector3zero;
			frontiersLayer.transform.localRotation = Quaternion.Euler (Misc.Vector3zero);
			frontiersLayer.layer = gameObject.layer;

			for (int k = 0; k < frontiers.Length; k++) {
				GameObject flayer = new GameObject ("flayer");
				flayer.hideFlags = HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy;
				flayer.transform.SetParent (frontiersLayer.transform, false);
				flayer.transform.localPosition = Misc.Vector3zero;
				flayer.transform.localRotation = Quaternion.Euler (Misc.Vector3zero);
				flayer.layer = frontiersLayer.layer;

				Mesh mesh = new Mesh ();
				mesh.vertices = frontiers [k];
				mesh.SetIndices (frontiersIndices [k], MeshTopology.Lines, 0);
				mesh.RecalculateBounds ();
				mesh.hideFlags = HideFlags.DontSaveInEditor;

				MeshFilter mf = flayer.AddComponent<MeshFilter> ();
				mf.sharedMesh = mesh;

				MeshRenderer mr = flayer.AddComponent<MeshRenderer> ();
				mr.receiveShadows = false;
				mr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
				mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
				mr.sharedMaterial = frontiersMat;
			}

			// Toggle frontiers visibility layer according to settings
			frontiersLayer.SetActive (_showFrontiers);
		}

		#endregion



		#region Country highlighting

		/// <summary>
		/// Disables all country regions highlights. This doesn't remove custom materials.
		/// </summary>
		public void HideCountryRegionHighlights (bool destroyCachedSurfaces) {
			HideCountryRegionHighlight ();
			if (countries == null)
				return;
			for (int c = 0; c < countries.Length; c++) {
				Country country = countries [c];
				for (int cr = 0; cr < country.regions.Count; cr++) {
					Region region = country.regions [cr];
					int cacheIndex = GetCacheIndexForCountryRegion (c, cr);
					if (surfaces.ContainsKey (cacheIndex)) {
						GameObject surf = surfaces [cacheIndex];
						if (surf == null) {
							surfaces.Remove (cacheIndex);
						} else {
							if (destroyCachedSurfaces) {
								surfaces.Remove (cacheIndex);
								DestroyImmediate (surf);
							} else {
								if (region.customMaterial == null) {
									surf.SetActive (false);
								} else {
									ApplyMaterialToSurface (surf, region.customMaterial);
								}
							}
						}
					}
				}
			}
		}


		void HideCountryRegionHighlight () {
			HideProvinceRegionHighlight ();
			HideCityHighlight ();

			if (_countryRegionHighlightedIndex >= 0 && _countryRegionHighlightedIndex >= 0) {
				if (_countryHighlighted != null) {
					int rCount = _countryHighlighted.regions.Count;
					for (int k = 0; k < rCount; k++) {
						HideCountryRegionHighlightSingle (k);
					}
				}
				countryRegionHighlightedObj = null;
				// Raise exit event
				if (OnCountryExit != null && _countryHighlightedIndex >= 0)
					OnCountryExit (_countryHighlightedIndex, _countryRegionHighlightedIndex);
			}
			hudMatCountry.mainTexture = null;

			_countryHighlighted = null;
			_countryHighlightedIndex = -1;
			_countryRegionHighlighted = null;
			_countryRegionHighlightedIndex = -1;
		}


		void HideCountryRegionHighlightSingle (int regionIndex) {
			int cacheIndex = GetCacheIndexForCountryRegion (_countryHighlightedIndex, regionIndex);
			Region region = _countryHighlighted.regions [regionIndex];
			GameObject surf = null;
			surfaces.TryGetValue (cacheIndex, out surf);
			if (surf == null)
				surfaces.Remove (cacheIndex);
			if (surf != null) {
				Material mat = region.customMaterial;
				if (mat != null) {
					surf.GetComponent<Renderer> ().sharedMaterial = mat;
				} else {
					surf.SetActive (false);
				}
			}
		}

		/// <summary>
		/// Highlights the country region specified. Returns the generated highlight surface gameObject.
		/// Internally used by the Map UI and the Editor component, but you can use it as well to temporarily mark a country region.
		/// </summary>
		/// <param name="refreshGeometry">Pass true only if you're sure you want to force refresh the geometry of the highlight (for instance, if the frontiers data has changed). If you're unsure, pass false.</param>
		public GameObject HighlightCountryRegion (int countryIndex, int regionIndex, bool refreshGeometry, bool drawOutline) {
			if (_countryHighlightedIndex == countryIndex && _countryRegionHighlightedIndex == regionIndex && !refreshGeometry)
				return countryRegionHighlightedObj;
			if (countryRegionHighlightedObj != null)
				HideCountryRegionHighlight ();
			if (countryIndex < 0 || countryIndex >= countries.Length || regionIndex < 0 || regionIndex >= countries [countryIndex].regions.Count)
				return null;

			if (OnCountryHighlight != null) {
				bool allowHighlight = true;
				OnCountryHighlight (countryIndex, regionIndex, ref allowHighlight);
				if (!allowHighlight)
					return null;
			}

			if (_enableCountryHighlight) {
				countryRegionHighlightedObj = HighlightCountryRegionSingle (countryIndex, regionIndex, refreshGeometry, drawOutline, outlineColor);
				if (_highlightAllCountryRegions) {
					Country country = countries [countryIndex];
					int rCount = country.regions.Count;
					for (int r = 0; r < rCount; r++) {
						if (r != regionIndex) { 
							HighlightCountryRegionSingle (countryIndex, r, refreshGeometry, drawOutline, outlineColor);
						}
					}
				}

				if (currentDecoratorCount > 0) {
					decorator.ForceUpdateDecorators ();
				}
			}

			_countryHighlightedIndex = countryIndex;
			_countryRegionHighlighted = countries [countryIndex].regions [regionIndex];
			_countryRegionHighlightedIndex = regionIndex;
			_countryHighlighted = countries [countryIndex];

			return countryRegionHighlightedObj;
		}

		GameObject HighlightCountryRegionSingle (int countryIndex, int regionIndex, bool refreshGeometry, bool drawOutline, Color outlineColor) {
			int cacheIndex = GetCacheIndexForCountryRegion (countryIndex, regionIndex); 
			GameObject obj;
			bool existsInCache = surfaces.TryGetValue (cacheIndex, out obj);
			if (refreshGeometry && existsInCache) {
				surfaces.Remove (cacheIndex);
				DestroyImmediate (obj);
				existsInCache = false;
			}

			bool doHighlight = true;
			if (_countryHighlightMaxScreenAreaSize < 1f) {
				// Check screen area size
				Region region = countries [countryIndex].regions [regionIndex];
				doHighlight = CheckScreenAreaSizeOfRegion (region);
			}
			if (doHighlight) {
				if (existsInCache) {
					obj = surfaces [cacheIndex];
					if (obj == null) {
						surfaces.Remove (cacheIndex);
					} else {
						if (!obj.activeSelf) {
							obj.SetActive (true);
						}
						Renderer r = obj.GetComponent <Renderer> ();
						Material surfMat = r.sharedMaterial;
						hudMatCountry.mainTexture = null;
						if (surfMat != hudMatCountry) {
							if (surfMat.mainTexture != null) {
								hudMatCountry.mainTexture = surfMat.mainTexture;
							}
							r.sharedMaterial = hudMatCountry;
						}
						return obj;
					}
				}
				obj = GenerateCountryRegionSurface (countryIndex, regionIndex, hudMatCountry, Misc.Vector2one, Misc.Vector2zero, 0, drawOutline);
			}
			return obj;
		}


		bool CheckScreenAreaSizeOfRegion (Region region) {
			Camera cam = mainCamera;
			Vector2 scrTR = cam.WorldToViewportPoint (transform.TransformPoint (region.rect2D.max));
			Vector2 scrBL = cam.WorldToViewportPoint (transform.TransformPoint (region.rect2D.min));
			Rect scrRect = new Rect (scrBL.x, scrTR.y, Math.Abs (scrTR.x - scrBL.x), Mathf.Abs (scrTR.y - scrBL.y));
			float highlightedArea = Mathf.Clamp01 (scrRect.width * scrRect.height);
			return highlightedArea < _countryHighlightMaxScreenAreaSize;
		}


		GameObject GenerateCountryRegionSurface (int countryIndex, int regionIndex, Material material, bool drawOutline) {
			return GenerateCountryRegionSurface (countryIndex, regionIndex, material, Misc.Vector2one, Misc.Vector2zero, 0, drawOutline);
		}

		GameObject GenerateCountryRegionSurface (int countryIndex, int regionIndex, Material material, Vector2 textureScale, Vector2 textureOffset, float textureRotation, bool drawOutline) { //, bool applyTextureToAllRegions = true) {
			if (countryIndex < 0 || countryIndex >= countries.Length)
				return null;
			Country country = countries [countryIndex];
			Region region = country.regions [regionIndex];

			// Triangulate to get the polygon vertex indices
			int[] surfIndices = Triangulator.GetPoints (region.points);

			// Check surface orientation
			Vector3 v1 = region.points [surfIndices [0]];
			Vector3 v2 = region.points [surfIndices [1]];
			Vector3 v3 = region.points [surfIndices [2]];
			Vector3 vf = Vector3.Cross (v2 - v1, v3 - v1);
			if (Vector3.Dot (transform.forward, vf) > 0) {
				// Flip surface
				int indicesLength = surfIndices.Length;
				for (int k = 0; k < indicesLength; k += 3) {
					int a = surfIndices [k];
					surfIndices [k] = surfIndices [k + 1];
					surfIndices [k + 1] = a;
				}
			}

			// Prepare surface cache entry and deletes older surface if exists
			int cacheIndex = GetCacheIndexForCountryRegion (countryIndex, regionIndex); 
			string cacheIndexSTR = cacheIndex.ToString ();
			Transform t = surfacesLayer.transform.Find (cacheIndexSTR);
			if (t != null)
				DestroyImmediate (t.gameObject); // Deletes potential residual surface

			// Creates surface mesh
			GameObject surf = Drawing.CreateSurface (cacheIndexSTR, region.points, surfIndices, material, region.rect2D, textureScale, textureOffset, textureRotation);									
			surf.transform.SetParent (surfacesLayer.transform, false);
			surf.transform.localPosition = Misc.Vector3zero;
			surf.transform.localRotation = Quaternion.Euler (Misc.Vector3zero);
			surf.layer = gameObject.layer;
			surfaces[cacheIndex] = surf;

			// Draw polygon outline
			if (drawOutline) {
				DrawCountryRegionOutline (region, surf);
			}
			return surf;
		}

		GameObject DrawCountryRegionOutline (Region region, GameObject surf) {
			// Proxy function which now uses the packed method to reduce draw calls; use only if you color all countries
//												return DrawCountryRegionOutlinePacked (region, surf);
			return DrawCountryRegionOutlineLineStrips (region, surf);
		}

		GameObject DrawCountryRegionOutlineLineStrips (Region region, GameObject surf) {
			if (surf == null)
				return null;
			int[] indices = new int[region.points.Length + 1];
			for (int k = 0; k < indices.Length; k++) {
				indices [k] = k;
			}
			indices [indices.Length - 1] = 0; 
			Transform t = surf.transform.Find (COUNTRY_OUTLINE_GAMEOBJECT_NAME);
			if (t != null)
				DestroyImmediate (t.gameObject);
			GameObject boldFrontiers = new GameObject (COUNTRY_OUTLINE_GAMEOBJECT_NAME);
			boldFrontiers.transform.SetParent (surf.transform, false);
			boldFrontiers.transform.localPosition = Misc.Vector3zero;
			boldFrontiers.transform.localRotation = Quaternion.Euler (Misc.Vector3zero);
			boldFrontiers.layer = surf.layer;
				
			Mesh mesh = new Mesh ();
			mesh.vertices = region.points; 
			mesh.SetIndices (indices, MeshTopology.LineStrip, 0);
				
			MeshFilter mf = boldFrontiers.AddComponent<MeshFilter> ();
			mf.sharedMesh = mesh;
				
			MeshRenderer mr = boldFrontiers.AddComponent<MeshRenderer> ();
			mr.receiveShadows = false;
			mr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
			mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			mr.sharedMaterial = outlineMat;
				
			return boldFrontiers;
		}


		#endregion

		
		#region Map Labels

		/// <summary>
		/// Forces redraw of all labels.
		/// </summary>
		public void RedrawMapLabels () {
			DestroyMapLabels ();
			DrawMapLabels ();
		}

		/// <summary>
		/// Draws the map labels. Note that it will update cached textmesh objects if labels are already drawn.
		/// </summary>
		void DrawMapLabels () { 
			#if TRACE_CTL
			Debug.Log ("CTL " + DateTime.Now + ": Draw map labels");
			#endif
			
			if (!_showCountryNames || !gameObject.activeInHierarchy)
				return;
			
			// Set colors
			labelsFont.material.color = _countryLabelsColor;
			labelsShadowMaterial.color = _countryLabelsShadowColor;
			
			// Create texts
			DestroyMapLabels ();
			GameObject overlay = GetOverlayLayer (true);
			Transform t = overlay.transform.Find (OVERLAY_TEXT_ROOT);
			if (t == null) {
				textRoot = new GameObject (OVERLAY_TEXT_ROOT);
				textRoot.hideFlags = HideFlags.DontSaveInEditor;
				textRoot.layer = overlay.layer;
			} else {
				textRoot = t.gameObject;
			}
			
			List<MeshRect> meshRects = new List<MeshRect> ();
			for (int countryIndex = 0; countryIndex < countries.Length; countryIndex++) {
				Country country = countries [countryIndex];
				if (country.hidden || !country.labelVisible || country.regions.Count == 0)
					continue;
				
				Vector2 center = new Vector2 (country.center.x * mapWidth, country.center.y * mapHeight) + country.labelOffset;
				if (country.mainRegionIndex >= country.regions.Count)
					country.mainRegionIndex = 0;
				Region region = country.regions [country.mainRegionIndex];

#if WPM_STANDARD_MAP
				float zoomFactor = transform.localScale.x / 200.0f;
				// Special countries adjustements
				if (_frontiersDetail == FRONTIERS_DETAIL.Low) {
					switch (countryIndex) {
					case 6: // Antartica
						center.y += 3f * zoomFactor;
						break;
					case 65: // Greenland
						center.y -= 3f * zoomFactor;
						break;
					case 22: // Brazil
						center.y += 4f * zoomFactor;
						center.x += 1.0f * zoomFactor;
						break;
					case 73: // India
						center.x -= 2f * zoomFactor;
						break;
					case 168: // USA
						center.x -= 1f * zoomFactor;
						break;
					case 27: // Canada
						center.x -= 3f * zoomFactor;
						break;
					case 30: // China
						center.x -= 1f * zoomFactor;
						center.y -= 1f * zoomFactor;
						break;
					}
				} else {
					switch (countryIndex) {
					case 114: // Russia
						center.x -= 4f * zoomFactor;
						center.y += 1f * zoomFactor;
						break;
					case 92: // Canada
						center.x -= 5f * zoomFactor;
						break;
					case 185: // USA
						center.x -= 2f * zoomFactor;
						break;
					case 37: // Antartica
						center.y += 2f * zoomFactor;
						break;
					case 84: // Brazil
						center.y += 4f * zoomFactor;
						center.x += 2f * zoomFactor;
						break;
					case 95: // China
						center.x -= 3f * zoomFactor;
						break;
					}
				}
#endif				
				// Adjusts country name length
				string countryName = country.customLabel != null ? country.customLabel : country.name.ToUpper ();
				bool introducedCarriageReturn = false;
				if (countryName.Length > 15) {
					int spaceIndex = countryName.IndexOf (' ', countryName.Length / 2);
					if (spaceIndex >= 0) {
						countryName = countryName.Substring (0, spaceIndex) + "\n" + countryName.Substring (spaceIndex + 1);
						introducedCarriageReturn = true;
					}
				}
				
				// add caption
				GameObject textObj;
				if (country.labelGameObject == null) {
					Color labelColor = country.labelColorOverride ? country.labelColor : _countryLabelsColor;
					Font customFont = country.labelFontOverride ?? labelsFont;
					Material customLabelShadowMaterial = country.labelFontShadowMaterial ?? labelsShadowMaterial;
					textObj = Drawing.CreateText (countryName, null, center, customFont, labelColor, _showLabelsShadow, customLabelShadowMaterial, _countryLabelsShadowColor);
					country.labelGameObject = textObj;
					Bounds bounds = textObj.GetComponent<Renderer> ().bounds;
					country.labelMeshWidth = bounds.size.x;
					country.labelMeshHeight = bounds.size.y;
					country.labelMeshCenter = center;
					textObj.transform.SetParent (textRoot.transform, false);
					textObj.transform.localPosition = center;
					textObj.layer = textRoot.gameObject.layer;
					if (_showLabelsShadow)
						textObj.transform.Find ("shadow").gameObject.layer = textObj.layer;
				} else {
					textObj = country.labelGameObject;
					textObj.transform.localPosition = center;
				}
				
				float meshWidth = country.labelMeshWidth;
				float meshHeight = country.labelMeshHeight;
				
				// adjusts caption
				Rect rect = new Rect (region.rect2D.xMin * mapWidth, region.rect2D.yMin * mapHeight, region.rect2D.width * mapWidth, region.rect2D.height * mapHeight);
				float absoluteHeight;
				if (country.labelRotation > 0) {
					textObj.transform.localRotation = Quaternion.Euler (0, 0, country.labelRotation);
					absoluteHeight = Mathf.Min (rect.height * _countryLabelsSize, rect.width);
				} else if (rect.height > rect.width * 1.45f) {
					float angle;
					if (rect.height > rect.width * 1.5f) {
						angle = 90;
					} else {
						angle = Mathf.Atan2 (rect.height, rect.width) * Mathf.Rad2Deg;
					}
					textObj.transform.localRotation = Quaternion.Euler (0, 0, angle);
					absoluteHeight = Mathf.Min (rect.width * _countryLabelsSize, rect.height);
				} else {
					absoluteHeight = Mathf.Min (rect.height * _countryLabelsSize, rect.width);
				}
				
				// adjusts scale to fit width in rect
				float adjustedMeshHeight = introducedCarriageReturn ? meshHeight * 0.5f : meshHeight;
				float scale = absoluteHeight / adjustedMeshHeight;
				if (country.labelFontSizeOverride) {
					scale = country.labelFontSize;
				} else {
					float desiredWidth = meshWidth * scale;
					if (desiredWidth > rect.width) {
						scale = rect.width / meshWidth;
					}
					if (adjustedMeshHeight * scale < _countryLabelsAbsoluteMinimumSize) {
						scale = _countryLabelsAbsoluteMinimumSize / adjustedMeshHeight;
					}
				}
				
				// stretchs out the caption
				float displayedMeshWidth = meshWidth * scale;
				float displayedMeshHeight = meshHeight * scale;
				string wideName = countryName;
				if (_countryLabelSpread) {
					int times = Mathf.FloorToInt(rect.width * 0.45f / (meshWidth * scale));
					if (times > 10)
						times = 10;
					if (times > 0) {
						StringBuilder sb = new StringBuilder();
						string spaces = new string(' ', times * 2);
						for (int c = 0; c < countryName.Length; c++) {
							sb.Append(countryName[c]);
							if (c < countryName.Length - 1) {
								sb.Append(spaces);
							}
						}
						wideName = sb.ToString();
					}
				}
				
				TextMesh tm = textObj.GetComponent<TextMesh> ();
				if (tm.text.Length != wideName.Length) {
					tm.text = wideName;
					displayedMeshWidth = textObj.GetComponent<Renderer> ().bounds.size.x * scale;
					displayedMeshHeight = textObj.GetComponent<Renderer> ().bounds.size.y * scale;
					if (_showLabelsShadow) {
						textObj.transform.Find ("shadow").GetComponent<TextMesh> ().text = wideName;
					}
				}
				
				// apply scale
				textObj.transform.localScale = new Vector3 (scale, scale, 1);
				
				// Save mesh rect for overlapping checking
				if (country.labelOffset == Misc.Vector2zero) {
					MeshRect mr = new MeshRect (countryIndex, new Rect (center.x - displayedMeshWidth * 0.5f, center.y - displayedMeshHeight * 0.5f, displayedMeshWidth, displayedMeshHeight));
					meshRects.Add (mr);
				}
			}
			
			// Simple-fast overlapping checking
			int cont = 0;
			bool needsResort = true;
			
			while (needsResort && ++cont < 10) {
				meshRects.Sort (overlapComparer);
				
				for (int c = 1; c < meshRects.Count; c++) {
					Rect thisMeshRect = meshRects [c].rect;
					for (int prevc = c - 1; prevc >= 0; prevc--) {
						Rect otherMeshRect = meshRects [prevc].rect;
						if (thisMeshRect.Overlaps (otherMeshRect)) {
							needsResort = true;
							int thisCountryIndex = meshRects [c].countryIndex;
							Country country = countries [thisCountryIndex];
							GameObject thisLabel = country.labelGameObject;
							
							//							if (country.name.Equals("Brazil")) Debug.Log (country.labelMeshCenter.x + " " + thisLabel.transform.localPosition);
							
							// displaces this label
							float offsety = (thisMeshRect.yMax - otherMeshRect.yMin);
							offsety = Mathf.Min (country.regions [country.mainRegionIndex].rect2D.height * mapHeight * 0.35f, offsety);
							thisLabel.transform.localPosition = new Vector3 (country.labelMeshCenter.x, country.labelMeshCenter.y - offsety, thisLabel.transform.localPosition.z);
							thisMeshRect = new Rect (thisLabel.transform.localPosition.x - thisMeshRect.width * 0.5f,
								thisLabel.transform.localPosition.y - thisMeshRect.height * 0.5f,
								thisMeshRect.width, thisMeshRect.height);
							meshRects [c].rect = thisMeshRect;
						}
					}
				}
			}
			
			// Adjusts parent
			textRoot.transform.SetParent (overlay.transform, false);
			textRoot.transform.localPosition = new Vector3 (0, 0, -0.001f);
			textRoot.transform.localRotation = Quaternion.Euler (Misc.Vector3zero);
			textRoot.transform.localScale = new Vector3 (1.0f / mapWidth, 1.0f / mapHeight, 1);
		}

		int overlapComparer (MeshRect r1, MeshRect r2) {
			return (r2.rect.center.y).CompareTo (r1.rect.center.y);
		}

		class MeshRect {
			public int countryIndex;
			public Rect rect;

			public MeshRect (int countryIndex, Rect rect) {
				this.countryIndex = countryIndex;
				this.rect = rect;
			}
		}

		void DestroyMapLabels () {
			#if TRACE_CTL			
			Debug.Log ("CTL " + DateTime.Now + ": destroy labels");
			#endif
			if (countries != null) {
				for (int k = 0; k < countries.Length; k++) {
					if (countries [k].labelGameObject != null) {
						DestroyImmediate (countries [k].labelGameObject);
						countries [k].labelGameObject = null;
					}
				}
			}
			if (textRoot != null)
				DestroyImmediate (textRoot);
			// Security check: if there're still gameObjects under TextRoot, also delete it
			if (overlayLayer != null) {
				Transform t = overlayLayer.transform.Find (OVERLAY_TEXT_ROOT);
				if (t != null && t.childCount > 0) {
					DestroyImmediate (t.gameObject);
				}
			}
		}

		#endregion


	}

}