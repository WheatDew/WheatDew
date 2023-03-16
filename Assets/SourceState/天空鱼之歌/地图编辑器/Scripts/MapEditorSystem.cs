using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyWhale
{
    public class MapEditorSystem : MonoBehaviour
    {
        private static MapEditorSystem _instance;
        public static MapEditorSystem Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }
        public Transform pageParent,elementParent;

        [SerializeField] private DragStorePage dragStorePagePrefab;
        [HideInInspector] public DragStorePage dragStorePage;

        public List<MapEditorDragComponent> models=new List<MapEditorDragComponent>();

        private void Start()
        {
            DisplayMapEditorPage();
        }

        public void CreateMapEditorPage()
        {
            dragStorePage = Instantiate(dragStorePagePrefab, pageParent);
        }

        public void DisplayMapEditorPage()
        {
            if (dragStorePage == null)
                CreateMapEditorPage();
            dragStorePage.gameObject.SetActive(true);
            dragStorePage.InitEvent.AddListener(delegate
            {
                MapEditorPageInit();
            });
            dragStorePage.DragEndEvent.AddListener(delegate
            {
                
            });
        }

        public void HiddenMapEditorPage()
        {
            dragStorePage.gameObject.SetActive(false);
        }

        public void DestroyMapEditorPage()
        {
            Destroy(dragStorePage);
        }

        public void MapEditorPageInit()
        {
            dragStorePage.CreateElement();
            dragStorePage.elementParent = elementParent;
        }

        //public void MapEditorPageEndDrag(string )
        //{
            
        //}
    }
}

