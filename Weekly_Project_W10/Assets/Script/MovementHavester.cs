using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class MovementHavester : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    private Camera _camera;
    private Ray _ray;
    private RaycastHit _hit;
    private static readonly int _GROUND_LAYER = 1 << 6;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, 1000f, _GROUND_LAYER))
            {
                _agent.destination = _hit.point;
            }
        }
    }
}
