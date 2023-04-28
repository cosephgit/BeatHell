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

    public void Shoot(Vector3 pos, Quaternion rot, int damageSet, Color colorSet, float speedSet, float lifeSet, Layer layer)
    {
        float scale = Global.BULLETSCALE + (damageSet * Global.BULLETSCALEPERDAM);
        transform.localScale = new Vector3(scale, scale, scale);
        transform.position = pos;
        transform.rotation = rot;
        sprite.color = colorSet;
        vee = Vector2.up * speedSet * Time.fixedDeltaTime;
        damage = damageSet;
        life = lifeSet;
        gameObject.layer = (int)layer;
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
