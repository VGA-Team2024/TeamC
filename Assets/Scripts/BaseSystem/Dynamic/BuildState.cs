public class BuildState
{
    const string _hash = "test build";

    public const string TeamID = "Foundation";

    public static string BuildHash
    {
        get
        {
#if UNITY_EDITOR
            return "UNITY_EDITOR";
#else
            return _hash;
#endif
        }
    }
};