using Elysium.Core.Timers;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.AI.GOAP
{
    public class Verifier : IVerifier
    {
        private Func<bool> evaluateFunc = null;
        private UnityEvent unityEvent = new UnityEvent();
        private ITimer timer = default;

        public Verifier(string _id, TimeSpan _interval)
        {
            timer = Timer.CreateScaledTimer(_interval);
            timer.ID = _id;
            timer.OnEnd.AddListener(Verify);            
        }

        public void Bind(Func<bool> _predicate, UnityAction _action)
        {
            this.evaluateFunc = _predicate;
            this.unityEvent.AddListener(_action);
            timer.AutoRestart = true;
            timer.Restart();
        }

        public void Unbind()
        {
            evaluateFunc = null;
            unityEvent.RemoveAllListeners();
            timer.AutoRestart = false;
            timer.Reset();
        }

        private void Verify()
        {
            if (evaluateFunc != null)
            {
                if (evaluateFunc())
                {
                    unityEvent?.Invoke();
                }
            }
        }

        ~Verifier()
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
        }
    }
}