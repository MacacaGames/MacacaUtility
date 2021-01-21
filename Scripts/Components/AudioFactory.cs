using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioFactory : MonoBehaviour
{
    static AudioFactory _instance;
    public static AudioFactory instance
    {
        get
        {
            if (_instance == null)
                _instance = Initiate();
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    [SerializeField]
    AudioMixerGroup sfxMixerGroup;
    [SerializeField]
    AudioMixerGroup voiceMixerGroup;

    public enum AudioOutput { Sfx, Voice }

    static AudioFactory Initiate()
    {
        GameObject host = new GameObject();
        host.name = "AudioManager";
        DontDestroyOnLoad(host);
        return host.AddComponent<AudioFactory>();
    }

    Queue<AudioSource> sources = new Queue<AudioSource>();

    AudioSource GetAvailableSource()
    {
        AudioSource result = null;
        if (sources.Count > 0)
            result = sources.Dequeue();
        else
        {
            result = gameObject.AddComponent<AudioSource>();
        }
        return result;
    }

    public static AudioSource PlaySFX(AudioClip clip, AudioOutput output = AudioOutput.Sfx)
    {
        if (clip == null)
            return null;

        AudioSource source = instance.GetAvailableSource();
        instance.StartCoroutine(instance.PlaySfxTask(clip, output));
        return source;
    }

    public static AudioSource PlaySFX(AudioClip[] clips, AudioOutput output = AudioOutput.Sfx)
    {
        return PlaySFX(clips[Random.Range(0, clips.Length)], output);
    }

    IEnumerator PlaySfxTask(AudioClip clip, AudioOutput output = AudioOutput.Sfx)
    {
        AudioSource source = GetAvailableSource();
        source.enabled = true;
        source.clip = clip;
        source.outputAudioMixerGroup = sfxMixerGroup;

        if (output == AudioOutput.Voice)
            source.outputAudioMixerGroup = voiceMixerGroup;

        source.Play();
        while (source.isPlaying)
            yield return null;

        source.clip = null;
        source.enabled = false;
        sources.Enqueue(source);
    }
}