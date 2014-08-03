using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileFormat
{
    class CompoundTypeCollection : ICollection<CompoundType>
    {
        Dictionary<string, CompoundType> items;

        public CompoundType this[string index]
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
            items = new Dictionary<string, CompoundType>();
        }

        public void Add(CompoundType item)
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
            foreach (CompoundType c in items.Values)
            {
                r = r | c.ContainsName(name);
            }
            return r;
        }

        public bool Contains(CompoundType item)
        {
            if (items.ContainsKey(item.Name)) return true;
            return false;
        }

        public void CopyTo(CompoundType[] array, int arrayIndex)
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

        public bool Remove(CompoundType item)
        {
            if (Contains(item))
            {
                items.Remove(item.Name);
                return true;
            }
            return false;
        }

        public IEnumerator<CompoundType> GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
