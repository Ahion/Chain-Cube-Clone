using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public int cubeIndex;   //cube index in list of cube prefabs
    public GameManager gameManagerScript;
    public GameObject confettiPrefab;

    private float jumpHeight = 3.6f; //cube jump height
    private float force = 10f;   //jump force
    
    public virtual bool ÑompareCollisionCubeNumber(Collision collisionCube)
    {
        MultiplyBonus bonus = collisionCube.gameObject.GetComponent<MultiplyBonus>();
        if (bonus)
        {
            return true;
        }
        Cube contactCube = collisionCube.gameObject.GetComponent<Cube>();
        if (!contactCube || contactCube.cubeIndex != cubeIndex)
        {
            //Debug.Log($"return false {collisionCube.gameObject.name}");
            return false;
        }
        return true;
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (!ÑompareCollisionCubeNumber(collision))
        {
            return;
        }
        //To prevent the code from working for both cubes, I choose the larger one by id.
        int id = GetInstanceID();
        //Debug.Log($"{gameObject.name} {id} ==  {collision.gameObject.name} {collision.gameObject.GetInstanceID()}");
        if (id > collision.gameObject.GetInstanceID())
        {
            //Debug.Log($"success {gameObject.name}: " + id);
            int newCubeIndex = cubeIndex + 1;
            GameObject newCube = gameManagerScript.SpawnCube(gameObject.transform.position, false, newCubeIndex);

            gameManagerScript.allCubes.Remove(collision.gameObject);
            gameManagerScript.allCubes.Remove(gameObject);
            Vector3 cubeToJump = FindTargetCube(newCubeIndex);
            gameManagerScript.allCubes.Add(newCube); // add new cube to list of all cubes
            gameManagerScript.UpdateScore(newCubeIndex);

            Cube cubeScript = newCube.GetComponent<Cube>();
            cubeScript.JumpCube(cubeToJump);
            cubeScript.PlayConfetti(confettiPrefab);

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
    private void JumpCube(Vector3 targetPosition)
    {
        Rigidbody cubeRb = GetComponent<Rigidbody>();
        cubeRb.angularDrag = 2f;    //reduce resistance to rotation

        Vector3 jumpDirection = targetPosition - transform.position;
        Vector3 jumpDirectionXZ = new Vector3(jumpDirection.x, 0f, jumpDirection.z);

        float jumpLength = jumpDirectionXZ.magnitude;
        float g = Mathf.Abs(Physics.gravity.y);

        float ctng = jumpLength / (4 * jumpHeight); //jump angle cotangent
        float sin = 1 / Mathf.Sqrt(Mathf.Pow(ctng, 2) + 1); //translate the cotangent to sine
        float jumpSpeed = Mathf.Sqrt(2 * jumpHeight * g) / sin;

        //calculation of the Y component for the velocity vector
        float jumpY = jumpLength / ctng; //knowing the angle cotangent and direction, we determine the magnitude of the lift along the Y-axis
        Vector3 jumpDirXYZ = new Vector3(jumpDirection.x, jumpY, jumpDirection.z);

        Vector3 speedVector = jumpDirXYZ.normalized * jumpSpeed;
        cubeRb.AddForce(speedVector, ForceMode.Impulse);
        cubeRb.AddTorque(Vector3.left * force, ForceMode.Impulse);

    }
    private Vector3 FindTargetCube(int number)
    {
        GameObject targetCube = null;
        foreach (GameObject cube in gameManagerScript.allCubes)
        {
            if (cube && cube.GetComponent<Cube>().cubeIndex == number)
            {
                targetCube = cube;
                break;
            }
        }
        if (targetCube)
        {
            //Debug.Log("Target Cube: " + targetCube.name);
            return targetCube.transform.position;
        }
        else
        {
            float z = 5.9f;
            Vector3 backLine = transform.position;
            backLine.z = z;
            return backLine;
        } 
    }
    private void PlayConfetti(GameObject confetti)
    {
        Material cubeMat = GetComponent<Renderer>().material;
        confetti.GetComponent<ParticleSystemRenderer>().material = cubeMat;
        GameObject effect = Instantiate(confetti, transform.position, confetti.transform.rotation);
        Destroy(effect, 3f);
    }
}
