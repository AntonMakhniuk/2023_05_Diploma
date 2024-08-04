using System.Collections.Generic;
using Tools.Base_Tools;
using UnityEngine;

namespace Tools.Bomb_Launcher
{
    public class BombLauncher : BaseTurret
    {
        private const float BombSpeed = 5f;
        private const float BombLifetime = 3f;
        
        [SerializeField] private GameObject bombPrefab;
        [SerializeField] private Transform muzzlePoint;
        [SerializeField] private float bombRange = 5f;
        
        private readonly List<Bomb> _activeBombs = new();

        private void OnDrawGizmos()
        {
            if (!IsActiveTool)
            {
                return;
            }
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, bombRange);
        }

        // Spawn a bomb
        protected override void PrimaryAction()
        {
            var bombObject = Instantiate(bombPrefab, muzzlePoint.position, muzzlePoint.rotation);
            bombObject.GetComponent<Rigidbody>().velocity = muzzlePoint.forward * BombSpeed;

            var bombComponent = bombObject.GetComponent<Bomb>();
            bombComponent.OnBombDestroyed += HandleBombDestroyed;
            _activeBombs.Add(bombComponent);
            
            Destroy(bombObject, BombLifetime);
        }

        private void HandleBombDestroyed(object sender, Bomb bomb)
        {
            bomb.OnBombDestroyed -= HandleBombDestroyed;
            _activeBombs.Remove(bomb);
        }
        
        // Detonate all bombs
        protected override void SecondaryAction()
        {
            foreach (var bomb in _activeBombs)
            {
                bomb.Detonate();
            }
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