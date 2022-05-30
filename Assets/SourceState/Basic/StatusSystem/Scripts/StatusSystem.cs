using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class StatusSystem : MonoBehaviour
    {
        private static StatusSystem _s;
        public static StatusSystem S {get { return _s; } }

        private void Awake()
        {
            if (!_s) _s = this;

            
        }

        private void Start()
        {
            CommandInit();
        }

        public Dictionary<string, StatusComponent> statusList = new Dictionary<string, StatusComponent>();

        [SerializeField] private StatusPage statusPagePrefab;
        [HideInInspector] public StatusPage statusPage;

        public void CommandInit()
        {
            CommandSystem.S.Declare("StatusFoodGain", StatusFoodGain);
        }

        //√¸¡Ó
        public InfoData StatusFoodGain(string[] values)
        {
            statusList[values[1]].statusData.food += float.Parse(values[2]);
            return null;
        }

        public void SwitchStatusPage(StatusData statusData)
        {
            if (statusPage == null)
            {
                statusPage = Instantiate(statusPagePrefab,FindObjectOfType<Canvas>().transform);
                statusPage.Init(statusData);
            }
            else
            {
                Destroy(statusPage.gameObject);
            }
        }
    }

    public class StatusData
    {
        public string name;
        public float food;

        public StatusData()
        {
            this.name = "default";
            this.food = 0;
        }
        public StatusData(string name,float food)
        {
            this.name = name;   
            this.food = food;
        }
    }
}


