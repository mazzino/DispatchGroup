using System;
using System.Collections;
using Scenes.DispatchGroup.Scripts;
using UnityEngine;

public class DispatchGroupAsyncTest : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("DispatchGroupAsyncTest Start");    
        
        DispatchGroup group = new DispatchGroup();
        AsyncTaskA(group, () => Debug.Log("AsyncTaskA Ended (custom callback)"));
        AsyncTaskB(group, null);
        AsyncTaskC(group, null);
        AsyncTaskD(group, null);
        group.Notify(this, () =>
        {
            Debug.Log("DispatchGroupAsyncTest Ended");    
        });
        Debug.Log("DispatchGroupAsyncTest ImmediateAfter");    
    }


    private void AsyncTaskA(DispatchGroup group, Action completionHandler)
    {
        group.Enter();
        StartCoroutine(AsyncTaskCoroutineHelper(2.0f, () =>
        {
            Debug.Log("AsyncTaskA Ended");
            group.Leave();
            if (completionHandler != null) completionHandler();
        }));
    }

    private void AsyncTaskB(DispatchGroup group, Action completionHandler)
    {
        group.Enter();
        StartCoroutine(AsyncTaskCoroutineHelper(1.0f, () =>
        {
            Debug.Log("AsyncTaskB Ended");
            group.Leave();
            if (completionHandler != null) completionHandler();
        }));
    }
    
    private void AsyncTaskC(DispatchGroup group, Action completionHandler)
    {
        group.Enter();
        StartCoroutine(AsyncTaskCoroutineHelper(2.0f, () =>
        {
            Debug.Log("AsyncTaskC Ended");
            group.Leave();
            if (completionHandler != null) completionHandler();
        }));
    }
    
    private void AsyncTaskD(DispatchGroup group, Action completionHandler)
    {
        group.Enter();
        StartCoroutine(AsyncTaskCoroutineHelper(1.0f, () =>
        {
            Debug.Log("AsyncTaskD Ended");
            group.Leave();
            if (completionHandler != null) completionHandler();
        }));
    }
    
    private IEnumerator AsyncTaskCoroutineHelper(float delay, Action completionHandler)
    {
        yield return new WaitForSeconds(delay);

        completionHandler();
    }
}