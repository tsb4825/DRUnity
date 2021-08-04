using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightScript : Interactable
{
    private int MaxIntensity = 280;
    private float IntensityChange = 2f;
    private float CurrentIntensity;

    private bool IsTurningOnLights;
    private bool IsTurningOffLights;

    private AudioSource LightOn;
    private AudioSource LightHum;
    private AudioSource LightOff;

    private bool IsLightOn = false;

    // Start is called before the first frame update
    void Start()
    {

        var streetLamp = GameObject.Find("StreetLamp").transform;
        var audioClips = streetLamp.GetComponents<AudioSource>();
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

        if (this.IsTurningOnLights)
        {
            CurrentIntensity += IntensityChange;
            if (CurrentIntensity >= MaxIntensity)
            {
                CurrentIntensity = MaxIntensity;
                this.IsTurningOnLights = false;
            }
        }

        if (this.IsTurningOffLights)
        {
            CurrentIntensity -= IntensityChange;
            if (CurrentIntensity <= 0)
            {
                CurrentIntensity = 0;
                this.IsTurningOffLights = false;
            }
        }

        if (this.IsTurningOnLights || this.IsTurningOffLights)
        {
            var streetLamp = GameObject.Find("StreetLamp").transform;
            List<Light> lights = new List<Light>();
            lights.AddRange(streetLamp.GetComponentsInChildren<Light>());
            foreach (var light in lights)
            {
                light.intensity = CurrentIntensity;
            }
        }
    }

    public override void Interact()
    {
        if (IsLightOn && !IsTurningOffLights && !IsTurningOnLights)
        {
            IsLightOn = false;
            LightHum.Stop();
            LightOff.Play();
            IsTurningOffLights = true;
        }
        else if (!IsTurningOffLights && !IsTurningOnLights)
        {
            IsLightOn = true;
            LightOn.Play();
            IsTurningOnLights = true;
        }
    }
}
