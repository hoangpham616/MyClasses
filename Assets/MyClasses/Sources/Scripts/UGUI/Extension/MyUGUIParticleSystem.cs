/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIParticleSystem (version 2.0)
 */

#pragma warning disable 0114
#pragma warning disable 0414
#pragma warning disable 0649

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MyClasses.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class MyUGUIParticleSystem : MonoBehaviour
    {
        #region ----- Variable -----

#if UNITY_EDITOR
        [SerializeField]
        private EParticleTemplate mETemplate = EParticleTemplate.ColorfulStarExplosion;
#endif

        [SerializeField]
        private bool mIsPlayOnAwake = true;
        [SerializeField]
        private bool mIsPlayOnEnable = true;
        [SerializeField]
        private bool mIsLooping = true;
        [SerializeField]
        private float mDelayTime = 0f;
        [SerializeField]
        private float mDuration = 5f;
        [SerializeField]
        private float mLifetime = 5f;
        [SerializeField]
        private float mEmissionRange = 90f;
        [SerializeField]
        private float mNumParticlePerSecond = 10f;
        [SerializeField]
        private int mMaxParticleQuantity = 100;
        [SerializeField]
        private Sprite mSprite;
        [SerializeField]
        private Gradient mColorOverLifetime = new Gradient();
        [SerializeField]
        private Vector3 mGravity = new Vector3(0, -9.81f, 0);
        [SerializeField]
        private Vector3 mDirection = new Vector3(0, 1f, 0);
        [SerializeField]
        private float mSize = 1f;
        [SerializeField]
        private AnimationCurve mSizeOverLifetime;
        [SerializeField]
        private float mSpeed = 5f;
        [SerializeField]
        private AnimationCurve mSpeedOverLifetime;
        [SerializeField]
        private Vector2 mRotationRange = Vector2.zero;

        private Image[] mParticles;
        private int mParticleIndex;

        private bool mIsPlaying = false;
        private float mPlaytime = 0f;

        #endregion

        #region ----- Property -----

        public bool IsPlaying
        {
            get { return mIsPlaying; }
        }

        #endregion

        #region ----- MonoBehaviour Implementation -----

        /// <summary>
        /// Awake.
        /// </summary>
        private void Awake()
        {
            if (mParticles == null)
            {
                _Initialize();
            }
            if (mIsPlayOnAwake)
            {
                Play();
            }
        }

        /// <summary>
        /// OnEnable.
        /// </summary>
        private void OnEnable()
        {
            if (mIsPlayOnEnable && !mIsPlaying)
            {
                Play();
            }
        }

        /// <summary>
        /// OnDisable.
        /// </summary>
        private void OnDisable()
        {
            mIsPlaying = false;
            for (int i = 0; i < mParticles.Length; ++i)
            {
                mParticles[i].gameObject.SetActive(false);
            }
        }

#if UNITY_EDITOR

        /// <summary>
        /// Update.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                for (int i = 0; i < mSizeOverLifetime.keys.Length; ++i)
                {
                    Debug.LogError("mSizeOverLifetime=(" + mSizeOverLifetime.keys[i].time + ", " + mSizeOverLifetime.keys[i].value + ", " + mSizeOverLifetime.keys[i].inTangent + ", " + mSizeOverLifetime.keys[i].outTangent + ", " + mSizeOverLifetime.keys[i].inWeight + ", " + mSizeOverLifetime.keys[i].outWeight + ", " + mSizeOverLifetime.keys[i].weightedMode + ")");
                }
                for (int i = 0; i < mSpeedOverLifetime.keys.Length; ++i)
                {
                    Debug.LogError("mSpeedOverLifetime=(" + mSpeedOverLifetime.keys[i].time + ", " + mSpeedOverLifetime.keys[i].value + ", " + mSpeedOverLifetime.keys[i].inTangent + ", " + mSpeedOverLifetime.keys[i].outTangent + ", " + mSpeedOverLifetime.keys[i].inWeight + ", " + mSpeedOverLifetime.keys[i].outWeight + ", " + mSpeedOverLifetime.keys[i].weightedMode + ")");
                }
            }
        }

#endif

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Play.
        /// </summary>
        public void Play()
        {
            if (!mIsPlaying)
            {
                StartCoroutine(_DoPlayParticles(mDelayTime));
            }
        }

#if UNITY_EDITOR

        /// <summary>
        /// Use a template.
        /// </summary>
        public void SetTemplate(EParticleTemplate template)
        {
            mETemplate = template;

            switch (mETemplate)
            {
                case EParticleTemplate.CircleSteam:
                    {
                        mLifetime = 2.4f;
                        mDuration = mLifetime;
                        mEmissionRange = 45f;
                        mNumParticlePerSecond = 6;
                        mMaxParticleQuantity = 100;

                        mGravity = Vector3.zero;
                        mDirection = new Vector3(0, 1, 0);

                        string[] paths = new string[] { "Assets/MyClasses", "Assets/Core/MyClasses", "Assets/Plugin/MyClasses", "Assets/Plugins/MyClasses", "Assets/Framework/MyClasses", "Assets/Frameworks/MyClasses" };
                        for (int j = 0; j < paths.Length; j++)
                        {
                            if (System.IO.File.Exists(paths[j] + "/Sources/Images/my_circle_128.png"))
                            {
                                mSprite = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(paths[j] + "/Sources/Images/my_circle_128.png", typeof(Sprite));
                                break;
                            }
                        }
                        GradientColorKey[] colorKeys = new GradientColorKey[2];
                        colorKeys[0] = new GradientColorKey(Color.white, 0);
                        colorKeys[1] = new GradientColorKey(Color.white, 1f);
                        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
                        alphaKeys[0] = new GradientAlphaKey(0.4f, 0);
                        alphaKeys[1] = new GradientAlphaKey(0, 1f);
                        mColorOverLifetime.SetKeys(colorKeys, alphaKeys);

                        mSize = 0.2f;
                        while (mSizeOverLifetime.keys.Length > 0)
                        {
                            mSizeOverLifetime.RemoveKey(0);
                        }
                        mSizeOverLifetime.AddKey(new Keyframe(0, 0, 6, 6, 0, 0.5f));
                        mSizeOverLifetime.keys[0].weightedMode = WeightedMode.Both;
                        mSizeOverLifetime.AddKey(new Keyframe(3, 0.2f, 0, 0, 0.25f, 1));
                        mSizeOverLifetime.keys[1].weightedMode = WeightedMode.Both;
                        mSizeOverLifetime.preWrapMode = WrapMode.ClampForever;
                        mSizeOverLifetime.postWrapMode = WrapMode.ClampForever;

                        mSpeed = 1f;
                        while (mSpeedOverLifetime.keys.Length > 0)
                        {
                            mSpeedOverLifetime.RemoveKey(0);
                        }
                        mSpeedOverLifetime.AddKey(new Keyframe(0, 1, 0, 0, 0, 0));
                        mSpeedOverLifetime.keys[0].weightedMode = WeightedMode.None;
                        mSpeedOverLifetime.AddKey(new Keyframe(1, 1, 0, 0, 0, 0));
                        mSpeedOverLifetime.keys[1].weightedMode = WeightedMode.None;
                        mSpeedOverLifetime.preWrapMode = WrapMode.ClampForever;
                        mSpeedOverLifetime.postWrapMode = WrapMode.ClampForever;

                        mRotationRange = Vector2.zero;
                    }
                    break;

                case EParticleTemplate.ColorfulBubbleExplosion:
                    {
                        mLifetime = 1f;
                        mDuration = mLifetime;
                        mEmissionRange = 360f;
                        mNumParticlePerSecond = 10;
                        mMaxParticleQuantity = 100;

                        mGravity = Vector3.zero;
                        mDirection = new Vector3(0, 1, 0);

                        string[] paths = new string[] { "Assets/MyClasses", "Assets/Core/MyClasses", "Assets/Plugin/MyClasses", "Assets/Plugins/MyClasses", "Assets/Framework/MyClasses", "Assets/Frameworks/MyClasses" };
                        for (int j = 0; j < paths.Length; j++)
                        {
                            if (System.IO.File.Exists(paths[j] + "/Sources/Images/my_circle_128.png"))
                            {
                                mSprite = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(paths[j] + "/Sources/Images/my_circle_128.png", typeof(Sprite));
                                break;
                            }
                        }
                        GradientColorKey[] colorKeys = new GradientColorKey[6];
                        colorKeys[0] = new GradientColorKey(new Color(0, 0, 1f), 0);
                        colorKeys[1] = new GradientColorKey(new Color(0, 1f, 1f), 0.15f);
                        colorKeys[2] = new GradientColorKey(new Color(0, 1f, 0.2f), 0.3f);
                        colorKeys[3] = new GradientColorKey(new Color(1f, 1f, 0), 0.45f);
                        colorKeys[4] = new GradientColorKey(new Color(1f, 0, 0), 0.6f);
                        colorKeys[5] = new GradientColorKey(new Color(1f, 0, 0.8f), 0.8f);
                        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
                        alphaKeys[0] = new GradientAlphaKey(1f, 0.45f);
                        alphaKeys[1] = new GradientAlphaKey(0, 1f);
                        mColorOverLifetime.SetKeys(colorKeys, alphaKeys);

                        mSize = 0.6f;
                        while (mSizeOverLifetime.keys.Length > 0)
                        {
                            mSizeOverLifetime.RemoveKey(0);
                        }
                        mSizeOverLifetime.AddKey(new Keyframe(0, 0.25f, 14f, 14f, 0, 0.09f));
                        mSizeOverLifetime.keys[0].weightedMode = WeightedMode.Both;
                        mSizeOverLifetime.AddKey(new Keyframe(1f, 0, -2f, -2f, 0.5f, 0.5f));
                        mSizeOverLifetime.keys[1].weightedMode = WeightedMode.Both;
                        mSizeOverLifetime.preWrapMode = WrapMode.ClampForever;
                        mSizeOverLifetime.postWrapMode = WrapMode.ClampForever;

                        mSpeed = 1f;
                        while (mSpeedOverLifetime.keys.Length > 0)
                        {
                            mSpeedOverLifetime.RemoveKey(0);
                        }
                        mSpeedOverLifetime.AddKey(new Keyframe(0, 2, 0, 0, 0, 0));
                        mSpeedOverLifetime.keys[0].weightedMode = WeightedMode.None;
                        mSpeedOverLifetime.AddKey(new Keyframe(1, 0, 0, 0, 0, 0));
                        mSpeedOverLifetime.keys[1].weightedMode = WeightedMode.None;
                        mSpeedOverLifetime.preWrapMode = WrapMode.ClampForever;
                        mSpeedOverLifetime.postWrapMode = WrapMode.ClampForever;

                        mRotationRange = Vector3.zero;
                    }
                    break;

                case EParticleTemplate.ColorfulStarExplosion:
                    {
                        mLifetime = 1f;
                        mDuration = mLifetime;
                        mEmissionRange = 360f;
                        mNumParticlePerSecond = 10;
                        mMaxParticleQuantity = 100;

                        mGravity = Vector3.zero;
                        mDirection = new Vector3(0, 1, 0);

                        string[] paths = new string[] { "Assets/MyClasses", "Assets/Core/MyClasses", "Assets/Plugin/MyClasses", "Assets/Plugins/MyClasses", "Assets/Framework/MyClasses", "Assets/Frameworks/MyClasses" };
                        for (int j = 0; j < paths.Length; j++)
                        {
                            if (System.IO.File.Exists(paths[j] + "/Sources/Images/my_circle_star_empty_128.png"))
                            {
                                mSprite = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(paths[j] + "/Sources/Images/my_circle_star_empty_128.png", typeof(Sprite));
                                break;
                            }
                        }
                        GradientColorKey[] colorKeys = new GradientColorKey[6];
                        colorKeys[0] = new GradientColorKey(new Color(0, 0, 1f), 0);
                        colorKeys[1] = new GradientColorKey(new Color(0, 1f, 1f), 0.15f);
                        colorKeys[2] = new GradientColorKey(new Color(0, 1f, 0.2f), 0.3f);
                        colorKeys[3] = new GradientColorKey(new Color(1f, 1f, 0), 0.45f);
                        colorKeys[4] = new GradientColorKey(new Color(1f, 0, 0), 0.6f);
                        colorKeys[5] = new GradientColorKey(new Color(1f, 0, 0.8f), 0.8f);
                        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
                        alphaKeys[0] = new GradientAlphaKey(1f, 0.6f);
                        alphaKeys[1] = new GradientAlphaKey(0, 1f);
                        mColorOverLifetime.SetKeys(colorKeys, alphaKeys);

                        mSize = 0.75f;
                        while (mSizeOverLifetime.keys.Length > 0)
                        {
                            mSizeOverLifetime.RemoveKey(0);
                        }
                        mSizeOverLifetime.AddKey(new Keyframe(0, 0.25f, 14f, 14f, 0, 0.09f));
                        mSizeOverLifetime.keys[0].weightedMode = WeightedMode.Both;
                        mSizeOverLifetime.AddKey(new Keyframe(1f, 0, -2f, -2f, 0.5f, 0.5f));
                        mSizeOverLifetime.keys[1].weightedMode = WeightedMode.Both;
                        mSizeOverLifetime.preWrapMode = WrapMode.ClampForever;
                        mSizeOverLifetime.postWrapMode = WrapMode.ClampForever;

                        mSpeed = 1f;
                        while (mSpeedOverLifetime.keys.Length > 0)
                        {
                            mSpeedOverLifetime.RemoveKey(0);
                        }
                        mSpeedOverLifetime.AddKey(new Keyframe(0, 2, 0, 0, 0, 0));
                        mSpeedOverLifetime.keys[0].weightedMode = WeightedMode.None;
                        mSpeedOverLifetime.AddKey(new Keyframe(1, 0, 0, 0, 0, 0));
                        mSpeedOverLifetime.keys[1].weightedMode = WeightedMode.None;
                        mSpeedOverLifetime.preWrapMode = WrapMode.ClampForever;
                        mSpeedOverLifetime.postWrapMode = WrapMode.ClampForever;

                        mRotationRange = new Vector2(-360, 360);
                    }
                    break;

                case EParticleTemplate.Mist:
                    {
                        mLifetime = 1.2f;
                        mDuration = mLifetime;
                        mEmissionRange = 360;
                        mNumParticlePerSecond = 20;
                        mMaxParticleQuantity = 100;

                        mGravity = Vector3.zero;
                        mDirection = new Vector3(0, 1, 0);

                        string[] paths = new string[] { "Assets/MyClasses", "Assets/Core/MyClasses", "Assets/Plugin/MyClasses", "Assets/Plugins/MyClasses", "Assets/Framework/MyClasses", "Assets/Frameworks/MyClasses" };
                        for (int j = 0; j < paths.Length; j++)
                        {
                            if (System.IO.File.Exists(paths[j] + "/Sources/Images/my_blur_dot_128.png"))
                            {
                                mSprite = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(paths[j] + "/Sources/Images/my_blur_dot_128.png", typeof(Sprite));
                                break;
                            }
                        }
                        GradientColorKey[] colorKeys = new GradientColorKey[2];
                        colorKeys[0] = new GradientColorKey(Color.white, 0);
                        colorKeys[1] = new GradientColorKey(Color.white, 1f);
                        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
                        alphaKeys[0] = new GradientAlphaKey(1f, 0);
                        alphaKeys[1] = new GradientAlphaKey(0, 1f);
                        mColorOverLifetime.SetKeys(colorKeys, alphaKeys);

                        mSize = 0.7f;
                        while (mSizeOverLifetime.keys.Length > 0)
                        {
                            mSizeOverLifetime.RemoveKey(0);
                        }
                        mSizeOverLifetime.AddKey(new Keyframe(0, 1, 0, 0, 0, 0));
                        mSizeOverLifetime.keys[0].weightedMode = WeightedMode.None;
                        mSizeOverLifetime.AddKey(new Keyframe(1, 1, 0, 0, 0, 0));
                        mSizeOverLifetime.keys[1].weightedMode = WeightedMode.None;
                        mSizeOverLifetime.preWrapMode = WrapMode.ClampForever;
                        mSizeOverLifetime.postWrapMode = WrapMode.ClampForever;

                        mSpeed = 1f;
                        while (mSpeedOverLifetime.keys.Length > 0)
                        {
                            mSpeedOverLifetime.RemoveKey(0);
                        }
                        mSpeedOverLifetime.AddKey(new Keyframe(0, 1, 0, 0, 0, 0));
                        mSpeedOverLifetime.keys[0].weightedMode = WeightedMode.None;
                        mSpeedOverLifetime.AddKey(new Keyframe(1, 1, 0, 0, 0, 0));
                        mSpeedOverLifetime.keys[1].weightedMode = WeightedMode.None;
                        mSpeedOverLifetime.preWrapMode = WrapMode.ClampForever;
                        mSpeedOverLifetime.postWrapMode = WrapMode.ClampForever;

                        mRotationRange = Vector2.zero;
                    }
                    break;

                case EParticleTemplate.Volcano:
                    {
                        mLifetime = 1.5f;
                        mDuration = mLifetime;
                        mEmissionRange = 30;
                        mNumParticlePerSecond = 30;
                        mMaxParticleQuantity = 100;

                        mGravity = new Vector3(0, -9.81f, 0);
                        mDirection = new Vector3(0, 1, 0);

                        string[] paths = new string[] { "Assets/MyClasses", "Assets/Core/MyClasses", "Assets/Plugin/MyClasses", "Assets/Plugins/MyClasses", "Assets/Framework/MyClasses", "Assets/Frameworks/MyClasses" };
                        for (int j = 0; j < paths.Length; j++)
                        {
                            if (System.IO.File.Exists(paths[j] + "/Sources/Images/my_blur_dot_128.png"))
                            {
                                mSprite = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(paths[j] + "/Sources/Images/my_blur_dot_128.png", typeof(Sprite));
                                break;
                            }
                        }
                        GradientColorKey[] colorKeys = new GradientColorKey[2];
                        colorKeys[0] = new GradientColorKey(new Color(1f, 0.8f, 0), 0);
                        colorKeys[1] = new GradientColorKey(new Color(1f, 0, 0), 0.7f);
                        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
                        alphaKeys[0] = new GradientAlphaKey(1f, 0.6f);
                        alphaKeys[1] = new GradientAlphaKey(0, 1f);
                        mColorOverLifetime.SetKeys(colorKeys, alphaKeys);

                        mSize = 0.7f;
                        while (mSizeOverLifetime.keys.Length > 0)
                        {
                            mSizeOverLifetime.RemoveKey(0);
                        }
                        mSizeOverLifetime.AddKey(new Keyframe(0, 1, 0, 0, 0, 0));
                        mSizeOverLifetime.keys[0].weightedMode = WeightedMode.None;
                        mSizeOverLifetime.AddKey(new Keyframe(1, 1, 0, 0, 0, 0));
                        mSizeOverLifetime.keys[1].weightedMode = WeightedMode.None;
                        mSizeOverLifetime.preWrapMode = WrapMode.ClampForever;
                        mSizeOverLifetime.postWrapMode = WrapMode.ClampForever;

                        mSpeed = 7f;
                        while (mSpeedOverLifetime.keys.Length > 0)
                        {
                            mSpeedOverLifetime.RemoveKey(0);
                        }
                        mSpeedOverLifetime.AddKey(new Keyframe(0, 1, 0, 0, 0, 0));
                        mSpeedOverLifetime.keys[0].weightedMode = WeightedMode.None;
                        mSpeedOverLifetime.AddKey(new Keyframe(1, 1, 0, 0, 0, 0));
                        mSpeedOverLifetime.keys[1].weightedMode = WeightedMode.None;
                        mSpeedOverLifetime.preWrapMode = WrapMode.ClampForever;
                        mSpeedOverLifetime.postWrapMode = WrapMode.ClampForever;

                        mRotationRange = Vector2.zero;
                    }
                    break;
            }
        }

#endif

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Initialize.
        /// </summary>
        private void _Initialize()
        {
            Rect rect = transform.GetComponent<RectTransform>().rect;

            int numParticle = (int)(mLifetime * mNumParticlePerSecond) + 1;
            if (numParticle > mMaxParticleQuantity)
            {
                numParticle = mMaxParticleQuantity;
            }
            mParticles = new Image[numParticle];
            for (int i = 0; i < mParticles.Length; ++i)
            {
                GameObject gameObject = new GameObject(string.Format("MyUGUIParticle ({0})", i));
                gameObject.SetActive(false);
                gameObject.transform.SetParent(transform, false);
                RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.width);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rect.height);
                Image particle = gameObject.AddComponent<Image>();
                particle.sprite = mSprite;

                mParticles[i] = particle;
            }
        }

        /// <summary>
        /// Play particles.
        /// </summary>
        private IEnumerator _DoPlayParticles(float delayTime)
        {
            if (delayTime > 0)
            {
                yield return new WaitForSeconds(delayTime);
            }

            mIsPlaying = true;
            mPlaytime = 0f;
            mParticleIndex = mParticles.Length - 1;

            float countTimeToSpawn = 0f;
            while (mIsPlaying && (mIsLooping || mPlaytime < mDuration))
            {
                if (!mIsLooping)
                {
                    mPlaytime += Time.deltaTime;
                }

                float spawnTime = 1f / mNumParticlePerSecond;
                countTimeToSpawn += Time.deltaTime;
                while (countTimeToSpawn > spawnTime)
                {
                    countTimeToSpawn -= spawnTime;
                    mParticleIndex -= 1;
                    if (mParticleIndex < 0)
                    {
                        mParticleIndex = mParticles.Length - 1;
                    }
                    if (!mParticles[mParticleIndex].gameObject.activeSelf)
                    {
                        StartCoroutine(_DoPlayParticle(mParticles[mParticleIndex]));
                    }
                }

                yield return new WaitForEndOfFrame();
            }

            mIsPlaying = false;
        }

        /// <summary>
        /// Play a particle.
        /// </summary>
        private IEnumerator _DoPlayParticle(Image particle)
        {
            particle.gameObject.SetActive(true);
            particle.transform.localPosition = Vector3.zero;

            Vector3 emissonAngle = Quaternion.AngleAxis(Random.Range(-mEmissionRange / 2f, mEmissionRange / 2f), Vector3.forward) * mDirection;
            emissonAngle.Normalize();
            emissonAngle.z = 0;

            float rotation = Random.Range(mRotationRange.x, mRotationRange.y);
            float lifetime = 0f;
            while (lifetime < mLifetime)
            {
                lifetime += Time.deltaTime;
                particle.transform.localPosition+=(emissonAngle * mSpeedOverLifetime.Evaluate(lifetime / mLifetime) * mSpeed + (mGravity * lifetime));
                particle.transform.localRotation = Quaternion.AngleAxis(rotation * lifetime, Vector3.forward);
                particle.transform.localScale = Vector3.one * mSizeOverLifetime.Evaluate(lifetime / mLifetime) * mSize;
                particle.color = mColorOverLifetime.Evaluate(lifetime / mLifetime);

                yield return new WaitForEndOfFrame();
            }

            particle.gameObject.SetActive(false);
        }

        #endregion

        #region ----- Enumeration ----

        public enum EParticleTemplate
        {
            None = 0,
            CircleSteam = 1,
            ColorfulBubbleExplosion = 2,
            ColorfulStarExplosion = 3,
            Mist = 4,
            Volcano = 5,
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyUGUIParticleSystem))]
    public class MyUGUIParticleEditor : Editor
    {
        private MyUGUIParticleSystem mScript;
        private RectTransform mRectTransform;

        private SerializedProperty mETemplate;
        private SerializedProperty mIsPlayOnAwake;
        private SerializedProperty mIsPlayOnEnable;
        private SerializedProperty mIsLooping;
        private SerializedProperty mDelayTime;
        private SerializedProperty mDuration;
        private SerializedProperty mLifetime;
        private SerializedProperty mSprite;
        private SerializedProperty mColorOverLifetime;
        private SerializedProperty mEmissionRange;
        private SerializedProperty mGravity;
        private SerializedProperty mDirection;
        private SerializedProperty mSize;
        private SerializedProperty mSizeOverLifetime;
        private SerializedProperty mSpeed;
        private SerializedProperty mSpeedOverLifetime;
        private SerializedProperty mRotationRange;
        private SerializedProperty mNumParticlePerSecond;
        private SerializedProperty mMaxParticleQuantity;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            mScript = (MyUGUIParticleSystem)target;

            mRectTransform = mScript.gameObject.GetComponent<RectTransform>();

            mIsPlayOnAwake = serializedObject.FindProperty("mIsPlayOnAwake");
            mIsPlayOnEnable = serializedObject.FindProperty("mIsPlayOnEnable");
            mIsLooping = serializedObject.FindProperty("mIsLooping");
            mDelayTime = serializedObject.FindProperty("mDelayTime");
            mDuration = serializedObject.FindProperty("mDuration");
            mLifetime = serializedObject.FindProperty("mLifetime");
            mSprite = serializedObject.FindProperty("mSprite");
            mColorOverLifetime = serializedObject.FindProperty("mColorOverLifetime");
            mEmissionRange = serializedObject.FindProperty("mEmissionRange");
            mGravity = serializedObject.FindProperty("mGravity");
            mDirection = serializedObject.FindProperty("mDirection");
            mSize = serializedObject.FindProperty("mSize");
            mSizeOverLifetime = serializedObject.FindProperty("mSizeOverLifetime");
            mSpeed = serializedObject.FindProperty("mSpeed");
            mSpeedOverLifetime = serializedObject.FindProperty("mSpeedOverLifetime");
            mRotationRange = serializedObject.FindProperty("mRotationRange");
            mNumParticlePerSecond = serializedObject.FindProperty("mNumParticlePerSecond");
            mMaxParticleQuantity = serializedObject.FindProperty("mMaxParticleQuantity");

            mETemplate = serializedObject.FindProperty("mETemplate");
            if ((MyUGUIParticleSystem.EParticleTemplate)mETemplate.enumValueIndex != MyUGUIParticleSystem.EParticleTemplate.None)
            {
                mScript.SetTemplate((MyUGUIParticleSystem.EParticleTemplate)mETemplate.enumValueIndex);
            }
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(mScript), typeof(MyUGUIParticleSystem), false);

            serializedObject.Update();

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Template", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            mETemplate.enumValueIndex = (int)(MyUGUIParticleSystem.EParticleTemplate)EditorGUILayout.EnumPopup("   Use A Template", (MyUGUIParticleSystem.EParticleTemplate)mETemplate.enumValueIndex);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                if ((MyUGUIParticleSystem.EParticleTemplate)mETemplate.enumValueIndex != MyUGUIParticleSystem.EParticleTemplate.None)
                {
                    mScript.SetTemplate((MyUGUIParticleSystem.EParticleTemplate)mETemplate.enumValueIndex);
                    mScript.Play();
                }
            }

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("System", EditorStyles.boldLabel);
            mIsPlayOnAwake.boolValue = EditorGUILayout.Toggle("   Play On Awake", mIsPlayOnAwake.boolValue);
            mIsPlayOnEnable.boolValue = EditorGUILayout.Toggle("   Play On Enable", mIsPlayOnEnable.boolValue);
            mIsLooping.boolValue = EditorGUILayout.Toggle("   Looping", mIsLooping.boolValue);
            mDelayTime.floatValue = Mathf.Max(EditorGUILayout.FloatField("   Delay Second", mDelayTime.floatValue), 0);
            mDuration.floatValue = Mathf.Max(EditorGUILayout.FloatField("   Duration", mDuration.floatValue), 0);

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Emission", EditorStyles.boldLabel);
            mLifetime.floatValue = Mathf.Max(EditorGUILayout.FloatField("   Lifetime", mLifetime.floatValue), 0);
            mEmissionRange.floatValue = EditorGUILayout.FloatField("   Angle Range", mEmissionRange.floatValue);
            mNumParticlePerSecond.floatValue = Mathf.Max(EditorGUILayout.FloatField("   Particles Per Second", mNumParticlePerSecond.floatValue), 0.01f);
            mMaxParticleQuantity.intValue = Mathf.Max(EditorGUILayout.IntField("   Max Particles", mMaxParticleQuantity.intValue), 1);

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Renderer", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(mSprite, new GUIContent("   Sprite"), true);
            EditorGUILayout.PropertyField(mColorOverLifetime, new GUIContent("   Color Over Lifetime"), true);

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Size", EditorStyles.boldLabel);
            mSize.floatValue = Mathf.Max(EditorGUILayout.FloatField("   Size", mSize.floatValue), 0);
            mSizeOverLifetime.animationCurveValue = EditorGUILayout.CurveField("   Size Over Lifetime", mSizeOverLifetime.animationCurveValue);

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Speed", EditorStyles.boldLabel);
            mSpeed.floatValue = Mathf.Max(EditorGUILayout.FloatField("   Speed", mSpeed.floatValue), 0);
            mSpeedOverLifetime.animationCurveValue = EditorGUILayout.CurveField("   Speed Over Lifetime", mSpeedOverLifetime.animationCurveValue);

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Direction", EditorStyles.boldLabel);
            mDirection.vector3Value = EditorGUILayout.Vector3Field("   Direction", mDirection.vector3Value);
            mGravity.vector3Value = EditorGUILayout.Vector3Field("   Gravity", mGravity.vector3Value);

            EditorGUILayout.LabelField(string.Empty);
            EditorGUILayout.LabelField("Rotation", EditorStyles.boldLabel);
            mRotationRange.vector2Value = EditorGUILayout.Vector2Field("   Rotation Per Second", mRotationRange.vector2Value);

            if (EditorGUI.EndChangeCheck())
            {
                mETemplate.enumValueIndex = (int)MyUGUIParticleSystem.EParticleTemplate.None;
                serializedObject.ApplyModifiedProperties();
                mScript.Play();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}