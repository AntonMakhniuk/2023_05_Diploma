using Assets.Scripts.Instruments;
using UnityEngine;

public class LaserV3 : Instrument
{
    [SerializeField] private Transform laserBarrel;
    [SerializeField] private Transform laserLeg;
    [SerializeField] private float rotationSpeed;
    
    [SerializeField] private LineRenderer _beam;
    [SerializeField] private Transform _muzzlePoint;
    [SerializeField] private float maxLenght;
    
       private PlayerInputActions _playerInputActions;
       private Rigidbody _rb;
       private Transform _mainCamera;
       
       private void Awake()
       {
           _playerInputActions = new PlayerInputActions();
           if (Camera.main != null) _mainCamera = Camera.main.transform;

           _playerInputActions.PlayerCamera.Enable();
           
           _beam.enabled = false;
           _beam.startWidth = 0.1f;
           _beam.endWidth = 0.1f;
       }
       
       private void Activate()
       {
           _beam.enabled = true;
       }
    
       private void Deactivate()
       {
           _beam.enabled = false;
           var position = _muzzlePoint.position;
           _beam.SetPosition(0, position);
           _beam.SetPosition(1, position);
       }
   
       private void Update() {
           RotateWithCamera();
           
           if (Input.GetMouseButtonDown(0))
           {
               Activate();
           }
           else if (Input.GetMouseButtonUp(0))
           {
               Deactivate();
           }
       }
       
       private void FixedUpdate()
       {
           if (!_beam.enabled)
           {
               return;
           }

           Ray ray = new Ray(_muzzlePoint.position, _muzzlePoint.forward);
           bool cast = Physics.Raycast(ray, out RaycastHit hit, maxLenght);
           Vector3 hitPosition = cast ? hit.point : _muzzlePoint.position + _muzzlePoint.forward * maxLenght;
           _beam.SetPosition(0, _muzzlePoint.position);
           _beam.SetPosition(1,hitPosition);
           if (cast && hit.collider.CompareTag("AsteroidPoint"))
           {
               CheckAndDestroyAsteroidPoint(hit.collider);
           }
       }
       private void CheckAndDestroyAsteroidPoint(Collider collider)
       {
           if (collider.CompareTag("AsteroidPoint"))
           {
               Destroy(collider.gameObject);

               Asteroid asteroid = collider.transform.parent.GetComponent<Asteroid>();
               asteroid.OnAsteroidPointDestroyed();
           }
       }
   
       public void RotateWithCamera() 
       {
           var eulerAngles = _mainCamera.eulerAngles;
           Quaternion targetRotationLeg = Quaternion.Euler(0, eulerAngles.y, 0);
           laserLeg.rotation = Quaternion.Lerp(laserLeg.rotation, targetRotationLeg, rotationSpeed * Time.deltaTime);
           
           Quaternion targetRotationBarrel = Quaternion.Euler(eulerAngles.x, laserLeg.eulerAngles.y, 0);
           laserBarrel.rotation = Quaternion.Lerp(laserBarrel.rotation, targetRotationBarrel, rotationSpeed * Time.deltaTime);
       }
}
