using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileFormat.Languages;

namespace FileFormat
{
    class CompoundTypeCollection<T> : ICollection<CompoundType<T>>
        where T : Language
    {
        Dictionary<string, CompoundType<T>> items;

        public CompoundType<T> this[string index]
        {
            get
            {
                if (index == null) throw new ArgumentNullException();
                if (index == "") throw new ArgumentException();
                return items[index];
            }
            set
            {
                if (index == null) throw new ArgumentNullException();
                if (index == "") throw new ArgumentException();
                items[index] = value;
            }
        }

        public CompoundTypeCollection()
        {
            items = new Dictionary<string, CompoundType<T>>();
        }

        public void Add(CompoundType<T> item)
        {
            if (!Contains(item)) items.Add(item.Name, item);
            else throw new ArgumentException();
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool ContainsName(string name)
        {
            bool r = false;
            foreach (CompoundType<T> c in items.Values)
            {
                r = r | c.ContainsName(name);
            }
            return r;
        }

        public bool Contains(CompoundType<T> item)
        {
            if (items.ContainsKey(item.Name)) return true;
            return false;
        }

        public void CopyTo(CompoundType<T>[] array, int arrayIndex)
        {
            items.Values.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(CompoundType<T> item)
        {
            if (Contains(item))
            {
                items.Remove(item.Name);
                return true;
            }
            return false;
        }

        public IEnumerator<CompoundType<T>> GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
