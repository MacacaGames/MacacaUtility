
public interface IParameterValueFinder
{
    public string TryGetParameter<T>(string description, T script);
}