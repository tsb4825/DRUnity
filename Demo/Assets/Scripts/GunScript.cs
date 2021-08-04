using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public Transform _shell;
    public Transform _bullet;
    public float _shellTimeToLive;
    public AudioSource GunShot;

    private float _relativeYPositionOfGunShell = .0592f;
    private Vector3 _relativeBulletPosition = new Vector3(-0.008349f, 0.051924f, -.63695f);
    private float _bulletThrust = 1000f;
    private float _shellThrust = 50f;
    private float _refireTime = 1.2f;
    private float _currentRefireTime = 0;
    private bool _canFire = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var animator = transform.GetComponent<Animator>();
        if (Input.GetMouseButtonDown(0) && _canFire)
        {
            _currentRefireTime = 0;
            _canFire = false;
            animator.SetTrigger("Fire");
            GunShot.Play();
            SpawnShellWithVelocity();
            LaunchBullet();
        }

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
        //var localOffset = new Vector3(0, _relativeYPositionOfGunShell, 0);
        //var worldOffset = transform.rotation * localOffset;
        //var spawnPosition = transform.position + worldOffset;
        //var shell = Instantiate(_shell, spawnPosition, transform.rotation);
        var shell = Instantiate(_shell, transform.position, transform.rotation);
        shell.GetComponent<Rigidbody>().AddRelativeForce(transform.up * _shellThrust * Random.Range(1, 1.8f));
        shell.GetComponent<Rigidbody>().AddRelativeForce(transform.right * _shellThrust * Random.Range(1, 1.8f));
        //StartCoroutine(DestroyObject(shell));
    }

    IEnumerator DestroyObject(Transform shell)
    {
        yield return new WaitForSeconds(_shellTimeToLive);
        Destroy(shell.gameObject);
    }

    void LaunchBullet()
    {
        var bullet = Instantiate(_bullet, transform.position + _relativeBulletPosition, transform.rotation);
        bullet.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * _bulletThrust);
        //StartCoroutine(DestroyObject(bullet));
    }
}
