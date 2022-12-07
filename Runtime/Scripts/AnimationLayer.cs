using GBG.VisualPlayable.Attribute;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace GBG.VisualPlayable
{
    public delegate void AnimationLayerEventHandler(string layerName, byte layerIndex, string mainAnimationTag);

    public class AnimationLayer
    {
        public string Name { get; }

        public byte Index { get; }

        internal AnimationMixerPlayable RootMixer { get; }


        private PlayableGraph _graph;


        internal AnimationLayer(PlayableGraph graph, string name, byte index)
        {
            _graph = graph;

            Name = name;
            Index = index;
            RootMixer = AnimationMixerPlayable.Create(_graph, 2);
            RootMixer.SetPropagateSetTime(true);
        }

        internal void Update(float deltaTime)
        {
            // Update cross fade
            UpdateCrossFade(deltaTime);
        }


        #region Layer Management

        public void PlayLayer()
        {
            RootMixer.SetSpeed(1);
        }

        public void StopLayer()
        {
            RootMixer.SetSpeed(0);
        }

        #endregion


        #region Animation Management

        public string MainAnimationTag { get; private set; } = string.Empty;

        public event AnimationLayerEventHandler OnClipEnter;

        public event AnimationLayerEventHandler OnClipExit;

        public event AnimationLayerEventHandler OnTransitionStart;

        public event AnimationLayerEventHandler OnTransitionComplete;

        public event AnimationLayerEventHandler OnTransitionInterrupted;


        private byte _mainInputIndex;

        private float _fixedCrossFadeTime;

        private float _fixedCorssFadeTimer;


        [FailureOutputTriggerOnReturnFalse(SuccessLabel = "True", FailureLabel = "False")]
        [OutputPortLabel("In Transition")]
        public bool IsInTransition()
        {
            return _fixedCorssFadeTimer < _fixedCrossFadeTime;
        }


        [OutputPortLabel("Speed")]
        public double GetSpeed()
        {
            var input = RootMixer.GetInput(_mainInputIndex);
            if (input.IsValid())
            {
                return input.GetSpeed();
            }

            return 0;
        }

        [OutputPortLabel("Mix Speeds")]
        public void GetMixSpeeds(IList<double> speeds)
        {
            speeds.Clear();

            var inputMixer = RootMixer.GetInput(_mainInputIndex);
            if (!inputMixer.IsValid())
            {
                return;
            }

            for (int i = 0; i < inputMixer.GetInputCount(); i++)
            {
                var input = inputMixer.GetInput(i);
                if (input.IsValid())
                {
                    speeds.Add(input.GetSpeed());
                }
                else
                {
                    speeds.Add(0);
                }
            }
        }

        public void SetSpeed(double speed)
        {
            var input = RootMixer.GetInput(_mainInputIndex);
            if (input.IsValid())
            {
                input.SetSpeed(speed);
            }
        }

        public void SetMixSpeeds(IReadOnlyList<float> speeds)
        {
            var inputMixer = RootMixer.GetInput(_mainInputIndex);
            if (!inputMixer.IsValid())
            {
                return;
            }

            var inputCount = inputMixer.GetInputCount();
            var speedCount = speeds.Count;
            if (inputCount != speedCount)
            {
                Debug.LogWarning($"The number of speed({speedCount}) doesn't match " +
                                 $"the number of main playable's input({inputCount}).");
            }

            for (int i = 0; i < inputCount && i < speedCount; i++)
            {
                var input = inputMixer.GetInput(i);
                if (input.IsValid())
                {
                    input.SetSpeed(speeds[i]);
                }
            }
        }


        [OutputPortLabel("Time")]
        public double GetTime()
        {
            var input = RootMixer.GetInput(_mainInputIndex);
            if (input.IsValid())
            {
                return input.GetTime();
            }

            return 0;
        }

        [OutputPortLabel("Mix Times")]
        public void GetMixTimes(IList<double> times)
        {
            times.Clear();

            var inputMixer = RootMixer.GetInput(_mainInputIndex);
            if (!inputMixer.IsValid())
            {
                return;
            }

            for (int i = 0; i < inputMixer.GetInputCount(); i++)
            {
                var input = inputMixer.GetInput(i);
                if (input.IsValid())
                {
                    times.Add(input.GetTime());
                }
                else
                {
                    times.Add(0);
                }
            }
        }

        public void SetTime(double time)
        {
            var input = RootMixer.GetInput(_mainInputIndex);
            if (input.IsValid())
            {
                input.SetTime(time);
            }
        }

        public void SetMixTimes(IReadOnlyList<float> times)
        {
            var inputMixer = RootMixer.GetInput(_mainInputIndex);
            if (!inputMixer.IsValid())
            {
                return;
            }

            var inputCount = inputMixer.GetInputCount();
            var timeCount = times.Count;
            if (inputCount != timeCount)
            {
                Debug.LogWarning($"The number of time({timeCount}) doesn't match " +
                                 $"the number of main playable's input({inputCount}).");
            }

            for (int i = 0; i < inputCount && i < timeCount; i++)
            {
                var input = inputMixer.GetInput(i);
                if (input.IsValid())
                {
                    input.SetTime(times[i]);
                }
            }
        }


        public void PlayClip(string tag, AnimationClip clip, float clipSpeed = 1, float clipOffsetTime = 0,
            bool isFixedTime = false)
        {
            var playable = AnimationClipPlayable.Create(_graph, clip);
            var fixedOffsetTime = GetFixedTime(clip, clipOffsetTime, isFixedTime);
            PlayPlayable(tag, playable, clipSpeed, fixedOffsetTime);
        }

        public void PlayMixClips(string tag, IReadOnlyList<AnimationClipInfo> clipInfos, float mixerSpeed = 1,
            float fixedMixerOffsetTime = 0, bool isFixedClipTime = false)
        {
            var clipCount = clipInfos.Count;
            var mixer = AnimationMixerPlayable.Create(_graph, clipCount);
            var totalWeight = 0f;
            for (int i = 0; i < clipCount; i++)
            {
                var clipInfo = clipInfos[i];
                var playable = AnimationClipPlayable.Create(_graph, clipInfo.Clip);
                playable.SetSpeed(clipInfo.Speed);
                var fixedClipOffsetTime = GetFixedTime(clipInfo.Clip, clipInfo.Time, isFixedClipTime);
                playable.SetTime(fixedClipOffsetTime);

                mixer.ConnectInput(i, playable, 0, clipInfo.Weight);

                totalWeight += clipInfo.Weight;
            }

            PlayPlayable(tag, mixer, mixerSpeed, fixedMixerOffsetTime);

            if (!Mathf.Approximately(1, totalWeight))
            {
                Debug.LogWarning("The sum of weight of animation clips is not equals to 1.");
            }
        }

        [Obsolete]
        internal void PlayBlendSpace(string tag, IBlendSpace blendSpace, float speed = 1, float fixedOffsetTime = 0)
        {
            PlayPlayable(tag, blendSpace.Mixer, speed, fixedOffsetTime);
        }

        public void PlayPlayable(string tag, Playable playable, float speed = 1, float fixedOffsetTime = 0)
        {
            ClearInputs();

            if (playable.IsValid())
            {
                playable.SetSpeed(speed);
                playable.SetTime(fixedOffsetTime);
            }

            RootMixer.ConnectInput(_mainInputIndex, playable, 0, 1);
            MainAnimationTag = tag;

            OnClipEnter?.Invoke(Name, Index, MainAnimationTag);
        }


        public void CrossFadeClip(string tag, AnimationClip clip, float speed = 1, float fixedFadeTime = 0.25f,
            float fixedOffsetTime = 0, bool frozeSource = false)
        {
            var playable = AnimationClipPlayable.Create(_graph, clip);
            CrossFadePlayable(tag, playable, speed, fixedFadeTime, fixedOffsetTime, frozeSource);
        }

        public void CrossFadeMixClips(string tag, IReadOnlyList<AnimationClipInfo> clipInfos, float mixerSpeed = 1,
            float fixedFadeTime = 0.25f, float fixedMixerOffsetTime = 0, bool frozeSource = false,
            bool isFixedClipTime = false)
        {
            var clipCount = clipInfos.Count;
            var mixer = AnimationMixerPlayable.Create(_graph, clipCount);
            var totalWeight = 0f;
            for (int i = 0; i < clipCount; i++)
            {
                var clipInfo = clipInfos[i];
                var animClipPlayable = AnimationClipPlayable.Create(_graph, clipInfo.Clip);
                animClipPlayable.SetSpeed(clipInfo.Speed);
                var fixedTime = GetFixedTime(clipInfo.Clip, clipInfo.Time, isFixedClipTime);
                animClipPlayable.SetTime(fixedTime);

                mixer.ConnectInput(i, animClipPlayable, 0, clipInfo.Weight);

                totalWeight += clipInfo.Weight;
            }

            CrossFadePlayable(tag, mixer, mixerSpeed, fixedFadeTime, fixedMixerOffsetTime, frozeSource);

            if (!Mathf.Approximately(1, totalWeight))
            {
                Debug.LogWarning("The sum of weight of animation clips is not equals to 1.");
            }
        }

        [Obsolete]
        internal void CrossFadeBlendSpace(string tag, IBlendSpace blendSpace, float speed = 1,
            float fixedFadeTime = 0.25f, float fixedOffsetTime = 0, bool frozeSource = false)
        {
            CrossFadePlayable(tag, blendSpace.Mixer, speed, fixedFadeTime, fixedOffsetTime, frozeSource);
        }

        public void CrossFadePlayable(string tag, Playable playable, float speed = 1, float fixedFadeTime = 0.25f,
            float fixedOffsetTime = 0, bool frozeSource = false)
        {
            if (playable.IsValid())
            {
                playable.SetSpeed(speed);
                playable.SetTime(fixedOffsetTime);
            }

            var validInputCount = GetValidInputCount();
            if (validInputCount == 0)
            {
                RootMixer.ConnectInput(0, playable, 0, 1);
                RootMixer.SetInputWeight(1, 0);
                _mainInputIndex = 0;

                OnTransitionStart?.Invoke(Name, Index, MainAnimationTag);
                OnTransitionComplete?.Invoke(Name, Index, MainAnimationTag);
                return;
            }

            Playable sourcePlayable;
            if (validInputCount == 2) // In transition
            {
                sourcePlayable = AnimationMixerPlayable.Create(_graph, 2);

                var input0 = RootMixer.GetInput(0);
                var weight0 = RootMixer.GetInputWeight(0);
                RootMixer.DisconnectInput(0);
                sourcePlayable.ConnectInput(0, input0, 0, weight0);

                var input1 = RootMixer.GetInput(1);
                var weight1 = RootMixer.GetInputWeight(1);
                RootMixer.DisconnectInput(1);
                sourcePlayable.ConnectInput(1, input1, 0, weight1);
            }
            else
            {
                sourcePlayable = RootMixer.GetInput(_mainInputIndex);
                RootMixer.DisconnectInput(_mainInputIndex);
            }

            if (frozeSource)
            {
                sourcePlayable.SetSpeed(0);
            }

            ClearInputs();

            RootMixer.ConnectInput(_mainInputIndex, playable, 0, 0);
            RootMixer.ConnectInput(1 - _mainInputIndex, sourcePlayable, 0, 1);
            MainAnimationTag = tag;

            _fixedCrossFadeTime = fixedFadeTime;

            OnTransitionStart?.Invoke(Name, Index, MainAnimationTag);
        }

        private void UpdateCrossFade(float deltaTime)
        {
            if (IsInTransition())
            {
                _fixedCorssFadeTimer += deltaTime;
                var alpha = Mathf.Clamp01(_fixedCorssFadeTimer / _fixedCrossFadeTime);
                RootMixer.SetInputWeight(_mainInputIndex, alpha);
                RootMixer.SetInputWeight(1 - _mainInputIndex, 1 - alpha);

                if (!IsInTransition())
                {
                    var crossFadeSourcePlayable = RootMixer.GetInput(1 - _mainInputIndex);
                    RootMixer.DisconnectInput(1 - _mainInputIndex);
                    crossFadeSourcePlayable.Destroy();

                    OnTransitionComplete?.Invoke(Name, Index, MainAnimationTag);
                }
            }
        }


        private byte GetValidInputCount()
        {
            byte validInputCount = 0;

            var input0 = RootMixer.GetInput(0);
            if (input0.IsValid())
            {
                validInputCount++;
            }

            var input1 = RootMixer.GetInput(1);
            if (input1.IsValid())
            {
                validInputCount++;
            }

            return validInputCount;
        }

        private void ClearInputs()
        {
            var isInTransition = IsInTransition();
            var hasValidInput = false;

            var input0 = RootMixer.GetInput(0);
            if (input0.IsValid())
            {
                hasValidInput = true;
                RootMixer.DisconnectInput(0);
                input0.Destroy();
            }

            var input1 = RootMixer.GetInput(1);
            if (input1.IsValid())
            {
                hasValidInput = true;
                RootMixer.DisconnectInput(1);
                input1.Destroy();
            }

            RootMixer.SetInputWeight(0, 0);
            RootMixer.SetInputWeight(1, 0);

            MainAnimationTag = string.Empty;

            _mainInputIndex = 0;
            _fixedCrossFadeTime = 0;
            _fixedCorssFadeTimer = 0;

            if (isInTransition)
            {
                OnTransitionInterrupted?.Invoke(Name, Index, MainAnimationTag);
            }
            else if (hasValidInput)
            {
                OnClipExit?.Invoke(Name, Index, MainAnimationTag);
            }
        }

        private static float GetFixedTime(AnimationClip clip, float time, bool isFixedTime)
        {
            if (isFixedTime)
            {
                return time;
            }

            if (!clip)
            {
                return 0;
            }

            return clip.length * time;
        }

        #endregion
    }
}