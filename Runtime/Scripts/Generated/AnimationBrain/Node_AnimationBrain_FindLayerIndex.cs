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
	// GBG.VisualPlayable.AnimationBrain.FindLayerIndex
[UnitCategory("Visual Playable/Animation Brain")]
[UnitTitle("Find Layer Index")]
internal class Node_AnimationBrain_FindLayerIndex : Unit
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
	// Input parameter: layerName
[PortLabel("Layer Name")]
[DoNotSerialize]
public ValueInput Input_layerName { get; private set; }

	// Output params
	// Output(return value): Int32
[PortLabel("Layer Index")]
[DoNotSerialize]
public ValueOutput Output_Int32 { get; private set; }

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

	// Output parameters
Output_Int32=ValueOutput<Int32>(nameof(Output_Int32));

	// Requirements
Requirement(Input_layerName, InputTrigger);

	// Successions
Succession(InputTrigger, OutputTrigger);
Succession(InputTrigger, FailureOutputTrigger);

	// Assignments
Assignment(InputTrigger, Output_Int32);

}

private ControlOutput OnTriggered(Flow flow)
{
var failed = false;
try{
	// Input parameter values
var Value_Input_AnimationBrain = flow.GetValue<AnimationBrain>(Input_AnimationBrain);
var Value_Input_layerName = flow.GetValue<String>(Input_layerName);

	// Output parameter values
var Value_Output_Int32 = Value_Input_AnimationBrain.FindLayerIndex(Value_Input_layerName);
failed = Value_Output_Int32 < 0;
flow.SetValue(Output_Int32, Value_Output_Int32);
} catch(Exception e) {
Debug.LogException(e); }

if(failed) { return FailureOutputTrigger; }
return OutputTrigger;
}

}
}

