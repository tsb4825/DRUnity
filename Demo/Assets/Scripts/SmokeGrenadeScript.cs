using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;

public class SmokeGrenadeScript : Weapon
{
    public float ForceThrow;
    public float TimeToLiveOnceThrown;
    public float TimeToSmokeOnceActivated;
    public float TimeToSmokeOnceThrown;

    private float _currentTimeSinceThrown;
    private float _currentTimeSmokeActivated;
    private bool _hasFired;

    private bool _isSmoking;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_hasFired)
        {
            _currentTimeSinceThrown += Time.deltaTime;
        }

        if (_currentTimeSinceThrown > TimeToSmokeOnceThrown && _currentTimeSmokeActivated < TimeToSmokeOnceActivated)
        {
            _currentTimeSmokeActivated += Time.deltaTime;
            if (!_isSmoking)
            {
                _isSmoking = true;
                var smoke = transform.GetComponent<ParticleSystem>();
                smoke.transform.rotation = new Quaternion(0, 0, 0, 0);
                smoke.Play();
            }
        }

        if (_currentTimeSinceThrown > TimeToSmokeOnceThrown && _currentTimeSmokeActivated > TimeToSmokeOnceActivated)
        {
            var smoke = transform.GetComponent<ParticleSystem>();
            smoke.Stop();
        }
    }

    public override bool Fire()
    {
        _hasFired = true;
        transform.parent = null;
        transform.GetComponent<Rigidbody>().AddRelativeForce(Vector3.right * -1 * ForceThrow);
        transform.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * ForceThrow);
        transform.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(12f,0f,20f));
        transform.GetComponent<Rigidbody>().useGravity = true;

        StartCoroutine(DestroyObject(transform));

        return true;
    }

    private IEnumerator DestroyObject(Transform gameTransform)
    {
        yield return new WaitForSeconds(TimeToLiveOnceThrown);
        Destroy(gameTransform.gameObject);
    }
}
