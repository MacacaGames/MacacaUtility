using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


public partial class SROptions
{
    private string _desc;

    [Category("DescriptionBuilder")]
    public string desc
    {
        get { return _desc; }
        set { _desc = value; }
    }
    
    [Category("DescriptionBuilder")]
    public void RunDescriptionBuilder()
    {
        TestClass testClass = new TestClass();
        NameTest nameTest = new NameTest("Hehehe");
        Debug.Log(DescriptionBuilder.GetDescriptionFormat(desc, testClass));
    }
}

#region Testing

public class NameTest
{
    public string myName { get; set; }

    public NameTest(string name)
    {
        myName = name;
    }
}

public class TestClass
{
    public int hp;
    public string atk;
    public int level { get; set; }
    public string name { get; set; }
    public NameTest nameTest { get; set; }
    public List<string> sss { get; set; }
    public Dictionary<string, string> dddd { get; set; }
    public Dictionary<int, float> intDic { get; set; }
    
    public Dictionary<string, NameTest> nameTests { get; set; }
    
    public Dictionary<int, NameTest> nameIntTests { get; set; }
    
    public Dictionary<string, List<int>> dicList { get; set; }

    public TestClass()
    {
        hp = 10;
        atk = "50000";
        level = 9999;
        name = "Dio";
        nameTest = new NameTest("Hello World");
        sss = new List<string> { "bb", "eee" };
        dddd = new Dictionary<string, string>()
        {
            {"a", "ss"},
            {"b", "ww"},
        };
        intDic = new Dictionary<int, float>()
        {
            {0, 1.5f},
            {5, 0.9f},
        };
        
        nameTests = new Dictionary<string, NameTest>()
        {
            {"name01", new NameTest("牛燒肉丼")},
            {"name02", new NameTest("超值碗牛丼")},
        };
        nameIntTests = new Dictionary<int, NameTest>()
        {
            {8, new NameTest("好吃炒飯")},
            {66, new NameTest("咖哩乓乓")},
        };
        dicList = new Dictionary<string, List<int>>()
        {
            {"dicList01", new List<int>(){4, 14}},
            {"dicList02", new List<int>(){7777, 66666}},
        };
    }
}

#endregion
