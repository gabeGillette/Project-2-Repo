using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager instance;

    [SerializeField] private TextMeshProUGUI debugText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keeps DebugManager active across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        UpdateDebugInfo();
    }

    // Method to update the debug information on screen
    private void UpdateDebugInfo()
    {
        // Uncomment and modify the following line when PlayerController is available: Signed: (Jonathan Busby)
        // Vector3 playerPosition = PlayerController.instance.transform.position;
        // debugText.text = $"Player Position: {playerPosition}\n";

        debugText.text = "Debug Mode Active\n";
        debugText.text += "Player data will be displayed here when available.\n";
    }

    // Public method to add custom debug info, can be called from anywhere
    public void Log(string message)
    {
        debugText.text += $"{message}\n";
    }
}
