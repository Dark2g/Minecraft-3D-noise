using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeData : MonoBehaviour
{
    

    void Start()
    {
        if (Physics.Raycast(transform.position, transform.up))
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }
    }

}
