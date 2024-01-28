public abstract class Utility
{
    public static T GetRandomEnum<T>()
    {
        System.Array a = System.Enum.GetValues(typeof(T));
        T v = (T)a.GetValue(UnityEngine.Random.Range(0,a.Length));
            
        return v;
    }
}