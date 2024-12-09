using DG.Tweening;
using Player.Inventory;
using UnityEngine;

namespace Player.Ship.Tools.Marker
{
    public class MarkerBounds : MonoBehaviour
    {
        [SerializeField] private float fadeTime = 1f;

        private void Start()
        {
            transform.localScale = new Vector3(0, 0, 0);
        }

        public void StartBounds(float targetScale, float timeToScale)
        {
            transform.localScale = new Vector3(0,0,0);
            transform.DOScale(targetScale, timeToScale).OnComplete(() =>
            {
                GetComponent<Renderer>().material.DOFade(0, fadeTime)
                    .OnComplete(() => Destroy(gameObject));
            });
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Collectable>(out var collectable) 
                || collectable.state != CollectableState.Unmarked)
            {
                return;
            }
                
            collectable.TurnOnMark();
        }
    }
}