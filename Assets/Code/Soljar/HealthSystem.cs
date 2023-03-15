using Photon.Pun;
using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IPunObservable
{
    [SerializeField]
    private int _healthAccount;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent(out BulletHandler bulletHandler))
            return;

        _healthAccount -= bulletHandler.Damage;

        if (_healthAccount <= 0)
        {
            _healthAccount = 0;
            bulletHandler.SetKilledName(NickName);
        }

        UpdateHealth?.Invoke(_healthAccount);
    }

    public Action<int> UpdateHealth;
    public string NickName { get; private set; }

    internal int GetHealth()
    {
        return _healthAccount;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this._healthAccount);
        }
        else
        {
            this._healthAccount = (int)stream.ReceiveNext();
        }
    }
}
