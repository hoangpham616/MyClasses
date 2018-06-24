********** How to setup **********

1/ On Menu Bar, choose "MyClasses/UGUI/Config ID" to config UI ID

2/ On Menu Bar, choose "MyClasses/UGUI/Config Scenes" to config Unity Scenes, Scenes, HUDs

3/ On Menu Bar, choose "MyClasses/UGUI/Config Popups" to config Popups

4/ On Menu Bar, choose "MyClasses/UGUI/Create MyUGUIBooter" to create MyUGUIBooter

5/ On Hierarchy, choose "MyUGUIBooter" to config Boot Mode

6/ Click "Play" to start

***********************************





********** How to use **********

+ Copy scripts from "Scripts/UGUI/Sample" to re-use

+ Call methods from MyUGUIManager class:

  - MyUGUIManager.Instance.ShowUnityScene(EUnitySceneID.Main, ESceneID.Lobby)

  - MyUGUIManager.Instance.ShowScene(ESceneID.MainMenu)

  - MyUGUIManager.Instance.ShowPopup(EPopupID.Setting)

  - MyUGUIManager.Instance.ShowFloatPopup(EPopupID.BattleInvite)

  - MyUGUIManager.Instance.ShowLoadingIndicator(ELoadingIndicatorID.Circle, 30, onTimeOutCallback)

  - MyUGUIManager.Instance.ShowRunningText("This is Running Text", ERunningTextSpeed.Normal)

  - MyUGUIManager.Instance.ShowToast("This is Toast", EToastDuration.Medium)

  - MyUGUIManager.Instance.Back()

 + Methods that support AssetBundles:

  - MyUGUIManager.Instance.SetAssetBundleForCore("url", 1)

  - MyUGUIManager.Instance.SetAssetBundleForHUDs("url", 1)

  - MyUGUIManager.Instance.SetAssetBundleForScene(ESceneID.Login, "url", 1)

  - MyUGUIManager.Instance.SetAssetBundleForPopup(EPopupID.Event, "url", 1)

********************************





********** How to custom UI **********

+ Scenes: "Resources\Prefabs\UGUI\Scenes\"

+ Popups: "Resources\Prefabs\UGUI\Popups\"

+ HUDs: "Resources\Prefabs\UGUI\HUDs\"

+ Popup overlay, loading indicator, toast, running text...: "Resources\Prefabs\UGUI\Specials\"

**************************************