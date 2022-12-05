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
	// GBG.VisualPlayable.AnimationLayer.GetSpeed
[UnitCategory("Visual Playable/Animation Layer")]
[UnitTitle("Get Speed")]
internal class Node_AnimationLayer_GetSpeed : Unit
{
	// Input trigger
[PortLabelHidden][DoNotSerialize]
public ControlInput InputTrigger { get; private set; }

	// Output trigger
[PortLabelHidden][DoNotSerialize]
public ControlOutput OutputTrigger { get; private set; }

	// Input params
	// Input instance
[PortLabel("Animation Layer")]
[DoNotSerialize]
public ValueInput Input_AnimationLayer { get; private set; }

	// Output params
	// Output(return value): Double
[PortLabel("Speed")]
[DoNotSerialize]
public ValueOutput Output_Double { get; private set; }

protected override void Definition()
{
	// InputTrigger
InputTrigger = ControlInput(nameof(InputTrigger), OnTriggered);
	// OutputTrigger
OutputTrigger = ControlOutput(nameof(OutputTrigger));

	// Input parameters
Input_AnimationLayer = ValueInput<AnimationLayer>(nameof(Input_AnimationLayer));

	// Output parameters
Output_Double=ValueOutput<Double>(nameof(Output_Double));

	// Requirements

	// Successions
Succession(InputTrigger, OutputTrigger);

	// Assignments
Assignment(InputTrigger, Output_Double);

}

private ControlOutput OnTriggered(Flow flow)
{
try{
	// Input parameter values
var Value_Input_AnimationLayer = flow.GetValue<AnimationLayer>(Input_AnimationLayer);

	// Output parameter values
var Value_Output_Double = Value_Input_AnimationLayer.GetSpeed();
flow.SetValue(Output_Double, Value_Output_Double);
} catch(Exception e) {
Debug.LogException(e); }

return OutputTrigger;
}

}
}

