using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes.DispatchGroup.Scripts
{
    public struct CoroutineContextInfo
    {
        public IEnumerator Routine { get; }
        public MonoBehaviour Context { get; }

        public CoroutineContextInfo(IEnumerator routine, MonoBehaviour context)
        {
            Routine = routine;
            Context = context;
        }
    }

    public class DispatchGroup
    {
        private List<CoroutineContextInfo> _coroutines = new List<CoroutineContextInfo>();
        private int _referenceCount;

        public void AddCoroutine(CoroutineContextInfo routineInfo)
        {
            _coroutines.Add(routineInfo);
        }

        public void Enter()
        {
            _referenceCount += 1;
        }

        public void Leave()
        {
            if (_referenceCount == 0)
            {
                throw new Exception("Leaving CoroutinesGroup more times than it has been entered.");
            }

            _referenceCount -= 1;
        }

        public void Notify(MonoBehaviour context, Action completionHandler)
        {
            context.StartCoroutine(NotifyCoroutine(completionHandler));
        }

        public void Wait(MonoBehaviour context, Action completionHandler)
        {
            context.StartCoroutine(WaitCoroutine(completionHandler));
        }

        private IEnumerator NotifyCoroutine(Action completionHandler)
        {
            var coroutineHandlers = new Coroutine[_coroutines.Count];
            for (var index = 0; index < _coroutines.Count; index++)
            {
                var coroutine = _coroutines[index];
                coroutineHandlers[index] = coroutine.Context.StartCoroutine(coroutine.Routine);
            }

            foreach (var coroutineHandler in coroutineHandlers)
            {
                yield return coroutineHandler;
            }

            //wait for manually wrapped async tasks
            while (_referenceCount > 0)
            {
                yield return null;
            }

            if (completionHandler != null) completionHandler();
        }

        private IEnumerator WaitCoroutine(Action completionHandler)
        {
            for (var index = 0; index < _coroutines.Count; index++)
            {
                var coroutine = _coroutines[index];
                yield return coroutine.Context.StartCoroutine(coroutine.Routine);
            }

            if (completionHandler != null) completionHandler();
        }
    }

}