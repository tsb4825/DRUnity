using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorScript : Interactable
{
    private bool IsClosed = true;
    private bool ShouldRotate = false;
    private bool PlayedClosedSound = false;
    private AudioSource DoorOpen;
    private AudioSource DoorClose;
    public float RotationSpeed;

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
        if (this.ShouldRotate)
        {
            this.Rotate();
        }
    }

    private void Rotate()
    {
        var transformPivot = transform.parent.parent;
        if (this.IsClosed)
        {
            transformPivot.Rotate(0, this.RotationSpeed * Time.deltaTime * -1, 0);
            if (transformPivot.eulerAngles.y <= 250)
            {
                this.ShouldRotate = false;
                this.IsClosed = false;
            }
        }
        else
        {
            transformPivot.Rotate(0, this.RotationSpeed * Time.deltaTime, 0);
            if (transformPivot.eulerAngles.y >= 320 && !this.PlayedClosedSound)
            {
                this.PlayedClosedSound = true;
                this.DoorClose.Play();
            }
            if (transformPivot.eulerAngles.y <= 3)
            {
                transformPivot.eulerAngles = new Vector3(0, 0, 0);
                this.ShouldRotate = false;
                this.IsClosed = true;
                this.PlayedClosedSound = false;
            }
        }
    }

    public override void Interact()
    {
        //if (!this.ShouldRotate)
        //{
        //    this.ShouldRotate = true;
        //    if (this.IsClosed)
        //    {
        //        this.DoorOpen.Play();
        //    }
        //}
        //var lamp = GameObject.Find("StreetLamp");
        //lamp.GetComponent<LightScript>().Interact();

        //var enemy = GameObject.Find("Enemy");
        //enemy.GetComponent<EnemyScript>().Interact();

        var enemy = GameObject.Find("45 S&W");
        enemy.GetComponent<GunScript>().Interact();
    }
}
