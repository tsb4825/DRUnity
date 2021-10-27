using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float PlayerSpeed;
    public float LookSensitivity;

    private float modifiedPlayerSpeed;
    public List<Transform> Weapons;
    private Transform _activeWeapon;
    private bool _canSwapWeapons = true;
    private float _currentSwapWeaponTime;
    private float _allowSwapWeaponTime = 1f;
    private int _currentWeaponIndex;
    private float _currentRespawnWeaponTime;
    private float _allowRespawnWeaponTime = 1.5f;
    private bool _shouldRespawnWeapon;
    private bool _canFire = true;

    private GameObject Weapon;
    
    public bool InMiniGame = false;

    // Start is called before the first frame update
    void Start()
    {
        Weapon = GameObject.Find("Weapon");
        _activeWeapon = SwapWeapon(Weapons.First());
    }

    // Update is called once per frame
    void Update()
    {
        if (!InMiniGame)
        {
            MovePlayer();
            MoveCamera();
            HandleWeapons();
        }
    }

    public void HideWeapon()
    {
        Weapon.SetActive(false);
    }

    public void ShowWeapon()
    {
        Weapon.SetActive(true);
    }

    private void MovePlayer()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            modifiedPlayerSpeed = PlayerSpeed * 1.6f;
        }
        else
        {
            modifiedPlayerSpeed = PlayerSpeed;
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.position = transform.position + transform.forward * Time.deltaTime * modifiedPlayerSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position = transform.position + transform.forward * -1 * Time.deltaTime * modifiedPlayerSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position = transform.position + transform.right * -1 * Time.deltaTime * modifiedPlayerSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position = transform.position + transform.right * Time.deltaTime * modifiedPlayerSpeed;
        }
    }

    private void MoveCamera()
    {
        float rotateHorizontal = Input.GetAxis("Mouse X");
        transform.Rotate(transform.up * rotateHorizontal * LookSensitivity);

        float rotateVertical = Input.GetAxis("Mouse Y");
        var playerCamera = GameObject.Find("Main Camera");
        var weapon = GameObject.Find("Weapon");
        Vector3 localX = transform.TransformDirection(Vector3.right);
        //playerCamera.transform.Rotate(transform.right * rotateVertical * LookSensitivity);
        //gun.transform.Rotate(transform.right * rotateVertical * LookSensitivity);

        playerCamera.transform.RotateAround(playerCamera.transform.position, localX, rotateVertical * LookSensitivity);
        weapon.transform.RotateAround(weapon.transform.position, localX, rotateVertical * LookSensitivity);
    }

    private void HandleWeapons()
    {
        if (Input.GetMouseButtonDown(0) && _canFire)
        {
            _shouldRespawnWeapon = _activeWeapon.GetComponent<Weapon>().Fire();
            if (_shouldRespawnWeapon)
            {
                _canFire = false;
            }
        }

        if (_shouldRespawnWeapon && _currentRespawnWeaponTime > _allowRespawnWeaponTime)
        {
            _shouldRespawnWeapon = false;
            _canFire = true;
            _currentRespawnWeaponTime = 0;
            _activeWeapon = SwapWeapon(Weapons.Skip(_currentWeaponIndex).First());
        }
        else if (_shouldRespawnWeapon)
        {
            _currentRespawnWeaponTime += Time.deltaTime;
        }

        if (Input.mouseScrollDelta.y > 0 && _canSwapWeapons)
        {
            if (Weapons.Count > 1)
            {
                if (_currentWeaponIndex == Weapons.Count - 1)
                {
                    _currentWeaponIndex = 0;
                }
                else
                {
                    _currentWeaponIndex++;
                }

                _activeWeapon = SwapWeapon(Weapons.Skip(_currentWeaponIndex).First());
            }
        }

        if (!_canSwapWeapons)
        {
            _currentSwapWeaponTime += Time.deltaTime;
            if (_currentSwapWeaponTime >= _allowSwapWeaponTime)
            {
                _currentSwapWeaponTime = 0f;
                _canSwapWeapons = true;
            }
        }
    }

    private Transform SwapWeapon(Transform weaponTransform)
    {
        var weapon = GameObject.Find("Weapon");
        foreach (Transform child in weapon.transform)
        {
            Destroy(child.gameObject);
        }

        var newWeapon = Instantiate(weaponTransform, new Vector3(weapon.transform.position.x, weapon.transform.position.y, weapon.transform.position.z), weapon.transform.rotation);
        newWeapon.parent = weapon.transform;
        return newWeapon;
    }
}

