Survival Animset Pro vol.2
--------------------------

This pack contains 60 survival and crafting themed animations for Third Person Perspective (TPP) characters.

The animations are ideal for open world and sandbox survival games, where you camp, have to drink and eat, build shelter, craft weapons, cure wounds etc. They are also very helpful for NPC characters populating rural and fantasy worlds.

This pack does not contain a character controller.


The animations are baked on Autodesk HumanIK structured skeleton (native for Maya and Motionbuilder) and are in FBX format. They can be imported in to any 3D animation software and any 3D game engine, including Unity, Unreal and CryEngine. The skeleton is the same in all Kubold packs, except for the extra prop and IK bones, which are specific to Survival Animset Pro.

--------------------------

The animations can be played out-of-the-box on any character, that uses Unity's Humanoid Rig. However in this pack, the props (spoon, wrench, hammer etc.) are also animated. If you want to use those animations alongside the body animations, Your character must have the same bone structure as the original animation character. Please preceed with these steps:

1. Import your character model to a 3d program (for example Maya, 3ds max, Blender etc.)
2. Add a Root bone to your character, if it does not have one (most of them have it)
3. Rename the Root bone to "Root" if it has a different name
4. Add 4 new bones (joints) and parent them to the Root bone, name them


HelperRightProp

HelperLeftProp

IK_RightHand

IK_LeftHand

5. Export the character back to Unity

Now, when your character has the same bone structure as the original animation character, Unity will animate those bones. You parent the prop models to  HelperRightProp and HelperLeftProp bones.

To improve the quality of the retargeting further, please use Hand IK scripts, which are included in this pack.



TUTORIAL VIDEO of all those steps: https://youtu.be/hAZIFZy5_5g


The absolute best results with retargeting can be achieved by skinning your 3d character model to the skeleton included in this pack.

Additional tips:

Remember to turn on IK Pass on your Animation Layers and turn on Foot IK on every State in Animator.

---------------------
Http://www.kubold.com