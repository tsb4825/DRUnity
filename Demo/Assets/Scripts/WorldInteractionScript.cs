using UnityEngine;

public class WorldInteractionScript : MonoBehaviour
{
    public float RotationSpeed;

    private bool IsSunRotating;

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
            IsSunRotating = true;
        }

        if (IsSunRotating)
        {
            var sun = GameObject.Find("Sun");

            //sun.transform.Rotate(0, this.RotationSpeed * Time.deltaTime, 0);
            sun.transform.eulerAngles = new Vector3(40, sun.transform.eulerAngles.y + (this.RotationSpeed * Time.deltaTime), 0);
            if (sun.transform.eulerAngles.y >= 340)
            {
                sun.transform.eulerAngles = new Vector3(40, 340, 0);
                IsSunRotating = false;
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
