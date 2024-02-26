using System;
using System.Collections;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapon.Scripts
{
    public class WeaponShooting : MonoBehaviour
    {
        [Header("Settings", order = 0)]
        [SerializeField] private bool showTracers = true;

        [Header("Components")] 
        [SerializeField] private TMP_Text bulletCount;
        [SerializeField] private TrailRenderer  bulletTracer;
        [SerializeField] private ParticleSystem bulletImpactOnWall;
        [SerializeField] private Transform      muzzlePoint;
        private                  Camera         _mainCamera;
        private                  Transform      _mainCameratransform;
        private                  RaycastHit     _targetHit;
        //private LayerMask  _enemyLayer;

        [Header("Gun Variables")] 
        private float _timeBetweenEachShotBurst = .1f;
        // private float _damagePerBullet = 1f;
        private float   _spreadRadiusAt10Meters = .1f;
        private int     _numberOfAditionalShotsPerBurst;
        private float   _timeBetweenEachShotInABurst;
        private int     _numberOfAditionalBulletsPerShot;
        private int     _fullMagazineCapacity = 30;
        private int     _bulletsLeftInMag;
        private int     _bulletsLeftInReserve;
        private float   _reloadTimeInSeconds           = 2f;
        private float   _range                         = 100f;
        private bool    _isFullAuto                    = true;
        private float   _bulletTracerFrequencyOutOf100 = 80f;

        [Header("Gun Status")]
        private bool _isShooting;
        private bool _isReloading;
        private bool _isReadyToShoot = true;


        private void Awake()
        {
            _mainCamera = Camera.main;
            if (_mainCamera is not null) _mainCameratransform = _mainCamera.transform;
        }

        private void Start()
        {
            _bulletsLeftInMag     = _fullMagazineCapacity;
            _bulletsLeftInReserve = 3 * _fullMagazineCapacity;
        }

        private void Update()
        {
            GetInput();
            bulletCount.text = _bulletsLeftInMag + " / " + _bulletsLeftInReserve;
        }

        private void GetInput()
        {
            _isShooting = _isFullAuto ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);

            if (_isReloading) return;
            
            if (Input.GetKeyDown(KeyCode.R) && _bulletsLeftInMag < _fullMagazineCapacity)
                Reload();

            if (_isShooting && _isReadyToShoot && _bulletsLeftInMag > 0)
                Shoot();
        }

        private void Shoot()
        {
            _isReadyToShoot = false;

            ShootASingleBullet();
            
            --_bulletsLeftInMag;
            Invoke(nameof(ResetTimeBetweenShot), _timeBetweenEachShotBurst);

            for (var i = 0; i < _numberOfAditionalBulletsPerShot; i++)
            {
                ShootASingleBullet();
            }

            for (var i = 0; i < _numberOfAditionalShotsPerBurst; ++i)
            {
                Invoke(nameof(ShootASingleBullet), _timeBetweenEachShotInABurst * i);
                --_bulletsLeftInMag;
            }
        }

        private void ShootASingleBullet()
        {
            var x             = Random.Range(-_spreadRadiusAt10Meters, _spreadRadiusAt10Meters);
            var y             = Random.Range(-_spreadRadiusAt10Meters, _spreadRadiusAt10Meters);
            var shotDirection = _mainCameratransform.forward*10f + _mainCameratransform.right * x + _mainCameratransform.up * y;

            if (showTracers)
            {
                if (Random.Range(1, 101) > _bulletTracerFrequencyOutOf100) return;
                var tracer = Instantiate(bulletTracer, muzzlePoint.position, Quaternion.identity);
                StartCoroutine(InstantiateTracer(tracer, _mainCameratransform.position + shotDirection * _range));
            }
                
            
            if (Physics.Raycast(_mainCamera.transform.position, shotDirection.normalized, out _targetHit, _range))
            {
                //Do the shoot thing
                Instantiate(bulletImpactOnWall, _targetHit.point, Quaternion.LookRotation(_targetHit.normal));
            }
        }

        private IEnumerator InstantiateTracer(TrailRenderer tracer, Vector3 target)
        {
            var time   = 0f;
            while (time < 1f)
            {
                tracer.transform.position =  Vector3.Lerp(muzzlePoint.position, target, time);
                time                            += Time.deltaTime / tracer.time;
                yield return null;
            }
            tracer.transform.position = target;
            Destroy(tracer, tracer.time);
        }

        private void ResetTimeBetweenShot()
        {
            _isReadyToShoot = true;
        }

        private void Reload()
        {
            _isReloading          =  true;
            _bulletsLeftInReserve += _bulletsLeftInMag;
            _bulletsLeftInMag     =  0;
            Invoke(nameof(ReloadFinished), _reloadTimeInSeconds);
        }

        private void ReloadFinished()
        {
            _isReloading      = false;
            var bulletsReloaded = Mathf.Clamp(_fullMagazineCapacity, 0, _bulletsLeftInReserve);
            _bulletsLeftInMag     = bulletsReloaded;
            _bulletsLeftInReserve -= bulletsReloaded;
        }
    }
}
