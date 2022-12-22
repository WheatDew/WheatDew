using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using WPMF.PolygonClipping;

namespace WPMF
{

	public partial class WorldMap2D_Editor : MonoBehaviour
	{

		public int provinceIndex = -1, provinceRegionIndex = -1;
		public int GUIProvinceIndex;
		public string GUIProvinceName = "";
		public string GUIProvinceNewName = "";
		public string GUIProvinceNewCountryName = "";
		public int GUIProvinceTransferToCountryIndex = -1;
		public int GUIProvinceTransferToProvinceIndex = -1;
		public bool provinceChanges;
		// if there's any pending change to be saved

		int lastProvinceCount = -1;
		string[] _provinceNames;

		public string[] provinceNames {
			get {
				if (countryIndex == -1)
					return new string[0];
				if (map.countries [countryIndex].provinces != null && lastProvinceCount != map.countries [countryIndex].provinces.Length) {
					provinceIndex = -1;
					ReloadProvinceNames ();
				}
				return _provinceNames;
			}
		}

		
		#region Editor functionality

		
		public void ClearProvinceSelection ()
		{
			map.HideProvinceRegionHighlights (true);
			map.HideProvinces ();
			provinceIndex = -1;
			provinceRegionIndex = -1;
			GUIProvinceName = "";
			GUIProvinceNewName = "";
			GUIProvinceNewCountryName = "";
			GUIProvinceIndex = -1;
		}


		public bool ProvinceSelectByScreenClick (Ray ray)
		{
			int targetProvinceIndex, targetRegionIndex;
			if (map.GetProvinceIndex (ray, out targetProvinceIndex, out targetRegionIndex)) {
				if (countryIndex != map.provinces [targetProvinceIndex].countryIndex) {
					countryIndex = map.provinces [targetProvinceIndex].countryIndex;
					countryRegionIndex = 0;
					CountryRegionSelect ();
				}
				provinceIndex = targetProvinceIndex;
				if (provinceIndex >= 0 && countryIndex != map.provinces [provinceIndex].countryIndex) { // sanity check
					ClearSelection ();
					countryIndex = map.provinces [provinceIndex].countryIndex;
					countryRegionIndex = map.countries [countryIndex].mainRegionIndex;
					CountryRegionSelect ();
				}
				provinceRegionIndex = targetRegionIndex;
				ProvinceRegionSelect ();
				return true;
			}
			return false;
		}

		bool GetProvinceIndexByGUISelection ()
		{
			if (GUIProvinceIndex < 0 || GUIProvinceIndex >= provinceNames.Length)
				return false;
			string[] s = provinceNames [GUIProvinceIndex].Split (new char[] {
				'(',
				')'
			}, System.StringSplitOptions.RemoveEmptyEntries);
			if (s.Length >= 2) {
				GUIProvinceName = s [0].Trim ();
				if (int.TryParse (s [1], out provinceIndex)) {
					provinceRegionIndex = map.provinces [provinceIndex].mainRegionIndex;
					return true;
				}
			}
			return false;
		}

		public void ProvinceSelectByCombo (int selection)
		{
			GUIProvinceName = "";
			GUIProvinceIndex = selection;
			if (GetProvinceIndexByGUISelection ()) {
				if (Application.isPlaying) {
					map.BlinkProvince (provinceIndex, Color.black, Color.green, 1.2f, 0.2f);
				}
			}
			ProvinceRegionSelect ();
		}

		public void ReloadProvinceNames ()
		{
			if (map == null || map.provinces == null || countryIndex < 0 || countryIndex >= map.countries.Length) {
				return;
			}
			string oldProvinceTransferName = GetProvinceTransferIndexByGUISelection();
			string oldProvinceMergeTransferName = GetProvinceMergeIndexByGUISelection();
			_provinceNames = map.GetProvinceNames(countryIndex);
			lastProvinceCount = _provinceNames.Length; 
			lastMountPointCount = -1;
			SyncGUIProvinceTransferSelection(oldProvinceTransferName);
			SyncGUIProvinceMergeSelection(oldProvinceMergeTransferName);
			ProvinceRegionSelect(); // refresh selection
		}

		
		public void ProvinceRegionSelect ()
		{
			if (countryIndex < 0 || countryIndex >= map.countries.Length || provinceIndex < 0 || provinceIndex >= map.provinces.Length || editingMode != EDITING_MODE.PROVINCES)
				return;

			// Checks country selected is correct
			Province province = map.provinces [provinceIndex];
			if (province.countryIndex != countryIndex) {
				ClearSelection ();
				countryIndex = province.countryIndex;
				countryRegionIndex = map.countries [countryIndex].mainRegionIndex;
				CountryRegionSelect ();
			}

			// Just in case makes GUICountryIndex selects appropiate value in the combobox
			GUIProvinceName = province.name;
			SyncGUIProvinceSelection ();
			if (provinceIndex >= 0 && provinceIndex < map.provinces.Length) {
				GUIProvinceNewName = province.name;
				ProvinceHighlightSelection ();
			}
		}


		void ProvinceHighlightSelection ()
		{
			
			if (highlightedRegions == null)
				highlightedRegions = new List<Region> ();
			else
				highlightedRegions.Clear ();
			map.HideProvinceRegionHighlights (true);

			if (provinceIndex < 0 || provinceIndex >= map.provinces.Length || countryIndex < 0 || countryIndex >= map.countries.Length || map.countries [countryIndex].provinces == null ||
			             provinceRegionIndex < 0 || map.provinces [provinceIndex].regions == null || provinceRegionIndex >= map.provinces [provinceIndex].regions.Count)
				return;

			// Highlight current province
			for (int p = 0; p < map.countries [countryIndex].provinces.Length; p++) {
				Province province = map.countries [countryIndex].provinces [p];
				if (province.regions == null)
					continue;
				// if province is current province then highlight it
				if (province.name.Equals (map.provinces [provinceIndex].name)) {
					map.HighlightProvinceRegion (provinceIndex, provinceRegionIndex, false);
					highlightedRegions.Add (map.provinces [provinceIndex].regions [provinceRegionIndex]);
				} else {
					// if this province belongs to the country but it's not current province, add to the collection of highlighted (not colorize because country is already colorized and that includes provinces area)
					highlightedRegions.Add (province.regions [province.mainRegionIndex]);
				}
			}
			shouldHideEditorMesh = true;
		}

		void SyncGUIProvinceSelection ()
		{
			// recover GUI country index selection
			if (GUIProvinceName.Length > 0 && provinceNames != null) {
				for (int k = 0; k < _provinceNames.Length; k++) {
					if (_provinceNames [k].TrimStart ().StartsWith (GUIProvinceName)) {
						GUIProvinceIndex = k;
						provinceIndex = map.GetProvinceIndex (countryIndex, GUIProvinceName);
						return;
					}
				}
			}
			GUIProvinceIndex = -1;
			GUIProvinceName = "";
		}


		string GetProvinceTransferIndexByGUISelection() {
			if (GUIProvinceTransferToCountryIndex < 0 || GUIProvinceTransferToCountryIndex >= _countryNames.Length)
				return "";
			string[] s = _countryNames[GUIProvinceTransferToCountryIndex].Split(new char[]
				{
					'(',
					')'
				}, System.StringSplitOptions.RemoveEmptyEntries);
			if (s.Length >= 2) {
				return s[0].Trim();
			}
			return "";
		}

		void SyncGUIProvinceTransferSelection(string oldName) {
			// recover GUI province index selection
			if (oldName.Length > 0) {
				for (int k = 0; k < _countryNames.Length; k++) {  // don't use countryNames or the array will be reloaded again if grouped option is enabled causing an infinite loop
					if (_countryNames[k].TrimStart().StartsWith(oldName)) {
						GUIProvinceTransferToCountryIndex = k;
						return;
					}
				}
				SetInfoMsg("Country " + oldName + " not found in this geodata file.");
			}
			GUIProvinceTransferToCountryIndex = -1;
		}


		string GetProvinceMergeIndexByGUISelection() {
			if (GUIProvinceTransferToProvinceIndex < 0 || GUIProvinceTransferToProvinceIndex >= _provinceNames.Length)
				return "";
			string[] s = _provinceNames[GUIProvinceTransferToProvinceIndex].Split(new char[]
				{
					'(',
					')'
				}, System.StringSplitOptions.RemoveEmptyEntries);
			if (s.Length >= 2) {
				return s[0].Trim();
			}
			return "";
		}

		void SyncGUIProvinceMergeSelection(string oldName) {
			// recover GUI province index selection
			if (oldName.Length > 0) {
				for (int k = 0; k < _provinceNames.Length; k++) {  // don't use provinceNames or the array will be reloaded again if grouped option is enabled causing an infinite loop
					if (_provinceNames[k].TrimStart().StartsWith(oldName)) {
						GUIProvinceTransferToProvinceIndex = k;
						return;
					}
				}
				SetInfoMsg("Province " + oldName + " not found in this geodata file.");
			}
			GUIProvinceTransferToProvinceIndex = -1;
		}


		
		public bool ProvinceRename ()
		{
			if (countryIndex < 0 || provinceIndex < 0)
				return false;
			string prevName = map.provinces [provinceIndex].name;
			GUIProvinceNewName = GUIProvinceNewName.Trim ();
			if (prevName.Equals (GUIProvinceNewName))
				return false;
			if (map.ProvinceRename (countryIndex, prevName, GUIProvinceNewName)) {
				GUIProvinceName = GUIProvinceNewName;
				lastProvinceCount = -1;
				ReloadProvinceNames ();
				provinceChanges = true;
				return true;
			}
			return false;
		}


		/// <summary>
		/// Deletes current region or province if this was the last region
		/// </summary>
		public void ProvinceDelete ()
		{
			if (provinceIndex < 0 || provinceIndex >= map.provinces.Length)
				return;

			// Apply boolean operations on country polygons
			Province province = map.provinces [provinceIndex];
			Region provinceRegion = province.regions [provinceRegionIndex];
			Country sourceCountry = map.countries [countryIndex];

			// Extract from source country - only if province is in the frontier or is crossing the country
			for (int k = 0; k < sourceCountry.regions.Count; k++) {
				Region otherSourceRegion = sourceCountry.regions [k];
				otherSourceRegion.sanitized = true;
			}
			for (int k = 0; k < sourceCountry.regions.Count; k++) {
				Region otherSourceRegion = sourceCountry.regions [k];
				PolygonClipper pc = new PolygonClipper (otherSourceRegion, provinceRegion);
				if (pc.OverlapsSubjectAndClipping ()) {
					otherSourceRegion.sanitized = false;
					pc.Compute (PolygonOp.DIFFERENCE);
				}
			}

			// Remove invalid regions from source country
			for (int k = 0; k < sourceCountry.regions.Count; k++) {
				Region otherSourceRegion = sourceCountry.regions [k];
				if (!otherSourceRegion.sanitized && otherSourceRegion.points.Length < 5) {
					sourceCountry.regions.RemoveAt (k);
					k--;
				}
			}

			// Remove it from the country array
			List<Province> newProvinces = new List<Province> (map.countries [countryIndex].provinces.Length - 1);
			for (int k = 0; k < map.countries [countryIndex].provinces.Length; k++)
				if (!map.countries [countryIndex].provinces [k].name.Equals (GUIProvinceName))
					newProvinces.Add (map.countries [countryIndex].provinces [k]);
			map.countries [countryIndex].provinces = newProvinces.ToArray ();
			// Remove from the global array
			newProvinces = new List<Province> (map.provinces.Length - 1);
			for (int k = 0; k < map.provinces.Length; k++) {
				if (k != provinceIndex) {
					newProvinces.Add (map.provinces [k]);
				}
			}
			map.provinces = newProvinces.ToArray ();

			// Finish operation
			map.HideCountryRegionHighlights (true);
			map.HideProvinceRegionHighlights (true);
			map.RefreshCountryDefinition (countryIndex, null);
			ClearProvinceSelection ();
			map.OptimizeFrontiers ();
			map.Redraw ();
			countryChanges = true;
			provinceChanges = true;
			CountryRegionSelect ();
		}

		/// <summary>
		/// Deletes current region or province if this was the last region
		/// </summary>
		public void ProvinceRegionDelete ()
		{
			if (provinceIndex < 0 || provinceIndex >= map.provinces.Length)
				return;
			
			if (map.provinces [provinceIndex].regions != null && map.provinces [provinceIndex].regions.Count > 1) {
				map.provinces [provinceIndex].regions.RemoveAt (provinceRegionIndex);
				map.RefreshProvinceDefinition (provinceIndex);
			} 
			ClearProvinceSelection ();
			map.OptimizeFrontiers ();
			map.Redraw ();
			provinceChanges = true;
		}

		/// <summary>
		/// Delete all provinces of current country. Called from DeleteCountry.
		/// </summary>
		void mDeleteCountryProvinces ()
		{
			if (map.provinces == null)
				return;
			if (countryIndex < 0)
				return;
			
			map.HideProvinceRegionHighlights (true);
			map.countries [countryIndex].provinces = new Province[0];
			map.CountryDeleteProvinces (countryIndex);
			provinceChanges = true;
		}

		public void DeleteCountryProvinces ()
		{
			mDeleteCountryProvinces ();
			ClearSelection ();
			RedrawFrontiers ();
			map.RedrawMapLabels ();
		}


		/// <summary>
		/// Creates a new province with the current shape
		/// </summary>
		public void ProvinceCreate ()
		{
			if (newShape.Count < 3 || countryIndex < 0)
				return;

			provinceIndex = map.provinces.Length;
			provinceRegionIndex = 0;
			Province newProvince = new Province ("New Province" + (provinceIndex + 1).ToString (), countryIndex);
			Region region = new Region (newProvince, 0);
			region.points = newShape.ToArray ();
			newProvince.regions = new List<Region> ();
			newProvince.regions.Add (region);
			map.ProvinceAdd (newProvince);
			map.RefreshProvinceDefinition (provinceIndex);
			lastProvinceCount = -1;
			ReloadProvinceNames ();
			ProvinceRegionSelect ();
			provinceChanges = true;
		}

		/// <summary>
		/// Adds a new province to current province
		/// </summary>
		public void ProvinceRegionCreate ()
		{
			if (newShape.Count < 3 || provinceIndex < 0)
				return;
			
			Province province = map.provinces [provinceIndex];
			if (province.regions == null)
				province.regions = new List<Region> ();
			provinceRegionIndex = province.regions.Count;
			Region region = new Region (province, provinceRegionIndex);
			region.points = newShape.ToArray ();
			if (province.regions == null)
				province.regions = new List<Region> ();
			province.regions.Add (region);
			map.RefreshProvinceDefinition (provinceIndex);
			provinceChanges = true;
			ProvinceRegionSelect ();
		}

		/// <summary>
		/// Changes province's owner to specified country
		/// </summary>
		public void ProvinceTransferTo ()
		{
			if (provinceIndex < 0 || GUIProvinceTransferToCountryIndex < 0 || GUIProvinceTransferToCountryIndex >= countryNames.Length)
				return;

			// Get target country
			// recover GUI country index selection
			int targetCountryIndex = -1;
			string[] s = countryNames [GUIProvinceTransferToCountryIndex].Split (new char[] {
				'(',
				')'
			}, System.StringSplitOptions.RemoveEmptyEntries);
			if (s.Length >= 2) {
				if (!int.TryParse (s [1], out targetCountryIndex)) {
					return;
				}
			}
			ProvinceTransferTo (targetCountryIndex, provinceIndex);
			countryIndex = targetCountryIndex;
			countryRegionIndex = _map.countries [targetCountryIndex].mainRegionIndex;
			ProvinceRegionSelect ();
		}

		public void ProvinceTransferTo (int targetCountryIndex, int provinceIndex)
		{
			// Remove province form source country
			Province province = map.provinces [provinceIndex];
			Country sourceCountry = map.countries [countryIndex];
			if (map.countries [countryIndex].provinces != null) {
				List<Province> sourceProvinces = new List<Province> (sourceCountry.provinces);
				int provIndex = -1;
				for (int k = 0; k < sourceCountry.provinces.Length; k++)
					if (sourceCountry.provinces [k].name.Equals (province.name))
						provIndex = k;
				if (provIndex >= 0) {
					sourceProvinces.RemoveAt (provIndex);
					sourceCountry.provinces = sourceProvinces.ToArray ();
				}
			}

			// Adds province to target country
			Country targetCountry = map.countries [targetCountryIndex];
			if (targetCountry.provinces == null)
				targetCountry.provinces = new Province[0];
			List<Province> destProvinces = new List<Province> (targetCountry.provinces);
			destProvinces.Add (province);
			targetCountry.provinces = destProvinces.ToArray ();

			// Apply boolean operations on country polygons
			Region provinceRegion = province.regions [provinceRegionIndex];
			Region sourceRegion = sourceCountry.regions [sourceCountry.mainRegionIndex];
			Region targetRegion = targetCountry.regions [targetCountry.mainRegionIndex];

			// Extract from source country - only if province is in the frontier or is crossing the country
			for (int k = 0; k < sourceCountry.regions.Count; k++) {
				Region otherSourceRegion = sourceCountry.regions [k];
				otherSourceRegion.sanitized = true;
			}
			PolygonClipper pc = new PolygonClipper (sourceRegion, provinceRegion);
			if (pc.OverlapsSubjectAndClipping ()) {
				sourceRegion.sanitized = false;
				pc.Compute (PolygonOp.DIFFERENCE);
			} else {
				// Look for other regions to substract
				for (int k = 0; k < sourceCountry.regions.Count; k++) {
					Region otherSourceRegion = sourceCountry.regions [k];
					pc = new PolygonClipper (otherSourceRegion, provinceRegion);
					if (pc.OverlapsSubjectAndClipping ()) {
						otherSourceRegion.sanitized = false;
						pc.Compute (PolygonOp.DIFFERENCE);
					}
				}
			}

			// Remove invalid regions from source country
			for (int k = 0; k < sourceCountry.regions.Count; k++) {
				Region otherSourceRegion = sourceCountry.regions [k];
				if (!otherSourceRegion.sanitized && otherSourceRegion.points.Length < 5) {
					sourceCountry.regions.RemoveAt (k);
					k--;
				}
			}

			// Add region to target country's polygon - only if the province is touching or crossing target country frontier
			pc = new PolygonClipper (targetRegion, provinceRegion);
			if (pc.OverlapsSubjectAndClipping ()) {
				pc.Compute (PolygonOp.UNION);
			} else {
				// Add new region to country
				Region newCountryRegion = new Region (targetCountry, targetCountry.regions.Count);
				newCountryRegion.points = new List<Vector3> (provinceRegion.points).ToArray ();
				targetCountry.regions.Add (newCountryRegion);
			}

			// Transfer cities
			int cityCount = map.cities.Count;
			for (int k = 0; k < cityCount; k++) {
				City city = map.cities [k];
				if (city.countryIndex == countryIndex && city.province.Equals (province.name)) {
					city.countryIndex = targetCountryIndex;
				}
			}

			// Transfer mount points
			int mountPointCount = map.mountPoints.Count;
			for (int k = 0; k < mountPointCount; k++) {
				MountPoint mp = map.mountPoints [k];
				if (mp.countryIndex == countryIndex && mp.provinceIndex == provinceIndex) {
					mp.countryIndex = targetCountryIndex;
				}
			}

			// Finish operation
			map.HideCountryRegionHighlights (true);
			map.HideProvinceRegionHighlights (true);
			map.RefreshCountryDefinition (province.countryIndex, null);
			province.countryIndex = targetCountryIndex;
			map.RefreshProvinceDefinition (provinceIndex);
			map.RefreshCountryDefinition (targetCountryIndex, null);
			countryChanges = true;
			provinceChanges = true;
			cityChanges = true;
			mountPointChanges = true;
		}



		/// <summary>
		/// Merges province with target province
		/// </summary>
		public void ProvinceMerge() {
			if (provinceIndex < 0 || GUIProvinceTransferToProvinceIndex < 0 || GUIProvinceTransferToProvinceIndex >= provinceNames.Length)
				return;

			// Get target province
			// recover GUI country index selection
			int targetProvinceIndex = -1;
			string[] s = _provinceNames[GUIProvinceTransferToProvinceIndex].Split(new char[]
				{
					'(',
					')'
				}, System.StringSplitOptions.RemoveEmptyEntries);
			if (s.Length >= 2) {
				if (!int.TryParse(s[1], out targetProvinceIndex)) {
					return;
				}
			}

			Province targetProvince = null;
			if (targetProvinceIndex >= 0)
				targetProvince = map.provinces[targetProvinceIndex];
			else
				return;
			map.HideCountryRegionHighlights(true);
			map.HideProvinceRegionHighlights(true);
			if (ProvinceMerge(targetProvinceIndex, provinceIndex)) {
				countryChanges = true;
				provinceChanges = true;
				provinceIndex = map.GetProvinceIndex(targetProvince);
				provinceRegionIndex = map.provinces[provinceIndex].mainRegionIndex;
				CountryRegionSelect();
				ProvinceRegionSelect();
			}
		}


		/// <summary>
		/// Makes provinceIndex absorb another province providing any of its regions. All regions are transfered to target province.
		/// This function is quite slow with high definition frontiers.
		/// </summary>
		/// <param name="provinceIndex">Province index of the conquering province.</param>
		/// <param name="sourceRegion">Source region of the loosing province.</param>
		/// <param name="redraw">If set to true, map will be redrawn after operation finishes.</param>
		public bool ProvinceMerge(int targetProvinceIndex, int sourceProvinceIndex) {
			if (provinceIndex < 0 || sourceProvinceIndex < 0 || targetProvinceIndex == sourceProvinceIndex)
				return false;

			// Transfer cities
			Province sourceProvince = map.provinces[sourceProvinceIndex];
			Province targetProvince = map.provinces[targetProvinceIndex];
			if (sourceProvince.countryIndex != targetProvince.countryIndex) {
				// Transfer source province to target country province
				ProvinceTransferTo(targetProvince.countryIndex, sourceProvinceIndex);
				return true;
			}

			int cityCount = map.cities.Count;
			for (int k = 0; k < cityCount; k++) {
				if (map.cities[k].countryIndex == sourceProvince.countryIndex && map.cities[k].province.Equals(sourceProvince.name))
					map.cities[k].province = targetProvince.name;
			}

			// Transfer mount points
			int mountPointCount = map.mountPoints.Count;
			for (int k = 0; k < mountPointCount; k++) {
				if (map.mountPoints[k].provinceIndex == sourceProvinceIndex)
					map.mountPoints[k].provinceIndex = provinceIndex;
			}

			// Transfer regions
			int sprCount = sourceProvince.regions.Count;
			if (sprCount > 0) {
				List<Region> targetRegions = new List<Region>(targetProvince.regions);
				for (int k = 0; k < sprCount; k++) {
					targetRegions.Add(sourceProvince.regions[k]);
				}
				targetProvince.regions = targetRegions;
			}

			ProvinceMergeAdjacentRegions(targetProvince);
			targetProvince.mainRegionIndex = 0; // will be updated on RefreshProvinceDefinition

			// Remove province form source country
			Country sourceCountry = map.countries [sourceProvince.countryIndex];
			if (sourceCountry.provinces != null) {
				List<Province> sourceProvinces = new List<Province> (sourceCountry.provinces);
				int provIndex = -1;
				for (int k = 0; k < sourceCountry.provinces.Length; k++)
					if (sourceCountry.provinces [k].name.Equals (sourceProvince.name))
						provIndex = k;
				if (provIndex >= 0) {
					sourceProvinces.RemoveAt (provIndex);
					sourceCountry.provinces = sourceProvinces.ToArray ();
				}
			}

			// Finish operation
			map.RefreshProvinceDefinition (targetProvinceIndex);

			// Remove province from global array
			List<Province> newProvinces = new List<Province>();
			for (int k = 0; k < map.provinces.Length; k++) {
				if (k != sourceProvinceIndex) {
					newProvinces.Add (map.provinces [k]);
				}
			}
			map.provinces = newProvinces.ToArray ();

			if (targetProvinceIndex > sourceProvinceIndex) {
				targetProvinceIndex--;
			}

			if (provinceIndex == sourceProvinceIndex) {
				provinceIndex = targetProvinceIndex;
				provinceRegionIndex = targetProvince.mainRegionIndex;
			}

			map.Redraw ();
			return true;
		}


		void ProvinceMergeAdjacentRegions (Province targetProvince) {
			// Searches for adjacency - merges in first region
			int regionCount = targetProvince.regions.Count;
			for (int k = 0; k < regionCount; k++) {
				Region region1 = targetProvince.regions [k];
				for (int j = k + 1; j < regionCount; j++) {
					Region region2 = targetProvince.regions [j];
					PolygonClipper pc = new PolygonClipper (region1, region2);
					if (pc.OverlapsSubjectAndClipping ()) {
						region1.sanitized = false;
						pc.Compute (PolygonOp.UNION);
					
						// Add new neighbours
						int rnCount = region2.neighbours.Count;
						for (int n = 0; n < rnCount; n++) {
							Region neighbour = region2.neighbours [n];
							if (neighbour != null && neighbour != region1 && !region1.neighbours.Contains (neighbour)) {
								region1.neighbours.Add (neighbour);
							}
						}
						// Remove merged region
						targetProvince.regions.RemoveAt (j);
						j--;
						regionCount--;
					}
				}
			}
		}



		/// <summary>
		/// Separates a province from its current country producing a new country
		/// </summary>
		public void ProvinceSeparate (string newCountryName)
		{
			if (provinceIndex < 0 || provinceIndex >= map.provinces.Length)
				return;
			
			// Remove province form source country
			Province province = map.provinces [provinceIndex];
			Country sourceCountry = map.countries [countryIndex];
			if (map.countries [countryIndex].provinces != null) {
				List<Province> sourceProvinces = new List<Province> (sourceCountry.provinces);
				int provIndex = -1;
				for (int k = 0; k < sourceCountry.provinces.Length; k++)
					if (sourceCountry.provinces [k].name.Equals (province.name))
						provIndex = k;
				if (provIndex >= 0) {
					sourceProvinces.RemoveAt (provIndex);
					sourceCountry.provinces = sourceProvinces.ToArray ();
				}
			}
			
			// Adds province region to a new country
			Region regionProvince = province.regions [provinceRegionIndex];
			Country targetCountry = new Country (newCountryName, sourceCountry.continent);
			Region region = new Region (targetCountry, 0);
			region.points = new List<Vector3> (regionProvince.points).ToArray ();
			targetCountry.regions.Add (region);
			map.CountryAdd (targetCountry);
			int targetCountryIndex = map.countries.Length - 1;
			map.RefreshCountryDefinition (targetCountryIndex, null);
			lastCountryCount = -1;

			// Add province to the new country
			if (targetCountry.provinces == null)
				targetCountry.provinces = new Province[0];
			List<Province> destProvinces = new List<Province> (targetCountry.provinces);
			destProvinces.Add (province);
			targetCountry.provinces = destProvinces.ToArray ();
			
			// Apply boolean operations on country polygons
			Region provinceRegion = province.regions [provinceRegionIndex];
			Region sourceRegion = sourceCountry.regions [sourceCountry.mainRegionIndex];

			// Extract from source country - only if province is in the frontier or is crossing the country
			for (int k = 0; k < sourceCountry.regions.Count; k++) {
				Region otherSourceRegion = sourceCountry.regions [k];
				otherSourceRegion.sanitized = true;
			}
			PolygonClipper pc = new PolygonClipper (sourceRegion, provinceRegion);
			if (pc.OverlapsSubjectAndClipping ()) {
				sourceRegion.sanitized = false;
				pc.Compute (PolygonOp.DIFFERENCE);
			} else {
				// Look for other regions to substract
				for (int k = 0; k < sourceCountry.regions.Count; k++) {
					Region otherSourceRegion = sourceCountry.regions [k];
					pc = new PolygonClipper (otherSourceRegion, provinceRegion);
					if (pc.OverlapsSubjectAndClipping ()) {
						otherSourceRegion.sanitized = false;
						pc.Compute (PolygonOp.DIFFERENCE);
					}
				}
			}
			
			// Remove invalid regions from source country
			for (int k = 0; k < sourceCountry.regions.Count; k++) {
				Region otherSourceRegion = sourceCountry.regions [k];
				if (!otherSourceRegion.sanitized && otherSourceRegion.points.Length < 5) {
					sourceCountry.regions.RemoveAt (k);
					k--;
				}
			}

			// Transfer cities
			int cityCount = map.cities.Count;
			for (int k = 0; k < cityCount; k++) {
				City city = map.cities [k];
				if (city.countryIndex == countryIndex && city.province.Equals (province.name)) {
					city.countryIndex = targetCountryIndex;
				}
			}
			
			// Transfer mount points
			int mountPointCount = map.mountPoints.Count;
			for (int k = 0; k < mountPointCount; k++) {
				MountPoint mp = map.mountPoints [k];
				if (mp.countryIndex == countryIndex && mp.provinceIndex == provinceIndex) {
					mp.countryIndex = targetCountryIndex;
				}
			}

			// Finish operation
			map.HideCountryRegionHighlights (true);
			map.HideProvinceRegionHighlights (true);
			map.RefreshCountryDefinition (province.countryIndex, null);
			province.countryIndex = targetCountryIndex;
			map.RefreshProvinceDefinition (provinceIndex);
			map.RefreshCountryDefinition (targetCountryIndex, null);
			countryChanges = true;
			provinceChanges = true;
			cityChanges = true;
			mountPointChanges = true;
			ProvinceRegionSelect ();
		}

		#endregion

		#region IO stuff

		/// <summary>
		/// Returns the file name corresponding to the current province data file
		/// </summary>
		public string GetProvinceGeoDataFileName ()
		{
			return "provinces10.txt";
		}

		/// <summary>
		/// Exports the geographic data in packed string format.
		/// </summary>
		public string GetProvinceGeoData ()
		{
			
			StringBuilder sb = new StringBuilder ();
			for (int k = 0; k < map.provinces.Length; k++) {
				Province province = map.provinces [k];
				int countryIndex = province.countryIndex;
				if (countryIndex < 0 || countryIndex >= map.countries.Length)
					continue;
				string countryName = map.countries [countryIndex].name;
				if (k > 0)
					sb.Append ("|");
				sb.Append (province.name + "$");
				sb.Append (countryName + "$");
				if (province.regions == null)
					map.ReadProvincePackedString (province);
				for (int r = 0; r < province.regions.Count; r++) {
					if (r > 0)
						sb.Append ("*");
					Region region = province.regions [r];
					for (int p = 0; p < region.points.Length; p++) {
						if (p > 0)
							sb.Append (";");
						int x = (int)(region.points [p].x * WorldMap2D.MAP_PRECISION);
						int y = (int)(region.points [p].y * WorldMap2D.MAP_PRECISION);
						sb.Append (x.ToString (invariantCulture));
						sb.Append (",");
						sb.Append (y.ToString (invariantCulture));
					}
				}
			}
			return sb.ToString ();
		}

		#endregion

	}
}
