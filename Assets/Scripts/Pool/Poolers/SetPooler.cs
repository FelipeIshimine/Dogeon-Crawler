using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pooling.Poolers
{
	public class SetPooler : BasePooler, IPool
    {
        public bool disableWhenEnqueue = true;
		#region Fields / Properties
		public HashSet<Poolable> collection = new HashSet<Poolable>();
		#endregion

		#region Public

		public SetPooler SetPrefab(GameObject nPrefab)
		{
			prefab = nPrefab;
			return this;
		}

		public override void Enqueue (Poolable item)
		{
			base.Enqueue(item);
			if (collection.Contains(item))
				collection.Remove(item);
			else
                Debug.LogWarning($"Doesnt contains item {item.gameObject.name}");

            if (disableWhenEnqueue)
                item.gameObject.SetActive(false);
		}

		public override Poolable Dequeue ()
		{
			Poolable item = base.Dequeue();
			collection.Add(item);

            var selfPoolable = item.GetComponent<SelfPoolable>();
            if (selfPoolable)
                selfPoolable.pool = this;

			return item;
		}

        [Button]
		public override void EnqueueAll ()
		{
			foreach (Poolable item in collection)
				base.Enqueue(item);
			collection.Clear();
		}

        internal void Remove(Poolable poolable)
        {
            if (collection.Contains(poolable))
                collection.Remove(poolable);
        }

        #endregion
    }
}
