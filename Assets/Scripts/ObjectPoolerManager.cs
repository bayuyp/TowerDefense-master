using UnityEngine;

namespace Assets.Scripts
{
	public class ObjectPoolerManager : MonoBehaviour
	{
		public ObjectPooler ArrowFastPooler;
		public GameObject ArrowFastPrefab;
		public ObjectPooler ArrowMediumPooler;
		public GameObject ArrowMediumPrefab;
		public ObjectPooler ArrowSlowPooler;
		public GameObject ArrowSlowPrefab;
		public ObjectPooler AudioPooler;
		public static ObjectPoolerManager Instance { get; private set; }

		public void Awake()
		{
			Instance = this;
		}

		public void Start()
		{
			//just instantiate the pools
			if (ArrowMediumPooler == null)
			{
				var go = new GameObject("ArrowMediumPooler");
				ArrowMediumPooler = go.AddComponent<ObjectPooler>();
				ArrowMediumPooler.PooledObject = ArrowMediumPrefab;
				go.transform.parent = gameObject.transform;
				ArrowMediumPooler.Initialize();
			}

			if (ArrowFastPooler == null)
			{
				var go = new GameObject("ArrowFastPooler");
				ArrowFastPooler = go.AddComponent<ObjectPooler>();
				ArrowFastPooler.PooledObject = ArrowFastPrefab;
				go.transform.parent = gameObject.transform;
				ArrowFastPooler.Initialize();
			}

			if (ArrowSlowPooler == null)
			{
				var go = new GameObject("ArrowSlowPooler");
				ArrowSlowPooler = go.AddComponent<ObjectPooler>();
				ArrowSlowPooler.PooledObject = ArrowSlowPrefab;
				go.transform.parent = gameObject.transform;
				ArrowSlowPooler.Initialize();
			}

			if (AudioPooler == null)
			{
				var go = new GameObject("AudioPooler");
				AudioPooler = go.AddComponent<ObjectPooler>();
				go.transform.parent = gameObject.transform;
				AudioPooler.Initialize(typeof (AudioSource));
			}
		}
	}
}