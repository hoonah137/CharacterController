using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickMe : MonoBehaviour
{
    TpsController _controller;

    void Awake()
    {
        _controller = GetComponentInParent<TpsController>();
    }

    void OnTriggerEnter(Collider collider)
    {
       if(collider.gameObject.tag == "Grabby")
       {
        _controller.objectToGrab = collider.gameObject;
       } 
    }

    void OnTriggerExit(Collider collider)
    {
        _controller.objectToGrab = null;

    }
}
