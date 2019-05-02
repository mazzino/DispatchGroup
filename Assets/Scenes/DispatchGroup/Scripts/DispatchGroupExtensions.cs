using System.Collections;
using UnityEngine;

namespace Scenes.DispatchGroup.Scripts
{
    public static class DispatchGroupExtensions
    {
        public static void DispatchCoroutine(this MonoBehaviour monoBehaviour, DispatchGroup group, IEnumerator routine)
        {
            group.AddCoroutine(new CoroutineContextInfo(routine, monoBehaviour));
        }
            
    }
}