using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Object : MonoBehaviour
{
   
    private void OnCollisionEnter(Collision collision)
    {
        Mechanich.isTakedObject = true;
        GetComponent<Rigidbody>().useGravity = true;
    }
}
