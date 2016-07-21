using UnityEngine;
using System.Collections;

public class RailgunRaycast : MonoBehaviour
{
    public float timeBetweenBullets;
    public int damage = 10;
    public float range = 150;
    public AudioClip railgunShotSound;
    public AudioClip railgunChargeSound;

    int shootableMask;
    float timer;
    Ray shootRay;
    private LineRenderer gunLine;
    RaycastHit hit;
    private AudioSource railgunAudio;
    float effectsDisplayTime = 0.2f;
    ParticleSystem railgunCharge;
   

    // Use this for initialization
    void Awake()
    {
        // shootableMask = LayerMask.GetMask("Shootable");
        // gunLine = GetComponent<LineRenderer>();
        railgunAudio = GetComponent<AudioSource>();
        // railgunCharge = GetComponent<ParticleSystem>();
        // railgunShotSound = GetComponent<AudioClip>();
        // railgunChargeSound = GetComponent<AudioClip>();


    }
    IEnumerator TurnFalse() {
        playing = true ;
        yield return new WaitForSeconds(3) ;
        playing = false ;
    }
    public bool playing = false ;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        if(Input.GetButton("Fire1") && timer >= timeBetweenBullets) {
            if(!playing){ 
                playing = true ;
                railgunAudio.clip = railgunChargeSound;
                railgunAudio.Stop() ;
                railgunAudio.Play();
            }
            // Charge() ;
        }
        else if (Input.GetButtonUp("Fire1") && playing) {
            playing = false ;
            railgunAudio.Stop();
            shoot();
        }    

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            // DisableEffects();
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
        // gunLine.enabled = true;
        
        railgunAudio.Play();
        // gunLine.SetPosition(0, transform.position);

        // shootRay.origin = transform.position;
        // shootRay.direction = transform.right;
      
        // if (Physics.Raycast(shootRay, out hit, shootableMask))
        // {
         
        //     // EnemyHealth dmgscript = hit.collider.gameObject.GetComponent<EnemyHealth>();
        //     // if(dmgscript != null)

        //     //     {
        //     //         dmgscript.TakeDamage(damage, hit.point);
        //     //     }

        //         gunLine.SetPosition(1, hit.point);
        // }

        // else
        // {
        //     gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        // }
    }
}