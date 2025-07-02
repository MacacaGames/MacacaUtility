public class DefaultDescriptionBuilderParameterValueFinder : IParameterValueFinder
{
    public string TryGetParameter<T>(string param, T script)
    {
        string[] properties = param.Split('.');
            
        object currentObject = script;
        foreach (var prop in properties)
        {
            object result = TryGetParameter(prop, currentObject);

            if (currentObject == null)
            {
                return $"[DescriptionBuilder] ReturnValueInScriptByStr | <color=red>#{param} not found in script#</color>";
            }

            currentObject = result;
        }

        return currentObject.ToString();
    }
        
    /// <summary>
    /// 使用反射嘗試拿資料
    /// </summary>
    /// <param name="param"></param>
    /// <param name="script"></param>
    /// <returns></returns>
    static object TryGetParameter(string param,object script)
    {
        bool isSuccess = false;
            
        //先撈屬性和變數
        isSuccess = DescriptionBuilder.ReturnValueInScriptByStr(param, script, out var result);
            
        //再撈List
        if (!isSuccess)
        {
            isSuccess = DescriptionBuilder.ReturnListValueInScriptByStr(param, script, out result);
        }
            
        //再撈字典
        if (!isSuccess)
        {
            isSuccess = DescriptionBuilder.ReturnDictionaryValueInScript(param, script, out result);
        }

        return result;
    }
}