using UnityEngine;
using System.Text;

public class TestCode : MonoBehaviour
{
    public Sprite player;
    private void Start()
    {
        //var player = GameObject.FindGameObjectWithTag("Player");
        //var animator = player.GetComponent<Animator>();

        //if (animator == null)
        //{
        //    Assert.AreEqual(animator.runtimeAnimatorController.name, "Player",
        //        "The \"Player\" object does not have a Animator component");
        //}



        //var pathFile = Path.Combine("Assets", "Animations", "Idle.anim");
        //AnimationClip animation = AssetDatabase.LoadAssetAtPath<AnimationClip>(pathFile);
        //var propertiesAnimation = AnimationUtility.GetCurveBindings(animation);
        //var curveAnimation = AnimationUtility.GetEditorCurve(animation, propertiesAnimation[0]);


        var q = player.name;


        /***************************************************************************************/

        //var path = Path.Combine("Assets", "Animations", "Idle.anim");
        //AnimationClip animation = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);

        //var propertiesAnimation = AnimationUtility.GetCurveBindings(animation);
        //Debug.Log(propertiesAnimation.Where(p => p.propertyName.Contains("Scale")).Count() / 3);

        //if (!propertiesAnimation[0].propertyName.Contains("Scale"))
        //{
        //    Assert.AreEqual("Scale", propertiesAnimation[0].propertyName[2..],
        //        "The name of property is not equal");
        //}



        //var copyPlayer = new GameObject("PLayer");
        //DestroyImmediate(copyPlayer.GetComponent<Animator>());

        //Animator emptyAnimator = copyPlayer.AddComponent<Animator>();


        //var myBindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;

        //Type myType = typeof(SerializedProperty);

        //var properties = myType.GetProperties(myBindingFlags);

        //foreach (var property in properties)
        //{
        //    Debug.Log($"{property.Name}: {property.GetValue(animationType)}");
        //}
        //{
        //    var value = property.GetValue(animator);
        //    var valueEmptyAnimator = property.GetValue(emptyAnimator);

        //    if (!value.Equals(valueEmptyAnimator))
        //    {
        //        Debug.Log($"{property.Name}: {property.GetValue(animator)} - {property.GetValue(emptyAnimator)}");
        //    }
        //}
    }
}
