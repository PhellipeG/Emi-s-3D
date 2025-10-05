using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkMia : MonoBehaviour
{
    public bool naMira = false;

    void OnTriggerEnter(Collider coneCollider)
    {
        if(coneCollider.gameObject.tag == "Player")
        {
            naMira = true;
        }
    }
    void OnTriggerExit(Collider coneCollider)
    {
        if (coneCollider.gameObject.tag == "Player")
        {
            naMira = false;
        }
    }
}
