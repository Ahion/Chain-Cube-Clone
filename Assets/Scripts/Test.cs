using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float angle = 45f;
    public float x = 1f;
    public float y = 1f;
    public float z = 1f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject target = GameObject.Find("target");
            JumpCube(target.transform.position, angle);
            TorqueCube();
        }
    }
    public void JumpCube(Vector3 targetPosition, float angleInDegrees = 45)
    {
        Rigidbody cubeRb = GetComponent<Rigidbody>();

        Vector3 jumpDirection = targetPosition - transform.position;
        Vector3 jumpDirectionXZ = new Vector3(jumpDirection.x, 0f, jumpDirection.z);

        float targetX = jumpDirectionXZ.magnitude;
        float targetY = jumpDirection.y;
        float g = Physics.gravity.y;

        float angleInRadians = angleInDegrees * Mathf.PI / 180;

        float v2 = (g * targetX * targetX) / (2 * (targetY - Mathf.Tan(angleInRadians) * targetX) * Mathf.Pow(Mathf.Cos(angleInRadians), 2)); //speed squared
        float speed = Mathf.Sqrt(Mathf.Abs(v2));

        //calculation of the Y component for the velocity vector
        float jumpY = Mathf.Tan(angleInRadians) * targetX; //knowing the angle and direction, we determine the magnitude of the lift along the Y-axis
        Vector3 jumpDirXYZ = new Vector3(jumpDirection.x, jumpY, jumpDirection.z);

        Vector3 speedVector = jumpDirXYZ.normalized * speed;

        cubeRb.AddForce(speedVector, ForceMode.Impulse);
    }
    public void TorqueCube()
    {
        Rigidbody cubeRb = GetComponent<Rigidbody>();
        Vector3 vector = new Vector3(x, y, z);
        cubeRb.AddTorque(vector * angle, ForceMode.Impulse);
    }
}
