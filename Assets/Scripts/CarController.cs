using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{

    public float throttle;
    public float steer;
    public float strengthCoefficient = 20000f;
    public float brakingTorque = 10f;

    public List<WheelCollider> throttleWheels;
    public List<GameObject> steeringWheels;
    public List<WheelCollider> brakingWheels;
    public List<GameObject> wheelMeshes;

    public float maxTurnDegree = 20f;
    public float handBrake;
    public Rigidbody car_rb;

    public Transform cm;


    //public LightingManager lm;

    private void Start()
    {
        car_rb = GetComponent<Rigidbody>();
        car_rb.centerOfMass = cm.position;
    }

    void Update()
    {
        throttle = Input.GetAxis("Vertical");
        steer = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            handBrake = 1;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            handBrake = 0;
        }
    }

    
    void FixedUpdate()
    {
        //car_rb.AddForce(new Vector3(0,0,1f) * 10, ForceMode.Acceleration);
        foreach (WheelCollider wheel in throttleWheels)
        {
            wheel.motorTorque = strengthCoefficient * Time.fixedDeltaTime * throttle;
        }
        foreach (GameObject wheel in steeringWheels)
        {
            wheel.GetComponent<WheelCollider>().steerAngle = maxTurnDegree * steer;
            wheel.transform.localEulerAngles = new Vector3(0f, steer*maxTurnDegree, 0f);
        }
        foreach (WheelCollider wheel in brakingWheels)
        {
            wheel.brakeTorque = brakingTorque * handBrake;
        }

        foreach(GameObject mesh in wheelMeshes)
        {
            mesh.transform.Rotate(car_rb.velocity.magnitude * (transform.InverseTransformDirection(car_rb.velocity).z >=0 ? 1 : -1)/ (2 * Mathf.PI * 0.17f), 0f,0f);
        }
    }
}
