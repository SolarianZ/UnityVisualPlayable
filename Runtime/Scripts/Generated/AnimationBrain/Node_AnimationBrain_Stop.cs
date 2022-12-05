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
	// GBG.VisualPlayable.AnimationBrain.Stop
[UnitCategory("Visual Playable/Animation Brain")]
[UnitTitle("Stop")]
internal class Node_AnimationBrain_Stop : Unit
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

protected override void Definition()
{
	// InputTrigger
InputTrigger = ControlInput(nameof(InputTrigger), OnTriggered);
	// OutputTrigger
OutputTrigger = ControlOutput(nameof(OutputTrigger));

	// Input parameters
Input_AnimationBrain = ValueInput<AnimationBrain>(nameof(Input_AnimationBrain));


	// Requirements

	// Successions
Succession(InputTrigger, OutputTrigger);

}

private ControlOutput OnTriggered(Flow flow)
{
try{
	// Input parameter values
var Value_Input_AnimationBrain = flow.GetValue<AnimationBrain>(Input_AnimationBrain);

Value_Input_AnimationBrain.Stop();
} catch(Exception e) {
Debug.LogException(e); }

return OutputTrigger;
}

}
}

