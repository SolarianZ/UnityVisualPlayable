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
	// GBG.VisualPlayable.AnimationBrain.SetTimeUpdateMode
[UnitCategory("Visual Playable/Animation Brain")]
[UnitTitle("Set Time Update Mode")]
internal class Node_AnimationBrain_SetTimeUpdateMode : Unit
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
	// Input parameter: updateMode
[PortLabel("Update Mode")]
[DoNotSerialize]
public ValueInput Input_updateMode { get; private set; }

	// Output params

protected override void Definition()
{
	// InputTrigger
InputTrigger = ControlInput(nameof(InputTrigger), OnTriggered);
	// OutputTrigger
OutputTrigger = ControlOutput(nameof(OutputTrigger));

	// Input parameters
Input_AnimationBrain = ValueInput<AnimationBrain>(nameof(Input_AnimationBrain));
Input_updateMode = ValueInput<DirectorUpdateMode>(nameof(Input_updateMode));


	// Requirements
Requirement(Input_updateMode, InputTrigger);

	// Successions
Succession(InputTrigger, OutputTrigger);

}

private ControlOutput OnTriggered(Flow flow)
{
try{
	// Input parameter values
var Value_Input_AnimationBrain = flow.GetValue<AnimationBrain>(Input_AnimationBrain);
var Value_Input_updateMode = flow.GetValue<DirectorUpdateMode>(Input_updateMode);

Value_Input_AnimationBrain.SetTimeUpdateMode(Value_Input_updateMode);
} catch(Exception e) {
Debug.LogException(e); }

return OutputTrigger;
}

}
}

