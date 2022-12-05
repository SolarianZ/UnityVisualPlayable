﻿using GBG.VisualPlayable.Attribute;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace GBG.VisualPlayable
{
    public class AnimationBrain : IDisposable
    {
        private readonly Animator _animator;

        private PlayableGraph _graph;

        private AnimationLayerMixerPlayable _layerMixer;

        private bool _isManualUpdate;


        public AnimationBrain(Animator animator, string name)
        {
            _animator = animator;
            _graph = PlayableGraph.Create($"{nameof(AnimationBrain)}.{name}");
            _layerMixer = AnimationLayerMixerPlayable.Create(_graph);

            var driverPlayable = ScriptPlayable<AnimationBrainBehaviour>.Create(_graph);
            var driverBehaviour = driverPlayable.GetBehaviour();
            driverBehaviour.Initialize(this);
            var scriptOutput = ScriptPlayableOutput.Create(_graph, "Script Output");
            scriptOutput.SetSourcePlayable(driverPlayable);

            var animOutput = AnimationPlayableOutput.Create(_graph, "Animation Output", _animator);
            animOutput.SetSourcePlayable(_layerMixer);

            _graph.Play();
        }

        public void Dispose()
        {
            if (_graph.IsValid())
            {
                _graph.Destroy();
            }
        }


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
            _isManualUpdate = updateMode == DirectorUpdateMode.Manual;
        }

        public void Play()
        {
            _graph.Play();
        }

        public void Stop()
        {
            _graph.Stop();
        }

        public void Evaluate(float deltaTime)
        {
            _graph.Evaluate(deltaTime);
        }

        #endregion


        #region Layer Management

        private readonly List<AnimationLayer> _layers = new();


        internal void UpdateLayers(float deltaTime)
        {
            foreach (var layer in _layers)
            {
                layer.Update(deltaTime);
            }
        }


        [FailureOutputTriggerOnException]
        [OutputPortLabel("Layer Index")]
        public byte AddLayer(string layerName, float weight = 0, bool isAdditive = false, AvatarMask mask = null)
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

        [NodeSuffix("By Name")]
        [FailureOutputTriggerOnReturnNull]
        public AnimationLayer GetLayer(string layerName)
        {
            var layerIndex = FindLayerIndex(layerName);
            if (layerIndex < 0)
            {
                return null;
            }

            return GetLayer((byte)layerIndex);
        }

        [NodeSuffix("By Index")]
        [FailureOutputTriggerOnReturnNull]
        public AnimationLayer GetLayer(byte layerIndex)
        {
            return _layers[layerIndex];
        }

        [NodeSuffix("By Name")]
        [OutputPortLabel("Success")]
        [FailureOutputTriggerOnReturnFalse]
        public bool SetLayerWeight(string layerName, float weight)
        {
            var layerIndex = FindLayerIndex(layerName);
            if (layerIndex < 0)
            {
                return false;
            }

            return SetLayerWeight((byte)layerIndex, weight);
        }

        [NodeSuffix("By Index")]
        [OutputPortLabel("Success")]
        [FailureOutputTriggerOnReturnFalse]
        public bool SetLayerWeight(byte layerIndex, float weight)
        {
            if (layerIndex < _layerMixer.GetInputCount())
            {
                _layerMixer.SetInputWeight(layerIndex, weight);
                return true;
            }

            return false;
        }

        [NodeSuffix("By Name")]
        [OutputPortLabel("Success")]
        [FailureOutputTriggerOnReturnFalse]
        public bool SetLayerAdditive(string layerName, bool isAdditive)
        {
            var layerIndex = FindLayerIndex(layerName);
            if (layerIndex < 0)
            {
                return false;
            }

            return SetLayerAdditive((byte)layerIndex, isAdditive);
        }

        [NodeSuffix("By Index")]
        [OutputPortLabel("Success")]
        [FailureOutputTriggerOnReturnFalse]
        public bool SetLayerAdditive(byte layerIndex, bool isAdditive)
        {
            if (layerIndex < _layerMixer.GetInputCount())
            {
                _layerMixer.SetLayerAdditive((uint)layerIndex, isAdditive);
                return true;
            }

            return false;
        }

        [FailureOutputTriggerOnReturnMinus]
        [OutputPortLabel("Layer Index")]
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
    }
}