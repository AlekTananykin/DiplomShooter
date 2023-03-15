using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SoljsAnimation : MonoBehaviour
{
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();

        _animator.SetTrigger(_idle);
        _previousState = _idle;
    }

    void MoveAnimation(Vector3 velocity)
    {
        float dotValue = Vector3.Dot(velocity.normalized,
            transform.forward.normalized);

        if (dotValue > 0.5)
        {
            SwitchToState(_walk_front);
            return;
        }
        if (dotValue < -0.5)
        {
            SwitchToState(_walk_back);
            return;
        }

        float crossValueY = Vector3.Cross(
            velocity.normalized, transform.forward.normalized).y;

        if (crossValueY > 0)
        {
            SwitchToState(_walk_left);
        }
        else
        {
            SwitchToState(_walk_right);
        }
    }

    public void Idle()
    {
        SwitchToState(_idle);
    }

    public void Die()
    {
        SwitchToState(_die);
    }

    public void Shoot()
    {
        SwitchToState(_shoot);
    }

    public void StartMove(Vector3 velocity)
    {
        MoveAnimation(velocity);
    }

    public void Jump()
    {
        SwitchToState(_jump);
    }

    private void SwitchToState(string newStatate )
    {
        if (_previousState == newStatate || 
            _die == _previousState)
            return;

        _animator.SetTrigger(newStatate);
        _previousState = newStatate;
    }

    private Animator _animator;

    private const string _idle = "idle";
    private const string _walk_left = "walk_left";
    private const string _walk_front = "walk_front";
    private const string _walk_right = "walk_right";
    private const string _walk_back = "walk_back";

    private const string _jump = "jump";
    private const string _die = "die";
    private const string _shoot = "shoot";

    private string _previousState;
}
