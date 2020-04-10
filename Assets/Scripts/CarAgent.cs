using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CarAgent : Agent
{
    public GameObject ground;

    [HideInInspector]
    public Bounds areaBounds;


    /// <summary>
    /// The goal to push the block to.
    /// </summary>
    public GameObject parking;

    public GameObject area;

    [HideInInspector]
    public ParkingArea areaSettings;

    /// <summary>
    /// Detects when the block touches the outside
    /// </summary>
    [HideInInspector]
    public ParkingDetect parkingDetect;

    /// <summary>
    /// Detects when the block touches the outside
    /// </summary>
    [HideInInspector]
    public ObstacleDetect obstacleDetect;

    public bool useVectorObs;

    CarSetting carSetting;

    Rigidbody m_CarRb;  //cached on initialization

    //list of wheels and meshes
    public List<WheelCollider> throttleWheels;
    public List<GameObject> steeringWheels;
    public List<WheelCollider> brakingWheels;
    public List<GameObject> wheelMeshes;

    public float brakingTorque = 10f;
    public float maxTurnDegree = 20f;
    public float handBrake;

    public float strengthCoefficient = 20000f;


    void Awake()
    {
        carSetting = FindObjectOfType<CarSetting>();
        areaSettings = FindObjectOfType<ParkingArea>();
    }

    public override void Initialize()
    {
        m_CarRb = GetComponent<Rigidbody>();

        areaBounds = ground.GetComponent<Collider>().bounds;

        //SetResetParameters();
    }
    public Vector3 GetRandomSpawnPos()
    {
        var foundNewSpawnLocation = false;
        var randomSpawnPos = Vector3.zero;
        while (foundNewSpawnLocation == false)
        {
            var randomPosX = Random.Range(-areaBounds.extents.x * carSetting.spawnAreaMarginMultiplier,
                areaBounds.extents.x * carSetting.spawnAreaMarginMultiplier);

            var randomPosZ = Random.Range(-areaBounds.extents.z * carSetting.spawnAreaMarginMultiplier,
                areaBounds.extents.z * carSetting.spawnAreaMarginMultiplier);
            randomSpawnPos = ground.transform.position + new Vector3(randomPosX, 1f, randomPosZ);
            if (Physics.CheckBox(randomSpawnPos, new Vector3(2.5f, 0.01f, 2.5f)) == false)
            {
                foundNewSpawnLocation = true;
            }
        }
        return randomSpawnPos;
    }


    public void hitACar()
    {
        // punish the agent
        AddReward(-0.5f);
        //EndEpisode();
    }
    public void hitAWall()
    {
        // punish the agent
        AddReward(-0.1f);
    }

    public void parked()
    {
        // reward the agent
        AddReward(1f);
        EndEpisode();
    }

    public void MoveAgent(float[] act)
    {
        var steer = act[0];
        var throttle = act[1];

        foreach (WheelCollider wheel in throttleWheels)
        {
            wheel.motorTorque = strengthCoefficient * Time.fixedDeltaTime * throttle;
        }
        foreach (GameObject wheel in steeringWheels)
        {
            wheel.GetComponent<WheelCollider>().steerAngle = maxTurnDegree * steer;
            wheel.transform.localEulerAngles = new Vector3(0f, steer * maxTurnDegree, 0f);
        }
    }

    //Move agent for discrete actions
    /*
     public void MoveAgent(float[] act)
    {
        
        //act array of floats has forward/backward in position 0, turning in position 1, handbrake in position 2
        var throttle= act[0];
        var steer = act[1];
        //var handBrake = act[2];
        // when throttle is 1 move forward and when it is two move back
        if(throttle == 1f)
        {
            foreach (WheelCollider wheel in throttleWheels)
            {
                wheel.motorTorque = strengthCoefficient * Time.fixedDeltaTime * 1f;
            }
        }
        if(throttle == 2f)
        {
            foreach (WheelCollider wheel in throttleWheels)
            {
                wheel.motorTorque = strengthCoefficient * Time.fixedDeltaTime * -1;
            }
        }
        //when steer is one turn wheels left and when it is 2 turn right
        if(steer == 0f)
        {
            foreach (GameObject wheel in steeringWheels)
            {
                wheel.GetComponent<WheelCollider>().steerAngle = 0;
                wheel.transform.localEulerAngles = new Vector3(0f, 0f * maxTurnDegree, 0f);
            }
        }
        if(steer == 1f)
        {
            foreach (GameObject wheel in steeringWheels)
            {
                wheel.GetComponent<WheelCollider>().steerAngle = maxTurnDegree * 1;
                wheel.transform.localEulerAngles = new Vector3(0f, 1 * maxTurnDegree, 0f);
            }
        }
        if (steer == 2f)
        {
            foreach (GameObject wheel in steeringWheels)
            {
                wheel.GetComponent<WheelCollider>().steerAngle = maxTurnDegree * -1;
                wheel.transform.localEulerAngles = new Vector3(0f, -1 * maxTurnDegree, 0f);
            }
        }
        //when handbrake is 1 brake and when it is 2 release
        /*if(handBrake == 1f)
        {
            foreach (WheelCollider wheel in brakingWheels)
            {
                wheel.brakeTorque = brakingTorque * 1f;
            }
        }
        if(handBrake == 2f)
        {
            foreach (WheelCollider wheel in brakingWheels)
            {
                wheel.brakeTorque = brakingTorque * 0;
            }
        }
    }
*/
    private void FixedUpdate()
    {
        //visual turning of the wheels
        foreach (GameObject mesh in wheelMeshes)
        {
            mesh.transform.Rotate(m_CarRb.velocity.magnitude * (transform.InverseTransformDirection(m_CarRb.velocity).z >= 0 ? 1 : -1) / (2 * Mathf.PI * 0.17f), 0f, 0f);
        }
    }

    /// <summary>
    /// Called every step of the engine. Here the agent takes an action.
    /// </summary>
    public override void OnActionReceived(float[] vectorAction)
    {
        // Move the agent using the action.
        MoveAgent(vectorAction);

        // Penalty given each step to encourage agent to finish task quickly.
        AddReward(-1f / maxStep);
    }


    public override float[] Heuristic()
    {
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        //return action;


        /*var action = new float[3];

        // left right
        if (Input.GetKeyDown(KeyCode.D))
        {
            action[1] =  1f ; 
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            action[1] = 0f;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            action[1] = 2f;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            action[1] = 0f;
        }
        //forward backward
        if (Input.GetKey(KeyCode.W))
        {
            action[0] = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            action[0] = 2f;
        }*/
        // handbrake pressed or released
        /* if (Input.GetKeyDown(KeyCode.Space))
         {
             action[2] = 1f;
         }
         if (Input.GetKeyUp(KeyCode.Space))
         {
             action[2] = 2f;
         }*/
        return action;
    }
    
    /// <summary>
    /// In the editor, if "Reset On Done" is checked then AgentReset() will be
    /// called automatically anytime we mark done = true in an agent script.
    /// </summary>
    public override void OnEpisodeBegin()
    {
        var rotation = Random.Range(0, 4);
        var rotationAngle = rotation * 90f;
        area.transform.Rotate(new Vector3(0f, rotationAngle, 0f));

        areaSettings.resetArea();


        transform.position = GetRandomSpawnPos();

        m_CarRb.velocity = Vector3.zero;
        
        m_CarRb.angularVelocity = Vector3.zero;

        //removing torque from the wheels
        foreach (WheelCollider wheel in throttleWheels)
        {
            wheel.motorTorque = 0f;
        }
        //removing steer angle from the wheels
        foreach (GameObject wheel in steeringWheels)
        {
            wheel.GetComponent<WheelCollider>().steerAngle  = 0f;
            wheel.transform.localEulerAngles = new Vector3(0f, 0f * maxTurnDegree, 0f);
        }
        //removing handbrake
        /*foreach (WheelCollider wheel in brakingWheels)
        {
            wheel.brakeTorque = brakingTorque * 0;
        }*/
    }

}
