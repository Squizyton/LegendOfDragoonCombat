using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    
    public Transform target;

    public void Intialize(Transform target)
    {
        this.target = target;
    }
    
    void Update()
    {
        transform.LookAt(Camera.main.transform.position * -1);
    }
}
