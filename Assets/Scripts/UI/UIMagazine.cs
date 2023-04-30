using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// shows the contents of the player's magazine

public class UIMagazine : MonoBehaviour
{
    [SerializeField]private UIShot shotPrefab; // will be recursively added and stored to show all the bullets the player currently has
    [SerializeField]private GameObject shotNextPos; // an empty GameObject to position the next shot
    [SerializeField]private GameObject shotFillLeft; // an empty GameObject to position the left edge of the remaining shots
    [SerializeField]private GameObject shotFillRight; // an empty GameObject to position the right edge of the remaining shots
    [SerializeField]private float shotSpacingFakeMin = 10f; // the fake minimum number of shots for spacing calculations
    private List<UIShot> shotDisplay; // stores all the shots currently being displayed

    private void Awake()
    {
        shotDisplay = new List<UIShot>();
    }

    private void UpdateShotPositions()
    {
        Vector3 posInc = (shotFillRight.transform.position - shotFillLeft.transform.position) / (Mathf.Max(shotSpacingFakeMin, shotDisplay.Count) - 1f);
        for (int i = 0; i < shotDisplay.Count; i++)
        {
            if (i == 0) shotDisplay[i].transform.position = shotNextPos.transform.position;
            else
            {
                Vector3 pos = shotFillLeft.transform.position;
                pos += (posInc * (i - 1));
                shotDisplay[i].transform.position = pos;
            }
        }
    }

    public void SetShots(int count)
    {
        if (shotDisplay.Count > count)
        {
            for (int i = shotDisplay.Count - 1; i > count; i--)
            {
                Destroy(shotDisplay[i].gameObject);
                shotDisplay.RemoveAt(i);
            }
        }
        else if (shotDisplay.Count < count)
        {
            for (int i = shotDisplay.Count; i < count; i++)
            {
                UIShot shotNew = Instantiate(shotPrefab, transform.position, shotPrefab.transform.rotation, transform);
                shotDisplay.Add(shotNew);
            }
        }
        UpdateShotPositions();
    }

    // removes the first entry in the que, called when the player shoots
    public void Shoot()
    {
        if (shotDisplay.Count > 0)
        {
            Destroy(shotDisplay[0].gameObject);
            shotDisplay.RemoveAt(0);
            UpdateShotPositions();
        }
    }
}