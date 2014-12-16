﻿using UnityEngine;
using System.Collections;
using System;

public class ObjectPoolerManager : MonoBehaviour {

    //we'll need pools for arrows and audio objects
    public ObjectPooler ArrowPooler;
    public ObjectPooler AudioPooler;
	public ObjectPooler ArrowFastPooler;
	public ObjectPooler ArrowSlowPooler;

    public GameObject ArrowPrefab;
	public GameObject ArrowFastPrefab;
	public GameObject ArrowSlowPrefab;


    //basic singleton implementation
    public static ObjectPoolerManager Instance {get;private set;}
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //just instantiate the pools
        if (ArrowPooler == null)
        {
            GameObject go = new GameObject("ArrowPooler");
            ArrowPooler = go.AddComponent<ObjectPooler>();
            ArrowPooler.PooledObject = ArrowPrefab;
            go.transform.parent = this.gameObject.transform;
            ArrowPooler.Initialize();
        }

		if (ArrowFastPooler == null)
		{
			GameObject go = new GameObject("ArrowFastPooler");
			ArrowFastPooler = go.AddComponent<ObjectPooler>();
			ArrowFastPooler.PooledObject = ArrowFastPrefab;
			go.transform.parent = this.gameObject.transform;
			ArrowFastPooler.Initialize();
		}

		if (ArrowSlowPooler == null)
		{
			GameObject go = new GameObject("ArrowSlowPooler");
			ArrowSlowPooler = go.AddComponent<ObjectPooler>();
			ArrowSlowPooler.PooledObject = ArrowSlowPrefab;
			go.transform.parent = this.gameObject.transform;
			ArrowSlowPooler.Initialize();
		}

        if (AudioPooler == null)
        {
            GameObject go = new GameObject("AudioPooler");
            AudioPooler = go.AddComponent<ObjectPooler>();
            go.transform.parent = this.gameObject.transform;
            AudioPooler.Initialize(typeof(AudioSource));
        }

        
    }

}
