
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
namespace MacacaGames
{
    public class SimpleAudio : MonoBehaviour
    {
        static SimpleAudio _instance;
        public static SimpleAudio Instance
        {
            get
            {
                if (_instance == null)
                    _instance = Initiate();
                return _instance;
            }
        }

        static AudioMixer mixer;
        static AudioMixerGroup musicMixerGroup;
        static AudioMixerGroup lobbyMixerGroup;
        static AudioMixerGroup sfxMixerGroup;
        static AudioMixerGroup voiceMixerGroup;

        static SimpleAudio Initiate()
        {
            GameObject host = new GameObject();
            host.name = "SimpleAudio";
            var ins = host.AddComponent<SimpleAudio>();
            DontDestroyOnLoad(host);

            mixer = Resources.Load<AudioMixer>("SimpleAudio/Mixer");

            if (mixer)
            {
                sfxMixerGroup = mixer.FindMatchingGroups("Sfx")[0];
                voiceMixerGroup = mixer.FindMatchingGroups("Voice")[0];
                musicMixerGroup = mixer.FindMatchingGroups("Music")[0];
                lobbyMixerGroup = mixer.FindMatchingGroups("LobbyMusic")[0];
                mixer.GetFloat("Sfx", out oriVolumeSFX);
                mixer.GetFloat("Voice", out oriVolumeVoice);
                mixer.GetFloat("Music", out oriVolumeMusic);
                mixer.GetFloat("LobbyMusic", out oriVolumeLobby);
            }

            ins.ApplyMusicState();
            ins.ApplySFXState();
            return ins;
        }

        const string PP_IsMusicMute = "IsMusicMute";
        const string PP_IsSfxMute = "PP_IsSfxMute";
        public bool IsMusicMute
        {
            get
            {
                return PlayerPrefs.GetInt(PP_IsMusicMute, 0) == 1;
            }
            set
            {
                PlayerPrefs.SetInt(PP_IsMusicMute, value ? 1 : 0);
                ApplyMusicState();
            }
        }

        public bool IsSfxMute
        {
            get
            {
                return PlayerPrefs.GetInt(PP_IsSfxMute, 0) == 1;
            }
            set
            {
                PlayerPrefs.SetInt(PP_IsSfxMute, value ? 1 : 0);
                ApplySFXState();
            }
        }

        public void ApplyMusicState()
        {
            if (mixer)
            {
                if (!IsMusicMute)
                {
                    mixer.SetFloat("Music", oriVolumeMusic);
                    mixer.SetFloat("LobbyMusic", oriVolumeLobby);
                }
                else
                {
                    mixer.SetFloat("Music", -80);
                    mixer.SetFloat("LobbyMusic", -80);
                }
            }
            else
            {
                if (!IsMusicMute)
                {
                    AudioListener.volume = 1;
                }
                else
                {
                    AudioListener.volume = 0;
                }
            }
        }

        public void ApplySFXState()
        {
            if (mixer)
            {
                if (!IsSfxMute)
                {
                    mixer.SetFloat("Sfx", oriVolumeSFX);
                    mixer.SetFloat("Voice", oriVolumeVoice);
                }
                else
                {
                    mixer.SetFloat("Sfx", -80);
                    mixer.SetFloat("Voice", -80);
                }
            }
            else
            {
                if (IsSfxMute)
                {
                    AudioListener.volume = 1;
                }
                else
                {
                    AudioListener.volume = 0;
                }
            }
        }


        public enum AudioOutput { Sfx, Voice }

        static float oriVolumeSFX, oriVolumeVoice, oriVolumeMusic, oriVolumeLobby;

        /// <summary>
        /// AudioMixer setfloat not work in awake()
        /// </summary>
        void Start()
        {
            ApplyMusicState();
            ApplySFXState();
        }


        public void StopMusic()
        {
            GetMusicAudioSource().Stop();
        }

        public void PlayMusic(AudioClip clip)
        {
            GetMusicAudioSource().clip = clip;
            GetMusicAudioSource().time = 0;
            GetMusicAudioSource().Play();
            if (GetLobbyMusicAudioSource().isPlaying)
            {
                GetLobbyMusicAudioSource().Stop();
            }
        }

        public void PlayMusicPreview(AudioClip clip)
        {
            GetMusicAudioSource().clip = clip;
            GetMusicAudioSource().time = 10f;
            GetMusicAudioSource().Play();
            if (GetLobbyMusicAudioSource().isPlaying)
            {
                GetLobbyMusicAudioSource().Stop();
            }
        }

        public void PlayLobbyMusic(AudioClip clip)
        {
            GetLobbyMusicAudioSource().clip = clip;
            GetLobbyMusicAudioSource().Play();
        }

        public bool IsMusicPlaying()
        {
            return GetMusicAudioSource().isPlaying;
            // return false;
        }

        public bool IsMusicPlaying(AudioClip clip)
        {
            return GetMusicAudioSource().isPlaying && GetMusicAudioSource().clip == clip;
            // return false;
        }

        public float GetMusicProgress()
        {
            if (GetMusicAudioSource().clip == null)
            {
                return 0;
            }
            return GetMusicAudioSource().time / GetMusicAudioSource().clip.length;
        }

        public float GetMusicPlayTime()
        {
            return GetMusicAudioSource().time;
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

        AudioSource _musicAudioSource = null;
        AudioSource GetMusicAudioSource()
        {
            if (_musicAudioSource == null)
            {
                _musicAudioSource = gameObject.AddComponent<AudioSource>();
                _musicAudioSource.outputAudioMixerGroup = musicMixerGroup;
            }
            return _musicAudioSource;
        }
        AudioSource _lobbyMusicAudioSource = null;
        AudioSource GetLobbyMusicAudioSource()
        {
            if (_lobbyMusicAudioSource == null)
            {
                _lobbyMusicAudioSource = gameObject.AddComponent<AudioSource>();
                _lobbyMusicAudioSource.outputAudioMixerGroup = lobbyMixerGroup;
                _lobbyMusicAudioSource.loop = true;
            }
            return _lobbyMusicAudioSource;
        }

        /// <summary>
        /// Play sfx
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="output"></param>
        public void PlaySFX(AudioClip clip, AudioOutput output = AudioOutput.Sfx)
        {
            if (clip == null)
                return;
            StartCoroutine(SFXManager(clip, output));
        }

        /// <summary>
        /// Play sfx from on of the clips
        /// </summary>
        /// <param name="clips">clips</param>
        /// <param name="output"></param>
        public void PlaySFX(AudioClip[] clips, AudioOutput output = AudioOutput.Sfx)
        {
            PlaySFX(clips[Random.Range(0, clips.Length)], output);
        }

        IEnumerator SFXManager(AudioClip clip, AudioOutput output = AudioOutput.Sfx)
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
}