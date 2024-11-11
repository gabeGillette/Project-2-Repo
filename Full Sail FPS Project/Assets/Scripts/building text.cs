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
        if (label != null) return; // Don't create the label if it already exists

        // Create a new GameObject for the label
        label = new GameObject(buildingName + "_Label");

        // Set the label as a child of the building
        label.transform.SetParent(transform);

        // Dynamically calculate the building's height using the Renderer component
        Renderer buildingRenderer = GetComponent<Renderer>();
        if (buildingRenderer != null)
        {
            float buildingHeight = buildingRenderer.bounds.size.y; // Get building height
            label.transform.localPosition = new Vector3(0, buildingHeight + verticalOffset.y, 0);
        }
        else
        {
            label.transform.localPosition = verticalOffset; // Use the default offset if no Renderer
        }

        // Add and configure the TextMesh
        text = label.AddComponent<TextMesh>();
        text.text = buildingName;
        text.color = textColor;
        text.fontSize = fontSize;
        text.characterSize = 0.2f;
        text.anchor = TextAnchor.MiddleCenter; // Center the text

        if (labelFont != null)
        {
            text.font = labelFont;
            text.GetComponent<MeshRenderer>().material = labelFont.material;
        }

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
