﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerListEntry.cs" company="Exit Games GmbH">
//   Part of: Asteroid Demo,
// </copyright>
// <summary>
//  Player List Entry
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using TMPro;

public class PlayerEntry : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI PlayerNameText;

    //public Image PlayerColorImage;
    public Button PlayerReadyButton;
    //public Image PlayerReadyImage;

    private int _ownerId;
    private bool _isPlayerReady;

    #region UNITY

    public void OnEnable()
    {
        PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
    }

    public void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber != _ownerId)
        {
            PlayerReadyButton.gameObject.SetActive(false);
        }
        else
        {/*
            Hashtable initialProps = new Hashtable() { 
                { AsteroidsGame.PLAYER_READY, isPlayerReady }, 
                { AsteroidsGame.PLAYER_LIVES, AsteroidsGame.PLAYER_MAX_LIVES } 
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
            PhotonNetwork.LocalPlayer.SetScore(0);
            */
            PlayerReadyButton.onClick.AddListener(() =>
            {
                _isPlayerReady = !_isPlayerReady;
                SetPlayerReady(_isPlayerReady);

                //Hashtable props = new Hashtable() { { AsteroidsGame.PLAYER_READY, isPlayerReady } };
                //PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                //if (PhotonNetwork.IsMasterClient)
                //{
                 //   FindObjectOfType<LobbyPanel>().LocalPlayerPropertiesUpdated();
                //}
            });
            
        }
    }

    public void OnDisable()
    {
        PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerNumberingChanged;
    }

    #endregion

    public void Initialize(int playerId, string playerName)
    {
        _ownerId = playerId;
        PlayerNameText.text = playerName;
    }

    private void OnPlayerNumberingChanged()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.ActorNumber == _ownerId)
            {
                //PlayerColorImage.color = AsteroidsGame.GetColor(p.GetPlayerNumber());
            }
        }
    }

    public void SetPlayerReady(bool playerReady)
    {
        PlayerReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = playerReady ? "Ready!" : "Ready?";
        //PlayerReadyImage.enabled = playerReady;
    }
}
