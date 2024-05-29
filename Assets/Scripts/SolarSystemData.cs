using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct PlanetData
{
    public float mass;
    public Vector3 initialAphelieVelocity;
    public Vector3 initialPosition;
}

public class SolarSystemData : MonoBehaviour
{
    [SerializeField] public PlanetData mercuryData;
    [SerializeField] public PlanetData venusData;
    [SerializeField] public PlanetData earthData;
    [SerializeField] public PlanetData marsData;
    [SerializeField] public PlanetData jupiterData;
    [SerializeField] public PlanetData saturnData;
    [SerializeField] public PlanetData uranusData;
    [SerializeField] public PlanetData neptuneData;

    [SerializeField] public int DISTANCE_MULTIPLIER = 1;
    [SerializeField] public double UA = 149597828677.28f;
    [SerializeField] public double GRAVITATIONAL_CONSTANT = 6.67430f * Mathf.Pow(10, -11);
    [SerializeField] public double SUN_MASS = 1.989f * Mathf.Pow(10, 30);
    [SerializeField] public int step = 3600;

    [SerializeField] private Slider speedSlider;

    public bool isMoving = true;

    private void Start()
    {
        speedSlider = GameObject.Find("SpeedSlider").GetComponent<Slider>();
    }

    private void Update()
    {
        if (isMoving)
        {
            step = (int)speedSlider.value;
        }
        else
        {
            step = 0;
        }
    }

    public void StopMovement()
    {
        isMoving = false;
    }
    public void StartMovement()
    {
        isMoving = true;
    }   
}
