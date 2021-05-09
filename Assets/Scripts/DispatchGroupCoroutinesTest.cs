using System;
using System.Collections;
using Scenes.DispatchGroup.Scripts;
using UnityEngine;

public class DispatchGroupCoroutinesTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ParallelTest start");
        
        DispatchGroup group = new DispatchGroup();
        
        this.DispatchCoroutine(group, CoroutineB());
        this.DispatchCoroutine(group, CoroutineC());
        this.DispatchCoroutine(group, CoroutineD());

        group.Notify(this, () =>
        {
            Debug.Log("ParallelTest end");
        });
        
        /*group.Wait(this, () =>
        {
            Debug.Log("WaitTest end");
        });*/
        
        Debug.Log("ParallelTest immediate after");
        
    }
    
    private IEnumerator WaitTest(Action completionHandler)
    {
        yield return StartCoroutine(CoroutineB());
        yield return StartCoroutine(CoroutineC());
        yield return StartCoroutine(CoroutineD());
        
        Debug.Log("WaitTest initialized");

        completionHandler();
    }
    

    private IEnumerator ParallelTest()
    {
        
        
        Coroutine b = StartCoroutine(CoroutineB());
        Coroutine c = StartCoroutine(CoroutineC());
        Coroutine d = StartCoroutine(CoroutineD());
        
        Debug.Log("ParallelTest initialized");

        yield return b;
        yield return c;
        yield return d;
        
    }


    private IEnumerator CoroutineB()
    {
        Debug.Log("CoroutineB start");

        yield return new WaitForSeconds(2.0f);
        
        Debug.Log("CoroutineB end");
    }
    
    private IEnumerator CoroutineC()
    {
        Debug.Log("CoroutineC start");
        
        yield return new WaitForSeconds(1.0f);
        
        Debug.Log("CoroutineC end");
    }

    private IEnumerator CoroutineD()
    {
        Debug.Log("CoroutineD start");
        
        yield return new WaitForSeconds(1.0f);
        
        Debug.Log("CoroutineD end");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
