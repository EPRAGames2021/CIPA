using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CIPA
{
    public class PlateAndCoilGrid : MonoBehaviour
    {
        [SerializeField] private DraggableObject _pipe;

        [SerializeField] private List<FillableContainer> _containers;

        [SerializeField] private bool _filling;

        public bool Filling { get { return _filling; } set { _filling = value; } }


        public event System.Action OnContainerOverflow;


        private void Start()
        {
            Init();
        }

        private void Update()
        {
            FillContainers();
        }

        private void OnDestroy()
        {
            Finish();
        }


        private void Init()
        {
            for (int i = 0; i < _containers.Count; i++)
            {
                _containers[i].OnContainerIsFull += ContainerOverflow;
            }
        }

        private void Finish()
        {
            for (int i = 0; i < _containers.Count; i++)
            {
                _containers[i].OnContainerIsFull -= ContainerOverflow;
            }
        }


        private void ContainerOverflow()
        {
            OnContainerOverflow?.Invoke();

            _filling = false;

            ResetGrid();
        }


        public bool CheckForCompletion()
        {
            return _containers[0].IsReady && _containers[1].IsReady;
        }

        public void ResetGrid()
        {
            for (int i = 0; i < _containers.Count; i++)
            {
                _containers[i].ResetValue();
            }
        }


        private void FillContainers()
        {
            if (_filling)
            {
                _containers[_pipe.RotationIndex].AddToValue(Time.deltaTime);
            }
        }
    }
}
