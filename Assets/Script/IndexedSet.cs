using System;
using System.Collections;
using System.Collections.Generic;

internal class IndexedSet<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	private readonly List<T> m_List = new List<T>();
	/// <summary>
	/// key - 元素的值  value - 元素对应的下标
	/// </summary>
	private Dictionary<T, int> m_Dictionary = new Dictionary<T, int>();

	public int Count => m_List.Count;

	public bool IsReadOnly => false;

	public T this[int index]
	{
		get
		{
			return m_List[index];
		}
		set
		{
			T key = m_List[index];
			m_Dictionary.Remove(key);
			m_List[index] = value;
			m_Dictionary.Add(key, index);
		}
	}

	public void Add(T item)
	{
		m_List.Add(item);
		m_Dictionary.Add(item, m_List.Count - 1);
	}

	public bool AddUnique(T item)
	{
		if (m_Dictionary.ContainsKey(item))
		{
			return false;
		}
		m_List.Add(item);
		m_Dictionary.Add(item, m_List.Count - 1);
		return true;
	}

	public bool Remove(T item)
	{
		int value = -1;
		if (!m_Dictionary.TryGetValue(item, out value))
		{
			return false;
		}
		RemoveAt(value);
		return true;
	}

	public IEnumerator<T> GetEnumerator()
	{
		throw new NotImplementedException();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Clear()
	{
		m_List.Clear();
		m_Dictionary.Clear();
	}

	public bool Contains(T item)
	{
		return m_Dictionary.ContainsKey(item);
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		m_List.CopyTo(array, arrayIndex);
	}

	public int IndexOf(T item)
	{
		int value = -1;
		m_Dictionary.TryGetValue(item, out value);
		return value;
	}

	public void Insert(int index, T item)
	{
		throw new NotSupportedException("Random Insertion is semantically invalid, since this structure does not guarantee ordering.");
	}

	public void RemoveAt(int index1)
	{
		T val1 = m_List[index1];
		m_Dictionary.Remove(val1);
		if (index1 == m_List.Count - 1)
		{
			m_List.RemoveAt(index1);
			return;
		}
		int index2 = m_List.Count - 1;
		//最后一个元素
		T val2 = m_List[index2];
		m_List[index1] = val2;
		m_Dictionary[val2] = index1;
		m_List.RemoveAt(index2);
	}

	public void RemoveAll(Predicate<T> match)
	{
		int num = 0;
		while (num < m_List.Count)
		{
			T val = m_List[num];
			if (match(val))
			{
				Remove(val);
			}
			else
			{
				num++;
			}
		}
	}

	public void Sort(Comparison<T> sortLayoutFunction)
	{
		m_List.Sort(sortLayoutFunction);
		for (int i = 0; i < m_List.Count; i++)
		{
			T key = m_List[i];
			m_Dictionary[key] = i;
		}
	}
}
