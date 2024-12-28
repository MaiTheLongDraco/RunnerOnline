using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{   
    public static AppManager instance;
    public UIManager uIManager;
	private void Awake()
	{
		instance = this;
	}
	void Start()
    {
        uIManager= GetComponentInChildren<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
