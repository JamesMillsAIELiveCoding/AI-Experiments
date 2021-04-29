using UnityEngine;

public enum GizmoType { Never, SelectedOnly, Always }

public class Spawner : MonoBehaviour
{
    [SerializeField] private SteeringAgent prefab;
    [SerializeField] private float spawnRadius = 10;
    [SerializeField] private int spawnCount = 10;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private GizmoType showSpawnRegion;

    void Awake()
    {
        for(int i = 0; i < spawnCount; i++)
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            SteeringAgent boid = Instantiate(prefab);
            boid.transform.position = pos;
            boid.transform.forward = Random.insideUnitSphere;

            boid.SetColor(color);
        }
    }

    private void OnDrawGizmos()
    {
        if(showSpawnRegion == GizmoType.Always)
            DrawGizmos();
    }

    void OnDrawGizmosSelected()
    {
        if(showSpawnRegion == GizmoType.SelectedOnly)
            DrawGizmos();
    }

    void DrawGizmos()
    {
        Gizmos.color = new Color(color.r, color.g, color.b, 0.3f);
        Gizmos.DrawSphere(transform.position, spawnRadius);
    }
}
