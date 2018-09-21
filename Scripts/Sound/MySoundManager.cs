/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MySoundManager (version 2.1)
 */

using UnityEngine;
using System.Collections.Generic;

namespace MyClasses
{
    public partial class MySoundManager : MonoBehaviour
    {
        #region ----- Variable -----

        private AudioSource mAudioSourceBGM;
        private List<AudioSource> mListAudioSourceSFX;

        #endregion

        #region ----- Singleton -----

        private static object mSingletonLock = new object();
        private static MySoundManager mInstance;

        public static MySoundManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (mSingletonLock)
                    {
                        mInstance = (MySoundManager)FindObjectOfType(typeof(MySoundManager));
                        if (mInstance == null)
                        {
                            GameObject obj = new GameObject(typeof(MySoundManager).Name);
                            mInstance = obj.AddComponent<MySoundManager>();
                            DontDestroyOnLoad(obj);
                        }
                    }
                }
                return mInstance;
            }
        }

        #endregion

        #region ----- Property -----

        public bool IsEnableBGM
        {
            get { return VolumeBGM > 0 && !IsMuteBGM; }
        }

        public bool IsMuteBGM
        {
            get { return PlayerPrefs.GetInt("MyBGM_Mute", 0) == 1; }
            set
            {
                PlayerPrefs.SetInt("MyBGM_Mute", value ? 1 : 0);
                mAudioSourceBGM.volume = value ? 0 : VolumeBGM;
            }
        }

        public float VolumeBGM
        {
            get { return PlayerPrefs.GetFloat("MyBGM_Volume", 1f); }
            set
            {
                float volume = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat("MyBGM_Volume", volume);
                mAudioSourceBGM.volume = IsMuteBGM ? 0 : volume;
            }
        }

        public bool IsEnableSFX
        {
            get { return VolumeSFX > 0 && !IsMuteSFX; }
        }

        public bool IsMuteSFX
        {
            get { return PlayerPrefs.GetInt("MySFX_Mute", 0) == 1; }
            set
            {
                PlayerPrefs.SetInt("MySFX_Mute", value ? 1 : 0);
                float volume = value ? 0 : VolumeSFX;
                for (int i = mListAudioSourceSFX.Count - 1; i >= 0; i--)
                {
                    mListAudioSourceSFX[i].volume = volume;
                }
            }
        }

        public float VolumeSFX
        {
            get { return PlayerPrefs.GetFloat("MySFX_Volume", 1f); }
            set
            {
                float volume = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat("MySFX_Volume", volume);
                if (IsMuteSFX)
                {
                    volume = 0;
                }
                for (int i = mListAudioSourceSFX.Count - 1; i >= 0; i--)
                {
                    mListAudioSourceSFX[i].volume = volume;
                }
            }
        }

        public bool IsEnableVibrate
        {
            get { return PlayerPrefs.GetInt("MyVibrate", 1) == 1; }
            set { PlayerPrefs.SetInt("MyVibrate", value ? 1 : 0); }
        }

        #endregion

        #region ----- Implement MonoBehaviour -----

        /// <summary>
        /// Awake.
        /// </summary>
        void Awake()
        {
            mAudioSourceBGM = gameObject.AddComponent<AudioSource>();
            mListAudioSourceSFX = new List<AudioSource>();
        }

        #endregion

        #region ----- BGM -----

        /// <summary>
        /// Play a BGM.
        /// </summary>
        /// <param name="isLoop">enable looping for the audio clip</param>
        /// <param name="delayTime">delay time specified in seconds</param>
        public void PlayBGM(string filename, bool isLoop = true, float delayTime = 0)
        {
#if DEBUG_MY_SOUND
            Debug.Log("[" + typeof(MySoundManager).Name + "] <color=#0000FFFF>PlayBGM()</color>: filename=\"" + filename + "\"");
#endif

            AudioClip audioClip = Resources.Load<AudioClip>(filename);
            PlayBGM(audioClip, isLoop, delayTime);
        }

        /// <summary>
        /// Play a BGM.
        /// </summary>
        /// <param name="isLoop">enable looping for the audio clip</param>
        /// <param name="delayTime">delay time specified in seconds</param>
        public void PlayBGM(AudioClip audioClip, bool isLoop = true, float delayTime = 0)
        {
            mAudioSourceBGM.clip = audioClip;
            mAudioSourceBGM.loop = isLoop;
            mAudioSourceBGM.volume = VolumeBGM;
            mAudioSourceBGM.PlayDelayed(delayTime);
        }

        /// <summary>
        /// Pause BGM.
        /// </summary>
        public void PauseBGM()
        {
            mAudioSourceBGM.Pause();
        }

        /// <summary>
        /// Resume BGM.
        /// </summary>
        public void ResumeBGM()
        {
            mAudioSourceBGM.UnPause();
        }

        /// <summary>
        /// Stop BGM.
        /// </summary>
        public void StopBGM()
        {
            mAudioSourceBGM.Stop();
        }

        /// <summary>
        /// Is BGM playing right now?
        /// </summary>
        public bool IsPlayingBGM()
        {
            return mAudioSourceBGM != null && mAudioSourceBGM.clip != null && mAudioSourceBGM.isPlaying;
        }

        /// <summary>
        /// Is BGM playing right now?
        /// </summary>
        public bool IsPlayingBGM(string filename)
        {
            if (IsPlayingBGM())
            {
                return mAudioSourceBGM.clip.name.Equals(filename);
            }

            return false;
        }

        /// <summary>
        /// Is BGM playing right now?
        /// </summary>
        public bool IsPlayingBGM(AudioClip audioClip)
        {
            if (IsPlayingBGM())
            {
                return audioClip != null && mAudioSourceBGM.clip.name.Equals(audioClip.name);
            }

            return false;
        }

        #endregion

        #region ----- SFX -----

        /// <summary>
        /// Play a SFX.
        /// </summary>
        /// <param name="delayTime">delay time specified in seconds</param>
        public AudioSource PlaySFX(string filename, float delayTime = 0)
        {
#if DEBUG_MY_SOUND
            Debug.Log("[" + typeof(MySoundManager).Name + "] <color=#0000FFFF>PlaySFX()</color>: filename=\"" + filename + "\"");
#endif

            AudioClip audioClip = Resources.Load<AudioClip>(filename);
            return PlaySFX(audioClip);
        }

        /// <summary>
        /// Play a SFX.
        /// </summary>
        /// <param name="delayTime">delay time specified in seconds</param>
        public AudioSource PlaySFX(AudioClip audioClip, float delayTime = 0)
        {
            AudioSource audioSource = _GetAudioSourceSFX();
            audioSource.clip = audioClip;
            audioSource.volume = VolumeSFX;
            audioSource.PlayDelayed(delayTime);
            return audioSource;
        }

        /// <summary>
        /// Pause all SFXs.
        /// </summary>
        public void PauseAllSFXs()
        {
            for (int i = mListAudioSourceSFX.Count - 1; i >= 0; i--)
            {
                mListAudioSourceSFX[i].Pause();
            }
        }

        /// <summary>
        /// Resume all SFXs.
        /// </summary>
        public void ResumeAllSFXs()
        {
            for (int i = mListAudioSourceSFX.Count - 1; i >= 0; i--)
            {
                mListAudioSourceSFX[i].UnPause();
            }
        }

        /// <summary>
        /// Stop all SFXs.
        /// </summary>
        public void StopAllSFXs()
        {
            for (int i = mListAudioSourceSFX.Count - 1; i >= 0; i--)
            {
                mListAudioSourceSFX[i].Stop();
            }
        }

        /// <summary>
        /// Return a available audio source.
        /// </summary>
        private AudioSource _GetAudioSourceSFX()
        {
            foreach (AudioSource audioSource in mListAudioSourceSFX)
            {
                if (audioSource.clip == null || (!audioSource.isPlaying && audioSource.time == 0))
                {
                    return audioSource;
                }
            }

            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            newAudioSource.playOnAwake = false;
            mListAudioSourceSFX.Add(newAudioSource);
            return newAudioSource;
        }

        #endregion

        #region ----- Vibrate -----

        /// <summary>
        /// Vibrate.
        /// </summary>
        public void Vibrate()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (IsEnableVibrate)
            {
                Handheld.Vibrate();
            }
#endif
        }

        #endregion
    }
}
