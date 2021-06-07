/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MyUGUIDropdown (version 2.0)
 */

#pragma warning disable 0114
#pragma warning disable 0414
#pragma warning disable 0649

using UnityEngine;
using UnityEngine.UI;
using System;

namespace MyClasses.UI
{
    public class MyUGUIDropdown : Dropdown 
    {
        #region ----- Property -----

        public Action OnOpen { get; set; }
        public Action OnClose { get; set; }

        #endregion

        #region ----- Implement Dropdown -----

        /// <summary>
        /// CreateDropdownList.
        /// </summary>
        /// <returns></returns>
        protected override GameObject CreateDropdownList(GameObject template)
        {
            GameObject dropdownList = base.CreateDropdownList(template);

            if (OnOpen != null)
            {
                OnOpen();
            }

            return dropdownList;
        }

        /// <summary>
        /// DestroyDropdownList.
        /// </summary>
        protected override void DestroyDropdownList(GameObject dropdownList)
        {
            base.DestroyDropdownList(dropdownList);

            if (OnClose != null)
            {
                OnClose();
            }
        }

        #endregion
    }
}
