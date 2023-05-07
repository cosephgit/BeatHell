using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMousePointer : MonoBehaviour
{
    public static UIMousePointer instance;
    [SerializeField]private Image pointer;
    [SerializeField]private Canvas canvas;
    [SerializeField]private GameObject hintMove;
    [SerializeField]private GameObject hintShoot;
    [SerializeField]private GameObject hintAbsorb;
    [SerializeField]private TextMeshProUGUI[] hintText;
    [SerializeField]private int hintBeats = 4;
    [SerializeField]private int hintBeatsGap = 1;
    [Header("Color beats")]
    [SerializeField]private Color colorStart = Color.yellow;
    [SerializeField]private Color colorAlt = Color.magenta;
    private bool color;
    private bool hintActive = false; // make sure not to trigger a new hint until the existing one is done
    private bool hintMoveNew = true; // there are multiple places that can trigger the hintAbsorb so need to make sure its only triggered once
    private bool hintAbsorbNew = true; // there are multiple places that can trigger the hintAbsorb so need to make sure its only triggered once
    private int beatCount = 0;

    private void Awake()
    {
        if (instance)
        {
            if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }
        else
            instance = this;

        pointer.color = colorStart;
        for (int i = 0; i < hintText.Length; i++)
            hintText[i].color = colorStart;

        hintMove.SetActive(false);
        hintShoot.SetActive(false);
        hintAbsorb.SetActive(false);
    }

    void Start()
    {
        Cursor.visible = false;
        BeatManager.onBeat += MousePulseBeat;
    }

    void OnDestroy()
    {
        BeatManager.onBeat -= MousePulseBeat;
    }

    private void LateUpdate()
    {
        transform.position = Input.mousePosition; // / canvas.scaleFactor;
    }

    private void MousePulseBeat(int count)
    {
        if (hintActive)
            beatCount++;
        if (color)
        {
            pointer.color = colorStart;
            for (int i = 0; i < hintText.Length; i++)
                hintText[i].color = colorStart;
            color = false;
        }
        else
        {
            pointer.color = colorAlt;
            for (int i = 0; i < hintText.Length; i++)
                hintText[i].color = colorAlt;
            color = true;
        }
    }

    // this is triggered at the start of the first stage
    public void ShowHintMove()
    {
        if (hintMoveNew)
        {
            hintMoveNew = false;
            StartCoroutine(HintShow(hintMove));
        }
    }

    // this is triggered in the first stage when the first wave is spawned
    public void ShowHintShoot()
    {
        StartCoroutine(HintShow(hintShoot));
    }

    // this is triggered in the first stage the first time the player is hit by a bullet OR the first time they run out of bullets
    public void ShowHintAbsorb()
    {
        if (hintAbsorbNew)
        {
            // only show this hint the first time the player is hit by a bullet, or runs out of bullets
            hintAbsorbNew = false;
            StartCoroutine(HintShow(hintAbsorb));
        }
    }

    // generic hint show coroutine
    private IEnumerator HintShow(GameObject hint)
    {
        while (hintActive) yield return new WaitForEndOfFrame(); // hold until hintActive is false (wait for a currently visible hint to disppear)
        hintActive = true;
        hint.SetActive(true);
        beatCount = 0;
        while (beatCount < hintBeats)
            yield return new WaitForEndOfFrame();
        hint.SetActive(false);
        beatCount = 0;
        while (beatCount < hintBeatsGap)
            yield return new WaitForEndOfFrame();
        hintActive = false;
    }
}
