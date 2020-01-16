using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundImageController : MonoBehaviour
{
    public Image ImageBackground;
    public BackgroundImageList backgroundImageList;

    public void UpdateBackground(string sceneName)
    {
        switch (sceneName)
        {
            case "bedroom":
                ImageBackground.sprite = backgroundImageList.bedroom;
                break;
            case "store":
                ImageBackground.sprite = backgroundImageList.store;
                break;
            case "restaurant":
                ImageBackground.sprite = backgroundImageList.restaurant;
                break;
            case "park":
                ImageBackground.sprite = backgroundImageList.park;
                break;
            case "library":
                ImageBackground.sprite = backgroundImageList.library;
                break;
            case "JimingTemple":
                ImageBackground.sprite = backgroundImageList.JimingTemple;
                break;
            case "market":
                ImageBackground.sprite = backgroundImageList.market;
                break;
        }
    }
}
