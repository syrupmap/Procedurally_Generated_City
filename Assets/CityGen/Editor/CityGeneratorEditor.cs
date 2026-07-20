using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CityGenerator))]
public class CityGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draws all the normal fields (tilePieces, gridWidth, etc.) unchanged.
        DrawDefaultInspector();

        GUILayout.Space(10);

        CityGenerator generator = (CityGenerator)target;
        if (GUILayout.Button("Regenerate City", GUILayout.Height(30)))
        {
            generator.GenerateCity();
        }
    }
}