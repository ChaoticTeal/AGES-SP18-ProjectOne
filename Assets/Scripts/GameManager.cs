﻿using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Script adapted from TANKS tutorial
public class GameManager : MonoBehaviour
{
    public int m_NumRoundsToWin = 5;
    public CameraControl m_CameraControl;
    public float m_StartDelay = 3f;
    public float m_EndDelay = 3f;
    public GameObject m_PlayerPrefab;
    public List<PlayerManager> players;
    public Text m_MessageText;

    [SerializeField]
    float discoBallLifetime = 2f;
    [SerializeField]
    float maxDiscoBallX = 40f;
    [SerializeField]
    float maxDiscoBallZ = -10f;
    [SerializeField]
    float minDiscoBallX = 10f;
    [SerializeField]
    float minDiscoBallZ = -40f;
    [SerializeField]
    GameObject discoBallPrefab;


    private int m_RoundNumber;              
    private WaitForSeconds m_StartWait;     
    private WaitForSeconds m_EndWait;       
    private PlayerManager m_RoundWinner;
    private PlayerManager m_GameWinner;
    GameObject activeDiscoBall;


    private void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnAllPlayers();
        SetCameraTargets();

        StartCoroutine(GameLoop());
    }


    private void SpawnAllPlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].m_Instance =
                Instantiate(m_PlayerPrefab, players[i].m_SpawnPoint.position, players[i].m_SpawnPoint.rotation) as GameObject;
            players[i].m_PlayerNumber = i + 1;
            players[i].Setup();
        }
    }


    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[players.Count];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = players[i].m_Instance.transform;
        }

        m_CameraControl.m_Targets = targets;
    }


    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (m_GameWinner != null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
        ResetAllPlayers();
        DisablePlayerControl();

        m_CameraControl.SetStartPositionAndSize();

        m_RoundNumber++;
        m_MessageText.text = "ROUND " + m_RoundNumber;

        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        EnablePlayerControl();
        StartCoroutine(DiscoBallSpawn());

        m_MessageText.text = string.Empty;

        while(!OnePlayerLeft())
        {
            yield return null;
        }
    }


    private IEnumerator RoundEnding()
    {
        DisablePlayerControl();

        m_RoundWinner = null;

        m_RoundWinner = GetRoundWinner();

        if (m_RoundWinner != null)
            m_RoundWinner.m_Wins++;

        m_GameWinner = GetGameWinner();

        string message = EndMessage();
        m_MessageText.text = message;

        yield return m_EndWait;
    }

    IEnumerator DiscoBallSpawn()
    {
        while (!OnePlayerLeft())
        {
            yield return new WaitForSeconds(discoBallLifetime);
            if (activeDiscoBall == null)
            {
                float xPos = Random.Range(minDiscoBallX, maxDiscoBallX);
                float zPos = Random.Range(minDiscoBallZ, maxDiscoBallZ);
                activeDiscoBall = Instantiate(discoBallPrefab);
                activeDiscoBall.transform.position = new Vector3(xPos, 10f, zPos);
            }
            yield return new WaitForSeconds(Random.Range(0, 2));
        }
    }

    private bool OnePlayerLeft()
    {
        int numPlayersLeft = 0;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].m_Instance.activeSelf)
                numPlayersLeft++;
        }

        return numPlayersLeft <= 1;
    }


    private PlayerManager GetRoundWinner()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].m_Instance.activeSelf)
                return players[i];
        }

        return null;
    }


    private PlayerManager GetGameWinner()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].m_Wins == m_NumRoundsToWin)
                return players[i];
        }

        return null;
    }


    private string EndMessage()
    {
        string message = "DRAW!";

        if (m_RoundWinner != null)
            message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

        message += "\n\n\n\n";

        for (int i = 0; i < players.Count; i++)
        {
            message += players[i].m_ColoredPlayerText + ": " + players[i].m_Wins + " WINS\n";
        }

        if (m_GameWinner != null)
            message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

        return message;
    }


    private void ResetAllPlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].Reset();
        }
    }


    private void EnablePlayerControl()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].EnableControl();
        }
    }


    private void DisablePlayerControl()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].DisableControl();
        }
    }
}