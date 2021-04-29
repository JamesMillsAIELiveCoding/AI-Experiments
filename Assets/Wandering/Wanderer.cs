using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : MonoBehaviour
{
    [SerializeField, Range(.01f, .1f)] private float jitter = .05f;
    [SerializeField, Min(1f)] private float speed = 1;
    [SerializeField] private float smoothing = .5f;

    private Vector3 previousForce = Vector3.zero;
    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = (transform.forward + (CalculateForce() * speed)) * Time.deltaTime;
        Vector3 position = Vector3.SmoothDamp(transform.position, movement + transform.position, ref velocity, smoothing);
        Quaternion rotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(previousForce.normalized), Time.deltaTime);

        transform.localPosition = position;
        transform.localRotation = rotation;
    }

    private Vector3 CalculateForce()
    {
        Vector3 force = previousForce;

        Vector2 offset = new Vector2(Random.Range(-jitter, jitter), Random.Range(-jitter, jitter));

        force += transform.right * offset.x;
        force += transform.up * offset.y;

        previousForce = force;

        return force;
    }

    // Implement this OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, previousForce);

        Gizmos.DrawWireSphere(transform.position + previousForce, .1f);
    }
}
