using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace GBG.VisualPlayable
{
    public class AnimationLayer
    {
        public string Name { get; }

        public byte Index { get; }

        internal AnimationMixerPlayable RootMixer { get; }


        private PlayableGraph _graph;


        public AnimationLayer(PlayableGraph graph, string name, byte index)
        {
            _graph = graph;

            Name = name;
            Index = index;
            RootMixer = AnimationMixerPlayable.Create(_graph, 2);
        }

        public void Update(float deltaTime)
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

        // ClipStartEvent
        // ClipStopEvent
        // TransitionBeginEvent
        // TransitionEndEvent


        private byte _mainInputIndex;

        private float _fixedCrossFadeTime;

        private float _fixedCorssFadeTimer;


        public bool IsInTransition()
        {
            return _fixedCorssFadeTimer < _fixedCrossFadeTime;
        }


        public double GetSpeed()
        {
            var input = RootMixer.GetInput(_mainInputIndex);
            if (input.IsValid())
            {
                return input.GetSpeed();
            }

            return 0;
        }

        public void SetSpeed(double speed)
        {
            var input = RootMixer.GetInput(_mainInputIndex);
            if (input.IsValid())
            {
                input.SetSpeed(speed);
            }
        }

        public void SetSpeeds(IReadOnlyList<float> speeds)
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


        public double GetTime()
        {
            var input = RootMixer.GetInput(_mainInputIndex);
            if (input.IsValid())
            {
                return input.GetTime();
            }

            return 0;
        }

        public void SetTime(double time)
        {
            var input = RootMixer.GetInput(_mainInputIndex);
            if (input.IsValid())
            {
                input.SetTime(time);
            }
        }

        public void SetTimes(IReadOnlyList<float> times)
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


        public void PlayClip(AnimationClip clip, float speed = 1, float time = 0, bool isFixedTime = false)
        {
            var animClipPlayable = AnimationClipPlayable.Create(_graph, clip);
            var fixedTime = GetFixedTime(clip, time, isFixedTime);
            animClipPlayable.SetSpeed(speed);
            animClipPlayable.SetTime(fixedTime);

            PlayPlayable(animClipPlayable);
        }

        public void PlayClips(IReadOnlyList<AnimationClipInfo> clipInfos, bool isFixedTime = false)
        {
            var clipCount = clipInfos.Count;
            var mixer = AnimationMixerPlayable.Create(_graph, clipCount);
            var totalWeight = 0f;
            for (int i = 0; i < clipCount; i++)
            {
                var clipInfo = clipInfos[i];
                var animClipPlayable = AnimationClipPlayable.Create(_graph, clipInfo.Clip);
                animClipPlayable.SetSpeed(clipInfo.Speed);
                var fixedTime = GetFixedTime(clipInfo.Clip, clipInfo.Time, isFixedTime);
                animClipPlayable.SetTime(fixedTime);

                mixer.ConnectInput(i, animClipPlayable, 0, clipInfo.Weight);

                totalWeight += clipInfo.Weight;
            }

            PlayPlayable(mixer);

            if (!Mathf.Approximately(1, totalWeight))
            {
                Debug.LogWarning("The sum of weight of animation clips is not equals to 1.");
            }
        }

        public void PlayPlayable(Playable playable)
        {
            ClearInputs();

            RootMixer.ConnectInput(_mainInputIndex, playable, 0, 1);
        }


        public void CrossFadeClip(AnimationClip clip, float speed = 1, float fixedFadeTime = 0.25f,
            float fixedOffsetTime = 0, bool frozeSource = false)
        {
            var animClipPlayable = AnimationClipPlayable.Create(_graph, clip);
            animClipPlayable.SetSpeed(speed);
            CrossFadePlayable(animClipPlayable, fixedFadeTime, fixedOffsetTime, frozeSource);
        }

        public void CrossFadeClips(IReadOnlyList<AnimationClipInfo> clipInfos, float fixedFadeTime = 0.25f,
            float fixedOffsetTime = 0, bool frozeSource = false, bool isFixedClipInfoTime = false)
        {
            var clipCount = clipInfos.Count;
            var mixer = AnimationMixerPlayable.Create(_graph, clipCount);
            var totalWeight = 0f;
            for (int i = 0; i < clipCount; i++)
            {
                var clipInfo = clipInfos[i];
                var animClipPlayable = AnimationClipPlayable.Create(_graph, clipInfo.Clip);
                animClipPlayable.SetSpeed(clipInfo.Speed);
                var fixedTime = GetFixedTime(clipInfo.Clip, clipInfo.Time, isFixedClipInfoTime);
                animClipPlayable.SetTime(fixedTime);

                mixer.ConnectInput(i, animClipPlayable, 0, clipInfo.Weight);

                totalWeight += clipInfo.Weight;
            }

            CrossFadePlayable(mixer, fixedFadeTime, fixedOffsetTime, frozeSource);

            if (!Mathf.Approximately(1, totalWeight))
            {
                Debug.LogWarning("The sum of weight of animation clips is not equals to 1.");
            }
        }

        public void CrossFadePlayable(Playable playable, float fixedFadeTime = 0.25f, float fixedOffsetTime = 0,
            bool frozeSource = false)
        {
            var validInputCount = GetValidInputCount();
            if (validInputCount == 0)
            {
                RootMixer.ConnectInput(0, playable, 0, 1);
                RootMixer.SetInputWeight(1, 0);
                _mainInputIndex = 0;

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
                RootMixer.DisconnectInput(_mainInputIndex);
                sourcePlayable = RootMixer.GetInput(_mainInputIndex);
            }

            if (frozeSource)
            {
                sourcePlayable.SetSpeed(0);
            }

            ClearInputs();

            RootMixer.ConnectInput(_mainInputIndex, playable, 0, 0);
            RootMixer.ConnectInput(1 - _mainInputIndex, sourcePlayable, 0, 1);

            playable.SetTime(fixedOffsetTime);
            _fixedCrossFadeTime = fixedFadeTime;
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

                    // TODO: Transition end event
                }
            }
        }


        // BlendSpace1D
        // BlendSpace2D


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
            var input0 = RootMixer.GetInput(0);
            if (input0.IsValid())
            {
                RootMixer.DisconnectInput(0);
                input0.Destroy();
            }

            var input1 = RootMixer.GetInput(1);
            if (input1.IsValid())
            {
                RootMixer.DisconnectInput(1);
                input1.Destroy();
            }

            RootMixer.SetInputWeight(0, 0);
            RootMixer.SetInputWeight(1, 0);

            _mainInputIndex = 0;
            _fixedCrossFadeTime = 0;
            _fixedCorssFadeTimer = 0;
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