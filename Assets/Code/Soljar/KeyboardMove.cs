using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class KeyboardMove : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    SoljsAnimation _animation;


    [SerializeField]
    private float _sensHor = 9.0f;
    [SerializeField]
    public float _speed = 5.0f;
    private const float _gravity = -9.8f;
    [SerializeField] private float _jumpSpeed = 12f;

    private float _vertSpeed = 0;

    [SerializeField]
    GameObject[] _nonActiveElements;

    private CharacterController _charController;

    private void SwitchToPlayerMode()
    {
        for (int i = 0; i < _nonActiveElements.Length; ++i)
            _nonActiveElements[i].SetActive(false);
    }

    void Start()
    {
        _charController = GetComponent<CharacterController>();
        _vertSpeed = 0;

        _cameraHandler = gameObject.GetComponent<CameraHandler>();

        if (_cameraHandler != null)
        {
            if (photonView.IsMine)
            {
                _cameraHandler.OnStartFollowing();

                SwitchToPlayerMode();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><b>Missing</b></Color> CameraWork Component on player Prefab.", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            _cameraHandler.VerticalRotate(Input.GetAxis("Mouse Y"));

            this.HorizontalRotation(Input.GetAxis("Mouse X"));

            float deltaX = Input.GetAxis("Horizontal") * _speed;
            float deltaZ = Input.GetAxis("Vertical") * _speed;
            Vector3 movement = new Vector3(deltaX, 0, deltaZ);
            movement = Vector3.ClampMagnitude(movement, _speed);
           

            Debug.Log($"movement.magnitude : {movement.magnitude}, _vertSpeed: {_vertSpeed}, ");

            if (movement.magnitude < 0.1)
            {
                
                _animation.Idle();
            }
            else if (Mathf.Abs(_vertSpeed) > 0.1)
                _animation.Jump();
            else
                _animation.StartMove(movement);

            movement *= Time.deltaTime;
            movement = ProcessVerticalMove(movement);
            movement = transform.TransformDirection(movement);
            _charController.Move(movement);

            

        }
    }

    private void HorizontalRotation(float mouseX)
    {
        float delta = mouseX * _sensHor;

        float rotationY = transform.localEulerAngles.y + delta;
        transform.localEulerAngles = new Vector3(0, rotationY, 0);
    }

    private Vector3 ProcessVerticalMove(Vector3 movement)
    {
        if (_charController.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                _vertSpeed = _jumpSpeed;
            }
            else
                _vertSpeed = 0;
        }
        else
            _vertSpeed += _gravity * Time.deltaTime;

        return new Vector3(movement.x, _vertSpeed * Time.deltaTime, movement.z);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("Serilalize");
    }

    private CameraHandler _cameraHandler;
}
