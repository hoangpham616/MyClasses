/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyPoolManager (version 2.11)
 */

using UnityEngine;
using System.Collections.Generic;

namespace MyClasses
{
    public class MyPoolManager : MonoBehaviour
    {
        #region ----- Variable -----

        private Dictionary<string, MyPool> mDictPooler = new Dictionary<string, MyPool>();

        #endregion

        #region ----- Singleton -----

        private static object mSingletonLock = new object();
        private static MyPoolManager mInstance;

        public static MyPoolManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (mSingletonLock)
                    {
                        mInstance = (MyPoolManager)FindObjectOfType(typeof(MyPoolManager));
                        if (mInstance == null)
                        {
                            GameObject obj = new GameObject(typeof(MyPoolManager).Name);
                            mInstance = obj.AddComponent<MyPoolManager>();
                            DontDestroyOnLoad(obj);
                        }
                    }
                }
                return mInstance;
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Provide an available object. If no objects are present in the pool, a new object is created and returned.
        /// </summary>
        /// <param name="isForceCreate">always create new object</param>
        public GameObject Use(string path, bool isForceCreate = false)
        {
            if (mDictPooler.ContainsKey(path))
            {
                if (isForceCreate)
                {
                    return mDictPooler[path].Create();
                }

                return mDictPooler[path].Use();
            }

            GameObject root = MyUtilities.FindObjectInFirstLayer(gameObject, path);
            if (root == null)
            {
                root = new GameObject(path);
                root.transform.SetParent(transform, false);
            }

            GameObject rootFree = MyUtilities.FindObjectInFirstLayer(root, "Free");
            if (rootFree == null)
            {
                rootFree = new GameObject("Free");
                rootFree.transform.SetParent(root.transform, false);
            }

            GameObject rootOccupied = MyUtilities.FindObjectInFirstLayer(root, "Occupied");
            if (rootOccupied == null)
            {
                rootOccupied = new GameObject("Occupied");
                rootOccupied.transform.SetParent(root.transform, false);
            }

            mDictPooler.Add(path, new MyPool(path, rootFree, rootOccupied));

            return Use(path, isForceCreate);
        }

        /// <summary>
        /// Provide an available object. If no objects are present in the pool, a new object is created and returned.
        /// </summary>
        public GameObject Use(GameObject prefab)
        {
            if (mDictPooler.ContainsKey(prefab.name))
            {
                mDictPooler[prefab.name].SetTemplate(prefab);
                return mDictPooler[prefab.name].Use();
            }

            GameObject root = MyUtilities.FindObjectInFirstLayer(gameObject, prefab.name);
            if (root == null)
            {
                root = new GameObject(prefab.name);
                root.transform.SetParent(transform, false);
            }

            GameObject rootFree = MyUtilities.FindObjectInFirstLayer(root, "Free");
            if (rootFree == null)
            {
                rootFree = new GameObject("Free");
                rootFree.transform.SetParent(root.transform, false);
            }

            GameObject rootOccupied = MyUtilities.FindObjectInFirstLayer(root, "Occupied");
            if (rootOccupied == null)
            {
                rootOccupied = new GameObject("Occupied");
                rootOccupied.transform.SetParent(root.transform, false);
            }

            mDictPooler.Add(prefab.name, new MyPool(prefab.name, rootFree, rootOccupied));

            mDictPooler[prefab.name].SetTemplate(prefab);
            return Use(prefab.name, false);
        }

        /// <summary>
        /// Add an object back into the pool.
        /// </summary>
        public void Return(GameObject obj)
        {
            if (obj != null)
            {
                MyPooledObject poolObject = obj.GetComponent<MyPooledObject>();
                if (poolObject != null)
                {
                    if (mDictPooler.ContainsKey(poolObject.Pool))
                    {
                        mDictPooler[poolObject.Pool].Return(obj);
                    }
                }
            }
        }

        #endregion
    }

    public class MyPool
    {
        #region ----- Variable -----

        private string mPath;
        private string mObjectName;
        private GameObject mTemplate;
        private GameObject mFree;
        private GameObject mOccupied;
        private List<GameObject> mListFreeObject;
        private List<GameObject> mListOccupiedObject;

        #endregion

        #region ----- Property -----

        public string Name
        {
            get { return mPath; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyPool(string path, GameObject rootFree, GameObject rootOccupied)
        {
            mPath = path;
            mObjectName = path.Substring(path.LastIndexOf("/") + 1);
            mFree = rootFree;
            mOccupied = rootOccupied;
            mListFreeObject = new List<GameObject>();
            mListOccupiedObject = new List<GameObject>();
        }

        #endregion

        #region ----- Public Method -----
        
        /// <summary>
        /// Set the template.
        /// </summary>
        public void SetTemplate(GameObject template)
        {
            mTemplate = template;
        }

        /// <summary>
        /// Provide an available object. If no objects are present in the pool, a new object is created and returned.
        /// </summary>
        public GameObject Use()
        {
            if (mListFreeObject != null)
            {
                GameObject oldObj;

                int count = mListFreeObject.Count;
                for (int i = 0; i < count; i++)
                {
                    oldObj = mListFreeObject[i];
                    if (!oldObj.activeInHierarchy)
                    {
                        oldObj.transform.SetParent(mOccupied.transform, false);
                        oldObj.SetActive(true);

                        if (!mListOccupiedObject.Contains(oldObj))
                        {
                            mListOccupiedObject.Add(oldObj);
                        }

                        mListFreeObject.Remove(oldObj);

                        return oldObj;
                    }
                }
            }

            return Create();
        }

        /// <summary>
        /// Add an object back into the pool.
        /// </summary>
        public void Return(GameObject obj)
        {
            if (obj != null)
            {
                obj.transform.SetParent(mFree.transform, false);
                obj.SetActive(false);

                if (!mListFreeObject.Contains(obj))
                {
                    mListFreeObject.Add(obj);
                }

                mListOccupiedObject.Remove(obj);
            }
        }

        /// <summary>
        /// Create a new object.
        /// </summary>
        public GameObject Create()
        {
            if (mTemplate == null)
            {
                mTemplate = Resources.Load(mPath) as GameObject;
                if (mTemplate == null)
                {
                    Debug.LogError("[" + typeof(MyPool).Name + "] Create(): Could not find file \"" + mPath + "\"");
                    return null;
                }
            }

            GameObject newObj = GameObject.Instantiate(mTemplate);
            newObj.SetActive(true);
            newObj.name = mObjectName;
            newObj.transform.SetParent(mOccupied.transform, false);

            MyPooledObject poolObject = newObj.GetComponent<MyPooledObject>();
            if (poolObject == null)
            {
                poolObject = newObj.AddComponent<MyPooledObject>();
            }
            poolObject.SetPool(mPath);

            mListOccupiedObject.Add(newObj);

            return newObj;
        }

        #endregion
    }
}