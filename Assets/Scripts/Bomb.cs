using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Cube contactCube = collision.gameObject.GetComponent<Cube>();
        if (contactCube)
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
