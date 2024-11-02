using UnityEngine;

public class PathToWaypoints : MonoBehaviour
{
    public GameObject character;
    public float speed = 3f;

    private Vector3[] pathPoints;
    private int currentWaypointIndex = 0;

    void Start()
    {
        // Get vertices from the mesh
        Mesh pathMesh = GetComponent<MeshFilter>().mesh;
        pathPoints = pathMesh.vertices;

        // Convert local to world positions for each vertex
        for (int i = 0; i < pathPoints.Length; i++)
        {
            pathPoints[i] = transform.TransformPoint(pathPoints[i]);
        }
    }

    void Update()
    {
        if (pathPoints.Length == 0) return;

        // Move the character towards the current waypoint
        Vector3 targetPosition = pathPoints[currentWaypointIndex];
        character.transform.position = Vector3.MoveTowards(character.transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the character has reached the waypoint
        if (Vector3.Distance(character.transform.position, targetPosition) < 0.1f)
        {
            currentWaypointIndex++;

            // Loop or stop at the end of the path
            if (currentWaypointIndex >= pathPoints.Length)
            {
                currentWaypointIndex = 0; // Set to 0 if you want the path to loop
            }
        }
    }
}
