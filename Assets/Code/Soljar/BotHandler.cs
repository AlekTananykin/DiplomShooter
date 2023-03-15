using Photon.Pun;
using System;
using UnityEngine;

public class BotHandler : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private float _horizontalSpeed = 150.0f;
    [SerializeField]
    private float _verticalSpeed = 10.0f;

    [SerializeField]
    private float _gunHeight = 1.2f;
    [SerializeField]
    private float _directionEdition = 9.0f;

    [SerializeField]
    private float _speedRotation = 50f;

    [SerializeField]
    private float _watchDistance = 100.0f;
    [SerializeField]
    private float _attackDistance = 10.0f;

    // Start is called before the first frame update
    void Start()
    {

        _stepSound = gameObject.GetComponent<StepSound>();

        _isDead = false;

        _soljaAnimation = gameObject.GetComponent<SoljsAnimation>();
        _combatSystem = gameObject.GetComponent<CombatSystem>();
        _combatSystem.OnNewKillSuccess += NewKillSuccess;

        _healthSystem = gameObject.GetComponent<HealthSystem>();
        _healthSystem.UpdateHealth += UpdateHealth;

        _rigidbody = gameObject.GetComponent<Rigidbody>();

        FindPlayers();

    }

    void Update()
    {
        if (_isDead)
            return;

        _stepSound.UpdateTimer();

        if (IsGrounded())
        {
            SelectTarget();
            if (_rigidbody.velocity.magnitude > 0.1)
            {
                _soljaAnimation.StartMove(_rigidbody.velocity);
            }
            else
                _soljaAnimation.Idle();
        }
    }

    private bool IsGrounded()
    {
        if (Physics.Raycast(
            transform.position + Vector3.up * 0.1f, Vector3.down, .2f, 3))
        {
            return true;
        }
        return false;
    }

    public void ToDie()
    {
        _isDead = true;

        _rigidbody.isKinematic = true;
        var collider = gameObject.GetComponent<CapsuleCollider>();
        collider.enabled = false;
        _soljaAnimation.Die();

        _healthSystem.UpdateHealth -= UpdateHealth;
        _combatSystem.OnNewKillSuccess -= NewKillSuccess;
    }

    public void NewKillSuccess(string killedName)
    {
        ++_killsAccount;
    }

    public void UpdateHealth(int health)
    {
        Debug.Log(String.Format("{0}: health {1}", _botName, health));
        if (health <= 0)
        {
            ToDie();
        }
    }

    public void SetName(string botName)
    {
        _botName = botName;
    }

    void FindPlayers()
    {
        _players =
            (PlayerHandler[])GameObject.FindObjectsOfType(typeof(PlayerHandler));
    }

    PlayerHandler _lastPlayer;

    void SelectTarget()
    {
        _players =
            (PlayerHandler[])GameObject.FindObjectsOfType(typeof(PlayerHandler));

        PlayerHandler targetPlayer = null;

        for (int i = 0; i < _players.Length; ++i)
        {
            if (_players[i] == _lastPlayer)
            {
                targetPlayer = _lastPlayer;
                break;
            }
        }

        if (null == targetPlayer || 0 == targetPlayer.GetComponent<HealthSystem>().GetHealth())
        {
            targetPlayer = _players[_currentPlayer];
            _currentPlayer = (_currentPlayer + 1) % _players.Length;
        }

        if (null == targetPlayer || 0 == targetPlayer.GetComponent<HealthSystem>().GetHealth())
            return;

        Vector3 playerVector = targetPlayer.GetPosition() -
            _rigidbody.position;
      
        Vector3 forwardVector = 
            gameObject.transform.TransformDirection(Vector3.forward).normalized;


        if (Vector3.Dot(playerVector.normalized, forwardVector) < 0.5 || 
            playerVector.magnitude > _watchDistance)
            return;

        if (playerVector.magnitude > _attackDistance)
        {
            ToFollowThePlayer(playerVector);

            _rigidbody.AddForce(playerVector.normalized * _horizontalSpeed, 
                ForceMode.Impulse);

            if (_stepSound.IsReadyToSound())
                _stepSound.PlayStepSound();
        }
        else
        {
            forwardVector.y = 0;
            Vector3 startPoint = _rigidbody.position + 
                forwardVector.normalized * 0.6f +
                Vector3.up * _gunHeight;

            _combatSystem.Shot(_botName, startPoint, 
                playerVector + Vector3.up * _directionEdition);
        }
    }

    private void ToFollowThePlayer(Vector3 playerDirection)
    {
        Quaternion lookRotation = Quaternion.LookRotation(
            playerDirection.normalized);
        lookRotation.x = 0;
        lookRotation.z = 0;
        _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation,
            lookRotation, Time.deltaTime * _speedRotation);
    }

    private StepSound _stepSound;
    private SoljsAnimation _soljaAnimation;
    private CombatSystem _combatSystem;
    private HealthSystem _healthSystem;

    private Rigidbody _rigidbody;

    private bool _isDead = false;
    private int _killsAccount = 0;
    private string _botName;

    PlayerHandler[] _players;
    int _currentPlayer;

    private AudioSource _audioSource;
}
