using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class TransformationBehaviour : MonoBehaviour
    {
        public ScriptableObject[] _transformations;

        private int _currentTransformation = 0;

        private Dictionary<string, object> _context = new Dictionary<string, object>();

        void Start()
        {
            InitializeTransformations();
        }

        public void InitializeTransformations()
        {
            _currentTransformation = 0;

            foreach (var transformation in _transformations)
            {
                if (transformation is ITransformation)
                {
                    ((ITransformation)transformation).Initialize(_context);
                }
            }
        }


        // Update is called once per frame
        void Update()
        {
            if (_currentTransformation < _transformations.Length)
            {
                var transformation = _transformations[_currentTransformation] as ITransformation;

                if (transformation != null)
                {                   
                    if (transformation.State == TransformationState.Active)
                    {
                        transformation.Iterate(_context);
                    }

                    if (transformation.State == TransformationState.Complete)
                    {
                        _currentTransformation++;
                    }
                }
                else
                {
                    Debug.LogWarning("Cannot handle transformation " + _transformations[_currentTransformation]  + ", it does not implement " + nameof(ITransformation));
                    _currentTransformation++;
                }

            }
        }
    }
}