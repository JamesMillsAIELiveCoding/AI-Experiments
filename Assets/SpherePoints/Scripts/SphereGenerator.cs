using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGenerator : MonoBehaviour
{
    [SerializeField]
    private int numPoints = 100;
    [SerializeField, Range(45f, 360f)]
    private float viewAngle = 180f;
    [SerializeField, Min(.1f)]
    private float radius = 1;

    private float InverseAngle => 360 - viewAngle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool IsInCone(Vector3 _pos)
    {
        return Vector3.Distance(_pos, transform.position + (transform.forward * radius)) > InverseAngle;
    }

    // Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected
    private void OnDrawGizmosSelected()
    {
        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < numPoints; i++)
        {
            float t = (float)i / numPoints;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);

            Vector3 pos = new Vector3(x, y, z) * radius;
            Gizmos.color = IsInCone(pos) ? Color.blue : Color.red;

            Gizmos.DrawSphere(pos, .025f);
        }
    }
}
