using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellScript : MonoBehaviour
{
    private AudioSource ShellHit;

    // Start is called before the first frame update
    void Start()
    {
        ShellHit = transform.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter()
    {
        ShellHit.Play();
    }
}
