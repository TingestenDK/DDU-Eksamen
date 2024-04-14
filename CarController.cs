using UnityEngine;
using TMPro;
using System.Collections;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle;
    private bool isBraking;
    private int currentGear = 1;

    // Settings
    [SerializeField] private float[] gearSpeeds = { 0, 60, 120, 180, 240, 300 };
    [SerializeField] private float[] gearTorques = { 1000, 900, 800, 700, 600, 500 };
    [SerializeField] private float[] gearBrakeTorques = { 20000, 15000, 10000, 9000, 8000, 7000 };
    [SerializeField] private float maxSteerAngle = 30f;
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private Canvas canvas;

    private TextMeshProUGUI speedText;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    private void Start()
    {
        // Find the TextMeshProUGUI element in the Canvas
        speedText = canvas.GetComponentInChildren<TextMeshProUGUI>();

        // Check if speedText is found
        if (speedText == null)
        {
            Debug.LogError("TextMeshProUGUI element not found in the Canvas!");
        }
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        DisplaySpeed();
    }

    private void GetInput()
    {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        if (Input.GetKey(KeyCode.S)) // Check if 'S' key is pressed
        {
            verticalInput = 1; // Set the input to move the car backward
        }
        else if (Input.GetKey(KeyCode.W)) // Check if 'W' key is pressed
        {
            verticalInput = -1; // Set the input to move the car forward
        }
        else
        {
            verticalInput = 0; // No acceleration input
        }

        // Braking Input
        isBraking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        // Limit speed
        if (GetComponent<Rigidbody>().velocity.magnitude * 3.6f > maxSpeed) // Convert m/s to km/h
        {
            verticalInput = 0;
        }

        float currentSpeed = GetComponent<Rigidbody>().velocity.magnitude * 3.6f;

        // Find the current gear
        int newGear = 0;
        for (int i = gearSpeeds.Length - 1; i >= 0; i--)
        {
            if (currentSpeed >= gearSpeeds[i])
            {
                newGear = i + 1;
                break;
            }
        }

        // Change gear if necessary
        if (newGear != currentGear)
        {
            currentGear = newGear;
        }

        // Apply motor force to each wheel collider based on current gear
        float torque = verticalInput * gearTorques[currentGear - 1];
        frontLeftWheelCollider.motorTorque = torque;
        frontRightWheelCollider.motorTorque = torque;
        rearLeftWheelCollider.motorTorque = torque;
        rearRightWheelCollider.motorTorque = torque;

        // Apply braking force
        float currentBrakeForce = isBraking ? gearBrakeTorques[currentGear - 1] : 0f;
        frontRightWheelCollider.brakeTorque = currentBrakeForce;
        frontLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearRightWheelCollider.brakeTorque = currentBrakeForce;

    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void DisplaySpeed()
    {
        if (speedText != null)
        {
            // Calculate speed in km/h
            float speed = GetComponent<Rigidbody>().velocity.magnitude * 3.6f; // m/s to km/h
            speedText.text = "Speed: " + Mathf.Round(speed) + " km/h";
        }
    }
}
