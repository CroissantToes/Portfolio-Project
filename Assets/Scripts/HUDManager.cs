using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    #region Singleton
    public static HUDManager Instance { get; private set; }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    [Header("Unit Info")]
    public GameObject sideBar;

    public Image portraitBox;
    public TMP_Text classText;
    public TMP_Text hpText;
    public TMP_Text distanceText;

    public List<GameObject> abilityPortraits;
    public List<GameObject> abilityNames;

    public GameObject descriptionBox;
    public TMP_Text descriptionHeader;
    public TMP_Text descriptionValue;
    public TMP_Text descriptionBody;

    private UnitInfo selectedUnitInfo;

    [Header("Barrier Strength")]
    public Slider barrierSlider;
    public TMP_Text barrierText;

    public void SetSidebar(UnitInfo info, int health, int maxHealth, int moveDistance)
    {
        selectedUnitInfo = info;

        portraitBox.sprite = info.portrait;
        classText.text = info.className;
        hpText.text = $"HP: {health}/{maxHealth}";
        distanceText.text = $"Move Range: {moveDistance}";

        for(int i = 0; i < info.abilityPortraits.Count; i++)
        {
            abilityPortraits[i].GetComponent<Image>().sprite = info.abilityPortraits[i];
            abilityPortraits[i].SetActive(true);

            abilityNames[i].GetComponent<TMP_Text>().text = info.abilityNames[i];
            abilityNames[i].SetActive(true);
        }

        sideBar.SetActive(true);
    }

    public void ShowDescriptionBox(int i)
    {
        descriptionHeader.text = selectedUnitInfo.abilityNames[i];
        string[] values = selectedUnitInfo.abilityDescriptions[i].Split('/');
        descriptionValue.text = values[0];
        descriptionBody.text = values[1];
        descriptionBox.SetActive(true);
    }

    public void HideDescriptionBox()
    {
        descriptionBox.SetActive(false);
    }

    public void SetBarrierHealth(int health, int maxHealth)
    {
        barrierSlider.value = health;
        barrierText.text = $"{health}/{maxHealth}";
    }
}
