using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketLock : MonoBehaviour
{
    public GameObject lockedTo;
    public Material lockedColor;

    SocketFunction lockSocketFuncRef;
    Renderer renderer;
    Material baseColor;
    SphereCollider collider;



    // Start is called before the first frame update
    void Start()
    {
        lockSocketFuncRef = lockedTo.GetComponent<SocketFunction>();
        renderer = gameObject.GetComponent<MeshRenderer>();
        collider = gameObject.GetComponent<SphereCollider>();

        baseColor = renderer.material;
        renderer.material = lockedColor;
    }

    // Update is called once per frame
    void Update()
    {
        if(lockSocketFuncRef.socketPlugged)
        {
            renderer.material = baseColor;
            collider.enabled = true;
        }
        else
        {
            renderer.material = lockedColor;
            collider.enabled = false;
        }
    }
}
