using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;
using Vector3 = UnityEngine.Vector3;

public class Asteroid : MonoBehaviour
{

    public AsteroidFieldData asteroidFieldData;
    public AsteroidFieldManager asteroidFieldManager;
    //private List<GameObject> m_asteroids = new List<GameObject>();

    private Vector3d m_lastPos;
    private Vector3d m_pos;
    private Vector3d m_velocity;
    private Vector3d m_acceleration;
    private Vector3d m_lastAcceleration;

    public double mass;
    public float initalVelocity;

    Vector3 ToVector3(Vector3d _vector3d)
    {
        return new Vector3((float)_vector3d.x, (float)_vector3d.y, (float)_vector3d.z);
    }

    Vector3d ToSpaceCoords(Vector3 _vector3)
    {
        return new Vector3d(_vector3.x * asteroidFieldData.UA, _vector3.y * asteroidFieldData.UA, _vector3.z * asteroidFieldData.UA);
    }

    Vector3 ToUnityCoords(Vector3d _vector3d)
    {
        return new Vector3((float)(_vector3d.x / asteroidFieldData.UA), (float)(_vector3d.y / asteroidFieldData.UA), (float)(_vector3d.z / asteroidFieldData.UA));
    }

    Vector3d CalculateNewPos(float _STEP)
    {
        Vector3d newPos = m_lastPos + m_velocity * _STEP + 0.5f * _STEP * _STEP * m_lastAcceleration;
        return newPos;
    }

    Vector3d CalculateAsteroidToAsteroidAcceleration(GameObject _asteroid)
    {
        Vector3d direction = ToSpaceCoords(_asteroid.transform.position - transform.position);
        double distance = direction.magnitude;
        direction.Normalize();
        double force = asteroidFieldData.GRAVITATIONAL_CONSTANT * (_asteroid.GetComponent<Asteroid>().mass * mass) / Mathf.Pow((float)distance, 2);
        Vector3d acceleration = direction * force / mass;

        return acceleration;
    }

    Vector3d CalculateNewAcceleration()
    {
        Vector3d acceleration = new Vector3d(0.0f, 0.0f, 0.0f);

        // Calculate gravitational field of all other objects
        for (int i = 0; i < asteroidFieldManager.asteroids.Count; i++)
        {
            if (asteroidFieldManager.asteroids[i].gameObject != gameObject)
            {
                if(asteroidFieldManager.asteroids[i])
                    acceleration += CalculateAsteroidToAsteroidAcceleration(asteroidFieldManager.asteroids[i]);
            }
        }

        return acceleration;
    }
    Vector3d CalculateNewVelocity()
    {
        Vector3d newVelocity = m_velocity + (m_acceleration + m_lastAcceleration) * 0.5f * asteroidFieldData.step;
        return newVelocity;
    }

    private void Start()
    {
        gameObject.AddComponent<TrailRenderer>();
        gameObject.GetComponent<TrailRenderer>().enabled = false;
        gameObject.GetComponent<TrailRenderer>().time = 500;
        gameObject.GetComponent<TrailRenderer>().widthMultiplier = 0.01f;

        // Set mass and speed
        int rng = Random.Range(0, 100);
        if (rng <= 60)
        {
            int exp = Random.Range(20, 25);
            mass = 1.989f * Mathf.Pow(10, exp);
            transform.localScale *= (1 + exp / 100.0f);
        }
        else if (rng > 60 && rng <= 85)
        {
            int exp = Random.Range(26, 29);
            mass = 1.989f * Mathf.Pow(10, exp);
            transform.localScale *= (1 + exp / 100.0f);
        }
        else
        {
            int exp = Random.Range(30, 32);
            mass = 1.989f * Mathf.Pow(10, exp);
            transform.localScale *= (1 + exp / 100.0f);
        }
        initalVelocity = Random.Range(20000.0f, 35000.0f);

        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");

        m_velocity = new Vector3d(transform.rotation.x, transform.rotation.y, transform.rotation.z) * initalVelocity;
        Vector3d t = ToSpaceCoords(new Vector3(transform.position.x, transform.position.y, transform.position.z));
        m_pos = t;
    }


    void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a GameObject with a specific tag
        if (other.gameObject != gameObject && other.gameObject.tag == "Asteroid")
        {
            if (mass <= other.gameObject.GetComponent<Asteroid>().mass)
            {
                other.gameObject.GetComponent<Asteroid>().mass += mass;
                other.gameObject.transform.localScale += transform.localScale;
                asteroidFieldManager.asteroids.Remove(gameObject);
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        if (asteroidFieldData.showTrails)
        {
            gameObject.GetComponent<TrailRenderer>().enabled = true;
        }
        else
        {
            gameObject.gameObject.GetComponent<TrailRenderer>().enabled = false;
        }


        m_lastPos = m_pos;
        m_lastAcceleration = m_acceleration;
        m_pos = CalculateNewPos(asteroidFieldData.step);
        transform.position = ToUnityCoords(m_pos);
        m_acceleration = CalculateNewAcceleration();
        m_velocity = CalculateNewVelocity();
        transform.Rotate(Vector3.up * 20.0f * Time.deltaTime);
    }
}
