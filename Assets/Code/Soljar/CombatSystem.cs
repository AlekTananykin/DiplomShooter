using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [SerializeField]
    private string _bulletPrefabName;
    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField]
    private float _bulletshift = 2.0f;
    [SerializeField]
    private float _bulletSpeed = 10.0f;
    [SerializeField]
    private int _bulletDamage = 10;
    [SerializeField]
    private int _bullets = 10;

    private void Start()
    {
        for (int i = 0; i < _bullets; ++i)
        {
            //GameObject bullet = PhotonNetwork.Instantiate(_bulletPrefabName,
             //    GetInitBulletPosition(), Quaternion.identity);

            if (null == _bulletPrefab)
                _bulletPrefab = (UnityEngine.GameObject)Resources.Load(_bulletPrefabName);

            GameObject bullet = Instantiate(_bulletPrefab,
                 GetInitBulletPosition(), Quaternion.identity);

            AddBulletToStorage(bullet);
            bullet.transform.SetParent(gameObject.transform);
        }
    }

    public void Shot(string killerName, Vector3 point, Vector3 direction)
    {
        var bulletHandler = CreateNewBullet();

        if (null == bulletHandler)
            return;

        Ray ray = new Ray(point, direction);
        bulletHandler.Shot(ref ray, _bulletshift,
            _bulletSpeed, _bulletDamage, killerName);
    }

    private void Update()
    {
        if (_resShotTime > 0)
            _resShotTime -= Time.deltaTime;
    }

    private void OnDestroy()
    {
        BulletHandler[] bullets = GetComponentsInChildren<BulletHandler>();
        foreach (BulletHandler bullet in bullets)
        {
            bullet.transform.SetParent(null);
        }
    }

    public Action<string> OnNewKillSuccess;

    internal void SetKilledName(string killedName)
    {
        OnNewKillSuccess(killedName);
    }

    List<GameObject> _bulletsStorage = new List<GameObject>();
    //private UnityEngine.Object _bulletPrefab;

    private BulletHandler CreateNewBullet()
    {
        GameObject bullet;
        if (_bulletsStorage.Count > 0)
        {
            bullet = _bulletsStorage[0];
            _bulletsStorage.RemoveAt(0);
            bullet.SetActive(true);
        }
        else
        {
            return null;
            //if (null == _bulletPrefab)
            //    _bulletPrefab = Resources.Load(_bulletPrefabName);

            //bullet = (GameObject)Instantiate(_bulletPrefab,
            //   GetInitBulletPosition(), Quaternion.identity);

        }

        var bulletHandler = bullet.GetComponent<BulletHandler>();
        bulletHandler.SetSource(this);

        return bulletHandler;
    }

    public void AddBulletToStorage(GameObject bullet)
    {
        bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        bullet.gameObject.SetActive(false);
        //bullet.GetComponent<Rigidbody>().position = -100f * Vector3.up;
        _bulletsStorage.Add(bullet);
    }

    private Vector3 GetInitBulletPosition()
    {
        return transform.position + Vector3.down * 1000.0f;
    }

    private float _resShotTime;
}
