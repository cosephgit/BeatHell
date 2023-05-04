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
    private bool moving = false;
    float moveStart; // hacky hack hack

    public void Shoot(Vector3 pos, Quaternion rot, Shot shot, Color shotColor, Layer shotLayer, bool player)
    {
        float scale = shot.ShotScale();
        transform.parent = null;
        transform.localScale = new Vector3(scale, scale, scale);
        transform.position = pos;
        transform.rotation = rot;
        sprite.color = shotColor;
        sprite.sprite = shot.shotSprite;
        vee = Vector2.up * shot.shotSpeed * Time.fixedDeltaTime;
        if (player) vee = vee * 1.5f;
        damage = shot.shotDamage;
        life = shot.shotLife;
        gameObject.layer = (int)shotLayer;
        shotStored = shot;
        gameObject.SetActive(true);
        moving = true;
    }

    public void Clear()
    {
        moving = false;
        vee = Vector2.zero;
        gameObject.layer = 0;
    }

    void FixedUpdate()
    {
        if (gameObject.activeSelf && moving)
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
