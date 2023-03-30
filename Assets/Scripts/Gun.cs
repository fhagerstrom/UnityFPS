using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform muzzle;

    float timeSinceLastShot;

    private void Start()
    {
        PlayerShoot.shootInput += Shoot; 
        PlayerShoot.reloadInput += Reloading; 
    }

    public void Reloading()
    {
        if(!gunData.reloading) 
        {
            // Reload
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload() 
    {
        gunData.reloading = true;

        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmo = gunData.magSize;

        gunData.reloading = false;
    }

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1.0f / (gunData.fireRate / 60.0f);

    public void Shoot()
    {
        if(gunData.currentAmmo > 0)
        {
            if(CanShoot()) 
            {
                if(Physics.Raycast(muzzle.position, transform.forward, out RaycastHit hitInfo, gunData.maxDistance)) 
                {
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.TakeDamage(gunData.damage);
                }

                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                // OnGunShot();

            }
        }
        // Debug.Log("Fired weapon!");
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(muzzle.position, muzzle.forward);
    }

    private void OnGunShoot()
    {
        // throw new NotImplementedException();
    }
}
