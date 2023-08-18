using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionToParent : MonoBehaviour
{
    
    private void FixedUpdate()
    {
        transform.parent.position = transform.position;
        transform.localPosition = Vector3.zero;
    }
}
