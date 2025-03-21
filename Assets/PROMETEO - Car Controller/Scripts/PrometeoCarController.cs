using System;
using UnityEngine;

public class PrometeoCarController : MonoBehaviour
{
    public int maxSpeed = 90;
    public int maxReverseSpeed = 30;
    public int accelerationMultiplier = 2;
    public int maxSteeringAngle = 27;
    public float steeringSpeed = 0.5f;
    public int brakeForce = 350;
    public Vector3 bodyMassCenter;

    public WheelCollider frontLeftCollider;
    public WheelCollider frontRightCollider;
    public WheelCollider rearLeftCollider;
    public WheelCollider rearRightCollider;

    private Rigidbody carRigidbody;
    private float steeringAxis;
    private float throttleAxis;
    private float localVelocityZ;

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = bodyMassCenter;
        carRigidbody.angularDrag = 10.0f;
        carRigidbody.drag = 0.5f;

        AdjustWheelFriction(frontLeftCollider, 2.5f);
        AdjustWheelFriction(frontRightCollider, 2.5f);
        AdjustWheelFriction(rearLeftCollider, 3.5f);
        AdjustWheelFriction(rearRightCollider, 3.5f);
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        localVelocityZ = transform.InverseTransformDirection(carRigidbody.velocity).z;

        if (moveInput > 0)
        {
            GoForward();
        }
        else if (moveInput < 0)
        {
            if (localVelocityZ > 1f) // รถยังวิ่งไปข้างหน้า → เบรก
            {
                Brakes();
            }
            else
            {
                GoReverse(); // รถหยุดหรือถอย → ถอยหลัง
            }
        }
        else
        {
            ThrottleOff(); // ปล่อยคันเร่ง
        }

        if (turnInput > 0)
            TurnRight();
        else if (turnInput < 0)
            TurnLeft();
        else
            ResetSteeringAngle();
    }

    void Update()
    {
        float speedFactor = carRigidbody.velocity.magnitude / (maxSpeed / 3.6f);
        carRigidbody.drag = Mathf.Lerp(0.1f, 2.5f, speedFactor);
        carRigidbody.drag = 0.5f;
        carRigidbody.angularDrag = Mathf.Clamp(carRigidbody.angularVelocity.magnitude * 0.1f, 1.0f, 4.0f);
    }

    public void GoForward()
    {
        float force = accelerationMultiplier * carRigidbody.mass;
        float maxVelocity = maxSpeed / 3.6f;

        if (carRigidbody.velocity.magnitude < maxVelocity)
        {
            ApplyTorque(force);
        }
        else
        {
            StopTorque();
        }

        ClearBrake();
    }

    public void GoReverse()
    {
        float force = accelerationMultiplier * carRigidbody.mass;
        float maxVelocity = maxReverseSpeed / 3.6f;

        if (carRigidbody.velocity.magnitude < maxVelocity)
        {
            ApplyTorque(-force);
        }
        else
        {
            StopTorque();
        }

        ClearBrake();
    }

    public void ApplyTorque(float force)
    {
        frontLeftCollider.motorTorque = force;
        frontRightCollider.motorTorque = force;
        rearLeftCollider.motorTorque = force;
        rearRightCollider.motorTorque = force;
    }

    public void StopTorque()
    {
        frontLeftCollider.motorTorque = 0;
        frontRightCollider.motorTorque = 0;
        rearLeftCollider.motorTorque = 0;
        rearRightCollider.motorTorque = 0;
    }

    public void ClearBrake()
    {
        frontLeftCollider.brakeTorque = 0;
        frontRightCollider.brakeTorque = 0;
        rearLeftCollider.brakeTorque = 0;
        rearRightCollider.brakeTorque = 0;
    }

    public void Brakes()
    {
        StopTorque();
        float brakePower = brakeForce * carRigidbody.mass;

        frontLeftCollider.brakeTorque = brakePower;
        frontRightCollider.brakeTorque = brakePower;
        rearLeftCollider.brakeTorque = brakePower;
        rearRightCollider.brakeTorque = brakePower;
    }

    public void ThrottleOff()
    {
        StopTorque();
        carRigidbody.velocity *= 0.98f;
    }

    public void TurnLeft()
    {
        steeringAxis = Mathf.Clamp(steeringAxis - (Time.deltaTime * 3f * steeringSpeed), -1f, 1f);
        float angle = steeringAxis * maxSteeringAngle;
        frontLeftCollider.steerAngle = angle;
        frontRightCollider.steerAngle = angle;
    }

    public void TurnRight()
    {
        steeringAxis = Mathf.Clamp(steeringAxis + (Time.deltaTime * 3f * steeringSpeed), -1f, 1f);
        float angle = steeringAxis * maxSteeringAngle;
        frontLeftCollider.steerAngle = angle;
        frontRightCollider.steerAngle = angle;
    }

    public void ResetSteeringAngle()
    {
        steeringAxis = Mathf.Lerp(steeringAxis, 0, Time.deltaTime * 5f);
        float angle = steeringAxis * maxSteeringAngle;
        frontLeftCollider.steerAngle = angle;
        frontRightCollider.steerAngle = angle;
    }

    void AdjustWheelFriction(WheelCollider wheel, float stiffness)
    {
        WheelFrictionCurve friction = wheel.sidewaysFriction;
        friction.stiffness = stiffness;
        wheel.sidewaysFriction = friction;
    }
}
