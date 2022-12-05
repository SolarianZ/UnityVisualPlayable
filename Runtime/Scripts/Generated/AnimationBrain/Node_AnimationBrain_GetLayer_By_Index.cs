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
using GBG.VisualPlayable;

namespace GBG.VisualPlayable.VisualNode
{
	// GBG.VisualPlayable.AnimationBrain.GetLayer
[UnitCategory("Visual Playable/Animation Brain")]
[UnitTitle("Get Layer(By Index)")]
internal class Node_AnimationBrain_GetLayer_By_Index : Unit
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

	// Output params
	// Output(return value): AnimationLayer
[PortLabel("Animation Layer")]
[DoNotSerialize]
public ValueOutput Output_AnimationLayer { get; private set; }

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

	// Output parameters
Output_AnimationLayer=ValueOutput<AnimationLayer>(nameof(Output_AnimationLayer));

	// Requirements
Requirement(Input_layerIndex, InputTrigger);

	// Successions
Succession(InputTrigger, OutputTrigger);
Succession(InputTrigger, FailureOutputTrigger);

	// Assignments
Assignment(InputTrigger, Output_AnimationLayer);

}

private ControlOutput OnTriggered(Flow flow)
{
var failed = false;
try{
	// Input parameter values
var Value_Input_AnimationBrain = flow.GetValue<AnimationBrain>(Input_AnimationBrain);
var Value_Input_layerIndex = flow.GetValue<Byte>(Input_layerIndex);

	// Output parameter values
var Value_Output_AnimationLayer = Value_Input_AnimationBrain.GetLayer(Value_Input_layerIndex);
failed = Value_Output_AnimationLayer == null;
flow.SetValue(Output_AnimationLayer, Value_Output_AnimationLayer);
} catch(Exception e) {
Debug.LogException(e); }

if(failed) { return FailureOutputTrigger; }
return OutputTrigger;
}

}
}

