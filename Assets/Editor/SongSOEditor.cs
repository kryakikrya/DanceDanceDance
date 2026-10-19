using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SongSO))]
public class SongSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(8);

        if (GUILayout.Button("Generate Beats"))
        {
            SongSO song = (SongSO)target;

            Undo.RecordObject(song, "Generate Beats");
            song.GenerateBeats();

            EditorUtility.SetDirty(song);
            AssetDatabase.SaveAssets();
        }
    }
}