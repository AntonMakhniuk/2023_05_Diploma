using System.Collections.Generic;
using Systems.Mining.Tools.Bomb_Launcher;
using Tools.Base_Tools;
using UnityEngine;

namespace Tools.Bomb_Launcher
{
    // TODO: Change it so holding down LMB makes the bomb fire farther, adjustable fire length style
    public class BombLauncher : BaseTurret
    {
        private const float BombSpeed = 5f;
        private const float BombLifetime = 3f;
        
        [SerializeField] private GameObject bombPrefab;
        [SerializeField] private float explosionRange = 5f;
        
        private readonly List<Bomb> _activeBombs = new();

        private void OnDrawGizmos()
        {
            if (!IsActiveTool)
            {
                return;
            }
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRange);
        }


        protected override void PrimaryActionStarted()
        {
            // No action on start
        }

        // Spawn a bomb
        protected override void PrimaryActionPerformed()
        {
            var bombObject = Instantiate(bombPrefab, muzzlePoint.position, muzzlePoint.rotation);
            bombObject.GetComponent<Rigidbody>().velocity = muzzlePoint.forward * BombSpeed;

            var bombComponent = bombObject.GetComponent<Bomb>();
            bombComponent.OnBombDestroyed += HandleBombDestroyed;
            _activeBombs.Add(bombComponent);
            
            Destroy(bombObject, BombLifetime);
        }

        protected override void PrimaryActionCanceled()
        {
            // No action on end
        }

        protected override void SecondaryActionStarted()
        {
            // No action on start
        }
        
        // Detonate all bombs
        protected override void SecondaryActionPerformed()
        {
            foreach (var bomb in _activeBombs)
            {
                bomb.Detonate();
            }
        }

        protected override void SecondaryActionCanceled()
        {
            // No action on end
        }

        protected override void ThirdActionStarted()
        {
            // No third action
        }

        protected override void ThirdActionPerformed()
        {
            // No third action
        }

        protected override void ThirdActionCanceled()
        {
            // No third action
        }

        private void HandleBombDestroyed(object sender, Bomb bomb)
        {
            bomb.OnBombDestroyed -= HandleBombDestroyed;
            _activeBombs.Remove(bomb);
        }
        
        protected override void OnDestroy()
        {
            foreach (var bomb in _activeBombs)
            {
                bomb.OnBombDestroyed -= HandleBombDestroyed;
            }
            
            base.OnDestroy();
        }
    }
}