using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the player shooting class to take the player attack inputs

// only the player weapon has an audio source, enemy sounds are handled differently

public class PlayerShooting : BaseShooting
{
    [SerializeField]private AudioSource shootSound;

    protected override void Shoot()
    {
        base.Shoot();
        if (!Input.GetButton("Fire1")) shooting = false; // stop shooting now that at least one shot attempt has been fired
    }

    protected override void ShotFired()
    {
        shootSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1")) shooting = true; // only sets shooting to true, so the player can tap shoot and will still fire on their next shoot beat
        if (shooting)
        {
            // prepare to shoot in the direction of the mouse pointer
            Vector3 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            shotFacing = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
        }
    }
}
