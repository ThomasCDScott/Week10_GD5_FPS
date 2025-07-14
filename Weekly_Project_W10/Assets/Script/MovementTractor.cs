using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class MovementTractor : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    private Camera _camera;
    private Ray _ray;
    private RaycastHit _hit;
    private static readonly int _GROUND_LAYER = 1 << 6;
    public Transform[] plantingSpots;
    public GameObject plantPrefab;
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _camera = Camera.main;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, 1000f, _GROUND_LAYER))
            {
                _agent.destination = _hit.point;
                PlantAtLocation(_hit.point);
            }
        }
    }

    public void PlantAtLocation(Vector3 location)
    {
        Instantiate(plantPrefab, location, Quaternion.identity);
    }
}
