using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform cam;

    float timeSinceLastShot;

    private void Start()
    {
        PlayerShoot.shootInput += Shoot; 
        PlayerShoot.reloadInput += Reloading; 
    }

    private void OnDisable() => gunData.reloading = false;

    public void Reloading()
    {
        if(!gunData.reloading && this.gameObject.activeSelf) 
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
                if(Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, gunData.maxDistance)) 
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
        Debug.DrawRay(cam.position, cam.forward * gunData.maxDistance);
    }

    private void OnGunShoot()
    {
        // throw new NotImplementedException();
    }
}
