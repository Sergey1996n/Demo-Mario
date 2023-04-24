using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class Lesson4
{
    [Test]
    public void __ExistingDirectoriesAndFiles()
    {
        var pathDirectory = Path.Combine("Assets", "Animations");
        var exists = Directory.Exists(pathDirectory);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" directory", new object[] { "Assets", "Animations" });

        var pathFile = Path.Combine("Assets", "Animations", "Walk.anim");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" animation", new object[] { "Animations", "Walk" });

        pathFile = Path.Combine("Assets", "Animations", "Player.controller");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" contoller", new object[] { "Animations", "Player" });

        pathFile = Path.Combine("Assets", "Animations", "Idle.anim");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" animation", new object[] { "Animations", "Idle" });
    }

    //[Test]
    //public void ExistsDirectoryAnimations()
    //{
    //    var pathFile = Path.Combine("Assets", "Animations");
    //    var exists = Directory.Exists(pathFile);
    //    Assert.IsTrue(exists,
    //        "Not directory \"Animations\" in \"Assets\"!");
    //}

    [Test]
    public void _1ExistsFileWalkInAnimation()
    {
        var pathFile = Path.Combine("Assets", "Animations", "Walk.anim");
        //var exists = File.Exists(pathFile);
        //Assert.IsTrue(exists,
        //    "Not animation \"Walk\" in directory \"Animations\"!");

        AnimationClip animation = AssetDatabase.LoadAssetAtPath<AnimationClip>(pathFile);
        var propertiesAnimation = AnimationUtility.GetObjectReferenceCurveBindings(animation);
        Assert.AreEqual(1, propertiesAnimation.Length,
            "The count of properties is not equal to the \"Walk\" animation");

        Assert.AreEqual("Sprite", propertiesAnimation[0].propertyName[2..],
            "The name of property is not equal to the \"Walk\" animation");
        

        var curveAnimation = AnimationUtility.GetObjectReferenceCurve(animation, propertiesAnimation[0]);
        Assert.AreEqual(5, curveAnimation.Length,
            "The count of sprites is not equal  to the \"Walk\" animation");

        for (int i = 0; i < curveAnimation.Length; i++)
        {
            Assert.AreEqual(i * 10, Convert.ToInt32(curveAnimation[i].time * 60),
                $"In the \"Walk\" animation, {i + 1} element has the incorrect time");
            switch (i)
            {
                case 0:
                case 2:
                case 4:
                    Assert.AreEqual("Player_3", (curveAnimation[i].value as Sprite).name,
                        $"In the \"Walk\" animation, {i + 1} element has the incorrect value");
                    break;
                case 1:
                    Assert.AreEqual("Player_2", (curveAnimation[i].value as Sprite).name,
                        $"In the \"Walk\" animation, {i + 1} element has the incorrect value");
                    break;
                case 3:
                    Assert.AreEqual("Player_4", (curveAnimation[i].value as Sprite).name,
                        $"In the \"Walk\" animation, {i + 1} element has the incorrect value");
                    break;
                default:
                    break;
            }
        }
    }

    [Test]
    public void ExistsFileIdleInAnimation()
    {
        var pathFile = Path.Combine("Assets", "Animations", "Idle.anim");
        //var exists = File.Exists(pathFile);
        //Assert.IsTrue(exists,
        //    "Not animation \"Idle\" in directory \"Animations\"!");

        AnimationClip animation = AssetDatabase.LoadAssetAtPath<AnimationClip>(pathFile);
        var propertiesAnimation = AnimationUtility.GetCurveBindings(animation);
        Assert.AreEqual(3, propertiesAnimation.Where(p => p.propertyName.Contains("m_LocalScale")).Count(),
            "The count of properties is not equal to the \"Idle\" animation");

        var curveAnimation = AnimationUtility.GetEditorCurve(animation, propertiesAnimation[1]);
        Assert.AreEqual(3, curveAnimation.length,
            "The count of part animation is not equal to the \"Idle\" animation");

        for (int i = 0; i < curveAnimation.keys.Length; i++)
        {
            Assert.AreEqual(i * 15, Convert.ToInt32(curveAnimation.keys[i].time * 60),
                $"In the \"Idle\" animation, {i + 1} element has the incorrect time");
            switch (i)
            {
                case 0:
                case 2:
                    Assert.AreEqual(1, curveAnimation.keys[i].value,
                        $"In the \"Idle\" animation, {i + 1} element has the incorrect value");
                    break;
                case 1:
                    Assert.AreEqual(1.05f, curveAnimation.keys[i].value,
                        $"In the \"Idle\" animation, {i + 1} element has the incorrect value");
                    break;
                default:
                    break;
            }
        }
    }

    [Test]
    public void ExistsFilePlayerInAnimation()
    {
        var pathFile = Path.Combine("Assets", "Animations", "Player.controller");
        //var exists = File.Exists(pathFile);
        //Assert.IsTrue(exists,
        //    "Not animator controller \"Player\" in directory \"Animations\"!");

        AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(pathFile);
        var controllerLayers = controller.layers;

        Assert.AreEqual(1, controllerLayers.Length,
            "The count of layers is not equal to the \"Player\" animator controller");

        var states = controller.layers[0].stateMachine.states;
        //states.Where(s => s.state.name.Contains("Walk"))
        Assert.IsTrue(states.Where(s => s.state.name == "Walk").Count() == 1,
            "The \"Player\" animator controller not have animation \"Walk\"");

        Assert.IsTrue(states.Where(s => s.state.name == "Idle").Count() == 1,
            "The \"Player\" animator controller not have animation \"Idle\"");


        var stateWalk = states.Where(s => s.state.name == "Walk").First();
        var stateIdle = states.Where(s => s.state.name == "Idle").First();

        Assert.IsTrue(stateWalk.state.transitions.Length > 0,
            "There is no transition from the \"Walk\" animation");

        Assert.IsTrue(stateWalk.state.transitions.Where(t => t.destinationState.name == "Idle").Count() == 1,
            "There is no transition from the \"Walk\" to \"Idle\" animation");

        var stateWalkTransitionIdle = stateWalk.state.transitions.Where(t => t.destinationState.name == "Idle").First();

        Assert.IsFalse(stateWalkTransitionIdle.hasExitTime,
            "The \"Walk -> Idle\" transition has a check mark in the \"Has Exit Time\" field");

        Assert.IsTrue(stateIdle.state.transitions.Length > 0,
            "There is no transition from the \"Idle\" animation");

        Assert.IsTrue(stateIdle.state.transitions.Where(t => t.destinationState.name == "Walk").Count() == 1,
            "There is no transition from the \"Idle\" to \"Walk\" animation");

        var stateIdleTransitionWalk = stateIdle.state.transitions.Where(t => t.destinationState.name == "Walk").First();
        
        Assert.IsFalse(stateIdleTransitionWalk.hasExitTime,
            "The \"Idle -> Walk\" transition has a check mark in the \"Has Exit Time\" field");

        Assert.IsTrue(controller.parameters.Length > 0,
            "The \"Player\" animator control has no parameters");

        Assert.IsTrue(controller.parameters.Where(p => p.name == "State").Count() == 1,
            "The \"Player\" animator control has no \"State\" parameter");

        var controllerState = controller.parameters.Where(p => p.name == "State").First();
        Assert.AreEqual(controllerState.type, AnimatorControllerParameterType.Int,
            "The \"State\" parameter has an incorrect type.");

        Assert.IsTrue(stateWalkTransitionIdle.conditions.Length > 0,
            "The \"Walk -> Idle\" transition has no parameters");

        Assert.IsTrue(stateWalkTransitionIdle.conditions.Where(c => c.parameter == "State").Count() == 1,
            "The \"Walk -> Idle\" transition is missing the \"State\" parameter");

        var stateWalkTransitionIdleConditionState = stateWalkTransitionIdle.conditions.Where(c => c.parameter == "State").First();

        Assert.AreEqual(stateWalkTransitionIdleConditionState.mode, AnimatorConditionMode.Equals,
            "The \"Walk -> Idle\" transition in the \"State\" parameter has the incorrect compare type");

        Assert.AreEqual(stateWalkTransitionIdleConditionState.threshold, 0,
            "The \"Walk -> Idle\" transition has an incorrect value in the \"State\" parameter");

        Assert.IsTrue(stateIdleTransitionWalk.conditions.Length > 0,
            "The \"Idle -> Walk\" transition has no parameters");

        Assert.IsTrue(stateIdleTransitionWalk.conditions.Where(c => c.parameter == "State").Count() == 1,
            "The \"Idle -> Walk\" transition is missing the \"State\" parameter");

        var stateIdleTransitionWalkConditionState = stateIdleTransitionWalk.conditions.Where(c => c.parameter == "State").First();
        
        Assert.AreEqual(stateIdleTransitionWalkConditionState.mode, AnimatorConditionMode.Equals,
            "The \"Idle -> Walk\" transition in the \"State\" parameter has the incorrect compare type");

        Assert.AreEqual(stateIdleTransitionWalkConditionState.threshold, 1,
            "The \"Idle -> Walk\" transition has an incorrect value in the \"State\" parameter");
    }

    [Test]
    public void CheckingComponentAnimatorInObjectPlayer()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var animator = player.GetComponent<Animator>();

        if (animator == null)
        {
            Assert.AreEqual(player.AddComponent<Animator>(), animator,
                "The \"Player\" object does not have a Animator component");
        }

        var path = Path.Combine("Assets", "Animations", "Player.controller");
        AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
        Assert.AreEqual(controller, animator.runtimeAnimatorController,
            "The \"Player\" object does not have a controller in the \"Animator\" component");
    }

    [Test]
    public void CheckingScriptPlayer()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var p = player.GetComponent<Player>();
        if (p == null)
        {
            Assert.AreEqual(player.AddComponent<Player>(), p,
                "The \"Player\" object does not have a Player script");
        }

        TestAssistant.TestingFields(typeof(Player), "spriteRenderer", "SpriteRenderer", FieldAttributes.Private);
        TestAssistant.TestingFields(typeof(Player), "animator", "Animator", FieldAttributes.Private);
    }

    [Test]
    public void CheckingTask()
    {
        /***************************Animation****************************/
        var pathFile = Path.Combine("Assets", "Animations", "Jump.anim");
        var exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "Not animation \"Jump\" in directory \"Animations\"!");

        AnimationClip animation = AssetDatabase.LoadAssetAtPath<AnimationClip>(pathFile);
        var propertiesAnimation = AnimationUtility.GetObjectReferenceCurveBindings(animation);
        Assert.AreEqual(1, propertiesAnimation.Length,
            "The count of properties is not equal to the \"Jump\" animation");

        Assert.AreEqual("Sprite", propertiesAnimation[0].propertyName[2..],
            "The name of property is not equal to the \"Jump\" animation");

        var curveAnimation = AnimationUtility.GetObjectReferenceCurve(animation, propertiesAnimation[0]);
        Assert.AreEqual(1, curveAnimation.Length,
            "The count of sprites is not equal  to the \"Jump\" animation");

        Assert.AreEqual(0, Convert.ToInt32(curveAnimation[0].time * 60),
            $"In the \"Jump\" animation, {1} element has the incorrect time");

        Assert.AreEqual("Player_0", (curveAnimation[0].value as Sprite).name,
            $"In the \"Jump\" animation, {1} element has the incorrect value");

        /***************************Controller****************************/

        pathFile = Path.Combine("Assets", "Animations", "Player.controller");

        AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(pathFile);
        var controllerLayers = controller.layers;

        Assert.AreEqual(1, controllerLayers.Length,
            "The count of layers is not equal to the \"Player\" animator controller");

        var states = controller.layers[0].stateMachine.states;

        Assert.AreEqual(3, states.Length,
            "The \"Player\" animator controller has the incorrect count of animations");

        Assert.IsTrue(states.Where(s => s.state.name == "Jump").Count() == 1,
            "The \"Player\" animator controller not have animation \"Jump\"");

        var stateWalk = states.Where(s => s.state.name == "Walk").First();
        var stateIdle = states.Where(s => s.state.name == "Idle").First();
        var stateJump = states.Where(s => s.state.name == "Jump").First();

        /***************************StateWalk****************************/

        Assert.AreEqual(2, stateWalk.state.transitions.Length,
            "The \"Walk\" animation has the incorrect count of transitions");

        Assert.IsTrue(stateWalk.state.transitions.Where(t => t.destinationState.name == "Jump").Count() == 1,
            "There is no transition from the \"Walk\" to \"Jump\" animation");

        var stateWalkTransitionJump = stateWalk.state.transitions.Where(t => t.destinationState.name == "Jump").First();

        Assert.IsFalse(stateWalkTransitionJump.hasExitTime,
            "The \"Walk -> Jump\" transition has a check mark in the \"Has Exit Time\" field");

        Assert.AreEqual(1, stateWalkTransitionJump.conditions.Length,
            "The \"Walk -> Jump\" transition has the incorrect count of parameters");

        var stateWalkTransitionJumpConditionState = stateWalkTransitionJump.conditions[0];

        Assert.AreEqual("State", stateWalkTransitionJumpConditionState.parameter,
            "The \"Walk -> Jump\" transition is missing the \"State\" parameter");


        Assert.AreEqual(stateWalkTransitionJumpConditionState.mode, AnimatorConditionMode.Equals,
            "The \"Walk -> Jump\" transition in the \"State\" parameter has the incorrect compare type");

        Assert.AreEqual(stateWalkTransitionJumpConditionState.threshold, 2,
            "The \"Walk -> Jump\" transition has an incorrect value in the \"State\" parameter");


        /***************************StateIdle****************************/

        Assert.AreEqual(2, stateIdle.state.transitions.Length,
            "The \"Idle\" animation has the incorrect count of transitions");

        Assert.IsTrue(stateIdle.state.transitions.Where(t => t.destinationState.name == "Jump").Count() == 1,
            "There is no transition from the \"Idle\" to \"Jump\" animation");

        var stateIdleTransitionJump = stateIdle.state.transitions.Where(t => t.destinationState.name == "Jump").First();

        Assert.IsFalse(stateIdleTransitionJump.hasExitTime,
            "The \"Idle -> Jump\" transition has a check mark in the \"Has Exit Time\" field");

        Assert.AreEqual(1, stateIdleTransitionJump.conditions.Length,
            "The \"Idle -> Jump\" transition has the incorrect count of parameters");

        var stateIdleTransitionJumpConditionState = stateWalkTransitionJump.conditions[0];

        Assert.AreEqual("State", stateIdleTransitionJumpConditionState.parameter,
            "The \"Idle -> Jump\" transition is missing the \"State\" parameter");


        Assert.AreEqual(stateIdleTransitionJumpConditionState.mode, AnimatorConditionMode.Equals,
            "The \"Idle -> Jump\" transition in the \"State\" parameter has the incorrect compare type");

        Assert.AreEqual(stateIdleTransitionJumpConditionState.threshold, 2,
            "The \"Idle -> Jump\" transition has an incorrect value in the \"State\" parameter");

        /***************************StateJump****************************/

        Assert.AreEqual(2, stateJump.state.transitions.Length,
            "The \"Jump\" animation has the incorrect count of transitions");

        Assert.IsTrue(stateJump.state.transitions.Where(t => t.destinationState.name == "Walk").Count() == 1,
            "There is no transition from the \"Jump\" to \"Walk\" animation");

        Assert.IsTrue(stateJump.state.transitions.Where(t => t.destinationState.name == "Idle").Count() == 1,
            "There is no transition from the \"Jump\" to \"Idle\" animation");

        var stateJumpTransitionWalk = stateJump.state.transitions.Where(t => t.destinationState.name == "Walk").First();
        var stateJumpTransitionIdle = stateJump.state.transitions.Where(t => t.destinationState.name == "Idle").First();

        Assert.IsFalse(stateJumpTransitionWalk.hasExitTime,
            "The \"Jump -> Walk\" transition has a check mark in the \"Has Exit Time\" field");

        Assert.IsFalse(stateJumpTransitionIdle.hasExitTime,
            "The \"Jump -> Idle\" transition has a check mark in the \"Has Exit Time\" field");

        Assert.AreEqual(1, stateJumpTransitionWalk.conditions.Length,
            "The \"Jump -> Walk\" transition has the incorrect count of parameters");

        Assert.AreEqual(1, stateJumpTransitionIdle.conditions.Length,
            "The \"Jump -> Idle\" transition has the incorrect count of parameters");

        var stateJumpTransitionWalkConditionState = stateJumpTransitionWalk.conditions[0];
        var stateJumpTransitionIdleConditionState = stateJumpTransitionIdle.conditions[0];

        Assert.AreEqual("State", stateJumpTransitionWalkConditionState.parameter,
            "The \"Jump -> Walk\" transition is missing the \"State\" parameter");

        Assert.AreEqual("State", stateJumpTransitionIdleConditionState.parameter,
            "The \"Jump -> Idle\" transition is missing the \"State\" parameter");

        Assert.AreEqual(stateJumpTransitionWalkConditionState.mode, AnimatorConditionMode.Equals,
            "The \"Jump -> Walk\" transition in the \"State\" parameter has the incorrect compare type");

        Assert.AreEqual(stateJumpTransitionIdleConditionState.mode, AnimatorConditionMode.Equals,
            "The \"Jump -> Idle\" transition in the \"State\" parameter has the incorrect compare type");

        Assert.AreEqual(stateJumpTransitionWalkConditionState.threshold, 1,
            "The \"Jump -> Walk\" transition has an incorrect value in the \"State\" parameter");

        Assert.AreEqual(stateJumpTransitionIdleConditionState.threshold, 0,
            "The \"Jump -> Idle\" transition has an incorrect value in the \"State\" parameter");
    }
}
