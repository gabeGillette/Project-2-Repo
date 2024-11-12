using UnityEngine;

public class FaceCam : MonoBehaviour
{
    public Collider buildingTrigger; // The trigger collider for the building
    public GameObject label; // Reference to the label GameObject
    private bool isPlayerInside = true; // Tracks if the player is inside
    private MeshRenderer textRenderer; // MeshRenderer for the TextMesh

    void Start()
    {

        // Get the MeshRenderer component from the label
        textRenderer = label.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (!isPlayerInside && Camera.main != null)
        {
            // Make the text face the camera
            label.transform.rotation = Quaternion.LookRotation(label.transform.position - Camera.main.transform.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            textRenderer.enabled = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            textRenderer.enabled = true;
        }
    }
}
