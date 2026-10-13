
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HingeJoint2D))]
public class FlipperController : MonoBehaviour
{
    public KeyCode primaryKey = KeyCode.A;
    public KeyCode secondaryKey = KeyCode.LeftArrow;

    [Header("Motor")]
    public float motorSpeed = 900f;
    public float maxMotorTorque = 10000f;
    public bool flipPositive = true;

    private HingeJoint2D hinge;
    private bool isPressed;

    private void Awake()
    {
        hinge = GetComponent<HingeJoint2D>();
    }

    private void FixedUpdate()
    {
        isPressed =
            Input.GetKey(primaryKey) ||
            Input.GetKey(secondaryKey);

        JointMotor2D motor = hinge.motor;
        motor.maxMotorTorque = maxMotorTorque;

        if (isPressed)
        {
            // กดค้าง: ยก Flipper
            motor.motorSpeed =
                flipPositive ? motorSpeed : -motorSpeed;

            hinge.motor = motor;
            hinge.useMotor = true;
        }
        else
        {
            // ปล่อยปุ่ม: ปิดมอเตอร์ให้ Flipper กลับลง
            hinge.useMotor = false;
        }
    }
}
