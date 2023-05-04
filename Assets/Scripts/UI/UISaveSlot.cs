using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// manages loading/starting games in the main menu

public class UISaveSlot : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI slotText;
    [SerializeField]private GameObject slotNewGame; // contains the slot features for starting a new game (difficulty selection)
    [SerializeField]private GameObject slotLoadGame; // contains the slot features for loading a game (difficulty used, and delete option)
    // new game only features
    [SerializeField]private TMP_Dropdown slotDropdown; // dropdown menu for selecting slot difficulty
    // load game only features
    [SerializeField]private TextMeshProUGUI slotStage; // show the difficuly of an active slot
    [SerializeField]private TextMeshProUGUI slotDifficulty; // show the difficuly of an active slot
    [SerializeField]private TextMeshProUGUI slotScore; // show the difficuly of an active slot
    [SerializeField]private GameObject slotLoadDelete; // contains the slot features for deleting a saved game (confirm button)
    // slot type
    [SerializeField]private int slotIndex; // the numerical index for this save slot MUST BE LESS THAN Global.SAVESLOTS!!!
    [SerializeField]private float slotDeleteDuration = 3f; // duration of the slot delete confirmation box
    private string[] diffName = new string[3] { "Casual (125BPM)", "Intense (150BPM)", "Nightmare (190BPM)" }; // kinda hacky duplication of difficulty name data

    private void Awake()
    {
        if (slotIndex >= Global.SAVESLOTS)
        {
            Debug.LogError("Save slot UI has slot index " + slotIndex + " above maximum");
            slotIndex = 0;
        }
    }

    private void SlotInitNew()
    {
        slotNewGame.SetActive(true);
        slotLoadGame.SetActive(false);
        slotText.text = "New game " + (slotIndex + 1);
        // the dropdown menu will default to the first (easy) setting on scene load, and we want this default, so set the stored difficulty to this value (0)
        GameManager.instance.SetSlotDifficulty(slotIndex, 0);
    }

    private void Start()
    {
        if (GameManager.instance.stage[slotIndex] > 0)
        {
            slotNewGame.SetActive(false);
            slotLoadGame.SetActive(true);
            slotLoadDelete.SetActive(false);
            slotText.text = "Load slot " + (slotIndex + 1);
            slotStage.text = "Stage " + (GameManager.instance.stage[slotIndex] + 1);
            slotScore.text = "Score " + GameManager.instance.score[slotIndex];
            slotDifficulty.text = diffName[GameManager.instance.difficulty[slotIndex]];
        }
        else
        {
            SlotInitNew();
        }
    }

    // the dropdown menu has changed value, update the slot saved value
    public void DropdownDifficulty(System.Int32 index)
    {
        GameManager.instance.SetSlotDifficulty(slotIndex, index);
    }

    public void DeleteSlotRequest()
    {
        StopAllCoroutines();
        slotLoadDelete.SetActive(true);
        StartCoroutine(CloseDeleteConfirm());
    }

    public void DeleteSlotConfirm()
    {
        GameManager.instance.ResetStage(slotIndex);
        StopAllCoroutines();
        slotLoadDelete.SetActive(false);
        SlotInitNew();
    }

    public IEnumerator CloseDeleteConfirm()
    {
        yield return new WaitForSeconds(slotDeleteDuration);
        slotLoadDelete.SetActive(false);
    }
}
