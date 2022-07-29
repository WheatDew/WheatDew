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

        public GameObject currentSelection;

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
            //SelectionJob();
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                UpdateSelectionPage(currentSelection.transform.GetInstanceID().ToString());

            }
        }


        #region 命令


        #endregion

        #region 功能

        //更新选择界面
        public void UpdateSelectionPage(string key)
        {
            if(currentSelection != null)
            {
                if (!selectionMenu.gameObject.activeSelf)
                {
                    selectionStatusData = StatusSystem.S.statusList[key].statusData;
                    selectionMenu.gameObject.SetActive(true);
                    HashSet<string> values = new HashSet<string>();
                    if (BuildingSystem.S.components.ContainsKey(key))
                        values.Add("Build SwitchBluePrintPage");
                    if (PackSystem.S.components.ContainsKey(key))
                        values.Add("Pack SwitchPackPage");
                    if (StatusSystem.S.statusList.ContainsKey(key))
                        values.Add("Status SwitchStatusPage");

                    selectionMenu.ButtonInit(values, key);
                    CameraSystem.s.camera.enabled = false;
                    CharacterMovement.isMoving = false;
                }
                else
                {
                    selectionMenu.gameObject.SetActive(false);
                    CameraSystem.s.camera.enabled = true;
                    CharacterMovement.isMoving = true;
                }


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
                            {
                                SetLayer(currentSelection.transform, 9);
                                
                            }

                            SetLayer(result.collider.transform, 8);
                            currentSelection = result.collider.gameObject;
                            string targetKey = currentSelection.GetComponent<SelectionComponent>().key;
                            selectionStatusData = StatusSystem.S.statusList[targetKey].statusData;
                            selectionMenu.gameObject.SetActive(true);
                            UpdateSelectionPage(targetKey);
                            CameraSystem.s.camera.enabled=false;
                            CharacterMovement.isMoving = false;
                        }
                    }
                }
                else
                {
                    if (currentSelection != null)
                    {
                        SetLayer(currentSelection.transform, 9);
                        currentSelection = null;
                        selectionStatusData = null;
                        selectionMenu.gameObject.SetActive(false);
                        CameraSystem.s.camera.enabled = true;
                        CharacterMovement.isMoving = true;
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

        public void SetLayer(Transform t,int layer)
        {
            t.gameObject.layer = layer;
            for(int i = 0; i < t.childCount; i++)
            {
                SetLayer(t.GetChild(i), layer);
            }
        }
    }
}

