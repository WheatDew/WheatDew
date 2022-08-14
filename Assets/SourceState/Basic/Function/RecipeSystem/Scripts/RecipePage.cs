using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class RecipePage : MonoBehaviour
    {
        [HideInInspector] public PackComponent host,guest;
        public Transform content;
        public PrecipePageItem itemPrefab;

        public void CreateItem(string title,string recipe)
        {
            PrecipePageItem obj = Instantiate(itemPrefab, content);
            obj.title.text = title;
            obj.recipe = recipe;
        }

        public void ClosedCurrentPage()
        {
            Destroy(gameObject);
        }
    }
}

