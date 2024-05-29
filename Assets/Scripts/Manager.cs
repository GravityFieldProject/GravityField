using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] private GameObject sun;
    [SerializeField] private GameObject planet;
    [SerializeField] private GameObject arrow;

    private List<GameObject> arrows = new List<GameObject>();
    private List<GameObject> planets = new List<GameObject>();

    public int gridSize = 20;
    public float fieldStrength = 1f;
    public float maxDistance = 0.5f;
    public float interval = 1f;

    [SerializeField] private bool isFoucsedOnSun = true;
    [SerializeField] private Slider fieldStrengthSlider;
    [SerializeField] private Toggle sunGravity;

    [SerializeField] private Text textValX;
    [SerializeField] private Text textValY;
    [SerializeField] private Text textValZ;
    [SerializeField] private Text textMass;

    [SerializeField] public GameObject selectedObject;

    private void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        sun = GameObject.Find("Sun");

        fieldStrengthSlider = GameObject.Find("FieldStrength").GetComponent<Slider>();
        sunGravity = GameObject.Find("SunGravity").GetComponent<Toggle>();

        GameObject[] planetObjects = GameObject.FindGameObjectsWithTag("Planet");

        textValX = GameObject.Find("ValX").GetComponent<Text>();
        textValY = GameObject.Find("ValY").GetComponent<Text>();
        textValZ = GameObject.Find("ValZ").GetComponent<Text>();

        textMass = GameObject.Find("Mass").GetComponent<Text>();

        foreach (GameObject planetObject in planetObjects)
        {
            planets.Add(planetObject);
        }

        if (sun && arrow)
        {
            for (int x = -gridSize; x <= gridSize; x++)
            {
                for (int y = -gridSize; y <= gridSize; y++)
                {
                    UnityEngine.Vector3 position = new UnityEngine.Vector3(x * interval, 0, y * interval);
                    float distanceToSun = UnityEngine.Vector3.Distance(position, sun.transform.position);

                    if (distanceToSun > maxDistance)
                    {
                        continue; // Skip points outside the specified range
                    }

                    UnityEngine.Vector3 toSunDirection = (sun.transform.position - position).normalized;

                    float normalizedDistance = Mathf.Clamp01(distanceToSun / maxDistance);
                    float vectorLength = 1f - normalizedDistance;
                    UnityEngine.Vector3 fieldVector = toSunDirection * fieldStrength * vectorLength;

                    // Instantiate arrow prefab at position with proper rotation
                    GameObject arrowInstance = Instantiate(arrow, position, UnityEngine.Quaternion.identity);
                    arrowInstance.transform.forward = fieldVector.normalized;

                    // Adjust scale based on vector length
                    arrowInstance.transform.localScale = new UnityEngine.Vector3(fieldVector.magnitude, fieldVector.magnitude, fieldVector.magnitude);

                    // Change color based on distance
                    Renderer arrowRenderer = arrowInstance.GetComponentInChildren<Renderer>();
                    if (arrowRenderer)
                    {
                        arrowRenderer.material.color = new Color(1f, 1f - normalizedDistance, 0f, 1f);
                    }
                    arrows.Add(arrowInstance);
                }
            }
        }
    }

    private void Update()
    {
        fieldStrength = fieldStrengthSlider.value;
        isFoucsedOnSun = sunGravity.isOn;

        if (!isFoucsedOnSun)
        {
            foreach (var arrow in arrows)
            {
                GameObject closestPlanet = null;
                float closestPlanetDistance = float.MaxValue;

                foreach (var planet in planets)
                {
                    float distanceToPlanet = UnityEngine.Vector3.Distance(arrow.transform.position, planet.transform.position);
                    if (distanceToPlanet < closestPlanetDistance)
                    {
                        closestPlanetDistance = distanceToPlanet;
                        closestPlanet = planet;
                    }
                }

                if (closestPlanet != null)
                {
                    UnityEngine.Vector3 toPlanetDirection = (closestPlanet.transform.position - arrow.transform.position).normalized;
                    float normalizedDistance = Mathf.Clamp01(closestPlanetDistance / maxDistance);
                    float vectorLength = 1f - normalizedDistance;

                    // Adjust scale based on distance difference
                    float distanceDiff = Mathf.Clamp01(Mathf.Abs(closestPlanetDistance - maxDistance)); // Limiting the range to [0, 1]
                    float scaleValue = 1 / (distanceDiff + 1); // Inverse proportionality with a range of [0, 1]
                    vectorLength *= scaleValue;

                    UnityEngine.Vector3 fieldVector = toPlanetDirection * fieldStrength * vectorLength;

                    arrow.transform.forward = fieldVector.normalized;

                    // Adjust scale based on vector length
                    arrow.transform.localScale = new UnityEngine.Vector3(fieldVector.magnitude, fieldVector.magnitude, fieldVector.magnitude);

                    // Change color based on distance
                    Renderer arrowRenderer = arrow.GetComponentInChildren<Renderer>();
                    if (arrowRenderer)
                    {
                        arrowRenderer.material.color = new Color(1f, 1f - normalizedDistance, 0f, 1f);
                    }
                }
            }
        }
        else
        {
            foreach (var arrow in arrows)
            {
                float distanceToSun = UnityEngine.Vector3.Distance(arrow.transform.position, sun.transform.position);
                if (distanceToSun > maxDistance)
                {
                    continue; // Skip points outside the specified range
                }
                UnityEngine.Vector3 toTargetDirection = (sun.transform.position - arrow.transform.position).normalized;
                float normalizedDistance = Mathf.Clamp01(distanceToSun / maxDistance);
                float vectorLength = 1f - normalizedDistance;
                UnityEngine.Vector3 fieldVector = toTargetDirection * fieldStrength * vectorLength;
                // Instantiate arrow prefab at position with proper rotation
                arrow.transform.forward = fieldVector.normalized;
                // Adjust scale based on vector length
                arrow.transform.localScale = new UnityEngine.Vector3(fieldVector.magnitude, fieldVector.magnitude, fieldVector.magnitude);
                // Change color based on distance
                Renderer arrowRenderer = arrow.GetComponentInChildren<Renderer>();
                if (arrowRenderer)
                {
                    arrowRenderer.material.color = new Color(1f, 1f - normalizedDistance, 0f, 1f);
                }
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            // Shut a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Raycast to check if the ray intersects with any object
            if (Physics.Raycast(ray, out hit))
            {
                // If the ray intersects with an object, select it
                selectedObject = hit.collider.gameObject;

                // Show the context menu for the selected object
                ShowContextMenu(selectedObject);
            }
            else
            {
                // If clicked on empty space, deselect the object   
                DeselectObject();
            }
        }

        if (selectedObject)
        {
            // Update the position values of the selected object
            textValX.text = selectedObject.transform.position.x.ToString();
            textValY.text = selectedObject.transform.position.y.ToString();
            textValZ.text = selectedObject.transform.position.z.ToString();
            textMass.text = selectedObject.GetComponent<Planet>().m_mass.ToString();  
        }
    }

    // Show context menu for the selected object
    void ShowContextMenu(GameObject selectedObject)
    {
        Debug.Log("Selected object: " + selectedObject.name);
    }

    // Cancel selection of the object
    void DeselectObject()
    {
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
    public void HideAllArrows()
    {
        foreach (var arrow in arrows)
        {
            arrow.SetActive(false);
        }
    }
    public void ShowAllArrows()
    {
        foreach (var arrow in arrows)
        {
            arrow.SetActive(true);
        }
    }

    public void SwitchSceneToAsteroidField()
    {
        SceneManager.LoadScene("AsteroidField");
    }

    [SerializeField] private Canvas canvas;
    public void HideCanvas()
    {
        canvas.enabled = false;
    }
    public void ActiveCanvas()
    {
        canvas.enabled = true;
    }
}