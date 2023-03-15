using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [Tooltip("The hight we want camera to be above the character center.")]
    [SerializeField]
    float _height = 1.0f;

    [Tooltip("Set this as false if a component of a prefab being instanciated by Photon Network, and manually call OnStartFollowing() when and if needed.")]
    [SerializeField]
    private bool _isFollowOnStart = false;

    [SerializeField]
    public float _sensVert = 9.0f;

    [SerializeField]
    public float _minVert = -45.0f;
    [SerializeField]
    public float _maxVert = 45.0f;

    void Start()
    {
        if (_isFollowOnStart)
        {
            OnStartFollowing();
        }
    }

    public void OnStartFollowing()
    {
        _camera = Camera.main;

        _cameraTransform = _camera.transform;
        _cameraTransform.SetParent(this.gameObject.transform);
        _cameraTransform.transform.localPosition = 
            new Vector3(0.0f, _height, 0.0f);
    }

    public void VerticalRotate(float mouseMove)
    {
        if (null == _cameraTransform)
            return;

        _rotationX -= mouseMove * _sensVert;
        _rotationX = Mathf.Clamp(_rotationX, _minVert, _maxVert);
        _cameraTransform.localEulerAngles = new Vector3(_rotationX, 0, 0);
    }

    public Ray GetCameraRay()
    {
        Vector3 point = new Vector3(
             _camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);

        return _camera.ScreenPointToRay(point);
    }

    public void FreeCamera()
    {
        if (null != _cameraTransform)
            _cameraTransform.SetParent(null);

        _cameraTransform = null;
    }

    private Transform _cameraTransform = null;
    private float _rotationX = 0;
    private Camera _camera;
}
