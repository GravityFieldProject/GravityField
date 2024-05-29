using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public double SUN_MASS = 1.989f * Mathf.Pow(10, 30);
    private void Start()
    {
        Debug.Log(SUN_MASS);
    }
}