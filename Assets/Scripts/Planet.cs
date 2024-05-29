using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


public class Planet : MonoBehaviour
{
    [SerializeField] private bool m_isDebugActive = false;
    private TrailRenderer m_trail;

    [SerializeField] private SolarSystemData m_solarSystemData;
    [SerializeField] private GameObject m_sun;
    [SerializeField] private List<GameObject> m_planets = new List<GameObject>();

    [SerializeField] private string m_planetName;
    [SerializeField] private float m_rotSpeed;
    private Vector3d m_lastPos;
    private Vector3d m_pos;
    private Vector3d m_velocity;
    private Vector3d m_acceleration;
    private Vector3d m_lastAcceleration;

    [HideInInspector] public float m_mass;
    [HideInInspector] private float m_initalVelocity;


    Vector3 ToVector3(Vector3d _vector3d)
    {
        return new Vector3((float)_vector3d.x, (float)_vector3d.y, (float)_vector3d.z);
    }

    Vector3d ToSpaceCoords(Vector3 _vector3)
    {
        return new Vector3d(_vector3.x * m_solarSystemData.UA, _vector3.y * m_solarSystemData.UA, _vector3.z * m_solarSystemData.UA) / m_solarSystemData.DISTANCE_MULTIPLIER;
    }

    Vector3 ToUnityCoords(Vector3d _vector3d)
    {
        return new Vector3((float)(_vector3d.x / m_solarSystemData.UA), (float)(_vector3d.y / m_solarSystemData.UA), (float)(_vector3d.z / m_solarSystemData.UA)) * m_solarSystemData.DISTANCE_MULTIPLIER;
    }

    Vector3d CalculateNewPos(float _STEP)
    {
        Vector3d newPos = m_lastPos + m_velocity * _STEP + 0.5f * _STEP * _STEP * m_lastAcceleration;
        return newPos;
    }

    Vector3d CalculatePlanetToPlanetAcceleration(GameObject _planet)
    {
        Vector3d direction = ToSpaceCoords(_planet.transform.position - transform.position);
        double distance = direction.magnitude;
        direction.Normalize();
        double force = m_solarSystemData.GRAVITATIONAL_CONSTANT * (_planet.GetComponent<Planet>().m_mass * m_mass) / Mathf.Pow((float)distance, 2);
        Vector3d acceleration = direction * force / m_mass;
        
        return acceleration;
    }

    Vector3d CalculateNewAcceleration()
    {
        // Calculate sun gravitational field
        Vector3d direction = ToSpaceCoords(m_sun.GetComponent<Transform>().position - transform.position);
        double distance = direction.magnitude;
        direction.Normalize();
        double force = m_solarSystemData.GRAVITATIONAL_CONSTANT * (m_sun.GetComponent<Sun>().SUN_MASS * m_mass) / Mathf.Pow((float)distance, 2);
        Vector3d acceleration = direction * force / m_mass;

        // Calculate gravitational field of others objects
        for (int i = 0; i < m_planets.Count; i++) {
            acceleration += CalculatePlanetToPlanetAcceleration(m_planets[i]);
        }

        return acceleration;
    }

    Vector3d CalculateNewVelocity()
    {
        Vector3d newVelocity = m_velocity + (m_acceleration + m_lastAcceleration) * 0.5f * m_solarSystemData.step;
        return newVelocity;
    }

    private void Awake()
    {
        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");

        foreach (GameObject planet in planets) {
            if (planet != gameObject) {
                m_planets.Add(planet);
            }
            else if (planet.name == gameObject.name) {
                m_planetName = planet.name;
            }
        }

        // Depending on the planet name, assign current planet values on specified planet
        switch (m_planetName) {
            case "01_mercury":
                m_mass = m_solarSystemData.mercuryData.mass;
                m_initalVelocity = m_solarSystemData.mercuryData.initialAphelieVelocity.z;
                break;

            case "02_venus":
                m_mass = m_solarSystemData.venusData.mass;
                m_initalVelocity = m_solarSystemData.venusData.initialAphelieVelocity.z;
                break;

            case "03_earth":
                m_mass = m_solarSystemData.earthData.mass;
                m_initalVelocity = m_solarSystemData.earthData.initialAphelieVelocity.z;
                break;

            case "04_mars":
                m_mass = m_solarSystemData.marsData.mass;
                m_initalVelocity = m_solarSystemData.marsData.initialAphelieVelocity.z;
                break;

            case "06_jupiter":
                m_mass = m_solarSystemData.jupiterData.mass;
                m_initalVelocity = m_solarSystemData.jupiterData.initialAphelieVelocity.z;
                break;

            case "07_saturn":
                m_mass = m_solarSystemData.saturnData.mass;
                m_initalVelocity = m_solarSystemData.saturnData.initialAphelieVelocity.z;
                break;

            case "08_uranus":
                m_mass = m_solarSystemData.uranusData.mass;
                m_initalVelocity = m_solarSystemData.uranusData.initialAphelieVelocity.z;
                break;

            case "09_neptune":
                m_mass = m_solarSystemData.neptuneData.mass;
                m_initalVelocity = m_solarSystemData.neptuneData.initialAphelieVelocity.z;
                break;

        }
    }

    private void Start()
    {
        transform.position *= m_solarSystemData.DISTANCE_MULTIPLIER;
        m_velocity = Vector3d.forward * m_initalVelocity;
        Vector3d t = ToSpaceCoords(new Vector3(transform.position.x, transform.position.y, transform.position.z));
        m_pos = t;
        m_trail = GetComponentInParent<TrailRenderer>();
    }

    private void Update()
    {
        if (m_isDebugActive)
        {
            m_trail.enabled = true;
        }
        else if (m_isDebugActive == false && GetComponent<TrailRenderer>().enabled == true)
        {
            m_trail.enabled = false;
        }

        if (m_sun) 
        {
            m_lastPos = m_pos;
            m_lastAcceleration = m_acceleration;
            m_pos = CalculateNewPos(m_solarSystemData.step);
            transform.position = ToUnityCoords(m_pos);
            m_acceleration = CalculateNewAcceleration();
            m_velocity = CalculateNewVelocity();
            if (m_isDebugActive)
            {
                Debug.DrawLine(transform.position, transform.position + ToVector3(m_velocity) * 4f, Color.red);
            }
            transform.Rotate(Vector3.up * m_rotSpeed * Time.deltaTime);
        }
        else 
        {

        }
    }
}