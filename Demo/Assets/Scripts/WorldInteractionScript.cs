using UnityEngine;
using System;

public class WorldInteractionScript : MonoBehaviour
{
    public float RotationSpeed;
    public Material NightMaterial;
    public Material DayMaterial;

    private bool IsSunRotating;
    private bool IsSunSetting = true;
    private Vector3 SunRotation = new Vector3(40, 130, 0);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetInteraction();
        }

        if (Input.GetKeyUp(KeyCode.F1))
        {
            if (!IsSunRotating && !IsSunSetting)
            {
                RenderSettings.skybox = DayMaterial;
                var sun = GameObject.Find("Sun");
                SunRotation = new Vector3(0, 130, 0);
                sun.transform.eulerAngles = SunRotation;
                sun.transform.GetComponent<Light>().intensity = 1.2f;
            }

            IsSunRotating = true;
        }

        if (IsSunRotating)
        {
            if (IsSunSetting)
            {
                var sun = GameObject.Find("Sun");

                SunRotation.x += this.RotationSpeed * Time.deltaTime;
                sun.transform.eulerAngles = SunRotation;
                if (SunRotation.x >= 200)
                {
                    IsSunRotating = false;
                    IsSunSetting = false;
                    sun.transform.GetComponent<Light>().intensity = .5f;
                    RenderSettings.skybox = NightMaterial;
                    sun.transform.eulerAngles = new Vector3(90, 130, 0);
                }
            }
            else
            {
                var sun = GameObject.Find("Sun");
                SunRotation.x += this.RotationSpeed * Time.deltaTime;
                sun.transform.eulerAngles = SunRotation;
                if (SunRotation.x >= 40)
                {
                    IsSunRotating = false;
                    IsSunSetting = true;
                    sun.transform.eulerAngles = new Vector3(40, 130, 0);
                }
            }
        }
    }

    void GetInteraction()
    {
        Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit interactionInfo;
        if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity))
        {
            GameObject interactedObject = interactionInfo.collider.gameObject;
            if (interactedObject.tag == "Interactable Object")
            {
                interactedObject.GetComponent<Interactable>().Interact();
            }
        }
    }
}
