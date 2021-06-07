using UnityEngine;
using UnityEngine.EventSystems;
using MyClasses;
using MyClasses.UI;

namespace MyApp.UI
{
    public class GameScene : MyUGUIScene
    {
        #region ----- Variable -----
        
        private MyUGUIButton _btnMainMenuScene;

        #endregion

        #region ----- Constructor -----

        public GameScene(ESceneID id, string prefabName, bool isInitWhenLoadScene, bool isHideHUD = false, float fadeInDuration = 0.5f, float fadeOutDuration = 0.5f)
        : base(id, prefabName, isInitWhenLoadScene, isHideHUD, fadeInDuration, fadeOutDuration)
        {
        }

        #endregion

        #region ----- MyUGUIScene Implementation -----

        public override void OnUGUIInit()
        {
            Debug.Log("GameScene.OnUGUIInit()");

            base.OnUGUIInit();

            _btnMainMenuScene = MyUtilities.FindObject(GameObject, "Buttons/ButtonMainMenuScene").GetComponent<MyUGUIButton>();
        }

        public override void OnUGUIEnter()
        {
            Debug.Log("GameScene.OnUGUIEnter()");

            base.OnUGUIEnter();
            
            _btnMainMenuScene.OnEventPointerClick.AddListener(_OnClickMainMenuScene);
        }

        public override bool OnUGUIVisible()
        {
            Debug.Log("GameScene.OnUGUIVisible()");

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
            Debug.Log("GameScene.OnUGUIExit()");

            base.OnUGUIExit();

            _btnMainMenuScene.OnEventPointerClick.RemoveAllListeners();
        }

        public override bool OnUGUIInvisible()
        {
            Debug.Log("GameScene.OnUGUIInvisible()");

            if (base.OnUGUIInvisible())
            {
                return true;
            }
            return false;
        }

        public override void OnUGUIBackKey()
        {
            Debug.Log("GameScene.OnUGUIBackKey()");

            MyUGUIManager.Instance.Back();
        }

        #endregion

        #region ----- Button Event -----

        private void _OnClickMainMenuScene(PointerEventData arg0)
        {
            MyUGUIManager.Instance.Back();
        }

        #endregion

        #region ----- Public Method -----



        #endregion

        #region ----- Private Method -----



        #endregion
    }
}