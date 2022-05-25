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

        public Dictionary<string, StatusComponent> statusList = new Dictionary<string, StatusComponent>();

        [SerializeField] private StatusPage statusPagePrefab;
        [HideInInspector] public StatusPage statusPage;


        public void SwitchStatusPage(string name)
        {
            if (statusPage == null)
            {
                statusPage = Instantiate(statusPagePrefab);
            }
            else
            {
                Destroy(statusPage);
            }
        }
    }
}


