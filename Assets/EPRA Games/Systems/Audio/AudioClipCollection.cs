using UnityEngine;

namespace EPRA.Utilities
{
    [CreateAssetMenu(fileName = "Audio Clip Collection", menuName = "Scriptable Objects/Audio Clip Collection", order = 3)]

    public class AudioClipCollection : ScriptableObject
    {
        [Range(0f, 1f)] public float Volume = 1f;
        [SerializeField] private AudioClip[] _clips;

        public int Count => _clips.Length;

        public AudioClip GetFirstClip()
        {
            return _clips[0];
        }

        public AudioClip GetClip(int index)
        {
            return _clips[index];
        }

        public AudioClip GetRandomClip()
        {
            int randomIndex = Random.Range(0, _clips.Length);

            return _clips[randomIndex];
        }
    }
}

