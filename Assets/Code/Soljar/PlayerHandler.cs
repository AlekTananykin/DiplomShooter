using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider), 
    typeof(CameraHandler))]
public class PlayerHandler : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("Move system")]
    [SerializeField]
    private float _sensHor = 9.0f;
    [SerializeField]
    private float _horizontalSpeed = 5.0f;
    [SerializeField]
    private float _verticalSpeed = 10.0f;
    [SerializeField]
    private GameObject[] _nonActiveElements;


    void Start()
    {
        _stepSound = gameObject.GetComponent<StepSound>();
        _soljaAnimation = gameObject.GetComponent<SoljsAnimation>();
        _combatSystem = gameObject.GetComponent<CombatSystem>();
        _combatSystem.OnNewKillSuccess += NewKillSuccess;

        _healthSystem = gameObject.GetComponent<HealthSystem>();
        _healthSystem.UpdateHealth += UpdateHealth;

        _cameraHandler = gameObject.GetComponent<CameraHandler>();
        _rigidbody = gameObject.GetComponent<Rigidbody>();

        if (photonView.IsMine)
        {
            _cameraHandler.OnStartFollowing();
            SwitchToPlayerMode();
        }
    }

    private void SwitchToPlayerMode()
    {
        for (int i = 0; i < _nonActiveElements.Length; ++i)
            _nonActiveElements[i].SetActive(false);
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            _soljaGameManager.CheckPlayerWine();
            _stepSound.UpdateTimer();

            if (!_isInputEnabled)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _soljaGameManager.GiveUp();
                return;
            }

            ProcessMoveInputs();
            ProcessFireInputs();
        }
    }

    private void OnDestroy()
    {
        if (null != _cameraHandler)
            _cameraHandler.FreeCamera();

        if (null != _healthSystem)
            _healthSystem.UpdateHealth -= UpdateHealth;

        if (null != _combatSystem)
            _combatSystem.OnNewKillSuccess -= NewKillSuccess;

        _soljaAnimation = null;
        _combatSystem = null;
        _healthSystem = null;
        _soljaGameManager = null;

        _cameraHandler = null;
        _rigidbody = null;
    }

    public void SetGameManager(ISoljaGameManager gameManager)
    {
        _soljaGameManager = gameManager;
        if (null != _soljaGameManager)
        {
            _soljaGameManager.SetKills(_killsAccount);

            _healthSystem = gameObject.GetComponent<HealthSystem>();
            _soljaGameManager.SetHealth(_healthSystem.GetHealth());
        }
    }

    private void ProcessMoveInputs()
    {
        _cameraHandler.VerticalRotate(
            Input.GetAxis("Mouse Y"));

        this.HorizontalRotation(Input.GetAxis("Mouse X"));

        if (!IsGrounded())
            return;

        float jumpEnforce = Input.GetAxis("Jump");
        if (jumpEnforce > 0)
        {
            _soljaAnimation.Jump();
            _rigidbody.AddForce(
                Vector3.up * jumpEnforce * _verticalSpeed, 
                ForceMode.VelocityChange);

            //if (_stepSound.IsReadyToSound())
            //{
             //   photonView.RPC("StepSound", RpcTarget.All);
            //}

            return;
        }

        float deltaX = Input.GetAxis("Horizontal");
        float deltaZ = Input.GetAxis("Vertical");

        if (0 == deltaX && 0 == deltaZ)
            return;

        Vector3 direction = new Vector3(deltaX, 0, deltaZ);
        _soljaAnimation.StartMove(direction.normalized);

        Vector3 movement = transform.TransformDirection(
                direction.normalized) * _horizontalSpeed;

        _rigidbody.AddForce(movement, ForceMode.VelocityChange);

        if (_stepSound.IsReadyToSound())
        {
            //photonView.RPC("PlayerStepSound", RpcTarget.All);
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

    private void HorizontalRotation(float mouseX)
    {
        float delta = mouseX * _sensHor;

        float rotationY = transform.localEulerAngles.y + delta;
        transform.localEulerAngles = new Vector3(0, rotationY, 0);
    }

    private void ProcessFireInputs()
    {
        const int fireButton = 0;
        if (Input.GetMouseButtonDown(fireButton) || Input.GetKey(KeyCode.F))
        {
            var ray = _cameraHandler.GetCameraRay();
            _combatSystem.Shot(PhotonNetwork.NickName, ray.origin, ray.direction);
        }
    }

    public void ToDie()
    {
        _isInputEnabled = false;
        _cameraHandler.FreeCamera();
        _rigidbody.isKinematic = true;
        var collider = gameObject.GetComponent<CapsuleCollider>();
        collider.enabled = false;
        _soljaAnimation.Die();   
    }

    public void ToWine()
    {
        _isInputEnabled = false;
        _cameraHandler.FreeCamera();
        _rigidbody.isKinematic = true;
        var collider = gameObject.GetComponent<CapsuleCollider>();
        collider.enabled = false;
    }

    public void NewKillSuccess(string killedName)
    {
        _soljaGameManager.IncreaseKillCounter(++_killsAccount);
    }

    public void UpdateHealth(int health)
    {
        _soljaGameManager.OnUpdateHealth(health);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("Serilalize");
    }

    public Vector3 GetPosition()
    {
        return _rigidbody.position;
    }

    //[PunRPC]
    //private void PlayerStepSound()
    //{
    //    _stepSound.PlayStepSound();
    //}

    private StepSound _stepSound;
    private SoljsAnimation _soljaAnimation;
    private CombatSystem _combatSystem;
    private HealthSystem _healthSystem;
    private ISoljaGameManager _soljaGameManager;

    private CameraHandler _cameraHandler;
    private Rigidbody _rigidbody;
    private bool _isInputEnabled = true;

    private int _killsAccount = 0;


}
