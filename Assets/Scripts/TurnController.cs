using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Units;
using UnityEngine;
using UnityEngine.UI;

public class TurnController:MonoBehaviour
{
    public const float ENEMY_TURN_PAUSE = 1f;
    public const float SYSTEM_MESSAGE_PAUSE = 2f;
    public static readonly int ENEMY_MAX_ACTION_COUNT = SaveDataStorage.IS_DIFFICULT ? 3 : 2;
    [SerializeField] private Button passTurnButton;
    public bool IsPlayerTurn { get; private set; } = true;
    private Coroutine enemyTurn;

    public void SwitchTurn()
    {
        IsPlayerTurn = !IsPlayerTurn;
        LevelController.PassTurn();
        passTurnButton.interactable = IsPlayerTurn;
        if (!IsPlayerTurn)
        {
            List<Enemy> enemies = LevelController.GetEnemyList();
            enemyTurn = StartCoroutine(EnemyTurn(enemies));
        }
        else
        {
            if (enemyTurn != null) StopCoroutine(enemyTurn);
        }
    }

    private IEnumerator EnemyTurn(List<Enemy> enemies)
    {
        foreach (var enemy in enemies)
        {
            for (int i = 0; i < ENEMY_MAX_ACTION_COUNT; i++)
            {
                string action = enemy.Act();
                LevelController.DisplayMessage(action, false);
                yield return new WaitForSeconds(ENEMY_TURN_PAUSE);
            }
        }
        SwitchTurn();
    }

    public void InterruptTurn(bool forcePlayerTurn = false)
    {
        StopCoroutine(enemyTurn);
        passTurnButton.interactable = false;
        if(forcePlayerTurn != !IsPlayerTurn)
            SwitchTurn();
    }
}