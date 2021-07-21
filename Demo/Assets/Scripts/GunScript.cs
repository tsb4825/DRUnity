using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public Transform _shell;
    public float _shellTimeToLive;

    private float _relativeYPositionOfGun = .0592f;
    private float _shellThrust = 50f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            transform.GetComponent<Animator>().SetTrigger("Fire");
            // play sound
            SpawnShellWithVelocity();
            LaunchBullet();
        }
    }

    void SpawnShellWithVelocity()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + _relativeYPositionOfGun, transform.position.z);
        var shell = Instantiate(_shell, spawnPosition, transform.rotation);
        shell.GetComponent<Rigidbody>().AddForce(0, _shellThrust * Random.Range(1, 1.8f), _shellThrust * Random.Range(1, 1.8f));
        StartCoroutine(DestroyShell(shell));
    }

    IEnumerator DestroyShell(Transform shell)
    {
        yield return new WaitForSeconds(_shellTimeToLive);
        Destroy(shell.gameObject);
    }

    void LaunchBullet()
    {

    }
}
