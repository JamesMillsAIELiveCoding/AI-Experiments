using UnityEngine;

namespace Steering
{
    [CreateAssetMenu(menuName = "Steering/Avoidance", fileName = "Avoidance")]
    public class AvoidanceBehaviour : SteeringBehaviour
    {
        [SerializeField] private float viewDistance = 1f;

        internal override Vector3 Calculate(SteeringAgent _agent)
        {
            Vector3 force = _agent.CurrentForce;

            foreach(Vector3 direction in SteeringAgentHelper.DirectionsInCone(_agent))
            {
                if(Physics.Raycast(_agent.Position, direction, out RaycastHit hit, viewDistance))
                {
                    force += Vector3.Lerp(_agent.Forward, hit.normal, .25f);
                }
            }

            return force;
        }
    }
}