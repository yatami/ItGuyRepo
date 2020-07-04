using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketMove : MonoBehaviour
{

    public GameObject lockedTo;
    public GameObject moveTo;
    public float moveSpeed = 3f;

    SocketFunction lockSocketFuncRef;
    Vector3 startPos;
    Vector3 unitVector;
    Vector3 sensitivityVector = new Vector3(0.1f, 0.1f, 0.1f);

    // Start is called before the first frame update
    void Start()
    {
        lockSocketFuncRef = lockedTo.GetComponent<SocketFunction>();
        startPos = transform.position;

        unitVector = (moveTo.transform.position - startPos).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (lockSocketFuncRef.socketPlugged)
        {
            unitVector = (moveTo.transform.position - startPos).normalized;
            Debug.Log("difference = " + (transform.position - moveTo.transform.position));
            if ((transform.position - moveTo.transform.position).magnitude > sensitivityVector.magnitude)
            {
                transform.position = transform.position + unitVector * Time.deltaTime * moveSpeed;
            }
        }
        else
        {
            unitVector = (startPos - transform.position).normalized;
            if ((transform.position - startPos).magnitude > sensitivityVector.magnitude)
            {
                transform.position = transform.position + unitVector * Time.deltaTime * moveSpeed;
            }   
        }
    }
}
