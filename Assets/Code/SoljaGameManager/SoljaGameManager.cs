using Photon.Pun;
using System;
using TMPro;
using UnityEngine;

public class SoljaGameManager : MonoBehaviour, ISoljaGameManager
{
    [Header("UI")]
    [SerializeField]
    private TMP_Text _health_Text;
    [SerializeField]
    private TMP_Text _killsAccount_Text;
    [SerializeField]
    private TMP_Text _ResultkillsAccount_Text;

    [Header("GameObject")]
    [Tooltip("The prefab to use for representing the player")]
    [SerializeField]
    private string _playerPrefabName;
    [SerializeField]
    private GameObject _locationPrefab;

    [Header("Bots")]
    [SerializeField]
    private int _botsAccount = 10;
    [SerializeField]
    private string _botPrefabName;

    public void StartMatch()
    {
        _location = GameObject.Instantiate(
            _locationPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);

        //_location = PhotonNetwork.Instantiate(_locationPrefabName,
        //    new Vector3(0f, 0f, 0f), Quaternion.identity);

        if (null == _location)
        {
            Debug.LogError("Location can't be loaded");
        }

        CreatePlayer();

        if (PhotonNetwork.IsMasterClient)
        {
            CreateBots(_botsAccount);
        }
    }

    private void CreatePlayer()
    {
        _player = PhotonNetwork.Instantiate(_playerPrefabName,
          GetPosition(), Quaternion.identity);

        Destroy(_player.GetComponent<BotHandler>());

        _playerHandler = _player.GetComponent<PlayerHandler>();

        if (null == _playerHandler)
        {
            Debug.LogError("PlayerHandler is not present");
        }
        _playerHandler.enabled = true;

        _playerHandler.SetGameManager(this);

    }

    private Vector3 GetPosition()
    {
        const float min = 5.0f;
        const float max = 95.0f;
        float x = UnityEngine.Random.Range(min, max);
        float z = UnityEngine.Random.Range(min, max);
        return new Vector3(x, 0.1f, z);
    }

    public void ExitMatch()
    {
        _location.SetActive(false);
        GameObject.DestroyImmediate(_location);
        _location = null;

        _player.SetActive(false);
        PhotonNetwork.Destroy(_player);
        _player = null;

        if (PhotonNetwork.IsMasterClient)
        {
            DestroyBots();
        }
    }

    public void SetHealth(int health)
    {
        _health = health;
        _health_Text.text = string.Format("Health: {0}", health);
    }

    public void SetKills(int killsAccount)
    {
        _killsAccount = killsAccount;
        _killsAccount_Text.text = string.Format("Kills: {0}", killsAccount);
        _ResultkillsAccount_Text.text = _killsAccount_Text.text;
    }

    public void Die()
    {
        _playerHandler.ToDie();
        _canvas.ShowMatchResults(_killsAccount, _health, "You lose");
    }

    public void GiveUp()
    {
        if (_health > 0)
        {
            _playerHandler.ToDie();
            _canvas.ShowMatchResults(_killsAccount, _health, "You gave up");
        }
    }


    public void CheckPlayerWine()
    {
        if (IsPlayerWine())
        {
            _playerHandler.ToWine();
            _canvas.ShowMatchResults(_killsAccount, _health, "You won");
        }
    }

    private void CreateBots(int botsAccount)
    {
        _bots = new GameObject[botsAccount];
        for (int i = 0; i < botsAccount; ++i)
        {
            GameObject bot = PhotonNetwork.Instantiate(_botPrefabName,
                GetPosition(), Quaternion.identity);
            _bots[i] = bot;

            Destroy(bot.GetComponent<PlayerHandler>());

            var botHandler = bot.GetComponent<BotHandler>();
            botHandler.enabled = true;

            botHandler.SetName(String.Format("Don_Bot_{0}", i));
        }
    }

    private void DestroyBots()
    {
        for (int i = 0; i < _bots.Length; ++i)
        {
            _bots[i].SetActive(false);
            PhotonNetwork.Destroy(_bots[i]);
            _bots[i] = null;
        }
        _bots = null;
    }

    private void OnDestroy()
    {
        _canvas = null;
    }

    private bool IsPlayerWine()
    {
        var healthes =
           (HealthSystem[])GameObject.FindObjectsOfType(typeof(HealthSystem));
        int aliveCount = 0;
        for (int i = 0; i < healthes.Length; ++i)
        {
            if (0 < healthes[i].GetHealth())
            {
                ++ aliveCount;
                if (aliveCount > 1)
                    return false;
            }
        }
        return true;
    }


    private GameObject _player;
    private PlayerHandler _playerHandler;

    private GameObject _location;
    private IGameManagerObserver _canvas;

    private GameObject[] _bots;

    internal void SetObserver(IGameManagerObserver canvasManager)
    {
        _canvas = canvasManager;
    }

    public void OnUpdateHealth(int health)
    {
        SetHealth(health);
        if (0 == health)
        {
            Die();
        }
    }

    public void IncreaseKillCounter(int killsAccount)
    {
        SetKills(killsAccount);
    }

    private int _health;
    private int _killsAccount;
}
