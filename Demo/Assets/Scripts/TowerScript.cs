using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    public float ReloadTime;
    public Transform Projectile;
    public float RotationSpeed;
    public float ProjectileSpeed;

    public const float RotationEnemyThreshold = 5;
    private bool _canFire;
    private float _timeReloading;
    private List<Transform> _targets = new List<Transform>();
    private bool _isAimedAtTarget;

    private const string MiniGameEnemy = "MiniGameEnemy";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _targets = _targets.Where(x => x != null).ToList();

        if (_canFire && _targets.Any() && _isAimedAtTarget)
        {
            Fire();
        }

        if (_targets.Any())
        {
            RotateTowardTarget();
        }

        if (!_canFire)
        {
            _timeReloading += Time.deltaTime;
            if (_timeReloading >= ReloadTime)
            {
                _canFire = true;
                _timeReloading = 0;
            }
        }
    }

    private void Fire()
    {
        var projectile = Instantiate(Projectile, transform.position, transform.rotation);
        projectile.GetComponent<Rigidbody>().AddRelativeForce(this.transform.forward * ProjectileSpeed);
        _canFire = false;
    }

    private void RotateTowardTarget()
    {
        _isAimedAtTarget = false;
        Vector3 direction = (_targets.First().transform.position - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(direction).y;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, new Quaternion(transform.localRotation.x, lookRotation, transform.localRotation.z, transform.localRotation.w),
            Time.deltaTime * RotationSpeed);
        if (lookRotation <= RotationEnemyThreshold)
        {
            _isAimedAtTarget = true;
        }
    }

    private void OnTriggerEnter(Collider otherObject)
    {
        if (otherObject.tag == MiniGameEnemy)
        {
            _targets.Add(otherObject.transform);
        }
    }

    private void OnTriggerExit(Collider otherObject)
    {
        if (otherObject.tag == MiniGameEnemy)
        {
            _targets.Remove(otherObject.transform);
        }
    }
}
