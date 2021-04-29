using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAgentManager : MonoBehaviour
{
    public Bounds Bounds => new Bounds(boundsCenter + transform.position, boundsSize);

    [SerializeField, Min(1f)] private float speed = 5;

    [Space]

    [SerializeField] private Vector3 boundsCenter = Vector3.zero;
    [SerializeField] private Vector3 boundsSize = Vector3.one;

    [Space]

    [SerializeField] private bool run = false;
    [SerializeField] private GizmoType showSpawnRegion;

    private SteeringAgent[] agents;

    // Start is called before the first frame update
    void Start()
    {
        agents = FindObjectsOfType<SteeringAgent>();
        foreach (SteeringAgent agent in agents)
        {
            agent.transform.parent = transform;
            agent.Initialise(speed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(SteeringAgent agent in agents)
        {
            if(run) 
                agent.UpdateAgent();

            if(!Bounds.Contains(agent.WorldPosition))
                agent.ApplyPosAndRot(Bounds.ClosestPoint(-agent.WorldPosition), agent.Rotation);
        }
    }

    // Implement this OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn
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
        Gizmos.color = new Color(0, 0, 1, .5f);
        Gizmos.DrawCube(transform.position + boundsCenter, boundsSize);

        Gizmos.color = new Color(0, 0, 1, 1f);
        Gizmos.DrawWireCube(transform.position + boundsCenter, boundsSize);
    }
}
