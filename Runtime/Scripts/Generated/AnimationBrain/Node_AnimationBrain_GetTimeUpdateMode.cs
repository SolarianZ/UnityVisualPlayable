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
	// GBG.VisualPlayable.AnimationBrain.GetTimeUpdateMode
[UnitCategory("Visual Playable/Animation Brain")]
[UnitTitle("Get Time Update Mode")]
internal class Node_AnimationBrain_GetTimeUpdateMode : Unit
{
	// Input trigger
[PortLabelHidden][DoNotSerialize]
public ControlInput InputTrigger { get; private set; }

	// Output trigger
[PortLabelHidden][DoNotSerialize]
public ControlOutput OutputTrigger { get; private set; }

	// Input params
	// Input instance
[PortLabel("Animation Brain")]
[DoNotSerialize]
public ValueInput Input_AnimationBrain { get; private set; }

	// Output params
	// Output(return value): DirectorUpdateMode
[PortLabel("Director Update Mode")]
[DoNotSerialize]
public ValueOutput Output_DirectorUpdateMode { get; private set; }

protected override void Definition()
{
	// InputTrigger
InputTrigger = ControlInput(nameof(InputTrigger), OnTriggered);
	// OutputTrigger
OutputTrigger = ControlOutput(nameof(OutputTrigger));

	// Input parameters
Input_AnimationBrain = ValueInput<AnimationBrain>(nameof(Input_AnimationBrain));

	// Output parameters
Output_DirectorUpdateMode=ValueOutput<DirectorUpdateMode>(nameof(Output_DirectorUpdateMode));

	// Requirements

	// Successions
Succession(InputTrigger, OutputTrigger);

	// Assignments
Assignment(InputTrigger, Output_DirectorUpdateMode);

}

private ControlOutput OnTriggered(Flow flow)
{
try{
	// Input parameter values
var Value_Input_AnimationBrain = flow.GetValue<AnimationBrain>(Input_AnimationBrain);

	// Output parameter values
var Value_Output_DirectorUpdateMode = Value_Input_AnimationBrain.GetTimeUpdateMode();
flow.SetValue(Output_DirectorUpdateMode, Value_Output_DirectorUpdateMode);
} catch(Exception e) {
Debug.LogException(e); }

return OutputTrigger;
}

}
}

