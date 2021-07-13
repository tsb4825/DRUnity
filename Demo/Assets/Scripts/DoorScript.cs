using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorScript : Interactable
{
    private bool IsClosed = true;
    public AudioSource DoorOpen;
    public AudioSource DoorClose;

    // Start is called before the first frame update
    void Start()
    {
        var audioClips = this.GetComponents<AudioSource>();
        DoorOpen = audioClips.First();
        DoorClose = audioClips.Skip(1).First();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        if (this.IsClosed)
        {
            this.DoorOpen.Play();
        }
        else
        {
            this.DoorClose.Play();
        }

        this.IsClosed = !this.IsClosed;
    }
}
