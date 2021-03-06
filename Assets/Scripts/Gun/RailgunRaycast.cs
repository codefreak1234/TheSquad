﻿using UnityEngine;
using System.Collections;

public class RailgunRaycast : MonoBehaviour
{
    public float timeBetweenBullets = 2.5f;
    public int damage = 10;
    public float range = 150;
    public float reloadTime = 2.5f;
    public AudioClip railgunShotSound;
    public AudioClip railgunChargeSound;
    public bool canFire;

    int shootableMask;
    float timer;
    Ray shootRay;
    private LineRenderer gunLine;
    RaycastHit hit;
    private AudioSource railgunAudio;
    float effectsDisplayTime = 0.2f;
    ParticleSystem railgunCharge;
    private bool playing = false;

    // Use this for initialization
    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        gunLine = GetComponent<LineRenderer>();
        railgunAudio = GetComponent<AudioSource>();
        railgunCharge = GetComponent<ParticleSystem>();
      

    }

    void Start()
    {
        canFire = true;
    }
   
    // Update is called once per frame
    void Update()
    {
        if(ammoDisplay.ammo <= 0)
        {
            canFire = false;
            StartCoroutine("Reload", reloadTime);
            ammoDisplay.ammo = 10;

        }

        timer += Time.deltaTime;

        if (canFire && timer > timeBetweenBullets)
        {
            
            if (Input.GetButton("Fire1") )
            {

                if (!playing)
                {
                    playing = true;
                    railgunAudio.clip = railgunChargeSound;
                    railgunAudio.Stop();
                    railgunAudio.Play();
                }

            }
            else if (Input.GetButtonUp("Fire1") && playing)
            {
                playing = false;
                railgunAudio.Stop();
                shoot();

            }

        }

        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }

    }

    public void DisableEffects()
    {
        gunLine.enabled = false;
    }

    public void shoot()
    {
        timer = 0f;

        railgunAudio.clip = railgunShotSound;
        gunLine.enabled = true;
        
        railgunAudio.Play();
        ammoDisplay.ammo -= 1;
        gunLine.SetPosition(0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.right;
      
        if (Physics.Raycast(shootRay, out hit, shootableMask))
        {
         
            EnemyHealth dmgscript = hit.collider.gameObject.GetComponent<EnemyHealth>();
            if(dmgscript != null)

                {
                    dmgscript.TakeDamage(damage, hit.point);
                }

                gunLine.SetPosition(1, hit.point);
        }

        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }

    IEnumerator Reload(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        canFire = true;
    }
}