using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using MyClasses;
using MyClasses.UI;

namespace MyApp
{
    public class PoolPopup : MyUGUIPopup
    {
        #region ----- Variable -----

        private MyUGUIButton _btnClose;
        private MyUGUIButton _btnUse;
        private MyUGUIButton _btnReturn;
        private Transform _trfItemParent;

        private List<Text> _txtItems = new List<Text>();

        #endregion

        #region ----- Constructor -----

        public PoolPopup(EPopupID id, string prefabName, bool isFloat = false, bool isRepeatable = false)
            : base(id, prefabName, isFloat, isRepeatable)
        {
        }

        #endregion

        #region ----- MyUGUIPopup Implementation -----

        public override void OnUGUIInit()
        {
            Debug.Log("PoolPopup.OnUGUIInit()");

            base.OnUGUIInit();

            _btnClose = MyUtilities.FindObject(GameObject, "Container/ButtonClose").GetComponent<MyUGUIButton>();
            _btnUse = MyUtilities.FindObject(GameObject, "Container/ButtonUse").GetComponent<MyUGUIButton>();
            _btnReturn = MyUtilities.FindObject(GameObject, "Container/ButtonReturn").GetComponent<MyUGUIButton>();
            _trfItemParent = MyUtilities.FindObject(GameObject, "Container/Items").transform;
        }

        public override void OnUGUIEnter()
        {
            Debug.Log("PoolPopup.OnUGUIEnter()");

            base.OnUGUIEnter();

            _btnClose.OnEventPointerClick.AddListener(_OnClickClose);
            _btnUse.OnEventPointerClick.AddListener(_OnClickUse);
            _btnReturn.OnEventPointerClick.AddListener(_OnClickReturn);
        }

        public override bool OnUGUIVisible()
        {
            Debug.Log("PoolPopup.OnUGUIVisible()");

            if (base.OnUGUIVisible())
            {
                return true;
            }
            return false;
        }

        public override void OnUGUIUpdate(float deltaTime)
        {
        }

        public override void OnUGUIExit()
        {
            Debug.Log("PoolPopup.OnUGUIExit()");

            base.OnUGUIExit();

            _btnClose.OnEventPointerClick.RemoveAllListeners();
            _btnUse.OnEventPointerClick.RemoveAllListeners();
            _btnReturn.OnEventPointerClick.RemoveAllListeners();

            for (int i = _txtItems.Count - 1; i >= 0; --i)
            {
                MyPoolManager.Instance.Return(_txtItems[i].gameObject);
            }
            _txtItems.Clear();
        }

        public override bool OnUGUIInvisible()
        {
            Debug.Log("PoolPopup.OnUGUIInvisible()");

            if (base.OnUGUIInvisible())
            {
                return true;
            }
            return false;
        }

        public override void OnUGUIBackKey()
        {
            MyUGUIManager.Instance.Back();
        }

        #endregion

        #region ----- Button Event -----

        private void _OnClickClose(PointerEventData arg0)
        {
            Hide();
        }

        private void _OnClickUse(PointerEventData arg0)
        {
            Debug.Log("PoolManager._OnClickUse()");

            GameObject prefab = MyResourceManager.LoadPrefab("Prefabs/TextPoolObject");

            Text text = MyPoolManager.Instance.Use(prefab).GetComponent<Text>();
            text.transform.SetParent(_trfItemParent, false);
            text.transform.localPosition = new Vector3(UnityEngine.Random.Range(-300, 300), UnityEngine.Random.Range(-300, 300), 0);
            _txtItems.Add(text);
        }

        private void _OnClickReturn(PointerEventData arg0)
        {
            Debug.Log("PoolManager._OnClickReturn()");

            if (_txtItems.Count > 0)
            {
                MyPoolManager.Instance.Return(_txtItems[0].gameObject);
                _txtItems.RemoveAt(0);
            }
        }

        #endregion

        #region ----- Public Method -----



        #endregion

        #region ----- Private Method -----



        #endregion
    }
}