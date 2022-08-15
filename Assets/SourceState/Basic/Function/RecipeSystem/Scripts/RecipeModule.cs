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
            TaskSystem.s.Declare("ProduceItem", ProduceItem);
        }

        private void CreateRecipePage(string[] values, TaskData taskData)
        {
            RecipePage recipePage = RecipeSystem.s.Create();
            recipePage.host = PackSystem.S.components[values[1]];
            recipePage.guest= PackSystem.S.components[values[2]];
            string[] recipes = recipePage.host.recipes.Split('&');

            for(int i = 0; i < recipes.Length; i++)
            {
                string[] recipe = recipes[i].Split(' ');
                recipePage.CreateItem(recipe[0], recipe);
            }
        }

        private void ProduceItem(string[] values, TaskData taskData)
        {
            
            for(int index = 3; index < values.Length; index+=2)
            {
                if (PackSystem.S.PackItemGain(values[1], values[index], int.Parse(values[index + 1])).intValue != 0)
                    break;
            }
        }
    }
}


