using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    // KEYBINDS
    [SerializeField] KeyCode fireKey = KeyCode.Mouse0;
    [SerializeField] KeyCode reloadKey = KeyCode.R;
    [SerializeField] AudioSource weaponSfx;

    public static Action shootInput;
    public static Action reloadInput;

    private void Update()
    {
        if(Input.GetKeyDown(fireKey)) 
        {
            shootInput?.Invoke();
            weaponSfx.Play();
        }

        if (Input.GetKeyDown(reloadKey))
        {
            reloadInput?.Invoke();
        }
    }
}
