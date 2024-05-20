using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Mechanich : MonoBehaviour
{

    Rigidbody rb;
    public float speed;
    public float mouseSensivity;
    public Image cursor;
    float currentRotationY = 0f;
    GameObject hitObject;
    Transform fHitObjectR;
    Vector3 rayOriginY;
    bool isTakenObject;
    public static bool isTakedObject;
    bool isGrounded;
    string layerName;



    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        isTakenObject = false;
        isTakedObject = false;
        hitObject = null;
        layerName = "Grounded";
    }


    void Update()
    {
        Movement();
        ObjectTransform();
    }
    public void ObjectTransform()
    {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int layerIndex = LayerMask.NameToLayer(layerName);
        int layerMask = ~(1 << layerIndex);
        Image cursorImage = cursor.GetComponent<Image>();

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {

            Debug.DrawRay(ray.origin, hit.transform.position * hit.distance, Color.red);

            Rigidbody hitRigidbody = hit.transform.gameObject.GetComponent<Rigidbody>();
            GameObject pivot = GameObject.FindGameObjectWithTag("Pivot");
            GameObject hitTransform = hit.transform.gameObject;
            rayOriginY = new Vector3(hit.point.x, ray.origin.y, hit.point.z);


            if (Vector3.Distance(ray.origin, hit.point) <= 2f)
            {
                // Taked
                if (layerName == "Grounded")
                {
                    if (hitObject != null && hitObject != hitTransform)
                    {
                        hitObject.GetComponent<Outline>().enabled = false;
                        cursorImage.color = Color.white;
                    }
                    hitTransform.GetComponent<Outline>().enabled = true;
                    cursorImage.color = Color.red;
                    hitObject = hitTransform;
                }
                if (Input.GetKeyDown(KeyCode.E) && !isTakedObject)
                {
                    isTakenObject = !isTakenObject;
                    if (isTakenObject)
                    {
                        hitObject = hitTransform;
                        hitObject.SetActive(false);
                        fHitObjectR = hitObject.transform;
                        hitObject.transform.rotation = fHitObjectR.rotation;
                        layerName = ".";
                    }
                    if (!isTakenObject && hitObject != null)
                    {

                        hitObject.transform.position = rayOriginY;
                        if (Input.GetKey(KeyCode.LeftShift))
                            fHitObjectR.rotation = hitObject.transform.rotation;
                        else
                            hitObject.transform.rotation = Quaternion.Euler(Camera.main.transform.forward);
                        hitObject.SetActive(true);
                        hitObject = null;
                        fHitObjectR = null;
                        layerName = "Grounded";
                    }
                }
                //Hold

                if (Input.GetButtonDown("Fire1") && !isTakenObject)
                    isTakedObject = true;
                if (Input.GetButton("Fire1") && isTakedObject && !isTakenObject && hitObject != null)
                {
                    hitObject = hitTransform;
                    hitRigidbody.MovePosition(pivot.transform.position);
                    hitRigidbody.MoveRotation(Quaternion.LookRotation(Camera.main.transform.forward * Time.deltaTime));
                    hitRigidbody.useGravity = false;
                    hitRigidbody.isKinematic = true;
                    hitRigidbody.isKinematic = false;
                    hitObject.GetComponent<Outline>().enabled = false;
                    cursorImage.color = Color.white;
                    if (Input.GetKeyDown(KeyCode.LeftShift))
                    {
                        hitObject.GetComponent<Rigidbody>().velocity += Camera.main.transform.forward * 10;
                        hitObject.GetComponent<Rigidbody>().useGravity = true;
                        isTakedObject = false;

                    }
                }

                if (Input.GetButtonUp("Fire1"))
                {
                    hitRigidbody.useGravity = true;
                    if (hitObject != null)
                    {
                        hitObject.GetComponent<Outline>().enabled = true;
                        cursorImage.color = Color.red;
                    }
                }

            }
            else
            {
                if (layerName == "Grounded")
                {
                    hitTransform.GetComponent<Outline>().enabled = false;
                    cursorImage.color = Color.white;
                }

            }







        }
        else
        {
            Debug.DrawRay(ray.origin, transform.TransformDirection(Vector3.forward) * 1000, Color.green);
            if (hitObject != null)
            {
                hitObject.GetComponent<Outline>().enabled = false;
                cursorImage.color = Color.white;
            }


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

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity += new Vector3(0, 5, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

}
