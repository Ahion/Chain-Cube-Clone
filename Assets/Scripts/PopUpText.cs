using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpText : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * 20 * Time.deltaTime);
    }
    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
