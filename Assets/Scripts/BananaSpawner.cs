using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class BananaSpawner : MonoBehaviour
	{
		public GameObject Banana;

		public void StartBananaSpawning()
		{
			StartCoroutine(SpawnBananas());
		}

		public void StopBananaSpawning()
		{
			StopAllCoroutines();
		}

		private IEnumerator SpawnBananas()
		{
			while (true)
			{
				//select a random position
				var x = Random.Range(100, Screen.width - 100);
				var randomPosition = Camera.main.ScreenToWorldPoint(new Vector3(x, 0, 0));

				//create and drop a carrot
				var banana = Instantiate(Banana,
					new Vector3(randomPosition.x, transform.position.y, transform.position.z),
					Quaternion.identity) as GameObject;
				if (banana != null)
					banana.GetComponent<Banana>().FallSpeed = Random.Range(1f, 3f);

				//wait for random seconds, based on level parameters
				yield return new WaitForSeconds
					(Random.Range(GameManager.Instance.MinBananaSpawnTime,
						GameManager.Instance.MaxBananaSpawnTime));
			}
		}
	}
}