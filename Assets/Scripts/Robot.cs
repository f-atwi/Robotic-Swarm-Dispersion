using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{   
    private CharacterController controller;
    [SerializeField]
    private float linear_velocity = 0f;
    [SerializeField]
    private float angular_velocity = 0f;

    [SerializeField]
    private LayerMask layerMask;

    public float Linear_velocity { get => linear_velocity; set => linear_velocity = value; }
    public float Angular_velocity { get => angular_velocity; set => angular_velocity = value; }

    public bool wallDetection()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 3f , layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 3, Color.white);
            return false;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Angular_velocity * Time.deltaTime);
        controller.Move(this.transform.forward * Linear_velocity * Time.deltaTime);
    }
}
