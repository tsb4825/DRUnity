using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GunScript : Weapon
{
    public Transform _shell;
    public Transform _bullet;
    public float _shellTimeToLive;
    private AudioSource _gunShot;

    private float _relativeYPositionOfGunShell = .0592f;
    private Vector3 _relativeBulletPosition = new Vector3(0, .0592f, 0);
    private float _bulletThrust = 1000f;
    private float _shellThrust = 50f;
    private float _refireTime = 1.2f;
    private float _currentRefireTime = 0;
    public bool _canFire = true;

    void Awake()
    {
        _canFire = true;
        _gunShot = transform.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!_canFire)
        {
            _currentRefireTime += Time.deltaTime;
            if (_currentRefireTime >= _refireTime)
            {
                _canFire = true;
            }
        }
    }

    void SpawnShellWithVelocity()
    {
        var shell = Instantiate(_shell, new Vector3(transform.position.x, transform.position.y + _relativeYPositionOfGunShell, transform.position.z), transform.rotation);
        shell.GetComponent<Rigidbody>().AddRelativeForce(transform.up * _shellThrust * Random.Range(1, 1.8f));
        Vector3 rightForce = Vector3.forward * _shellThrust * Random.Range(1, 1.8f);
        shell.GetComponent<Rigidbody>().AddRelativeForce(rightForce);
        StartCoroutine(DestroyObject(shell));
    }

    IEnumerator DestroyObject(Transform shell)
    {
        yield return new WaitForSeconds(_shellTimeToLive);
        Destroy(shell.gameObject);
    }

    void SpawnBulletWithVelocity()
    {
        var bulletPosition = new Vector3(transform.position.x + _relativeBulletPosition.x, transform.position.y + _relativeBulletPosition.y, transform.position.z + _relativeBulletPosition.z);
        bulletPosition = bulletPosition + (transform.right * -1 *.65f);
        var bullet = Instantiate(_bullet, bulletPosition, transform.rotation);
        bullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.right * -1 * _bulletThrust);
        StartCoroutine(DestroyObject(bullet));
    }

    public override bool Fire()
    {
        var animator = transform.GetComponent<Animator>(); 
        if (_canFire)
        {
            _currentRefireTime = 0;
            _canFire = false;
            animator.SetTrigger("Fire");
            _gunShot.Play();
            SpawnShellWithVelocity();
            SpawnBulletWithVelocity();
        }

        return false;
    }
}
