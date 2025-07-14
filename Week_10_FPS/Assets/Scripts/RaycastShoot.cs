using UnityEngine;
using System.Collections;

public class RaycastShoot : MonoBehaviour
{
    //Shooting
    [SerializeField] int gunDamage = 1;
    [SerializeField] float fireRate = 0.5f;
    [SerializeField] float fireRange = 50;
    [SerializeField] float hitForce = 15;
    float nextFire;
    public int maxAmmo = 6;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    
   
    //Camera
    Camera FPSCam;
    
    //Audio/Visual
    AudioSource _as;
    LineRenderer _lr;
    [SerializeField] float waitTime;
    [SerializeField] Transform gunEnd;

    public LayerMask layerMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FPSCam = Camera.main;
        _as = GetComponent<AudioSource>();
        _lr = GetComponent<LineRenderer>();

     
            currentAmmo = maxAmmo;

    }

    // Update is called once per frame
    void Update()
    {

        //Raycast start
        Vector3 rayOrigin = FPSCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

        //Line rendered start
        _lr.SetPosition(0, gunEnd.position);

        Debug.DrawRay(rayOrigin, FPSCam.transform.forward);

        //Gun fire 
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            StartCoroutine(shootingEffects());

            //gives access to position it hit, that angle it hit the object , all components of that object and all public variables/methods that exposed in that.
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, FPSCam.transform.forward, out hit, fireRange))
            {
                _lr.SetPosition(1, hit.point);

                ShootableBox targetBox = hit.transform.GetComponent<ShootableBox>();

                if(targetBox != null)
                {
                    targetBox.Damage(gunDamage);
                    Debug.Log("Enemy hit");
                }

                Enemy ragdollSwitch = hit.transform.GetComponent<Enemy>();

                if (ragdollSwitch != null)
                {
                    ragdollSwitch.triggerRagdoll();
                }

                if(hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * hitForce, ForceMode.Impulse);
                }
            }
            else
            {
                _lr.SetPosition(1, FPSCam.transform.forward * fireRange);
            }

            currentAmmo--;
        }

        if (isReloading)
            return;

        if(currentAmmo <= 0)
        {
            StartCoroutine (Reload());
            return;
        }
    }

    private IEnumerator shootingEffects()
    {
        _as.Play();
        _lr.enabled = true;

        yield return new WaitForSeconds(waitTime);

        _lr.enabled = false;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        
            Debug.Log("Reloading");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
