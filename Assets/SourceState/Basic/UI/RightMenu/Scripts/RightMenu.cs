using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Origin
{
    public class RightMenu : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<RectTransform>().position = Input.mousePosition;
        }

        public void Init(List<CommandButton> commandButtonList,RightMenuItem rightMenuItemPrefab)
        {
            for (int i = 0; i < commandButtonList.Count; i++)
            {
                RightMenuItem item = Instantiate(rightMenuItemPrefab, transform);
                item.Init(commandButtonList[i].name, commandButtonList[i].command);
            }
        }

        public void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                StartCoroutine(DestroySelf());
            }
        }

        public IEnumerator DestroySelf()
        {
            yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
        }
    }
}

