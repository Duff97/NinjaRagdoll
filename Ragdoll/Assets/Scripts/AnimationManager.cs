using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;



public class AnimationManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private ThirdPersonCam camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<ThirdPersonCam>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
