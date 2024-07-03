using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CIPA
{
    public class PPEOutdoor : MonoBehaviour
    {
        [SerializeField] private List<EquipmentSO> _requiredEquipment;

        [SerializeField] private List<GameObject> _PPEsIcons;

        [SerializeField] private GameObject _PPEPrefab;
        [SerializeField] private GameObject _PPEsContainer;


        private void Start()
        {
            Init();
        }

        private void OnDestroy()
        {
            Finish();
        }


        private void Init()
        {
            _requiredEquipment = JobAreaManager.Instance.JobSectorAreaSO.CurrentJob.RequiredEquipmentSO;

            for (int i = 0; i < _requiredEquipment.Count; i++)
            {
                GameObject newEquipment = Instantiate(_PPEPrefab, _PPEsContainer.transform);

                newEquipment.TryGetComponent(out Image icon);

                if (icon != null)
                {
                    icon.sprite = _requiredEquipment[i].Icon;
                }

                _PPEsIcons.Add(newEquipment);
            }
        }

        private void Finish()
        {
            while (_PPEsIcons.Count > 0)
            {
                Destroy(_PPEsIcons[0]);

                _PPEsIcons.RemoveAt(0);
            }
        }
    }
}
