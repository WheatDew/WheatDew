using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Origin
{
    public class SelectionSystem : MonoBehaviour
    {
        private static SelectionSystem _s;
        public static SelectionSystem s { get { return _s; } }

        [SerializeField] private SelectionInfo selectionInfoPrefab;
        [HideInInspector] public SelectionInfo selectionInfo;
        [SerializeField] private SelectionMenu selectionMenuPrefab;
        [HideInInspector] public SelectionMenu selectionMenu;

        public Dictionary<string, SelectionComponent> components = new Dictionary<string, SelectionComponent>();

        [HideInInspector] public GameObject currentSelection;
        public StatusData selectionStatusData;

        private void Awake()
        {
            if (!_s) { _s = this; }
        }

        private void Start()
        {
            selectionInfo = Instantiate(selectionInfoPrefab,FindObjectOfType<Canvas>().transform);
            selectionMenu = Instantiate(selectionMenuPrefab, FindObjectOfType<Canvas>().transform);

        }

        public void Update()
        {
            SelectionJob();
        }


        #region 命令


        #endregion

        #region 功能

        //更新选择界面
        public void UpdateSelectionPage(string key)
        {
            if(currentSelection != null)
            {
                HashSet<string> values = new HashSet<string>();
                if (BuildingSystem.S.components.ContainsKey(key))
                    values.Add("Build SwitchBluePrintPage");
                if (PackSystem.S.components.ContainsKey(key))
                    values.Add("Pack SwitchPackPage");
                if (StatusSystem.S.statusList.ContainsKey(key))
                    values.Add("Status SwitchStatusPage");

                selectionMenu.ButtonInit(values,key);
            }
        }


        //选择功能
        public void SelectionJob()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit result;
                if (Physics.Raycast(ray, out result, 100, LayerMask.GetMask("Selection", "Object")))
                {
                    if (result.collider.gameObject.layer == 9)
                    {
                        if (currentSelection != result.collider.gameObject)
                        {
                            if (currentSelection != null)
                                currentSelection.layer = 9;
                            result.collider.gameObject.layer = 8;
                            currentSelection = result.collider.gameObject;
                            string targetKey = currentSelection.GetComponent<SelectionComponent>().key;
                            selectionStatusData = StatusSystem.S.statusList[targetKey].statusData;
                            selectionMenu.gameObject.SetActive(true);
                            UpdateSelectionPage(targetKey);
                        }
                    }
                }
                else
                {
                    if (currentSelection != null)
                    {
                        currentSelection.layer = 9;
                        currentSelection = null;
                        selectionStatusData = null;
                        selectionMenu.gameObject.SetActive(false);
                    }
                }
            }

            if (selectionStatusData != null)
            {
                selectionInfo.gameObject.SetActive(true);
                selectionInfo.nameInfo.text = selectionStatusData.name;
            }
            else
            {
                selectionInfo.gameObject.SetActive(false);
            }
        }

        #endregion
    }
}

