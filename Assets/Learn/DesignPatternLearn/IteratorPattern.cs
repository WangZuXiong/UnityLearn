using UnityEngine;
/// <summary>
/// 迭代器模式
/// </summary>
public class IteratorPattern : MonoBehaviour
{
    public interface IIterator
    {
        bool HasNext();
        object Next();
    }

    public interface IContainer
    {
        IIterator GetIterator();
    }
    public class NameRepository : IContainer
    {
        public static string[] Names = { "Robert", "John", "Julie", "Lora" };
        public IIterator GetIterator()
        {
            return new NameIterator();
        }

        private class NameIterator : IIterator
        {
            int Index;
            public bool HasNext()
            {
                return Index < Names.Length;
            }

            public object Next()
            {
                if (HasNext())
                {
                    return Names[Index++];
                }

                return null;
            }
        }
    }


    public void Main()
    {
        NameRepository nameRepository = new NameRepository();

        for (var temp = nameRepository.GetIterator(); temp.HasNext();)
        {
            Debug.Log("name:" + (string)temp.Next());
        }
    }
}
