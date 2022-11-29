using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ObstacleCamera : MonoBehaviour
{
    public CinemachineFreeLook cameraSetting;

    public void SetIndoorView()
    {
        cameraSetting.m_Orbits[0].m_Height = 3f;
        cameraSetting.m_Orbits[0].m_Radius = 1f;
        cameraSetting.m_Orbits[1].m_Height = 2.5f;
        cameraSetting.m_Orbits[1].m_Radius = 1f;
        cameraSetting.m_Orbits[2].m_Height = 1f;
        cameraSetting.m_Orbits[2].m_Radius = 0.5f;
    }

    public void SetOutdoorView()
    {
        cameraSetting.m_Orbits[0].m_Height = 4f;
        cameraSetting.m_Orbits[0].m_Radius = 2f;
        cameraSetting.m_Orbits[1].m_Height = 2.5f;
        cameraSetting.m_Orbits[1].m_Radius = 4f;
        cameraSetting.m_Orbits[2].m_Height = 0f;
        cameraSetting.m_Orbits[2].m_Radius = 1f;
    }

}
