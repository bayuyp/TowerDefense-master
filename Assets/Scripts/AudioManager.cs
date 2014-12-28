using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
	public class AudioManager : MonoBehaviour
	{
		public AudioClip ArrowAudioClip, DeathSoundAudioClip;
		public static AudioManager Instance { get; private set; }

		public void PlayArrowSound()
		{
			StartCoroutine(PlaySound(ArrowAudioClip));
		}

		public void PlayDeathSound()
		{
			StartCoroutine(PlaySound(DeathSoundAudioClip));
		}

		public void Awake()
		{
			Instance = this;
		}

		private static IEnumerator PlaySound(AudioClip clip)
		{
			var go = ObjectPoolerManager.Instance.AudioPooler.GetPooledObject();
			go.SetActive(true);
			go.GetComponent<AudioSource>().PlayOneShot(clip);
			yield return new WaitForSeconds(clip.length);
			go.SetActive(false);
		}
	}
}