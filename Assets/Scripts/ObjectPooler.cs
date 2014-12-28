using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
	public class ObjectPooler : MonoBehaviour
	{
		private Type[] componentsToAdd;
		public Transform Parent;
		public GameObject PooledObject;
		private List<GameObject> pooledObjects;
		public int PoolLength = 10;

		public void Initialize()
		{
			pooledObjects = new List<GameObject>();
			for (var i = 0; i < PoolLength; i++)
				CreateObjectInPool();
		}

		public void Initialize(params Type[] componentsToAdd)
		{
			this.componentsToAdd = componentsToAdd;
			Initialize();
		}

		public GameObject GetPooledObject()
		{
			foreach (var t in pooledObjects.Where(t => !t.activeInHierarchy))
				return t;
			var indexToReturn = pooledObjects.Count;
			CreateObjectInPool();
			return pooledObjects[indexToReturn];
		}

		private void CreateObjectInPool()
		{
			//if we don't have a prefab set, instantiate a new gameobject
			//else instantiate the prefab
			GameObject go;
			if (PooledObject == null)
				go = new GameObject(name + " PooledObject");
			else
				go = Instantiate(PooledObject) as GameObject;

			//set the new object as inactive and add it to the list
			if (go != null)
			{
				go.SetActive(false);
				pooledObjects.Add(go);

				//if we have components to add
				//add them
				if (componentsToAdd != null)
					foreach (var itemType in componentsToAdd)
						go.AddComponent(itemType);

				//if we have set the parent, assign it as the new object's parent
				if (Parent != null)
					go.transform.parent = Parent;
			}
		}
	}
}