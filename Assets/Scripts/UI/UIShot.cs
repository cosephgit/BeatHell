using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShot : MonoBehaviour
{
    [SerializeField]private Image shotImage;

    public void SetShot(Shot shotNew)
    {
        float scale = shotNew.ShotScale();
        shotImage.sprite = shotNew.shotSprite;
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
