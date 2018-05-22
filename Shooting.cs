using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    public float coolDownTime;
    private float coolDownTimeCurrent;
    public float reloadTime;
    private float reloadTimeCurrent;

    public GameObject weapon;
    private Vector3 weaponNormalPos;
    private Quaternion weaponNormalRot;
    public Transform weaponReloadTransform;
    public Transform weaponRMBtransform;
    //public Transform weaponNORMtransform;

    public Text ammoGUI;

    public int ammo;
    public int clipCapacity;
    public int bulletsInClip;
    private AudioSource m_AudioSource;
    public AudioClip shoot;
    public AudioClip outOfAmmo;
    public AudioClip reload;

    delegate void CurrentState();
    CurrentState currentState;

    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
        m_AudioSource = GetComponent<AudioSource>();
        coolDownTimeCurrent = 0;
        reloadTimeCurrent = 0;
        weaponNormalPos = weapon.transform.localPosition;
        weaponNormalRot = weapon.transform.localRotation;
        currentState = Shoot;
        bulletsInClip = clipCapacity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ammoGUI.text = bulletsInClip + "|" + ammo;
        currentState();
        //aim();
    }

    void Update()
    {

    }

    void Shoot()
    {
        if (coolDownTimeCurrent == 0)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (bulletsInClip > 0)
                {
                    Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
                    //Ray ray = Camera.main.ScreenPointToRay(Vector3.zero);
                    RaycastHit hit = new RaycastHit();
                    if (Physics.Raycast(rayOrigin, Camera.main.transform.forward, out hit, Mathf.Infinity, LayerMask.GetMask("Target")))
                    {
                        print("fuck");
                        hit.collider.gameObject.GetComponentInParent<TargetScript>().beHitted();
                        print(hit.collider.gameObject.name);
                        
                    }

                    bulletsInClip -= 1;
                    coolDownTimeCurrent = coolDownTime;
                    m_AudioSource.PlayOneShot(shoot);
                    weapon.transform.localPosition += Vector3.back / 5;

                    var bullet = (GameObject)Instantiate(
                        bulletPrefab,
                        bulletSpawn.position,
                        bulletSpawn.rotation);

                    bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * 300;

                    Destroy(bullet, 2.0f);
                    currentState = CoolDown;
                }
                else
                {
                    m_AudioSource.PlayOneShot(outOfAmmo);
                    currentState = OutOfAmmo;
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
        }
    }

    void Aim()
    {
        if (currentState != CoolDown || currentState != reloadState)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                weapon.transform.localPosition = weaponRMBtransform.localPosition;
            }
            else
            {
                if (Input.GetKeyUp(KeyCode.Mouse1))
                {
                    weapon.transform.localPosition = weaponNormalPos;
                }
            }
        }
    }

    void CoolDown()
    {
        if (coolDownTimeCurrent > 0)
        {
            coolDownTimeCurrent -= Time.deltaTime;
            weapon.transform.localPosition =
                Vector3.Lerp(weaponNormalPos + Vector3.back / 5, weaponNormalPos, (coolDownTime - coolDownTimeCurrent) / coolDownTime);
        }
        else
        {
            coolDownTimeCurrent = 0;
            weapon.transform.localPosition =
                Vector3.Lerp(weaponNormalPos + Vector3.back / 5, weaponNormalPos, (coolDownTime - coolDownTimeCurrent) / coolDownTime);
            currentState = Shoot;
        }
    }

    void OutOfAmmo()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            m_AudioSource.PlayOneShot(outOfAmmo);
        }
    }

    void reloadState()
    {
        if (reloadTimeCurrent > 0)
        {
            reloadTimeCurrent -= Time.deltaTime;
            weapon.transform.localPosition =
                Vector3.Lerp(weaponNormalPos, weaponNormalPos + Vector3.right * 0.5f, (reloadTime - reloadTimeCurrent) * 5 / reloadTime);
            weapon.transform.localRotation =
                Quaternion.Lerp(weaponNormalRot, weaponReloadTransform.localRotation, (reloadTime - reloadTimeCurrent) * 5 / reloadTime);
        }
        else
        {
            reloadTimeCurrent = 0;
            weapon.transform.localPosition = weaponNormalPos;
            weapon.transform.localRotation = weaponNormalRot;
            currentState = Shoot;
        }
    }

    void Reload()
    {
        reloadTimeCurrent = reloadTime;
        if (bulletsInClip != clipCapacity)
        {
            if (ammo >= clipCapacity)
            {
                ammo -= clipCapacity - bulletsInClip;
                bulletsInClip = clipCapacity;
                m_AudioSource.PlayOneShot(reload);
                currentState = reloadState;
            }
            else
            {
                if (ammo != 0)
                {
                    bulletsInClip = ammo;
                    ammo = 0;
                    m_AudioSource.PlayOneShot(reload);
                    currentState = reloadState;
                }
                else
                {
                    m_AudioSource.PlayOneShot(outOfAmmo);
                    currentState = reloadState;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("ammo"))
        {
            ammo += 20;
        }
    }
}