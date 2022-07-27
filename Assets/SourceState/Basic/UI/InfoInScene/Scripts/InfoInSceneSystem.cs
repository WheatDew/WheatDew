using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class InfoInSceneSystem : MonoBehaviour
    {
        private static InfoInSceneSystem _s;
        public static InfoInSceneSystem s { get { return _s; } }

        private void Awake()
        {
            if (_s == null) _s = this;
        }


        [SerializeField] private InfoInScenePage prefab;
        private InfoInScenePage current;
        public RangeComponent currentTarget;

        //显示场景中信息
        public void DisplayItemInfoInScene(string info)
        {
            if(currentTarget.currentRange.Count != 0)
            {
                if (current != null)
                {
                    current.infoText.text = info;
                }
                else
                {
                    current = Instantiate(prefab, FindObjectOfType<Canvas>().transform);
                    current.infoText.text = info;
                }
            }

        }

        public void HiddenItemInfoInScene()
        {
            if (current != null&& currentTarget.currentRange.Count == 0)
            {
                Destroy(current.gameObject);
            }
        }
    }
}

