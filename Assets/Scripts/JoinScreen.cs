using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JoinScreen : MonoBehaviour 
{
    [SerializeField]
    List<GameObject> joinPanels;
    public const int MaxPlayers = 4;
    
    
    public static int NumberOfJoinedPlayers
    {
        get
        {
            if (joinedPlayers == null)
                return 0;
            return joinedPlayers.Count(c => c);
        }
    }
    
    private string joinButtonName = "Fire";
    private static bool[] joinedPlayers;

    private void Start()
    {
        InitializePlayerList();
    }

    private void Update()
    {
        CheckForJoiningPlayers();
    }

    private void CheckForJoiningPlayers()
    {
        for(int i = 0; i < MaxPlayers; i++)
        {
            if (joinedPlayers[i] == true)
                continue;
            if(Input.GetButtonDown("P" + (i + 1).ToString() + "-" + joinButtonName))
            {
                joinPanels[i].transform.Find("Text").GetComponent<Text>().text = "Player " + (i + 1).ToString() + " Joined!";
                joinedPlayers[i] = true;
            }
        }
    }

    private void InitializePlayerList()
    {
        Debug.Log("Initializing");
        joinedPlayers = new bool[MaxPlayers];

        for (int i = 0; i < MaxPlayers; i++)
        {
            joinedPlayers[i] = false;
        }
    }
}
