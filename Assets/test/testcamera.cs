using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcamera : MonoBehaviour
{
    GameObject cam1;
    GameObject cam2;
    GameObject cam3;
    // Start is called before the first frame update
    void Start()
    {
        cam1 = GameObject.Find("Virtual Camera1");
        cam2 = GameObject.Find("Virtual Camera2");
        cam3 = GameObject.Find("Virtual Camera3");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Portal_1")
        {
            cam1.SetActive(true);
            cam2.SetActive(false);
            cam3.SetActive(false);
        }
        if (other.gameObject.name == "Portal_2" || other.gameObject.name == "Portal_3")
        {
            cam1.SetActive(false);
            cam2.SetActive(true);
            cam3.SetActive(false);
        }
        if (other.gameObject.name == "Portal_4")
        {
            cam1.SetActive(false);
            cam2.SetActive(false);
            cam3.SetActive(true);
        }
    }
}
