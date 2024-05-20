using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Outline))]
public class T_Object : MonoBehaviour
{


    private void OnCollisionEnter(Collision collision)
    {

        Mechanich.isTakedObject = false;
        GetComponent<Rigidbody>().useGravity = true;
        
    }

}
