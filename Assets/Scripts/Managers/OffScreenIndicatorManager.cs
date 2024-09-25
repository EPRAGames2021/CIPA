using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CIPA
{
    public class OffScreenIndicatorManager : MonoBehaviour
    {
        public List<OffScreenIndicator> targetIndicators;

        [SerializeField] private GameObject _indicatorPrefab;
        [SerializeField] private float _checkTime = 0.1f;
        [SerializeField] private Vector2 _offset;
        [SerializeField] private Vector2 _offsetScale;
        [SerializeField] private float _distanceLimit;
        [SerializeField] private Transform _parentToInstantiate;

        [SerializeField] private Player _player;
        private Camera _activeCamera;

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            UpdateIndicators();
        }



        private void Init()
        {
            _activeCamera = Camera.main;

            InstantiateIndicators();
        }


        public void SetupIndicators(List<IndicatorWrapper> objectsToIndicate)
        {
            while (targetIndicators.Count > 0)
            {
                Destroy(targetIndicators[0].IndicatorUI.gameObject);
                targetIndicators.Remove(targetIndicators[0]);
            }

            foreach (IndicatorWrapper indicatorWrapper in objectsToIndicate)
            {
                AddTarget(indicatorWrapper);
            }
        }


        public void AddTarget(IndicatorWrapper indicatorWrapper)
        {
            OffScreenIndicator indicator = new()
            {
                Target = indicatorWrapper.GameObject.transform,
                Sprite = indicatorWrapper.Icon
            };

            targetIndicators.Add(indicator);

            InstantiateIndicators();
        }
        

        private void InstantiateIndicators()
        {
            foreach (var targetIndicator in targetIndicators)
            {
                if (targetIndicator.IndicatorUI == null)
                {
                    targetIndicator.IndicatorUI = Instantiate(_indicatorPrefab).transform;
                    targetIndicator.IndicatorUI.transform.parent = _parentToInstantiate;
                }

                var rectTransform = targetIndicator.IndicatorUI.GetComponent<RectTransform>();

                if (rectTransform == null)
                {
                    rectTransform = targetIndicator.IndicatorUI.gameObject.AddComponent<RectTransform>();
                }

                targetIndicator.RectTransform = rectTransform;


                IndicatorBaloon indicator = targetIndicator.IndicatorUI.GetComponent<IndicatorBaloon>();
                indicator.SetIcon(targetIndicator.Sprite);
            }
        }

        private void UpdateIndicators()
        {
            foreach (var targetIndicator in targetIndicators)
            {
                targetIndicator.RectTransform.localScale = _offsetScale;
                UpdatePosition(targetIndicator);
            }
        }

        private void UpdatePosition(OffScreenIndicator targetIndicator)
        {
            //turn off UI indicator if object it is pointing to is disabled
            if (!targetIndicator.Target.gameObject.active)
            {
                targetIndicator.IndicatorUI.gameObject.SetActive(false);
                return;
            }

            var rect = targetIndicator.RectTransform.rect;
            var indicatorPosition = _activeCamera.WorldToScreenPoint(targetIndicator.Target.position);

            if (indicatorPosition.z < 0)
            {
                indicatorPosition.y = -indicatorPosition.y;
                indicatorPosition.x = -indicatorPosition.x;
            }

            var newPosition = new Vector3(indicatorPosition.x, indicatorPosition.y, indicatorPosition.z);

            indicatorPosition.x = Mathf.Clamp(indicatorPosition.x, rect.width / 2, Screen.width - rect.width / 2) + _offset.x;
            indicatorPosition.y = Mathf.Clamp(indicatorPosition.y, rect.height / 2, Screen.height - rect.height / 2) + _offset.y;
            indicatorPosition.z = 0;

            targetIndicator.IndicatorUI.up = (newPosition - indicatorPosition).normalized;
            targetIndicator.IndicatorUI.position = indicatorPosition;


            bool shoudDisplay = Vector3.Distance(targetIndicator.Target.gameObject.transform.position, _player.transform.position) > _distanceLimit;

            targetIndicator.IndicatorUI.gameObject.SetActive(shoudDisplay);
        }
    }
}
