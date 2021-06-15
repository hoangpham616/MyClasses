using UnityEngine;
using UnityEngine.EventSystems;
using MyClasses;
using MyClasses.UI;

namespace MyApp.UI
{
    public class MainMenuScene : MyUGUIScene
    {
        #region ----- Variable -----

        private MyUGUIButton _btnRunningMessage;
        private MyUGUIButton _btnFlyingMessage;
        private MyUGUIButton _btnToastMessage;
        private MyUGUIButton _btnLoadingIndicator;
        private MyUGUIButton _btnDialog2Buttons;
        private MyUGUIButton _btnGameScene;
        private MyUGUIButton _btnPool;
        private MyUGUIButton _btnLocalization;
        private MyUGUIButton _btnAdMob;

        #endregion

        #region ----- Constructor -----

        public MainMenuScene(ESceneID id, string prefabName, bool isInitWhenLoadScene, bool isHideHUD = false, float fadeInDuration = 0.5f, float fadeOutDuration = 0.5f)
        : base(id, prefabName, isInitWhenLoadScene, isHideHUD, fadeInDuration, fadeOutDuration)
        {
        }

        #endregion

        #region ----- MyUGUIScene Implementation -----

        public override void OnUGUIInit()
        {
            base.OnUGUIInit();

            _btnRunningMessage = MyUtilities.FindObject(GameObject, "Buttons/ButtonRunningMessage").GetComponent<MyUGUIButton>();
            _btnFlyingMessage = MyUtilities.FindObject(GameObject, "Buttons/ButtonFlyingMessage").GetComponent<MyUGUIButton>();
            _btnToastMessage = MyUtilities.FindObject(GameObject, "Buttons/ButtonToastMessage").GetComponent<MyUGUIButton>();
            _btnLoadingIndicator = MyUtilities.FindObject(GameObject, "Buttons/ButtonLoadingIndicator").GetComponent<MyUGUIButton>();
            _btnDialog2Buttons = MyUtilities.FindObject(GameObject, "Buttons/ButtonDialog2Buttons").GetComponent<MyUGUIButton>();
            _btnGameScene = MyUtilities.FindObject(GameObject, "Buttons/ButtonGameScene").GetComponent<MyUGUIButton>();
            _btnPool = MyUtilities.FindObject(GameObject, "Buttons/ButtonPool").GetComponent<MyUGUIButton>();
            _btnLocalization = MyUtilities.FindObject(GameObject, "Buttons/ButtonLocalization").GetComponent<MyUGUIButton>();
            _btnAdMob = MyUtilities.FindObject(GameObject, "Buttons/ButtonAdMob").GetComponent<MyUGUIButton>();
        }

        public override void OnUGUIEnter()
        {
            Debug.Log("MainMenuScene.OnUGUIEnter()");

            base.OnUGUIEnter();

            _btnRunningMessage.OnEventPointerDoubleClick.AddListener(_OnClickRunningMessage);
            _btnFlyingMessage.OnEventPointerClick.AddListener(_OnClickFlyingMessage);
            _btnToastMessage.OnEventPointerClick.AddListener(_OnClickToastMessage);
            _btnLoadingIndicator.OnEventPointerClick.AddListener(_OnClickLoadingIndicator);
            _btnDialog2Buttons.OnEventPointerClick.AddListener(_OnClickDialog2Buttons);
            _btnGameScene.OnEventPointerClick.AddListener(_OnClickGameScene);
            _btnPool.OnEventPointerClick.AddListener(_OnClickPool);
            _btnLocalization.OnEventPointerClick.AddListener(_OnClickLocalization);
            _btnAdMob.OnEventPointerClick.AddListener(_OnClickAdMob);
        }

        public override bool OnUGUIVisible()
        {
            Debug.Log("MainMenuScene.OnUGUIVisible()");

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
            Debug.Log("MainMenuScene.OnUGUIExit()");

            base.OnUGUIExit();

            _btnRunningMessage.OnEventPointerClick.RemoveAllListeners();
            _btnFlyingMessage.OnEventPointerClick.RemoveAllListeners();
            _btnToastMessage.OnEventPointerClick.RemoveAllListeners();
            _btnLoadingIndicator.OnEventPointerClick.RemoveAllListeners();
            _btnDialog2Buttons.OnEventPointerClick.RemoveAllListeners();
            _btnGameScene.OnEventPointerClick.RemoveAllListeners();
            _btnPool.OnEventPointerClick.RemoveAllListeners();
            _btnLocalization.OnEventPointerClick.RemoveAllListeners();
            _btnAdMob.OnEventPointerClick.RemoveAllListeners();
        }

        public override bool OnUGUIInvisible()
        {
            Debug.Log("MainMenuScene.OnUGUIInvisible()");

            if (base.OnUGUIInvisible())
            {
                return true;
            }
            return false;
        }

        public override void OnUGUIBackKey()
        {
            Debug.Log("MainMenuScene.OnUGUIBackKey()");
        }

        #endregion

        #region ----- Button Event -----

        private void _OnClickRunningMessage(PointerEventData arg0)
        {
            Debug.Log("MainMenuScene._OnClickRunningMessage()");

            MyUGUIManager.Instance.SetRunningMessageMaxQueue(MyUGUIRunningMessage.EType.Default, 3);
            MyUGUIManager.Instance.ShowRunningMessage("This is Running Message 1 (will not show because out of queue)", ERunningMessageSpeed.Normal, MyUGUIRunningMessage.EType.Default);
            MyUGUIManager.Instance.ShowRunningMessage("This is Running Message 2 (will not show because out of queue)", ERunningMessageSpeed.Normal, MyUGUIRunningMessage.EType.Default);
            MyUGUIManager.Instance.ShowRunningMessage("This is Running Message 3", ERunningMessageSpeed.Normal, MyUGUIRunningMessage.EType.Default);
            MyUGUIManager.Instance.ShowRunningMessage("This is Running Message 4", ERunningMessageSpeed.Normal, MyUGUIRunningMessage.EType.Default);
            MyUGUIManager.Instance.ShowRunningMessage("This is Running Message 5", ERunningMessageSpeed.Normal, MyUGUIRunningMessage.EType.Default);
        }

        private void _OnClickFlyingMessage(PointerEventData arg0)
        {
            Debug.Log("MainMenuScene._OnClickFlyingMessage()");

            MyUGUIManager.Instance.ShowFlyingMessage("This is Flying Message", MyUGUIFlyingMessage.EType.ShortFlyFromBot);
        }

        private void _OnClickToastMessage(PointerEventData arg0)
        {
            Debug.Log("MainMenuScene._OnClickToastMessage()");

            MyUGUIManager.Instance.ShowToastMessage("This is Toast Message", EToastMessageDuration.Medium);
        }

        private void _OnClickLoadingIndicator(PointerEventData arg0)
        {
            Debug.Log("MainMenuScene._OnClickLoadingIndicator()");

            int id1 = MyUGUIManager.Instance.ShowLoadingIndicator(1, () =>
            {
                Debug.Log("Loading Indicator 1 timeout");
            });
            int id2 = MyUGUIManager.Instance.ShowLoadingIndicator(2, () =>
            {
                Debug.Log("Loading Indicator 2 timeout");
            });
            int id3 = MyUGUIManager.Instance.ShowLoadingIndicator(3, () =>
            {
                Debug.Log("Loading Indicator 3 timeout");
            });
        }

        private void _OnClickDialog2Buttons(PointerEventData arg0)
        {
            Debug.Log("MainMenuScene._OnClickDialog2Buttons()");

            MyUGUIPopup2Buttons popup = (MyUGUIPopup2Buttons)MyUGUIManager.Instance.ShowPopup(EPopupID.Dialog2ButtonsPopup);
            popup.SetData("TITLE", "Body", "Left", (data) =>
            {
                Debug.Log("Click Left Button");
            }, "Right", (data) =>
            {
                Debug.Log("Click Right Button");
            }, (data) =>
            {
                Debug.Log("Click Close Button");
            }, false);
        }

        private void _OnClickGameScene(PointerEventData arg0)
        {
            Debug.Log("MainMenuScene._OnClickGameScene()");

            MyUGUIManager.Instance.ShowScene(ESceneID.GameScene);
        }

        private void _OnClickPool(PointerEventData arg0)
        {
            Debug.Log("MainMenuScene._OnClickPool()");

            MyUGUIManager.Instance.ShowPopup(EPopupID.PoolPopup);
        }

        private void _OnClickLocalization(PointerEventData arg0)
        {
            Debug.Log("MainMenuScene._OnClickLocalization()");

            MyUGUIManager.Instance.ShowPopup(EPopupID.LocalizationPopup);
        }

        private void _OnClickAdMob(PointerEventData arg0)
        {
            Debug.Log("MainMenuScene._OnClickAdMob()");

            MyUGUIManager.Instance.ShowPopup(EPopupID.AdMobPopup);
        }

        #endregion

        #region ----- Public Method -----



        #endregion

        #region ----- Private Method -----



        #endregion
    }
}