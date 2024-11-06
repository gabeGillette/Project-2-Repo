using UnityEditor;
using UnityEngine;

public class PPFXWindow : EditorWindow
{
    private void OnEnable()
    {
        // Subscribe to the new playModeStateChanged event
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }

    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            // Code to run when entering play mode
            Debug.Log("Entered Play Mode");
        }
        else if (state == PlayModeStateChange.ExitingPlayMode)
        {
            // Code to run when exiting play mode
            Debug.Log("Exiting Play Mode");
        }
    }
}
