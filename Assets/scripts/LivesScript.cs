using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LivesScript : NetworkBehaviour {
    public int maxLives;
    public int startIncome;
    public int player1Lives;
    public int player2Lives;
    private int player1Income;
    private int player2Income;
    public float updateTime = 10f;
    public Text playerInfoText;
    public BuildScript carpenter;
    public int localPlayerTeam;

    // Use this for initialization
    void Start ()
    {
        player1Income = startIncome;
        player2Income = startIncome;
        player1Lives = maxLives;
        player2Lives = maxLives;
        gameObject.GetComponent<Text>().text = maxLives.ToString();
	}
	
	// Update is called once per frame
	void Update () {

        if (!isServer)
            return;

        if (Time.time >= updateTime)
        {
            updateTime = Time.time + 10;
            CmdPostInfo();

        }
    }

    [Command]
    public void CmdSubtractLives(int team)
    {
        switch (team) {
            case 1:
                player2Lives -= 1;
                break;
            case 2:
                player1Lives -= 1;
                break;
        }

        if (player1Lives == 0)
        {
            KillPlayer(1);
        }

        if (player2Lives == 0)
        {
            KillPlayer(2);
        }
    }

    [Command]
    public void CmdAddIncome(int team, int incomeAmt)
    {

        switch (team) {
            case 1:
                player1Income += incomeAmt;
                break;
            case 2:
                player2Income += incomeAmt;
                break;
        }
    }

    public void KillPlayer(int player)
    {
        if (!isServer) //Only server can kill players
            return;


        switch (player)
        {
            case 1:
                Debug.Log("Player 2 Wins");
                break;
            case 2:
                Debug.Log("Player 1 Wins");
                break;
        }
    }

    public void OnChangeLivesPlayer1()
    {

    }

    [Command]
    public void CmdPostInfo()
    {
        RpcPostInfo(player1Lives, player2Lives, player1Income, player2Income);
    }

    [ClientRpc]
    public void RpcPostInfo(int player1Lives, int player2Lives, int player1IncomeAmt, int player2IncomeAmt)
    {
        string playerInfo = ("Update:\n" + "Player 1: Lives - " + player1Lives + "  and Income - " + player1IncomeAmt + "\n" + "Player 2: Lives - " + player2Lives + "  and Income - " + player2IncomeAmt);
        playerInfoText.text = playerInfo;
        StartCoroutine(FadeText());

        switch (localPlayerTeam)
        {
            case 1:
                carpenter.AddGold(player1IncomeAmt);
                break;
            case 2:
                carpenter.AddGold(player2IncomeAmt);
                break;
        }
    }

    IEnumerator FadeText()
    {
        playerInfoText.CrossFadeAlpha(1, 0.15f, false);
        yield return new WaitForSeconds(5);
        playerInfoText.CrossFadeAlpha(0, 2, false);


    }
}
