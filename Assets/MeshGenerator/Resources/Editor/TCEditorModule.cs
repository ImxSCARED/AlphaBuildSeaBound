using UnityEngine;
using UnityEditor;

/*===================================================================
    -----------------------------------------------------------------
   _____________________
   \                   /
    \_____       _____/   _________               ____
          |     |        /   _____  \     _______[____]_______
          |     |       /   /     \__\    |       ____       |
          |     |       |   |       ___   |-------\()/-------|
          |     |       |   |      /  /   |__________________|
          |     |        \   \____/  /    ____________________
          |_____|    ()   \_________/    /  TERRAIN CREATOR  |
    -----------------------------------------------------------------
 ===================================================================*/

[CustomEditor (typeof(TerrainCreator))]
[System.Serializable]
public class TCEditorModule : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainCreator terrainCreator = (TerrainCreator)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();

        if (GUILayout.Button("Create")) //Clear the data in the terrain system.
        {
            terrainCreator.EditorMeshCreate();
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Clear")) //Clear the data in the terrain system.
        {
            terrainCreator.ClearData();
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Restore")) // Restore all the values that were cleared.
        {
            terrainCreator.RestoreValues();
        }
    }
}
