using UnityEngine;
using UnityEngine.AI;

namespace Elysium.AI.GOAP
{
    public class NavMeshAgentNavigator : INavigator
    {
        private NavMeshAgent navAgent = default;
        private float samplePositionDistance = default;
        private int areaMask = default;

        public Transform transform => navAgent.transform;

        public bool HasArrived()
        {
            return !navAgent.pathPending 
                && navAgent.remainingDistance <= navAgent.stoppingDistance 
                && (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f);
        }

        public NavMeshAgentNavigator(NavMeshAgent _navAgent, float _samplePositionDistance = 100, int _areaMask = -1)
        {
            this.navAgent = _navAgent;
            this.samplePositionDistance = _samplePositionDistance;
            this.areaMask = _areaMask;
        }

        public void Pause()
        {
            navAgent.isStopped = true;
        }

        public void Resume()
        {
            navAgent.isStopped = false;
        }

        public bool SetDestination(Vector3 _position)
        {
            if (NavMesh.SamplePosition(_position, out NavMeshHit _hit, samplePositionDistance, areaMask))
            {
                navAgent.isStopped = false;
                navAgent.SetDestination(_hit.position);
                return true;
            }
            return false;
        }

        public void Stop()
        {
            navAgent.SetDestination(navAgent.transform.position);
            navAgent.isStopped = true;
        }
    }
}