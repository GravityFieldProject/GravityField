using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AsteroidField : MonoBehaviour
{
    [SerializeField] public Vector3 minBounds;
    [SerializeField] public Vector3 maxBounds;

    [SerializeField] public GameObject asteroid_1;
    [SerializeField] public GameObject asteroid_2;
    [SerializeField] public GameObject asteroid_3;
    [SerializeField] public GameObject asteroid_4;


    void DrawBounds()
    {
        // Bottom face
        Gizmos.DrawLine(new Vector3(minBounds.x, minBounds.y, minBounds.z), new Vector3(maxBounds.x, minBounds.y, minBounds.z));
        Gizmos.DrawLine(new Vector3(minBounds.x, minBounds.y, minBounds.z), new Vector3(minBounds.x, minBounds.y, maxBounds.z));
        Gizmos.DrawLine(new Vector3(minBounds.x, minBounds.y, maxBounds.z), new Vector3(maxBounds.x, minBounds.y, maxBounds.z));
        Gizmos.DrawLine(new Vector3(maxBounds.x, minBounds.y, minBounds.z), new Vector3(maxBounds.x, minBounds.y, maxBounds.z));

        // Left face
        Gizmos.DrawLine(new Vector3(minBounds.x, minBounds.y, minBounds.z), new Vector3(minBounds.x, maxBounds.y, minBounds.z));
        Gizmos.DrawLine(new Vector3(minBounds.x, maxBounds.y, minBounds.z), new Vector3(minBounds.x, maxBounds.y, maxBounds.z));
        Gizmos.DrawLine(new Vector3(minBounds.x, minBounds.y, maxBounds.z), new Vector3(minBounds.x, maxBounds.y, maxBounds.z));

        // Right face
        Gizmos.DrawLine(new Vector3(maxBounds.x, minBounds.y, minBounds.z), new Vector3(maxBounds.x, maxBounds.y, minBounds.z));
        Gizmos.DrawLine(new Vector3(maxBounds.x, maxBounds.y, minBounds.z), new Vector3(maxBounds.x, maxBounds.y, maxBounds.z));
        Gizmos.DrawLine(new Vector3(maxBounds.x, minBounds.y, maxBounds.z), new Vector3(maxBounds.x, maxBounds.y, maxBounds.z));

        // Front face
        Gizmos.DrawLine(new Vector3(minBounds.x, maxBounds.y, minBounds.z), new Vector3(maxBounds.x, maxBounds.y, minBounds.z));
        
        // Back face
        Gizmos.DrawLine(new Vector3(minBounds.x, maxBounds.y, maxBounds.z), new Vector3(maxBounds.x, maxBounds.y, maxBounds.z));
    }

    private void OnDrawGizmos()
    {
        DrawBounds();
    }
}
