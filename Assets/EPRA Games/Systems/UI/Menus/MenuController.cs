using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EPRA.Utilities
{
    public abstract class MenuController : MonoBehaviour
    {
        [SerializeField] protected MenuType _menu;

        public MenuType Menu 
        { 
            get 
            { 
                return _menu;
            } 
            set 
            { 
                _menu = value;
            } 
        }
    }
}


