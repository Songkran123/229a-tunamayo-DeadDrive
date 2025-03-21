using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public float acceleration = 5000f;
    public float maxSpeed = 120f;
    public float turnSpeed = 50f;
    public float dragFactor = 0.1f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        Debug.Log("Center of Mass: " + rb.centerOfMass);
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical"); // W = 1, S = -1
        float turnInput = Input.GetAxis("Horizontal"); // A = -1, D = 1

        // Debug เช็คค่าปุ่ม
        Debug.Log("Move Input: " + moveInput);
        Debug.Log("Turn Input: " + turnInput);
        Vector3 forcePosition = transform.position + transform.TransformPoint(new Vector3(0, -0.5f, -1.5f));

        // การเร่งเครื่องยนต์ (เพิ่ม ForceMode Impulse)
        if (Mathf.Abs(moveInput) > 0.1f)
        {
            rb.AddForce(transform.forward * moveInput * acceleration, ForceMode.Impulse);
        }

        // การเลี้ยวรถ (เพิ่มเงื่อนไขให้หมุนเฉพาะตอนเคลื่อนที่)
        if (Mathf.Abs(turnInput) > 0.1f && rb.velocity.magnitude > 1f)
        {
            rb.AddTorque(Vector3.up * turnInput * turnSpeed, ForceMode.Impulse);
        }

        // ตั้งค่า Drag คงที่
        rb.drag = 0.1f;
    }
}