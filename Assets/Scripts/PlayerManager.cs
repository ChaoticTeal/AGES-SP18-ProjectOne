using System;
using UnityEngine;

// Script adapted from TANKS tutorial
[Serializable]
public class PlayerManager
{
    public Color m_PlayerColor;            
    public Transform m_SpawnPoint;         
    [HideInInspector] public int m_PlayerNumber;             
    [HideInInspector] public string m_ColoredPlayerText;
    [HideInInspector] public GameObject m_Instance;          
    [HideInInspector] public int m_Wins;


    private PlayerMovement m_Movement;
    private GameObject m_CanvasGameObject;
    private ParticleSystem particleSystem;


    public void Setup()
    {
        m_Movement = m_Instance.GetComponent<PlayerMovement>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;
        particleSystem = m_Instance.GetComponent<PlayerHealth>().m_ExplosionPrefab.GetComponent<ParticleSystem>();
        var systemMain = particleSystem.main;
        systemMain.startColor = m_PlayerColor;

        m_Movement.PlayerNumber = m_PlayerNumber;

        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        renderers[0].material.color = m_PlayerColor;

    }


    public void DisableControl()
    {
        m_Movement.enabled = false;
        m_CanvasGameObject.SetActive(false);
    }


    public void EnableControl()
    {
        m_Movement.enabled = true;

        m_CanvasGameObject.SetActive(true);
    }


    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
