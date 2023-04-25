using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    // KEYBINDS
    [SerializeField] KeyCode fireKey = KeyCode.Mouse0;
    [SerializeField] KeyCode reloadKey = KeyCode.R;
    [SerializeField] AudioSource silencedPistolSfx;
    [SerializeField] AudioSource sniperRifleSfx;
    [SerializeField] WeaponSwitch whichWeapon;

    public static Action shootInput;
    public static Action reloadInput;

    private void Update()
    {
        if(Input.GetKeyDown(fireKey)) 
        {
            shootInput?.Invoke();
            if (whichWeapon.selectedWeapon == 0) // If the pistol is chosen...
                silencedPistolSfx.Play();

            else if(whichWeapon.selectedWeapon == 1) // If the sniper rifle is chosen...
                sniperRifleSfx.Play();
        }

        if (Input.GetKeyDown(reloadKey))
        {
            reloadInput?.Invoke();
        }
    }
}
