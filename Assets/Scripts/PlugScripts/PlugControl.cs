using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugControl : MonoBehaviour
{
    GameObject cameraRef;
    GameObject controlledPlug;
    GameObject plugLevelController;
    Rigidbody rb;
    public GameObject plugBase;
    public float cableMaxDistance = 7f;


    [HideInInspector]
    public int colorArrIndex;
    [HideInInspector]
    public string[] colorArr = new string[] { "Red", "Green", "Blue"};

    Vector3 initHitPosition;
    GameObject inRangeSocket;
    float initPlugY;
    bool plugged;
    

    // Start is called before the first frame update
    void Start()
    {
        cameraRef = GameObject.FindGameObjectWithTag("MainCamera");
        plugLevelController = GameObject.FindGameObjectWithTag("PlugLevelControl");
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        // Pick the plug
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawLine(cameraRef.transform.position, hit.point, Color.white, 0.5f);
                if (hit.transform.CompareTag("Plug"))
                {
                   if(hit.transform.gameObject == gameObject)
                    {
                        initHitPosition = Input.mousePosition;
                        controlledPlug = hit.transform.gameObject;
                        if (plugged)
                        {
                            unplugFromSocket();
                        }
                        initPlugY = controlledPlug.transform.position.y;
                        controlledPlug.transform.rotation = Quaternion.Euler(-30, 0, 0);
                    }
                }
            }
        }

        // Drag plug
        if (Input.GetMouseButton(0) && controlledPlug != null)
        {

            if (Physics.Raycast(ray, out hit))
            {
               float distance = Vector3.Distance(plugBase.transform.position, hit.point);
                if(distance < cableMaxDistance)
                {
                    controlledPlug.transform.position = new Vector3(hit.point.x, initPlugY, hit.point.z);
                }
            }
        }

        // Put down or plugin
        if (Input.GetMouseButtonUp(0) && controlledPlug != null)
        {
            if(inRangeSocket != null)
            {
                plugToSocket();
            }
            else
            {
                controlledPlug.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            controlledPlug = null;
        }

    }

    private void unplugFromSocket()
    {
        SocketFunction socketF = inRangeSocket.gameObject.GetComponent<SocketFunction>();
        plugLevelController.GetComponent<PlugLevelController>().decreaseConnectedPlugCount();
        setRigidBodyConstraints();

        plugged = false;
        socketF.socketPlugged = false;
    }

    private void plugToSocket()
    {
        SocketFunction socketF = inRangeSocket.gameObject.GetComponent<SocketFunction>();
        controlledPlug.transform.position = new Vector3(inRangeSocket.transform.position.x, controlledPlug.transform.position.y, inRangeSocket.transform.position.z);
        controlledPlug.transform.rotation = Quaternion.Euler(-90, 0, 0);
        //inRangeSocket = null;

        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        plugged = true;
        socketF.socketPlugged = true;
        plugLevelController.GetComponent<PlugLevelController>().incrementConnectedPlugCount();
    }

    private void OnTriggerEnter(Collider other)
    {
       SocketFunction socketF = other.gameObject.GetComponent<SocketFunction>();
       if (colorArr[colorArrIndex] == socketF.colorArr[socketF.colorArrIndex])
        {
            inRangeSocket = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(inRangeSocket != null)
        {
            inRangeSocket = null;
        }
    }

    private void setRigidBodyConstraints()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

}
