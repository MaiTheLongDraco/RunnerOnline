using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> _executionQueue = new Queue<Action>();

    private static MainThreadDispatcher _instance;

    public static MainThreadDispatcher Instance
    {
        get
        {
            //if (!_instance)
            //{
            //    _instance = FindObjectOfType<MainThreadDispatcher>();

            //    if (!_instance)
            //    {
            //        var obj = new GameObject("UnityMainThreadDispatcher");
            //        _instance = obj.AddComponent<MainThreadDispatcher>();
            //        DontDestroyOnLoad(obj);
            //    }
            //}

            return _instance;
        }
    }
	private void Awake()
	{
        if (!_instance)
        {
            _instance = this;
        }
	}

	public void Enqueue(Action action)
    {
        if (action == null)
            throw new ArgumentNullException(nameof(action));

        lock (_executionQueue)
        {
            _executionQueue.Enqueue(action);
        }
    }

    private void Update()
    {
        if (_executionQueue.Count > 0)
        {
            lock (_executionQueue)
            {
                while (_executionQueue.Count > 0)
                {
                    var action = _executionQueue.Dequeue();
                    action?.Invoke();
                }
            }
        }
    }

    public static void RunOnMainThread(Action action)
    {
        Instance.Enqueue(action);
    }
}
