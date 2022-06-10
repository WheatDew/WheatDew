using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class SelectionSystem : MonoBehaviour
    {
        private static SelectionSystem _s;
        public static SelectionSystem s { get { return _s; } }

        [SerializeField] private SelectionInfo selectionInfoPrefab;
        [HideInInspector] public SelectionInfo selectionInfo;

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

        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit result;
                if (Physics.Raycast(ray, out result, 100, LayerMask.GetMask("Selection", "Object")))
                {
                    print(result.collider.gameObject.name);
                    if (result.collider.gameObject.layer == 9)
                    {
                        if (currentSelection != result.collider.gameObject)
                        {
                            if (currentSelection != null)
                                currentSelection.layer = 9;
                            result.collider.gameObject.layer = 8;
                            currentSelection = result.collider.gameObject;
                            selectionStatusData = StatusSystem.S.statusList[currentSelection.GetComponent<SelectionComponent>().key].statusData;
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
    }
}

