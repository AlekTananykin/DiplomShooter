
using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using TMPro;
using Photon.Pun;

public class PlayerDoor : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text PlayerName_Text;

    public Image PlayerColorImage;
    public Button PlayerReady_Button;
    public Image PlayerReadyImage;
    public Image PlayerNotReadyImage;

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
            PlayerReady_Button.gameObject.SetActive(false);
        }
        else
        {
            Hashtable initialProps = new Hashtable()
            {
                {GameUtils.PLAYER_READY, _isPlayerReady},
                {GameUtils.PLAYER_HEALTH, GameUtils.PLAYER_MAX_HEALTH}
            };

            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
            PhotonNetwork.LocalPlayer.SetScore(0);

            SetPlayerReady(_isPlayerReady);

            PlayerReady_Button.onClick.AddListener(() =>
            {
                _isPlayerReady = !_isPlayerReady;

                Debug.Log($"onClick=> {_isPlayerReady}");
                SetPlayerReady(_isPlayerReady);

                Hashtable props = new Hashtable() { { GameUtils.PLAYER_READY, _isPlayerReady } };
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                if (PhotonNetwork.IsMasterClient)
                {
                    FindObjectOfType<CanvasManager>().LocalPlayerPropertiesUpdated();
                }
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
        PlayerName_Text.text = playerName;
    }

    private void OnPlayerNumberingChanged()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.ActorNumber == _ownerId)
            {
                PlayerColorImage.color = GameUtils.GetColor(p.GetPlayerNumber());

                Debug.Log($"color: { PlayerColorImage.color}");
            }
        }
    }

    public void SetPlayerReady(bool playerReady)
    {
        PlayerReady_Button.GetComponentInChildren<TMP_Text>().text = 
            playerReady ? "Ready!" : "Ready?";
        
        PlayerReadyImage.enabled = playerReady;
        PlayerNotReadyImage.enabled = !playerReady;
    }
}
