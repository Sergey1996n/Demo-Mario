using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.TestTools;

public class TestAssistant
{
    private const BindingFlags BINDING_FLAGS = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;

    private static FieldAttributes AccessModifierField(FieldInfo field)
    {
        if (field.IsPublic)
            return FieldAttributes.Public;
        else if (field.IsPrivate)
            return FieldAttributes.Private;
        else if (field.IsFamily)
            return FieldAttributes.Family;
        else if (field.IsAssembly)
            return FieldAttributes.Assembly;
        else
            return FieldAttributes.FamORAssem;
    }
    private static MethodAttributes AccessModifierMethod(MethodInfo method)
    {
        if (method.IsPublic)
            return MethodAttributes.Public;
        else if (method.IsPrivate)
            return MethodAttributes.Private;
        else if (method.IsFamily)
            return MethodAttributes.Family;
        else if (method.IsAssembly)
            return MethodAttributes.Assembly;
        else
            return MethodAttributes.FamORAssem;
    }

    private static MethodAttributes AccessModifierProperty(MethodInfo methodGet, MethodInfo methodSet)
    {
        if (methodGet.IsPublic || methodSet.IsPublic)
            return MethodAttributes.Public;
        else if (methodGet.IsPrivate || methodSet.IsPrivate)
            return MethodAttributes.Private;
        else if (methodGet.IsFamily || methodSet.IsFamily)
            return MethodAttributes.Family;
        else if (methodGet.IsAssembly || methodSet.IsAssembly)
            return MethodAttributes.Assembly;
        else
            return MethodAttributes.FamORAssem;
    }

    [Obsolete]
    public static void TestingFields(Type script, string name, string type, FieldAttributes attributes, bool serializeField = false)
    {
        //var myBindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;

        var field = script.GetField(name, BINDING_FLAGS);
        Assert.IsNotNull(field,
            $"The \"{name}\" field is missing in the \"{script}\" class");

        Assert.AreEqual(type, field.FieldType.Name,
            $"The \"{name}\" field has an incorrect data type");

        Assert.AreEqual(serializeField, field.CustomAttributes.Any(a => a.AttributeType.Name == "SerializeField"),
            $"In the \"{script.Name}\" script, the \"{name}\" field is {(serializeField ? "missing" : "present")} the \"SerializeField\" attribute");

        Assert.AreEqual(attributes, field.Attributes,
            $"The \"{name}\" field has an incorrect access modifier");
    }

    public static void TestingField(Type script, string name, Type type, FieldAttributes attributes, bool serializeField = false)
    {
        var field = script.GetField(name, BINDING_FLAGS);
        Assert.IsNotNull(field,
            $"The \"{script}\" class is missing the \"{name}\" field");

        Assert.AreEqual(type, field.FieldType,
            $"In the class \"{script}\" the \"{name}\" field has an incorrect data type");

        Assert.AreEqual(serializeField, field.CustomAttributes.Any(a => a.AttributeType.Name == "SerializeField"),
            $"In the class \"{script}\" the \"{name}\" field is {(serializeField ? "missing" : "present")} the \"SerializeField\" attribute");

        Assert.AreEqual(attributes, AccessModifierField(field),
            $"In the class \"{script}\" the \"{name}\" field has an incorrect access modifier");
    }

    public static void TestingProperty(Type script, string name, Type type, MethodAttributes attributesGet = MethodAttributes.Public, MethodAttributes attributesSet = MethodAttributes.Public, MethodAttributes attributesProperty = MethodAttributes.Public)
    {
        var property = script.GetProperty(name, BINDING_FLAGS);

        Assert.IsNotNull(property,
            $"The \"{script}\" class is missing the \"{name}\" property");

        Assert.AreEqual(type, property.PropertyType,
            $"In the class \"{script}\" the \"{name}\" property has an incorrect data type");

        Assert.IsTrue(property.CanRead,
            $"In the class \"{script}\" the \"{name}\" property does not have a \"get\" accessor");

        MethodInfo getAccessor = property.GetMethod;

        Assert.AreEqual(attributesGet, AccessModifierMethod(getAccessor),
            $"In the class \"{script}\" the \"{name}\" property of the \"get\" accessor has an incorrect access modifier");

        Assert.IsTrue(property.CanWrite,
            $"In the class \"{script}\" the \"{name}\" property does not have a \"set\" accessor");

        MethodInfo setAccessor = property.SetMethod;

        Assert.AreEqual(attributesSet, AccessModifierMethod(setAccessor),
            $"In the class \"{script}\" the \"{name}\" property of the \"set\" accessor has an incorrect access modifier");

        Assert.AreEqual(attributesProperty, AccessModifierProperty(getAccessor, setAccessor),
            $"In the class \"{script}\" the \"{name}\" property has an incorrect access modifier");
    }



    public static void TestingFieldValue(Type script, string name, object component, object value = null)
    {
        var field = script.GetField(name, BINDING_FLAGS);

        Assert.AreEqual(value, field.GetValue(component),
            $"The \"{name}\" field of the \"{component}\" component has an incorrect initial value");

    public static void TestingPropertyValue(Type script, string name, object component, object value = null)
    {
        var property = script.GetProperty(name, BINDING_FLAGS);

        Assert.AreEqual(value, property.GetValue(component),
            $"The \"{name}\" property of the \"{component}\" script has an incorrect initial value");
    }

    public static object GetValueField(Type script, string name, object component)
    {
        var field = script.GetField(name, BINDING_FLAGS);

        return field.GetValue(component);
    }

    public static void SetValueField(Type script, string name, object component, object value)
    {
        var field = script.GetField(name, BINDING_FLAGS);

        field.SetValue(component, value);
    }

    [Obsolete]
    public static void TestingMethods(Type script, string name, string type, MethodAttributes attributes, MyParameterInfo[] parameters)
    {
        //var myBindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;

        var method = script.GetMethod(name, BINDING_FLAGS);
        Assert.IsNotNull(method,
            $"The \"{name}\" method is missing in the \"{script}\" class!");

        Assert.AreEqual(type, method.ReturnType.Name,
            $"The \"{name}\" method has an incorrect data type!");

        Assert.AreEqual(attributes, method.Attributes,
            $"The \"{name}\" method has an incorrect access modifier!");

        int index = 0;
        foreach (var parameter in method.GetParameters())
        {
            Assert.AreEqual(parameters[index].ParameterType.Name, parameter.ParameterType.Name,
                $"In the \"{name}\" method, a parameter with the \"{parameters[index].ParameterType.Name}\" data type was expected!");
            Assert.AreEqual(parameters[index].Name, parameter.Name,
                $"In the \"{name}\" method, a parameter with the named \"{parameters[index].Name}\" was expected!");

            index++;
        }
    }

    public static MethodInfo GetMethod(Type script, string name)
    {
        return script.GetMethod(name, BINDING_FLAGS);
    }

    public static void TestingMethod(Type script, string name, Type type, MethodAttributes attributes, MyParameterInfo[] parameters = null)
    {
        var method = script.GetMethod(name, BINDING_FLAGS);
        Assert.IsNotNull(method,
            $"The \"{script}\" class is missing the \"{name}\" method");

        Assert.AreEqual(type, method.ReturnType,
            $"In the class \"{script}\" the \"{name}\" method has an incorrect data type");

        Assert.AreEqual(attributes, AccessModifierMethod(method),
            $"In the class \"{script}\" the \"{name}\" method has an incorrect access modifier");

        int index = 0;
        foreach (var parameter in method.GetParameters())
        {
            Assert.AreEqual(parameters[index].Name, parameter.Name,
                $"In the class \"{script}\" method \"{name}\" has an incorrect parameter");

            Assert.AreEqual(parameters[index].ParameterType, parameter.ParameterType,
                $"In the class \"{script}\" in the \"{name}\" method, the parameter \"{parameters[index].Name}\" has an incorrect data type");
            
            if (parameters[index].HasDefaultValue)
            {
                Assert.AreEqual(parameters[index].DefaultValue, parameter.DefaultValue,
                    $"In the class \"{script}\" in the \"{name}\" method the \"{parameters[index].Name}\" parameter has an incorrect default value");
            }
            index++;
        }
    }

    public static IEnumerator WaitUntilForSeconds(Func<bool> checker, float maxTime, string message, params object[] args)
    {
        float timer = 0;
        while (timer < maxTime)
        {
            bool check = checker.Invoke();
            if (check)
            {
                yield break;
            }
            yield return null;
            timer += Time.deltaTime;
        }

        Assert.Fail(message, args);
    }
}

public class MyParameterInfo : ParameterInfo
{
    public override bool HasDefaultValue => GetDefaultValue() != default;
    public override object DefaultValue => DefaultValueImpl;
    private object GetDefaultValue()
    {
        if (DefaultValueImpl == null)
        {
            return null;
        }
        Type type = DefaultValueImpl.GetType();

        return Activator.CreateInstance(type);
    }

    public MyParameterInfo(Type type, string name)
    {
        this.ClassImpl = type;
        this.NameImpl = name;
    }
    public MyParameterInfo(Type type, string name, object value): 
        this(type, name)
    {
        this.DefaultValueImpl = value;
    }
}
