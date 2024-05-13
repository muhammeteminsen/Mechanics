using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mechanich : MonoBehaviour
{

    Rigidbody rb;
    public float speed;
    float currentRotationY = 0f;
    public float mouseSensivity;
    GameObject hitObject;
    Transform fHitObjectR;
    Vector3 rayOriginX;
    bool isTakenObject;
    public static bool isTakedObject;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        isTakenObject = false;
        isTakedObject = false;
        hitObject = null;

    }


    void Update()
    {
        Movement();
        ObjectTransform();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(0, 5, 0);
        }

    }
    void ObjectTransform()
    {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);



        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(ray.origin, hit.transform.position * hit.distance, Color.red);
            Debug.Log(hitObject);
            Debug.Log("Object Distance: " + Vector3.Distance(ray.origin, hit.point));
            Rigidbody hitRigidbody = hit.transform.gameObject.GetComponent<Rigidbody>();
            GameObject pivot = GameObject.FindGameObjectWithTag("Pivot");
            rayOriginX = new Vector3(hit.point.x,ray.origin.y,hit.point.z);
           
            if (Vector3.Distance(ray.origin, hit.point) <= 2f)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    isTakenObject = !isTakenObject;
                    Debug.Log(isTakenObject);
                    Debug.Log("Girdi");

                    if (hit.transform.gameObject.CompareTag("Grounded"))
                        isTakenObject = false;
                        

                    if (isTakenObject && hitObject == null && !hit.transform.gameObject.CompareTag("Grounded"))
                    {
                        
                        hitObject = hit.transform.gameObject;
                        fHitObjectR = hitObject.transform;
                        hitObject.transform.rotation = fHitObjectR.rotation;
                        hitObject.GetComponent<MeshRenderer>().enabled=false;
                        hit.transform.GetChild(0).gameObject.SetActive(true);
                        
                        
                    }
                    if (!isTakenObject && hitObject != null && hit.transform.gameObject.CompareTag("Grounded"))
                    {
                        hitObject.transform.position = rayOriginX;
                        hitObject.SetActive(true);
                        if (Input.GetKey(KeyCode.LeftShift))
                            fHitObjectR.rotation = hitObject.transform.rotation;
                        else
                            hitObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                        hit.transform.GetChild(0).gameObject.SetActive(false);
                        hitObject = null;
                        fHitObjectR = null;

                    }
                }
                if (isTakenObject)
                {
                    hit.transform.GetChild(0).GetComponent<Rigidbody>().MovePosition(pivot.transform.position);
                    hit.transform.GetChild(0).GetComponent<Rigidbody>().MoveRotation(Quaternion.LookRotation(Camera.main.transform.forward));

                }
                
            }
            if (Input.GetButton("Fire1") && !hit.transform.gameObject.CompareTag("Grounded") && !isTakedObject)
            {

                

                hitRigidbody.MovePosition(pivot.transform.position);
                hitRigidbody.MoveRotation(Quaternion.LookRotation(Camera.main.transform.forward));
                hitRigidbody.useGravity = false;
                hitRigidbody.isKinematic = true;
                hitRigidbody.isKinematic = false;

            }
            if (Input.GetButtonUp("Fire1"))
            {
                isTakedObject = false;
                if (!hit.transform.gameObject.CompareTag("Grounded"))
                {
                    isTakedObject = false;
                    hitRigidbody.useGravity = true;

                }

            }



        }
        else
        {
            Debug.DrawRay(ray.origin, transform.TransformDirection(Vector3.forward) * 1000, Color.green);



        }

    }
    void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime;

        Vector3 movement = new Vector3(horizontal, 0, vertical);
        transform.Translate(movement * speed);


        float mouseX = Input.GetAxis("Mouse X") * mouseSensivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivity;

        currentRotationY -= mouseY;
        currentRotationY = Mathf.Clamp(currentRotationY, -50f, 50f);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + mouseX, 0);
        Camera.main.transform.localRotation = Quaternion.Euler(currentRotationY, 0f, 0f);

    }
}
