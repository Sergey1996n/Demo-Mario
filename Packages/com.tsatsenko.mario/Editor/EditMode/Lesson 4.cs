using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Lesson4
{
    private string pathScene = Path.Combine("Assets", "Scenes", "Level.unity");

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

    [Test]
    public void __ExistingObjectsOnScene()
    {
        GameObject gameObjectPlayer = GameObject.Find("Player");
        Assert.IsNotNull(gameObjectPlayer,
            "There is no \"{0}\" object on the scene", new object[] { "Player" });

        if (!gameObjectPlayer.TryGetComponent(out Player player))
        {
            Assert.AreEqual(gameObjectPlayer.AddComponent<Player>(), player,
                "The \"{0}\" object does not have a \"{1}\" scpipt", new object[] { gameObjectPlayer.name, "Player" });
        }

        if (!gameObjectPlayer.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            Assert.AreEqual(gameObjectPlayer.AddComponent<SpriteRenderer>(), spriteRenderer,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectPlayer.name, "SpriteRenderer" });
        }
    }

    [Test]
    public void _1CheckingFileWalkInAnimation()
    {
        var pathFile = Path.Combine("Assets", "Animations", "Walk.anim");
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
    public void _2CheckingFileIdleInAnimation()
    {
        var pathFile = Path.Combine("Assets", "Animations", "Idle.anim");
        AnimationClip animation = AssetDatabase.LoadAssetAtPath<AnimationClip>(pathFile);

        var propertiesAnimation = AnimationUtility.GetCurveBindings(animation);
        Assert.AreEqual(4, propertiesAnimation.Length,
            "The \"Idle\" animation has an incorrect number of properties");

        var propertyScale = propertiesAnimation.Where(p => p.propertyName.Contains("m_LocalScale")).ToArray();

        Assert.IsNotNull(propertyScale,
            "The \"Idle\" animation does not have the \"Scale\" property");

        var curveAnimationScale = AnimationUtility.GetEditorCurve(animation, propertyScale[1]);
        Assert.AreEqual(3, curveAnimationScale.length,
            "The \"Idle\" animation in the \"Scale\" property has a different number of parts");

        var propertySize = propertiesAnimation.Where(p => p.propertyName.Contains("m_Size.y")).FirstOrDefault();

        Assert.IsNotNull(propertySize,
            "The \"Idle\" animation does not have the \"Box Collider 2D.Size.y\" property");

        var curveAnimationSize = AnimationUtility.GetEditorCurve(animation, propertySize);
        Assert.AreEqual(3, curveAnimationSize.length,
            "The \"Idle\" animation in the \"Box Collider 2D.Size.y\" property has a different number of parts");


        for (int i = 0; i < curveAnimationScale.keys.Length; i++)
        {
            Assert.AreEqual(i * 15, Convert.ToInt32(curveAnimationScale.keys[i].time * 60),
                $"The \"Idle\" animation in the \"Scale\" property, {i + 1} element has the incorrect time");
            Assert.AreEqual(i * 15, Convert.ToInt32(curveAnimationSize.keys[i].time * 60),
                $"The \"Idle\" animation in the \"Box Collider 2D.Size.y\" property, {i + 1} element has the incorrect time");
            switch (i)
            {
                case 0:
                case 2:
                    Assert.AreEqual(1, curveAnimationScale.keys[i].value,
                        $"The \"Idle\" animation in the \"Scale\" property, {i + 1} element has the incorrect value");
                    Assert.AreEqual(1, curveAnimationSize.keys[i].value,
                        $"The \"Idle\" animation in the \"Box Collider 2D.Size.y\" property, {i + 1} element has the incorrect value");
                    break;
                case 1:
                    Assert.AreEqual(1.05f, curveAnimationScale.keys[i].value,
                        $"The \"Idle\" animation in the \"Scale\" property, {i + 1} element has the incorrect value");
                    Assert.AreEqual(0.95f, curveAnimationSize.keys[i].value,
                        $"The \"Idle\" animation in the \"Box Collider 2D.Size.y\" property, {i + 1} element has the incorrect value");
                    break;
                default:
                    break;
            }
        }
    }

    [Test]
    public void _3CheckingFilePlayerInAnimation()
    {
        var pathFile = Path.Combine("Assets", "Animations", "Player.controller");
        AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(pathFile);

        var controllerLayers = controller.layers;
                Assert.AreEqual(1, controllerLayers.Length,
            "The count of layers is not equal to the \"Player\" animator controller");

        var states = controller.layers[0].stateMachine.states;

        var stateWalk = states.Where(s => s.state.name == "Walk").FirstOrDefault();
        Assert.IsNotNull(stateWalk,
            "The \"Player\" animator controller not have animation \"Walk\"");

        var stateIdle = states.Where(s => s.state.name == "Idle").FirstOrDefault();
        Assert.IsNotNull(stateIdle,
            "The \"Player\" animator controller not have animation \"Idle\"");

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
    public void _4CheckingComponentAnimatorInObjectPlayer()
    {
        var gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");

        /***************************Animator*************************/

        string nameComponent = "Animator";

        if (!gameObjectPlayer.TryGetComponent(out Animator animator))
        {
            Assert.AreEqual(gameObjectPlayer.AddComponent<Animator>(), animator,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectPlayer.name, nameComponent });
        }

        var path = Path.Combine("Assets", "Animations", "Player.controller");
        AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
        Assert.AreEqual(controller, animator.runtimeAnimatorController,
            "The \"{0}\" object does not have a controller in the \"{1}\" component", new object[] { gameObjectPlayer.name, nameComponent });
    }

    [Test]
    public void _5CheckingScriptPlayer()
    {
        Type type = typeof(Player);
        TestAssistant.TestingField(type, "spriteRenderer", typeof(SpriteRenderer), FieldAttributes.Private);
        TestAssistant.TestingField(type, "animator", typeof(Animator), FieldAttributes.Private);

        GameObject gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");
        Player scriptPlayer = gameObjectPlayer.GetComponent<Player>();

        TestAssistant.TestingFieldValue(type, "spriteRenderer", scriptPlayer);
        TestAssistant.TestingFieldValue(type, "animator", scriptPlayer);
    }

    [Test]
    public void _6InitializingVariablesScriptPlayer()
    {
        GameObject gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");
        Player scriptPlayer = gameObjectPlayer.GetComponent<Player>();

        var methodAwake = TestAssistant.GetMethod(typeof(Player), "Awake");
        methodAwake.Invoke(scriptPlayer, null);

        SpriteRenderer fieldSpriteRenderer = TestAssistant.GetValueField(typeof(Player), "spriteRenderer", scriptPlayer) as SpriteRenderer;
        Animator fieldAnimator = TestAssistant.GetValueField(typeof(Player), "animator", scriptPlayer) as Animator;

        Assert.AreEqual(gameObjectPlayer.GetComponent<SpriteRenderer>(), fieldSpriteRenderer,
            "The \"{0}\" method does not work correctly in the \"{1}\" class (there is no reference to the component in the \"{2}\" field)", new object[] { methodAwake.Name, scriptPlayer.name, "spriteRenderer" });

        Assert.AreEqual(gameObjectPlayer.GetComponent<Animator>(), fieldAnimator,
            "The \"{0}\" method does not work correctly in the \"{1}\" class (there is no reference to the component in the \"{2}\" field)", new object[] { methodAwake.Name, scriptPlayer.name, "animator" });

        EditorSceneManager.OpenScene(pathScene);
    }

    [Test]
    public void _7CheckingTask()
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
