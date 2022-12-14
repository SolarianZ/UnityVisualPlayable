using GBG.VisualPlayable.Attribute;
using System;
using UnityEngine.Playables;

namespace GBG.VisualPlayable
{
    [ExcludeFromNodeGenerate]
    public static class VisualPlayableExtensions
    {
        public static bool IsNull(Playable playable)
        {
            return playable.IsNull();
        }

        public static bool IsValid(Playable playable)
        {
            return playable.IsValid();
        }

        public static void Destroy(Playable playable)
        {
            playable.Destroy();
        }

        public static PlayableGraph GetGraph(Playable playable)
        {
            return playable.GetGraph();
        }

        [Obsolete("SetPlayState() has been deprecated. Use Play(), Pause() or SetDelay() instead", false)]
        public static void SetPlayState(Playable playable, PlayState value)
        {
            if (value == PlayState.Delayed)
                throw new ArgumentException("Can't set Delayed: use SetDelay() instead");

            switch (value)
            {
                case PlayState.Playing:
                    playable.Play();
                    break;

                case PlayState.Paused:
                    playable.Pause();
                    break;
            }
        }

        public static PlayState GetPlayState(Playable playable)
        {
            return playable.GetPlayState();
        }

        public static void Play(Playable playable)
        {
            playable.Play();
        }

        public static void Pause(Playable playable)
        {
            playable.Pause();
        }

        public static void SetSpeed(Playable playable, double value)
        {
            playable.SetSpeed(value);
        }

        public static double GetSpeed(Playable playable)
        {
            return playable.GetSpeed();
        }

        public static void SetDuration(Playable playable, double value)
        {
            playable.SetDuration(value);
        }

        public static double GetDuration(Playable playable)
        {
            return playable.GetDuration();
        }

        public static void SetTime(Playable playable, double value)
        {
            playable.SetTime(value);
        }

        public static double GetTime(Playable playable)
        {
            return playable.GetTime();
        }

        public static double GetPreviousTime(Playable playable)
        {
            return playable.GetPreviousTime();
        }

        public static void SetDone(Playable playable, bool value)
        {
            playable.SetDone(value);
        }

        public static bool IsDone(Playable playable)
        {
            return playable.IsDone();
        }

        public static void SetPropagateSetTime(Playable playable, bool value)
        {
            playable.SetPropagateSetTime(value);
        }

        public static bool GetPropagateSetTime(Playable playable)
        {
            return playable.GetPropagateSetTime();
        }

        public static bool CanChangeInputs(Playable playable)
        {
            return playable.CanChangeInputs();
        }

        public static bool CanSetWeights(Playable playable)
        {
            return playable.CanSetWeights();
        }

        public static bool CanDestroy(Playable playable)
        {
            return playable.CanDestroy();
        }

        public static void SetInputCount(Playable playable, int value)
        {
            playable.SetInputCount(value);
        }

        public static int GetInputCount(Playable playable)
        {
            return playable.GetInputCount();
        }

        public static void SetOutputCount(Playable playable, int value)
        {
            playable.SetOutputCount(value);
        }

        public static int GetOutputCount(Playable playable)
        {
            return playable.GetOutputCount();
        }

        public static Playable GetInput(Playable playable, int inputPort)
        {
            return playable.GetInput(inputPort);
        }

        public static Playable GetOutput(Playable playable, int outputPort)
        {
            return playable.GetOutput(outputPort);
        }

        public static void SetInputWeight(Playable playable, int inputIndex, float weight)
        {
            playable.SetInputWeight(inputIndex, weight);
        }

        public static void SetInputWeight(Playable playable, Playable input, float weight)
        {
            playable.SetInputWeight(input, weight);
        }

        public static float GetInputWeight(Playable playable, int inputIndex)
        {
            return playable.GetInputWeight(inputIndex);
        }

        public static void ConnectInput(Playable playable, int inputIndex, Playable sourcePlayable,
            int sourceOutputIndex)
        {
            ConnectInput(playable, inputIndex, sourcePlayable, sourceOutputIndex, 0.0f);
        }

        public static void ConnectInput(Playable playable, int inputIndex, Playable sourcePlayable,
            int sourceOutputIndex, float weight)
        {
            playable.ConnectInput(inputIndex, sourcePlayable, sourceOutputIndex, weight);
        }

        public static void DisconnectInput(Playable playable, int inputPort)
        {
            playable.GetGraph().Disconnect(playable, inputPort);
        }

        public static int AddInput(Playable playable, Playable sourcePlayable, int sourceOutputIndex,
            float weight = 0.0f)
        {
            var inputIndex = playable.GetInputCount();
            playable.SetInputCount(inputIndex + 1);
            playable.ConnectInput(inputIndex, sourcePlayable, sourceOutputIndex, weight);
            return inputIndex;
        }

        [Obsolete("SetDelay is obsolete; use a custom ScriptPlayable to implement this feature", false)]
        public static void SetDelay(Playable playable, double delay)
        {
            playable.SetDelay(delay);
        }

        [Obsolete("GetDelay is obsolete; use a custom ScriptPlayable to implement this feature", false)]
        public static double GetDelay(Playable playable)
        {
            return playable.GetDelay();
        }

        [Obsolete("IsDelayed is obsolete; use a custom ScriptPlayable to implement this feature", false)]
        public static bool IsDelayed(Playable playable)
        {
            return playable.IsDelayed();
        }

        public static void SetLeadTime(Playable playable, float value)
        {
            playable.SetLeadTime(value);
        }

        public static float GetLeadTime(Playable playable)
        {
            return playable.GetLeadTime();
        }

        public static PlayableTraversalMode GetTraversalMode(Playable playable)
        {
            return playable.GetTraversalMode();
        }

        public static void SetTraversalMode(Playable playable, PlayableTraversalMode mode)
        {
            playable.SetTraversalMode(mode);
        }
    }
}