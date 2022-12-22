using UnityEngine;
using UnityEditor;
using System.Collections;

namespace WPMF {
    static class WPMFMenuExtensions {

        [MenuItem("GameObject/3D Object/World Map 2D Edition - Main Map", false)]
        static void CreateGlobeMenuOption(MenuCommand menuCommand) {
            GameObject go = Object.Instantiate(Resources.Load<GameObject>("WPMF/Prefabs/WorldMap2D")) as GameObject;
            go.name = "WorldMap2D";
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        [MenuItem("GameObject/3D Object/World Map 2D Edition - Viewport", false)]
        static void CreateViewport() {
            GameObject go = Object.Instantiate(Resources.Load<GameObject>("WPMF/Prefabs/Viewport"));
            go.name = "Viewport";
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
    }

}