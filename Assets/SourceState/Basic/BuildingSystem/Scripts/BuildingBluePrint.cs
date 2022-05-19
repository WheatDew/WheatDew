using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class BuildingBluePrint : MonoBehaviour
    {

        [HideInInspector] public string buildingName;

        private bool isCheckout = false;

        private void Update()
        {

            if (Input.GetMouseButtonUp(0))
            {
                isCheckout = true;
            }
            
            if(!isCheckout)
                CheckBlueprint();
        }


        //¿∂Õº∑≈÷√ºÏ≤È
        public void CheckBlueprint()
        {
            RaycastHit result;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out result, 100, 1 << 7))
            {
                transform.position = result.point;
            }
        }

        //Àÿ≤ƒ∑≈÷√ºÏ≤È
        public void SettingMaterials()
        {
            
        }
    }
}

