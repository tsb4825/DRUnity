using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightScript : Interactable
{
    private AudioSource LightOn;
    private AudioSource LightHum;
    private AudioSource LightOff;

    private bool IsLightOn = false;

    // Start is called before the first frame update
    void Start()
    {
        var audioClips = this.GetComponents<AudioSource>();
        LightOn = audioClips.First();
        LightHum = audioClips.Skip(1).First();
        LightOff = audioClips.Skip(2).First();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLightOn && !LightOn.isPlaying && !LightHum.isPlaying)
        {
            LightHum.Play();
        }
    }

    public override void Interact()
    {
        if (IsLightOn)
        {
            IsLightOn = false;
            LightHum.Stop();
            LightOff.Play();
            List<Light> lights = new List<Light>();
            lights.AddRange(transform.GetComponentsInChildren<Light>());
            foreach(var light in lights)
            {
                light.enabled = false;
            }
        }
        else
        {
            IsLightOn = true;
            LightOn.Play();
            List<Light> lights = new List<Light>();
            lights.AddRange(transform.GetComponentsInChildren<Light>());
            foreach (var light in lights)
            {
                light.enabled = true;
            }
        }
    }
}
