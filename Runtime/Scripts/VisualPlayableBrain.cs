using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace GBG.VisualPlayable
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public class VisualPlayableBrain : MonoBehaviour
    {
        private Animator _animator;

        private PlayableGraph _graph;

        private AnimationLayerMixerPlayable _layerMixer;


        #region Graph Management

        public bool IsPlaying()
        {
            return _graph.IsPlaying();
        }

        public DirectorUpdateMode GetTimeUpdateMode()
        {
            return _graph.GetTimeUpdateMode();
        }

        public void SetTimeUpdateMode(DirectorUpdateMode updateMode)
        {
            _graph.SetTimeUpdateMode(updateMode);
        }

        // TODO: Update layers
        public void ManualUpdate(float deltaTime)
        {
            _graph.Evaluate(deltaTime);
        }

        public void Play()
        {
            _graph.Play();
        }

        public void Stop()
        {
            _graph.Stop();
        }

        #endregion


        #region Layer Management

        private readonly List<AnimationLayer> _layers = new();


        public byte AddLayer(string layerName, byte inputCount = 0,
            float weight = 0, bool isAdditive = false, AvatarMask mask = null)
        {
            if (layerName == null)
            {
                throw new ArgumentNullException(nameof(layerName));
            }

            if (FindLayerIndex(layerName) > -1)
            {
                throw new ArgumentException($"Layer '{layerName}' already exists.", nameof(layerName));
            }

            var existLayerCount = _layerMixer.GetInputCount();
            if (existLayerCount > byte.MaxValue)
            {
                throw new NotSupportedException($"Number of animation layer reaches the upper limit({byte.MaxValue}).");
            }

            var newLayerIndex = (byte)existLayerCount;
            var layer = new AnimationLayer(_graph, layerName, newLayerIndex);
            _layerMixer.AddInput(layer.RootMixer, 0, weight);
            _layerMixer.SetLayerAdditive(newLayerIndex, isAdditive);
            if (mask)
            {
                _layerMixer.SetLayerMaskFromAvatarMask(newLayerIndex, mask);
            }

            _layers.Add(layer);

            return newLayerIndex;
        }

        public AnimationLayer GetLayer(string layerName)
        {
            var layerIndex = FindLayerIndex(layerName);
            if (layerIndex < 0)
            {
                return null;
            }

            return GetLayer((byte)layerIndex);
        }

        public AnimationLayer GetLayer(byte layerIndex)
        {
            return _layers[layerIndex];
        }

        public bool SetLayerWeight(string layerName, float weight)
        {
            var layerIndex = FindLayerIndex(layerName);
            if (layerIndex < 0)
            {
                return false;
            }

            return SetLayerWeight((byte)layerIndex, weight);
        }

        public bool SetLayerWeight(byte layerIndex, float weight)
        {
            if (layerIndex < _layerMixer.GetInputCount())
            {
                _layerMixer.SetInputWeight(layerIndex, weight);
                return true;
            }

            return false;
        }

        public bool SetLayerAdditive(string layerName, bool isAdditive)
        {
            var layerIndex = FindLayerIndex(layerName);
            if (layerIndex < 0)
            {
                return false;
            }

            return SetLayerAdditive((byte)layerIndex, isAdditive);
        }

        public bool SetLayerAdditive(byte layerIndex, bool isAdditive)
        {
            if (layerIndex < _layerMixer.GetInputCount())
            {
                _layerMixer.SetLayerAdditive((uint)layerIndex, isAdditive);
                return true;
            }

            return false;
        }

        public int FindLayerIndex(string layerName)
        {
            for (int i = 0; i < _layers.Count; i++)
            {
                if (_layers[i].Name.Equals(layerName))
                {
                    return i;
                }
            }

            return -1;
        }

        #endregion


        #region Mono Messages

        private void OnEnable()
        {
            if (!_graph.IsValid())
            {
                _animator = GetComponent<Animator>();
                _graph = PlayableGraph.Create($"{nameof(VisualPlayableBrain)}.{name}");
                _layerMixer = AnimationLayerMixerPlayable.Create(_graph);

                var animOutput = AnimationPlayableOutput.Create(_graph, "Animation Output", _animator);
                animOutput.SetSourcePlayable(_layerMixer);
            }

            _graph.Play();
        }

        private void OnDisable()
        {
            if (_graph.IsValid())
            {
                _graph.Stop();
            }
        }

        private void OnDestroy()
        {
            if (_graph.IsValid())
            {
                _graph.Destroy();
            }
        }

        #endregion
    }
}