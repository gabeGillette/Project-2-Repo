using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class LabelPersistenceManager
{
    static LabelPersistenceManager()
    {
        // Register callback to detect when the play mode state changes
        EditorApplication.playModeStateChanged += HandlePlayModeStateChanged;
    }

    private static void HandlePlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            // Handle the label persistence here
            RestoreLabelsInScene();
        }
    }

    private static void RestoreLabelsInScene()
    {
        // Logic to restore labels from a static list or any kind of storage
        BuildingText[] buildingTexts = GameObject.FindObjectsOfType<BuildingText>();

        foreach (var buildingText in buildingTexts)
        {
            if (buildingText != null && buildingText.label == null)
            {
                buildingText.createLabel();
            }
        }
    }
}
