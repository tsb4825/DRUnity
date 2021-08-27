using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrcScript : MonoBehaviour
{
    public float WalkSpeed;
    public float Hitpoints;
    public float GoldAwarded;

    private List<Vector3> _wayPoints;
    private List<float> _rotations = new List<float>{0f, 55f, -70f, 0f};
    private int _currentWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        _wayPoints = new List<Vector3>
        {
            GameObject.Find("Waypoint1").transform.localPosition,
            GameObject.Find("Waypoint2").transform.localPosition,
            GameObject.Find("Waypoint3").transform.localPosition,
            GameObject.Find("Waypoint4").transform.localPosition,
            GameObject.Find("Waypoint5").transform.localPosition,
        };
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, _wayPoints.Skip(_currentWaypoint).First(), WalkSpeed * Time.deltaTime);
        if (transform.localPosition == _wayPoints.Skip(_currentWaypoint).First())
        {
            _currentWaypoint += 1;
            Vector3 direction = Vector3.RotateTowards(transform.localPosition, _wayPoints.Skip(_currentWaypoint).First(), 999f,999f);
            Vector3 eulerRotation = transform.localEulerAngles;
            eulerRotation.x = _rotations.Skip(_currentWaypoint - 1).First();
            transform.localRotation = Quaternion.Euler(eulerRotation);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "MinigameProjectile")
        {
            //float damage = collider.GetComponent<MinigameProjectileScript>().DamageHitAmount;
            //Hitpoints -= damage;
            if (Hitpoints <= 0)
            {
                //GameObject.Find("MiniGameBoard").GetComponent<MiniGameScript>().AwardGold(GoldAwarded);
                Destroy(transform.gameObject);
            }
        }

        if (collider.gameObject.tag == "MinigameHome")
        {

            //GameObject.Find("MiniGameBoard").GetComponent<MiniGameScript>().BaseHit();
            Destroy(transform.gameObject);
        }
    }
}
