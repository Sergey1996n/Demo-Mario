using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;

public class Lesson10
{
    [Test]
    public void __CheckingProductName()
    {
        Assert.AreEqual("Mario", PlayerSettings.productName,
            "The \"{0}\" field has an incorrect value", new object[] { "Product Name"});
    }
}
