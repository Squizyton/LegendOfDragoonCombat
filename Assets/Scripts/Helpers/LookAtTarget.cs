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
    
    private void Update()
    {
        var lookPos = target.position - transform.position;
        lookPos.y = 0;
        
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 20);
    }
}
