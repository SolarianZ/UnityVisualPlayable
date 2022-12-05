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
	// GBG.VisualPlayable.AnimationLayer.PlayMixClips
[UnitCategory("Visual Playable/Animation Layer")]
[UnitTitle("Play Mix Clips")]
internal class Node_AnimationLayer_PlayMixClips : Unit
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
	// Input parameter: tag
[PortLabel("Tag")]
[DoNotSerialize]
public ValueInput Input_tag { get; private set; }

	// Input parameter: clipInfos
[PortLabel("Clip Infos")]
[DoNotSerialize]
public ValueInput Input_clipInfos { get; private set; }

	// Input parameter: mixerSpeed
[PortLabel("Mixer Speed")]
[DoNotSerialize]
public ValueInput Input_mixerSpeed { get; private set; }

	// Input parameter: fixedMixerOffsetTime
[PortLabel("Fixed Mixer Offset Time")]
[DoNotSerialize]
public ValueInput Input_fixedMixerOffsetTime { get; private set; }

	// Input parameter: isFixedClipTime
[PortLabel("Is Fixed Clip Time")]
[DoNotSerialize]
public ValueInput Input_isFixedClipTime { get; private set; }

	// Output params

protected override void Definition()
{
	// InputTrigger
InputTrigger = ControlInput(nameof(InputTrigger), OnTriggered);
	// OutputTrigger
OutputTrigger = ControlOutput(nameof(OutputTrigger));

	// Input parameters
Input_AnimationLayer = ValueInput<AnimationLayer>(nameof(Input_AnimationLayer));
Input_tag = ValueInput<String>(nameof(Input_tag));
Input_clipInfos = ValueInput<IReadOnlyList<AnimationClipInfo>>(nameof(Input_clipInfos));
Input_mixerSpeed = ValueInput<Single>(nameof(Input_mixerSpeed), 1f);
Input_fixedMixerOffsetTime = ValueInput<Single>(nameof(Input_fixedMixerOffsetTime), 0f);
Input_isFixedClipTime = ValueInput<Boolean>(nameof(Input_isFixedClipTime), false);


	// Requirements
Requirement(Input_tag, InputTrigger);
Requirement(Input_clipInfos, InputTrigger);

	// Successions
Succession(InputTrigger, OutputTrigger);

}

private ControlOutput OnTriggered(Flow flow)
{
try{
	// Input parameter values
var Value_Input_AnimationLayer = flow.GetValue<AnimationLayer>(Input_AnimationLayer);
var Value_Input_tag = flow.GetValue<String>(Input_tag);
var Value_Input_clipInfos = flow.GetValue<IReadOnlyList<AnimationClipInfo>>(Input_clipInfos);
var Value_Input_mixerSpeed = flow.GetValue<Single>(Input_mixerSpeed);
var Value_Input_fixedMixerOffsetTime = flow.GetValue<Single>(Input_fixedMixerOffsetTime);
var Value_Input_isFixedClipTime = flow.GetValue<Boolean>(Input_isFixedClipTime);

Value_Input_AnimationLayer.PlayMixClips(Value_Input_tag, Value_Input_clipInfos, Value_Input_mixerSpeed, Value_Input_fixedMixerOffsetTime, Value_Input_isFixedClipTime);
} catch(Exception e) {
Debug.LogException(e); }

return OutputTrigger;
}

}
}

