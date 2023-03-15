using Photon.Pun;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class BulletHandler : MonoBehaviour//MonoBehaviourPunCallbacks
{
    [SerializeField]
    AudioClip _shotSound;
    [SerializeField]
    AudioClip _burstSound;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //if (!photonView.IsMine)
         //   return;

        if (GetComponent<Rigidbody>().position.y < 0)
        {
            _combatSystem.AddBulletToStorage(gameObject);
            //photonView.RPC("PlayBurstSound", RpcTarget.);
            PlayBurstSound();
        }
    }

    internal void SetSource(CombatSystem combatSystem)
    {
        _combatSystem = combatSystem;
    }

    internal void Shot(ref Ray ray, float bulletShift, float speed, int damage, string killerName)
    {
        Damage = damage;
        KillerName = killerName;

        var rigidBody = GetComponent<Rigidbody>();

        rigidBody.position = ray.GetPoint(bulletShift);

        Debug.DrawRay(rigidBody.position, ray.direction, Color.blue);

        rigidBody.AddForce(
            ray.direction * speed, ForceMode.VelocityChange);

        PlayShotSound();
        //photonView.RPC("PlayShotSound", RpcTarget.All);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //photonView.RPC("PlayBurstSound", RpcTarget.All);
        _combatSystem.AddBulletToStorage(gameObject);
    }

    public void SetKilledName(string killedName)
    {
        _combatSystem.SetKilledName(killedName);
        Debug.Log($"Killed name {killedName}");
    }


    private void PlayShotSound()
    {
        if (null == _audioSource)
        {
            _audioSource = GetComponent<AudioSource>();
        }
        _audioSource.volume = 0.5f;
        _audioSource.PlayOneShot(_shotSound);
    }


    private void PlayBurstSound()
    {
        if (null == _audioSource)
        {
            _audioSource = GetComponent<AudioSource>();
        }
        _audioSource.volume = 1.0f;
        _audioSource.PlayOneShot(_burstSound);
    }

    private CombatSystem _combatSystem;

    public int Damage { get; internal set; }
    public string KillerName { get; internal set; }

    private AudioSource _audioSource;
}
