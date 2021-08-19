using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FightStageUIManager : MonoBehaviour
{
    private Fighter leftFighter;
    private Fighter rightFighter;

    [SerializeField] private TMP_Text leftFigherDamageText;
    [SerializeField] private TMP_Text rightFigherDamageText;


    //void Start()
    //{
    //    DetectFighters();
    //}

    void Update()
    {
        if (leftFighter == null || rightFighter == null)
        {
            DetectFighters();
        }
        UpdateFighterDamageUI(leftFighter, leftFigherDamageText);
        UpdateFighterDamageUI(rightFighter, rightFigherDamageText);
    }

    private void DetectFighters()
    {
        var fightersInScene = FindObjectsOfType<Fighter>();
        foreach (var fighter in fightersInScene)
        {
            if (leftFighter==null && fighter.CompareTag("Player"))
            {
                leftFighter = fighter;
            }
            else if (rightFighter == null && fighter.CompareTag("Enemy"))
            {
                rightFighter = fighter;
            }
        }
    }

    private void UpdateFighterDamageUI(Fighter selectedFighter, TMP_Text fighterDamageText)
    {
        if (selectedFighter == null)
        {
            fighterDamageText.text = "";
        }
        else
        {
            fighterDamageText.text = $"{selectedFighter.displayName}\n{selectedFighter.hurtPercentage}%";
        }

    }
}
