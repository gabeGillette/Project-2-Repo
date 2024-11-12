using UnityEngine;

public class BuildingText : MonoBehaviour
{
    public string buildingName = "Building Name";
    public Vector3 verticalOffset = new Vector3(0, 5, 0); // The height above the building
    public Color textColor = Color.white;
    public Font labelFont;
    public int fontSize = 25;

    public GameObject label; // Reference to the label GameObject
    private TextMesh text;

    void Start()
    {
        // Create the label when the game starts
        createLabel();
    }

    // Create the label on the building
    public void createLabel()
    {
        if (label != null) return; // Avoid creating duplicate labels

        // Create a new GameObject for the label
        label = new GameObject(buildingName + "_Label");

        // Set the label as a child of the building
        label.transform.SetParent(transform, false); // Maintain local transform properties

        // Calculate the exact position for the label
        Renderer buildingRenderer = GetComponent<Renderer>();
        if (buildingRenderer != null)
        {
            // Get world position of the top center of the building
            Vector3 topCenter = new Vector3(
                buildingRenderer.bounds.center.x,
                buildingRenderer.bounds.max.y,
                buildingRenderer.bounds.center.z
            );

            // Apply offset above the building
            label.transform.position = topCenter + verticalOffset;
        }
        else
        {
            label.transform.localPosition = verticalOffset; // Default if no Renderer
        }

        // Add and configure the TextMesh
        text = label.AddComponent<TextMesh>();
        text.text = buildingName;
        text.color = textColor;
        text.fontSize = fontSize;
        text.characterSize = 0.2f;
        text.anchor = TextAnchor.MiddleCenter;

        if (labelFont != null)
        {
            text.font = labelFont;
            text.GetComponent<MeshRenderer>().material = labelFont.material;
        }

        // Ensure TextMesh renders correctly
        MeshRenderer meshRenderer = label.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = 10;
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        // Make the label face the camera
        label.AddComponent<FaceCam>();
    }


    // Optionally change the label text during runtime
    public void changeLabel(string newName)
    {
        buildingName = newName;
        if (label != null)
        {
            text.text = buildingName;
        }
    }
}
