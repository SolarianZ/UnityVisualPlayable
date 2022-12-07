# UnityVisualPlayable

Playable integration for Unity Visual Scripting.

![Sample](./Documents~/imgs/sample_graph.png)

[中文](./README_CN.md)


## Features

- High Level Nodes for Animation
    - Nodes generated from class `AnimationBrain` : Used for managing `PlayableGraph` and `AnimationLayer` 
    - Nodes generated from class `AnimationLayer` : Used for managing animation playing and cross fading(You can also to use AnimationLayer to play or cross fade a general Playable instance, just make sure it's a animation Playable instance)

![High Level Animation Nodes](./Documents~/imgs/sample_high_level_animation_nodes.jpg)

- Low Level Nodes for all Playable
    - Nodes generated from class `VisualPlayableExtensions` (Specialization for generic type `UnityEngine.Playables.PlayableExtensions`) : Provides basic nodes to control Playable
    - Nodes generated from all other public Playable-related types

![Low Level Playable Nodes](./Documents~/imgs/sample_low_level_playable_nodes.jpg)


## How to use

Use menu item *Tools/Bamboo/Visual Playable/Setup Visual Playable* to register Playable-related types in bulk to Visual Scripting system, then you can use these nodes in Script Graph and State Graph.
