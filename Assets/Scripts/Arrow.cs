using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private GameObject sun;
    [SerializeField] private List<GameObject> planets;

    [SerializeField] private GravityCal gravityCal;


    [SerializeField] private Vector3 gravityField;
    [SerializeField] private Vector3 gradient;
    // Start is called before the first frame update
    void Start()
    {
        GameObject manager = GameObject.Find("Manager");    
        gravityCal = manager.gameObject.GetComponent<GravityCal>();

        sun = GameObject.Find("Sun");
        planets = new List<GameObject>();   
    }

    // Update is called once per frame
    void Update()
    {   
        // calculate the gravity forc on the arrow with the sun and planets or other two planets
        // formula : F = G * m1 * m2 / r^2

        // get the position of the arrow
        Vector3 arrowPosition = transform.position;

        // get the position of the sun
        Vector3 sunPosition = sun.transform.position;

    }
}
