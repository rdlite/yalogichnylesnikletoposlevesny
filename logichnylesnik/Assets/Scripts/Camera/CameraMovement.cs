using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _target;

    [SerializeField] private bool IsFollowByTarget = true;

    private Transform _transform;

    private void Start()
    {
        _transform = transform;

        if (_target == null)
        {
            _target = FindObjectOfType<PlayerComponentsManager>().transform;
        }
    }

    private void Update()
    {
        if (IsFollowByTarget && _target != null)
        {
            _transform.position = new Vector3(_transform.position.x, _target.position.y, _target.position.z);
        }
    }
}