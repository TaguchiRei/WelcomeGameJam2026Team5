using UnityEngine;
using UsefulTools.Utility.Runtime.Utility;

public class SeObject : MonoBehaviour, IRecyclable
{
    public int RecycleId { get; set; }

    [SerializeField] private AudioSource _audioSource;

    public void Play(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void OnRecycle()
    {
        _audioSource.Stop();
    }
}