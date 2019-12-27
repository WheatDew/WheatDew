using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;

public class TestGroup : MonoBehaviour
{
    public Slider happinessSlider;
    public Slider sadnessSlider;
    public Slider angerSlider;

    //private EmotionSystem_WDC emotionSystem_WDC;

    // Start is called before the first frame update
    void Start()
    {
        //EmotionSystem_WDC emotionSystem_WDC = World.Active.GetExistingSystem<EmotionSystem_WDC>();
        //happinessSlider.onValueChanged.AddListener(delegate { emotionSystem_WDC.Happiness = happinessSlider.value;});
        //sadnessSlider.onValueChanged.AddListener(delegate { emotionSystem_WDC.Sadness = sadnessSlider.value; });
        //angerSlider.onValueChanged.AddListener(delegate { emotionSystem_WDC.anger = angerSlider.value; });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
