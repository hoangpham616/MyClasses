/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUITouchZone (version 2.0)
 */

using UnityEngine.UI;

namespace MyClasses.UI
{
    public class MyUGUITouchZone : Graphic
    {
        #region ----- Implement Graphic -----

        /// <summary>
        /// OnPopulateMesh.
        /// </summary>
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }

        #endregion
    }
}
