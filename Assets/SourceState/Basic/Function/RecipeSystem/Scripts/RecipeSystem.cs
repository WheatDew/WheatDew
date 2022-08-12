using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Origin
{
    public class RecipeSystem : MonoBehaviour
    {
        private static RecipeSystem _s;
        public static RecipeSystem s { get { return _s; } }

        private void Awake()
        {
            if (_s == null) _s = this;
        }

        [SerializeField] private RecipePage RecipePagePrefab;
        [HideInInspector] public RecipePage recipePage;

        public RecipePage Create()
        {
            if(recipePage == null)
            {
                return Instantiate(RecipePagePrefab,FindObjectOfType<Canvas>().transform);
            }
            else
            {
                return recipePage;
            }
        }
    }
}

