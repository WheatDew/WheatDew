using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class RecipeModule : MonoBehaviour
    {
        private void Start()
        {
            TaskSystem.s.Declare("OpenRecipePage", CreateRecipePage);
        }

        private void CreateRecipePage(string[] values, TaskData taskData)
        {
            RecipePage recipePage = RecipeSystem.s.Create();
            recipePage.host = PackSystem.S.components[values[1]];
            recipePage.guest= PackSystem.S.components[values[2]];
        }

    }
}


