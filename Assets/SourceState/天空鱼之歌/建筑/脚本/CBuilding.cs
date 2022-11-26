using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBuilding : MonoBehaviour
{
    public HashSet<GameObject> EnterList=new HashSet<GameObject>();

    public List<Material> allMaterials = new List<Material>();
    public Material targetMaterial;
    public List<MeshRenderer> allMeshRenderer = new List<MeshRenderer>();

    private async void Start()
    {
        DBuilding.s.AddBuildingData(this);

        await new WaitForSeconds(5);
        SetAllMaterialList();
    }
    
    public void SetAllMaterialList()
    {
        var meshRenderer = transform.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            allMeshRenderer.Add(meshRenderer);
            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                meshRenderer.materials[i] = targetMaterial;
            }
        }

        GetChildrenMeshRenderer(transform);
    }

    public void GetChildrenMeshRenderer(Transform node)
    {
        for (int i = 0; i < node.childCount; i++)
        {
            
            var meshRenderer = node.GetChild(i).GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                allMeshRenderer.Add(meshRenderer);
                Material[] materials = new Material[meshRenderer.materials.Length];
                for (int j = 0; j < meshRenderer.materials.Length; j++)
                {
                    materials[j]= targetMaterial;
                }

                meshRenderer.materials = materials;
                //meshRenderer.material = targetMaterial;
            }

            GetChildrenMeshRenderer(node.GetChild(i));
        }
    }


    private void Update()
    {
        foreach(var item in EnterList)
        {
            if (DBuilding.s.characterList.ContainsKey(item.GetInstanceID()))
            {
                var characterData = DBuilding.s.characterList[item.GetInstanceID()];
                if (characterData.buildingPrepare)
                {
                    characterData.buildingPrepare = false;
                    characterData.isbuilding = true;
                }

                if (characterData.isbuilding)
                {
                    Debug.Log(characterData.gameObject.name);
                }
            }
        }


    }
}
