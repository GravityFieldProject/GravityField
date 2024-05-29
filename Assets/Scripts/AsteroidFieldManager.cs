using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AsteroidFieldManager : MonoBehaviour
{
    [SerializeField][Range(0, 100)] public int asteroidCount;
    [SerializeField] private GameObject m_asteroidFieldData;
    [SerializeField] private AsteroidField m_asteroidField;
    [HideInInspector] public List<GameObject> asteroids = new List<GameObject>();

    [SerializeField] private Slider countSlider;

    private bool init = false;

    public void ReloadScene()
    {
        // clean all asteroids
        foreach (GameObject asteroid in asteroids)
        {
            Destroy(asteroid);
        }   
        asteroids.Clear();
        init = false;
    }

    private void Start()
    {
        countSlider = GameObject.Find("CountSlider").GetComponent<Slider>();
    }

    private void Awake()
    {
        //for (int i = 0; i < asteroidCount; i++) 
        //{
        //    GameObject asteroid = null;
        //    // Select random asteroid
        //    int rng = Random.Range(1, 5);
        //    switch (rng) {
        //        case 1:
        //            asteroid = m_asteroidField.asteroid_1;
        //            break;

        //        case 2:
        //            asteroid = m_asteroidField.asteroid_2;
        //            break;

        //        case 3:
        //            asteroid = m_asteroidField.asteroid_3;
        //            break;

        //        case 4:
        //            asteroid = m_asteroidField.asteroid_4;
        //            break;

        //    }

        //    // Set random position within bounding box
        //    asteroid.transform.position = new Vector3(Random.Range(m_asteroidField.minBounds.x, m_asteroidField.maxBounds.x), Random.Range(m_asteroidField.minBounds.y, m_asteroidField.maxBounds.y), Random.Range(m_asteroidField.minBounds.z, m_asteroidField.maxBounds.z));

        //    // Set random rotation
        //    asteroid.transform.rotation = Random.rotation;

        //    // Set data
        //    asteroid.GetComponent<Asteroid>().asteroidFieldData = m_asteroidFieldData.GetComponent<AsteroidFieldData>();
        //    asteroid.GetComponent<Asteroid>().asteroidFieldManager = this;

        //    // Instanciate asteroid
        //    Instantiate(asteroid);
        //}

        //GameObject[] allAsteroids = GameObject.FindGameObjectsWithTag("Asteroid");

        //foreach (GameObject asteroid in allAsteroids)
        //{
        //    asteroids.Add(asteroid);
        //}
    }

    private void Update()
    {
        asteroidCount = (int)countSlider.value;

        if (!init)
        {
            for (int i = 0; i < asteroidCount; i++)
            {
                GameObject asteroid = null;
                // Select random asteroid
                int rng = Random.Range(1, 5);
                switch (rng)
                {
                    case 1:
                        asteroid = m_asteroidField.asteroid_1;
                        break;

                    case 2:
                        asteroid = m_asteroidField.asteroid_2;
                        break;

                    case 3:
                        asteroid = m_asteroidField.asteroid_3;
                        break;

                    case 4:
                        asteroid = m_asteroidField.asteroid_4;
                        break;

                }

                // Set random position within bounding box
                asteroid.transform.position = new Vector3(Random.Range(m_asteroidField.minBounds.x, m_asteroidField.maxBounds.x), Random.Range(m_asteroidField.minBounds.y, m_asteroidField.maxBounds.y), Random.Range(m_asteroidField.minBounds.z, m_asteroidField.maxBounds.z));

                // Set random rotation
                asteroid.transform.rotation = Random.rotation;

                // Set data
                asteroid.GetComponent<Asteroid>().asteroidFieldData = m_asteroidFieldData.GetComponent<AsteroidFieldData>();
                asteroid.GetComponent<Asteroid>().asteroidFieldManager = this;

                // Instanciate asteroid
                Instantiate(asteroid);
            }

            GameObject[] allAsteroids = GameObject.FindGameObjectsWithTag("Asteroid");

            foreach (GameObject asteroid in allAsteroids)
            {
                asteroids.Add(asteroid);
            }
            init = true;
        }
    }

    public void SwitchSceneToSolarSystem()
    {
        SceneManager.LoadScene("SolarSystem");
    }
}
