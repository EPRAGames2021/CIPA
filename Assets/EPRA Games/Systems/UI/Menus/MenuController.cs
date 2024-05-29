using UnityEngine;

namespace EPRA.Utilities
{
    public abstract class MenuController : MonoBehaviour
    {
        [SerializeField] protected MenuType _menu;

        public static event System.Action<MenuType> OnMenuClosed;

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


        protected void InvokeOnMenuClosed()
        {
            OnMenuClosed?.Invoke(_menu);
        }

        protected void CloseMenu()
        {
            CanvasManager.Instance.CloseMenu(_menu);
        }

        public abstract void SelectUI();
    }
}


