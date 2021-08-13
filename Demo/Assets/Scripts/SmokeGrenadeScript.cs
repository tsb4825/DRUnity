using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;

public class SmokeGrenadeScript : Weapon
{
    public float ForceThrow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool Fire()
    {
        transform.parent = null;
        transform.GetComponent<Rigidbody>().AddRelativeForce(Vector3.right * -1 * ForceThrow);
        transform.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * ForceThrow);
        transform.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(12f,0f,20f));
        transform.GetComponent<Rigidbody>().useGravity = true;

        return true;
    }
}
