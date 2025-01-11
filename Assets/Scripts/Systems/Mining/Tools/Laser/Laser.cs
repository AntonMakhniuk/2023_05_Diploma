//using System.Collections;
//using Resource_Nodes;
//using Tools.Base_Tools;
//using UnityEngine;

//namespace Tools.Laser
//{
//    public class Laser : BaseTurret
//    {
//        [SerializeField] private float laserDamagePerSecond = 100f;
//        [SerializeField] private LineRenderer beam;
//        [SerializeField] private Transform target;

//        private IEnumerator _shootCoroutine;
//        private bool _isShooting;


//        private void Awake()
//        {
//            beam.enabled = false;
//            beam.startWidth = 0.1f;
//            beam.endWidth = 0.1f;

//            _shootCoroutine = ShootCoroutine();
//        }

//private IEnumerator ShootCoroutine()
//{
//    while (true)
//    {
//        var beamEndPosition =
//            LookAtHitData?.point ?? muzzlePoint.position + transform.forward * maxRange;

//        beam.SetPosition(0, muzzlePoint.position);
//        beam.SetPosition(1, beamEndPosition);

//        if (LookAtHitData.HasValue && LookAtHitData.Value.collider != null)
//        {
//            if (LookAtHitData.Value.collider.TryGetComponent<IDestructible>(out var destructible))
//            {
//                Debug.Log(LookAtHitData.Value + " " + LookAtHitData.Value.transform);
//                destructible.OnLaserInteraction(laserDamagePerSecond * Time.deltaTime);
//            }
//            else
//            {
//                Enemy enemy = LookAtHitData.Value.collider.GetComponent<Enemy>();
//                if (enemy != null)
//                {
//                    enemy.TakeDamage(laserDamagePerSecond * Time.deltaTime);
//                }
//                else
//                {
//                    HomingRocket rocket = LookAtHitData.Value.collider.GetComponent<HomingRocket>();
//                    if (rocket != null)
//                    {
//                        rocket.OnLaserInteraction(laserDamagePerSecond * Time.deltaTime);
//                    }
//                }
//            }
//        }

//        yield return null;
//    }
//}


//        protected override void PrimaryActionStarted()
//        {
//            if (_isShooting)
//            {
//                return;
//            }

//            beam.enabled = true;
//            StartCoroutine(_shootCoroutine);
//            _isShooting = true;
//        }

//        protected override void PrimaryActionPerformed()
//        {
//        }

//        protected override void PrimaryActionCanceled()
//        {
//            if (!_isShooting)
//            {
//                return;
//            }

//            beam.enabled = false;
//            StopCoroutine(_shootCoroutine);
//            _isShooting = false;
//        }

//        protected override void SecondaryActionStarted()
//        {
//        }

//        protected override void SecondaryActionPerformed()
//        {
//        }

//        protected override void SecondaryActionCanceled()
//        {
//        }

//        protected override void ThirdActionStarted()
//        {
//        }

//        protected override void ThirdActionPerformed()
//        {
//        }

//        protected override void ThirdActionCanceled()
//        {
//        }
//    }
//}
