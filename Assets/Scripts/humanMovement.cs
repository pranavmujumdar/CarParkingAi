using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class humanMovement : MonoBehaviour
{

    public Rigidbody rb;
    public float minSpeed = 6f;
    public float maxSpeed = 12f;
    private float speed = 1f;

    public Animator ani;

    private Vector3 startPos;

    public bool execute_walking;
    public bool execute_running;

    private float t;
    [SerializeField] private float clearance = 16f;
    //[SerializeField] private float rightClearance = -16f;
    private Vector3 pointA;
    private Vector3 pointB;

    // public Transform obstacleDetection;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        startPos = transform.position;
        pointA = new Vector3((startPos.x+clearance), startPos.y, startPos.z);
        pointB = new Vector3((startPos.x-clearance), startPos.y, startPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime * speed;
        
        // Moves the object to target position
        transform.position = Vector3.Lerp(pointA, pointB, t);

        // Flip the points once it has reached the target
        if (t >= 1)
        {
            var b = pointB;
            var a = pointA;

            pointA = b;
            pointB = a;

            t = 0;
            ani.SetInteger("arms", 1);
            ani.SetInteger("legs", 1);
        }
    }
}
