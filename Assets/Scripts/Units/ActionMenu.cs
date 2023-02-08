using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour
{
    public GameObject Menu;
    public Button[] Buttons = new Button[3];
    public TMP_Text[] ButtonTexts = new TMP_Text[3];

    public void ShowMenu()
    {
        Menu.SetActive(true);
    }

    public void HideMenu()
    {
        Menu.SetActive(false);
    }

    public void SetButtonControls(Hero hero, UnitInfo info)
    {
        if (hero.state != UnitState.Waiting)
        {
            if(hero.state == UnitState.ReadyToMove)
            {
                Buttons[0].onClick.RemoveAllListeners();
                Buttons[0].onClick.AddListener(hero.ChooseMove);
                ButtonTexts[0].text = "Move";

                Buttons[1].onClick.RemoveAllListeners();
                Buttons[1].onClick.AddListener(hero.DeselectUnit);
                Buttons[1].onClick.AddListener(hero.MenuCancel);
                ButtonTexts[1].text = "Cancel";

                Buttons[2].gameObject.SetActive(false);
            }
            else if(hero.state == UnitState.ReadyToAct)
            {
                Buttons[0].onClick.RemoveAllListeners();
                Buttons[0].onClick.AddListener(hero.Action1);
                ButtonTexts[0].text = info.abilityNames[0];

                Buttons[1].onClick.RemoveAllListeners();
                Buttons[1].onClick.AddListener(hero.Action2);
                ButtonTexts[1].text = info.abilityNames[1];

                Buttons[2].onClick.RemoveAllListeners();
                Buttons[2].onClick.AddListener(hero.DeselectUnit);
                Buttons[2].onClick.AddListener(hero.MenuCancel);
                ButtonTexts[2].text = "Cancel";
                Buttons[2].gameObject.SetActive(true);
            }
        }
    }
}
