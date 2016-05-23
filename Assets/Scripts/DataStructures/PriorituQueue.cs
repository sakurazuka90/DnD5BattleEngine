using System.Collections;
using System.Collections.Generic;

public class PriorityQueue<K, V>{

	private SortedDictionary<K, LinkedList<V>> _dict = new SortedDictionary<K, LinkedList<V>> ();

	public void Enqueue(V pmValue, K pmPriority)
	{
		LinkedList<V> lvList = null;

		if (!_dict.TryGetValue(pmPriority, out lvList)) {
			lvList = new LinkedList<V> ();
			_dict.Add (pmPriority, lvList);
		}

		lvList.AddLast (pmValue);
	}

	public V Dequeue()
	{
		SortedDictionary<K, LinkedList<V>>.KeyCollection.Enumerator lvEnum = _dict.Keys.GetEnumerator();

		lvEnum.MoveNext ();

		K lvKey = lvEnum.Current;

		LinkedList<V> lvList = _dict [lvKey];
		V lvValue = lvList.First.Value;

		lvList.RemoveFirst ();

		if (lvList.Count == 0) {
			_dict.Remove (lvKey);
		}

		return lvValue;
	}

	public bool Empty()
	{
		return _dict.Count == 0;
	}




}
