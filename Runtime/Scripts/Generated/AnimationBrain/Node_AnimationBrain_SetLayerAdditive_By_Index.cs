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
	// GBG.VisualPlayable.AnimationBrain.SetLayerAdditive
[UnitCategory("Visual Playable/Animation Brain")]
[UnitTitle("Set Layer Additive(By Index)")]
internal class Node_AnimationBrain_SetLayerAdditive_By_Index : Unit
{
	// Input trigger
[PortLabelHidden][DoNotSerialize]
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
	// Input parameter: layerIndex
[PortLabel("Layer Index")]
[DoNotSerialize]
public ValueInput Input_layerIndex { get; private set; }

	// Input parameter: isAdditive
[PortLabel("Is Additive")]
[DoNotSerialize]
public ValueInput Input_isAdditive { get; private set; }

	// Output params
	// Output(return value): Boolean
[PortLabel("Success")]
[DoNotSerialize]
public ValueOutput Output_Boolean { get; private set; }

protected override void Definition()
{
	// InputTrigger
InputTrigger = ControlInput(nameof(InputTrigger), OnTriggered);
	// OutputTrigger
OutputTrigger = ControlOutput(nameof(OutputTrigger));
FailureOutputTrigger = ControlOutput(nameof(FailureOutputTrigger));

	// Input parameters
Input_AnimationBrain = ValueInput<AnimationBrain>(nameof(Input_AnimationBrain));
Input_layerIndex = ValueInput<Byte>(nameof(Input_layerIndex));
Input_isAdditive = ValueInput<Boolean>(nameof(Input_isAdditive));

	// Output parameters
Output_Boolean=ValueOutput<Boolean>(nameof(Output_Boolean));

	// Requirements
Requirement(Input_layerIndex, InputTrigger);
Requirement(Input_isAdditive, InputTrigger);

	// Successions
Succession(InputTrigger, OutputTrigger);
Succession(InputTrigger, FailureOutputTrigger);

	// Assignments
Assignment(InputTrigger, Output_Boolean);

}

private ControlOutput OnTriggered(Flow flow)
{
var failed = false;
try{
	// Input parameter values
var Value_Input_AnimationBrain = flow.GetValue<AnimationBrain>(Input_AnimationBrain);
var Value_Input_layerIndex = flow.GetValue<Byte>(Input_layerIndex);
var Value_Input_isAdditive = flow.GetValue<Boolean>(Input_isAdditive);

	// Output parameter values
var Value_Output_Boolean = Value_Input_AnimationBrain.SetLayerAdditive(Value_Input_layerIndex, Value_Input_isAdditive);
failed = !Value_Output_Boolean;
flow.SetValue(Output_Boolean, Value_Output_Boolean);
} catch(Exception e) {
Debug.LogException(e); }

if(failed) { return FailureOutputTrigger; }
return OutputTrigger;
}

}
}

