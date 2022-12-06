// AUTO GENERATED CODE
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantUsingDirective

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using Unity.VisualScripting;

namespace GBG.VisualPlayable.VisualNode
{
    // GBG.VisualPlayable.AnimationBrain.AddLayer
    [UnitCategory("Visual Playable/Animation Brain")]
    [UnitTitle("Add Layer")]
    internal class Node_AnimationBrain_AddLayer : Unit
    {
        // Input trigger
        [PortLabelHidden]
        [DoNotSerialize]
        public ControlInput InputTrigger { get; private set; }

        // Output trigger
        [PortLabel("Success")]
        [DoNotSerialize]
        public ControlOutput OutputTrigger { get; private set; }

        [PortLabel("Failure")]
        [DoNotSerialize]
        public ControlOutput FailureOutputTrigger { get; private set; }

        // Input params
        // Input instance
        [PortLabel("Animation Brain")]
        [DoNotSerialize]
        public ValueInput Input_AnimationBrain { get; private set; }

        // Input parameter: layerName
        [PortLabel("Layer Name")]
        [DoNotSerialize]
        public ValueInput Input_layerName { get; private set; }

        // Input parameter: weight
        [PortLabel("Weight")]
        [DoNotSerialize]
        public ValueInput Input_weight { get; private set; }

        // Input parameter: isAdditive
        [PortLabel("Is Additive")]
        [DoNotSerialize]
        public ValueInput Input_isAdditive { get; private set; }

        // Input parameter: mask
        [PortLabel("Mask")]
        [DoNotSerialize]
        public ValueInput Input_mask { get; private set; }

        // Output params
        // Output(return value): Byte
        [PortLabel("Layer Index")]
        [DoNotSerialize]
        public ValueOutput Output_Byte { get; private set; }

        #region Manual Codes

        [PortLabel("Layer")]
        [DoNotSerialize]
        public ValueOutput OutputLayer { get; private set; }

        #endregion

        protected override void Definition()
        {
            // InputTrigger
            InputTrigger = ControlInput(nameof(InputTrigger), OnTriggered);
            // OutputTrigger
            OutputTrigger = ControlOutput(nameof(OutputTrigger));
            FailureOutputTrigger = ControlOutput(nameof(FailureOutputTrigger));

            // Input parameters
            Input_AnimationBrain = ValueInput<AnimationBrain>(nameof(Input_AnimationBrain));
            Input_layerName = ValueInput<String>(nameof(Input_layerName));
            Input_weight = ValueInput<Single>(nameof(Input_weight), 0f);
            Input_isAdditive = ValueInput<Boolean>(nameof(Input_isAdditive), false);
            Input_mask = ValueInput<AvatarMask>(nameof(Input_mask), null);

            // Output parameters
            Output_Byte = ValueOutput<Byte>(nameof(Output_Byte));

            #region Manual Codes

            OutputLayer = ValueOutput<AnimationLayer>(nameof(OutputLayer));

            #endregion

            // Requirements
            Requirement(Input_layerName, InputTrigger);

            // Successions
            Succession(InputTrigger, OutputTrigger);
            Succession(InputTrigger, FailureOutputTrigger);

            // Assignments
            Assignment(InputTrigger, Output_Byte);
        }

        private ControlOutput OnTriggered(Flow flow)
        {
            var failed = false;
            try
            {
                // Input parameter values
                var Value_Input_AnimationBrain = flow.GetValue<AnimationBrain>(Input_AnimationBrain);
                var Value_Input_layerName = flow.GetValue<String>(Input_layerName);
                var Value_Input_weight = flow.GetValue<Single>(Input_weight);
                var Value_Input_isAdditive = flow.GetValue<Boolean>(Input_isAdditive);
                var Value_Input_mask = flow.GetValue<AvatarMask>(Input_mask);

                // Output parameter values
                var Value_Output_Byte = Value_Input_AnimationBrain.AddLayer(Value_Input_layerName, Value_Input_weight,
                    Value_Input_isAdditive, Value_Input_mask);
                flow.SetValue(Output_Byte, Value_Output_Byte);

                #region Manual Codes

                var valueLayer = Value_Input_AnimationBrain.GetLayer(Value_Output_Byte);
                flow.SetValue(OutputLayer, valueLayer);

                #endregion
            }
            catch (Exception e)
            {
                failed = true;
                Debug.LogException(e);
            }

            if (failed) { return FailureOutputTrigger; }

            return OutputTrigger;
        }
    }
}