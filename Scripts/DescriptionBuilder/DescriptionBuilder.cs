using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

public class DescriptionBuilder
{
    private static IParameterValueFinder parameterValueFinder = new DefaultDescriptionBuilderParameterValueFinder();

    public static void SetParameterValueFinder(IParameterValueFinder finder)
    {
        parameterValueFinder = finder;
    }

    /// <summary>
    /// 將描述中的params替換成script中的值
    /// </summary>
    /// <param name="description"></param>
    /// <param name="script"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string GetDescriptionFormat<T>(string description, T script)
    {
        string result = description;
        List<string> parameters = GetTargetParams(result);
        foreach (var parameter in parameters)
        {
            result = result.Replace("{[" + parameter + "]}", parameterValueFinder.TryGetParameter(parameter, script));
        }

        return result;
    }


    #region 反射

    /// <summary>
    /// 反射取值
    /// </summary>
    /// <param name="param"></param>
    /// <param name="script"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool ReturnValueInScriptByStr(string param, object script, out object result)
    {
        //獲取屬性
        PropertyInfo propInfo = script.GetType()
            .GetProperty(param, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (propInfo != null)
        {
            //獲取屬性值並轉換為字符串
            object value = propInfo.GetValue(script);
            result = value;
            return true;
        }

        //如果屬性不存在，嘗試獲取變數
        FieldInfo fieldInfo = script.GetType()
            .GetField(param, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (fieldInfo != null)
        {
            object value = fieldInfo.GetValue(script);
            result = value;
            return true;
        }

        result = $"[DescriptionBuilder] ReturnValueInScriptByStr | <color=red>#{param} not found in script#</color>";
        return false;
    }

    /// <summary>
    /// 獲取List的值
    /// </summary>
    /// <param name="param"></param>
    /// <param name="script"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool ReturnListValueInScriptByStr(string param, object script, out object result)
    {
        var match = Regex.Match(param, @"^([^\[\]]+)\[([0-9]+)\]$");
        if (match.Success)
        {
            // 獲取集合名和索引
            string listName = match.Groups[1].Value;
            int index = int.Parse(match.Groups[2].Value);

            // 使用反射找到集合
            var field = script.GetType().GetField(listName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var property = script.GetType().GetProperty(listName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            object list = field?.GetValue(script) ?? property?.GetValue(script);

            // 嘗試從集合中獲取值
            if (list is IList && index < ((IList)list).Count)
            {
                result = ((IList)list)[index];
                return true;
            }
        }

        result =
            $"[DescriptionBuilder] ReturnListValueInScriptByStr | <color=red>#{param} not found in script#</color>";
        return false;
    }

    /// <summary>
    /// 獲取Dictionary的值
    /// </summary>
    /// <param name="DicParam"></param>
    /// <param name="script"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool ReturnDictionaryValueInScript(string DicParam, object script, out object result)
    {
        result =
            $"[DescriptionBuilder] ReturnDictionaryValueInScript | <color=red>#{DicParam} not found in script#</color>";

        //解析字串來分離出屬性名和key
        int firstBracket = DicParam.IndexOf('[');
        int lastBracket = DicParam.LastIndexOf(']');
        if (firstBracket == -1 || lastBracket == -1 || lastBracket <= firstBracket + 1)
            return false;

        //使用反射找到對應的屬性並獲取其值
        string propertyName = DicParam.Substring(0, firstBracket);
        var targetKey = DicParam.Substring(firstBracket + 1, lastBracket - firstBracket - 1);
        if (targetKey.Contains('\"'))
        {
            targetKey = DicParam.Substring(firstBracket + 1, lastBracket - firstBracket - 1).Trim('\"');
        }

        PropertyInfo propertyInfo = script.GetType().GetProperty(propertyName);
        if (propertyInfo == null || string.IsNullOrEmpty(targetKey))
            return false;

        //使用獲取到的字典和鍵來讀取對應的值
        object propertyValue = propertyInfo.GetValue(script);
        if (propertyValue is IDictionary dictionary)
        {
            foreach (var k in dictionary.Keys)
            {
                if (k.ToString().Contains(targetKey))
                {
                    result = dictionary[k];
                    return true;
                }
            }
        }

        return false;
    }

    #endregion

    #region 字串解析

    /// <summary>
    /// 在description中提取{[ ]}中的字串
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    private static List<string> GetTargetParams(string description)
    {
        List<string> targetParams = new List<string>();
        // 為了避免裝飾用的{ }被一併當成需要替換的字串，所以沿用I2替換字串的{[]}
        // gpt寫ㄉ正規表達式來匹配"{[ ]}"中的內容
        Regex regex = new Regex(@"\{\[(.*?)\]\}");
        MatchCollection matches = regex.Matches(description);

        foreach (Match match in matches)
        {
            // 將匹配到的值加入到列表中
            if (match.Groups.Count > 1) // 確保有匹配到內容
            {
                targetParams.Add(match.Groups[1].Value); // Groups[1]是第一個括號匹配的內容
            }
        }

        return targetParams;
    }

    #endregion
}
