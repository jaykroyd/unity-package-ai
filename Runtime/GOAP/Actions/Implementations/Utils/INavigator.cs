using UnityEngine;

namespace Elysium.AI.GOAP
{
    public interface INavigator
    {
        Transform transform { get; }

        bool HasArrived();
        bool SetDestination(Vector3 _position);
        void Stop();
        void Resume();
        void Pause();               
    }
}