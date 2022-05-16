using System;
using UnityEngine.Events;

namespace Elysium.AI.GOAP
{
    public interface IVerifier
    {
        void Bind(Func<bool> _predicate, UnityAction _action);
        void Unbind();
    }
}