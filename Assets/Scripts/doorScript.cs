using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour
{
    
    public bool unlockedDoor = false;
    public bool openDoor = false;

    public Quaternion closedRot;
    public Quaternion openRot;
    public Vector3 closedPos;
    public Vector3 openPos;
    // Start is called before the first frame update
    void Start()
    {
        closedRot = transform.rotation;
        openRot = Quaternion.Euler(0,-90, 0);

        closedPos = transform.position;
        openPos = new Vector3(-5, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(openDoor)
        {
            //transform.rotation = Quaternion.Lerp(transform.rotation, openRot, Time.deltaTime);
            transform.position = openPos;
        }
        
    }
}


