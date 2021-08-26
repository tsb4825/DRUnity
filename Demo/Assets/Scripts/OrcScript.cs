using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrcScript : MonoBehaviour
{
    public float WalkSpeed;
    public float Hitpoints;
    public float GoldAwarded;

    private List<Vector3> _wayPoints;
    private int _currentWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        _wayPoints = new List<Vector3>
        {
            GameObject.Find("Waypoint1").transform.position,
            GameObject.Find("Waypoint2").transform.position,
            GameObject.Find("Waypoint3").transform.position,
            GameObject.Find("Waypoint4").transform.position,
            GameObject.Find("Waypoint5").transform.position,
        };
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _wayPoints.Skip(_currentWaypoint).First(), WalkSpeed * Time.deltaTime);
        if (transform.position == _wayPoints.Skip(_currentWaypoint).First())
        {
            _currentWaypoint += 1;
            Vector3 direction = _wayPoints.Skip(_currentWaypoint).First() - transform.position;
            Vector3 eulerRotation = transform.localEulerAngles;
            transform.rotation = new Quaternion(Quaternion.Euler(direction).x, eulerRotation.y, eulerRotation.z, 0f);
            Debug.Break();
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
        }
    }
}
