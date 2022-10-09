using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bullet;
    public float power = 1500f;
    // public float moveSpeed = 2f;

    // Update is called once per frame
    void Update()
    {
        // float h = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed; 
        // float v = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        // transform.Translate(h,v,0);

        if (Input.GetButtonUp("Fire1")) 
        { 
            GameObject instance = Instantiate (bullet, transform.position,
            transform.rotation) as GameObject; 
            Vector3 fwd = transform.TransformDirection (Vector3.forward); 
            instance.GetComponent<Rigidbody>().AddForce (fwd * power); 
        }
    }
}
// When looking down no longer shoots from middle of screen