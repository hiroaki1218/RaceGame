using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraFollow : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    public static Transform target;
    [SerializeField] float translateSpeed;
    [SerializeField] float rotationSpeed;

    private void FixedUpdate()
    {
        HandleTranslation();
        HandleRotation();
    }

    private void HandleTranslation()
    {
        if(target != null)
        {
            var targetPosition = target.TransformPoint(offset);
            transform.position = Vector3.Lerp(transform.position, targetPosition, translateSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        if (target != null)
        {
            var direction = target.position - transform.position;
            var rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }
}
