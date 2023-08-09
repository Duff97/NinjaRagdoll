using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionToParent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.parent.position = transform.position;
        transform.localPosition = Vector3.zero;
    }
}
