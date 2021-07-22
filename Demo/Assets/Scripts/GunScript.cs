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
    private float _relativeYPositionOfGunBullet = .637f;
    private float _bulletThrust = 10000f;
    private float _shellThrust = 50f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var animator = transform.GetComponent<Animator>();
        if (Input.GetMouseButtonDown(0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Fire"))
        {
            animator.SetTrigger("Fire");
            GunShot.Play();
            SpawnShellWithVelocity();
            //LaunchBullet();
        }
    }

    void SpawnShellWithVelocity()
    {
        var localOffset = new Vector3(0, _relativeYPositionOfGunShell, 0);
        var worldOffset = transform.rotation * localOffset;
        var spawnPosition = transform.position + worldOffset;
        var shell = Instantiate(_shell, spawnPosition, transform.rotation);
        shell.GetComponent<Rigidbody>().AddRelativeForce(transform.up * _shellThrust * Random.Range(1, 1.8f));
        shell.GetComponent<Rigidbody>().AddRelativeForce(transform.right * _shellThrust * Random.Range(1, 1.8f));
        StartCoroutine(DestroyShell(shell));
    }

    IEnumerator DestroyShell(Transform shell)
    {
        yield return new WaitForSeconds(_shellTimeToLive);
        Destroy(shell.gameObject);
    }

    void LaunchBullet()
    {
        var localOffset = new Vector3(0, _relativeYPositionOfGunShell, 0);
        var worldOffset = transform.rotation * localOffset;
        var spawnPosition = transform.position + (transform.forward * _relativeYPositionOfGunBullet);
        var bullet = Instantiate(_bullet, spawnPosition, transform.rotation);
        //bullet.GetComponent<Rigidbody>().AddRelativeForce(transform.up * _shellThrust);
        //StartCoroutine(DestroyShell(bullet));
    }
}
