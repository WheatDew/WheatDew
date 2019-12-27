using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class TestSystem : ComponentSystem
{

    protected override void OnCreate()
    {
        
    }

    protected override void OnStartRunning()
    {

    }

    protected override void OnUpdate()
    {
        Entities.ForEach((TestCom test) => {
            test.ID = 100;
        });

        if (Input.GetKeyDown(KeyCode.U))
            World.Active.GetExistingSystem<CorpusSystem>().CreateCommand();
    }
}
