using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// controls bullet behaviour

public class Bullet : MonoBehaviour
{
    [SerializeField]private SpriteRenderer sprite;
    public int damage { get; private set; }
    private Vector2 vee;
    private float life;
    public Shot shotStored { get; private set; } // store the key data structure for this shot in case it's needed later (for the player to store it)

    public void Shoot(Vector3 pos, Quaternion rot, Shot shot, Color shotColor, Layer shotLayer)
    {
        float scale = Global.BULLETSCALE + (shot.shotDamage * Global.BULLETSCALEPERDAM);
        transform.localScale = new Vector3(scale, scale, scale);
        transform.position = pos;
        transform.rotation = rot;
        sprite.color = shotColor;
        vee = Vector2.up * shot.shotSpeed * Time.fixedDeltaTime;
        damage = shot.shotDamage;
        life = shot.shotLife;
        gameObject.layer = (int)shotLayer;
        shotStored = shot;
        gameObject.SetActive(true);
    }

    void FixedUpdate()
    {
        if (gameObject.activeSelf)
        {
            transform.Translate(vee);
            life -= Time.fixedDeltaTime;
            if (life < 0)
            {
                BulletLibrary.instance.ReturnBullet(this);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        BulletLibrary.instance.ReturnBullet(this);
    }
}
