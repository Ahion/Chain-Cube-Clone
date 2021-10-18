using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplyBonus : Cube
{
    void Start()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    public override bool ÑompareCollisionCubeNumber(Collision collisionCube)
    {
        Cube contactCube = collisionCube.gameObject.GetComponent<Cube>();
        if (!contactCube)
        {
            //Debug.Log("return false");
            return false;
        }
        base.cubeIndex = contactCube.cubeIndex;
        //Debug.Log("contactCube.cubeIndex " + contactCube.cubeIndex);
        return true;
    }
}
