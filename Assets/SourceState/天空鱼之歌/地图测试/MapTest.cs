using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WPMF;

public class MapTest : MonoBehaviour
{
    public Texture2D flagTexture;
    private void Start()
    {
        WorldMap2D map = WorldMap2D.instance;
        //map.OnCountryClick += (int countryIndex, int regionIndex) =>
        //{
        //    map.ToggleCountryMainRegionSurface(countryIndex, true, flagTexture);
            
        //};
        map.OnProvinceClick += (int provinceIndex, int regionIndex) =>
        {
            map.ToggleProvinceRegionSurface(provinceIndex, regionIndex, true, Color.red);
        };


    }


}
