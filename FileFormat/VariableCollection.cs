using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileFormat
{
    class VariableCollection : ICollection<Variable>
    {
        Dictionary<string, Variable> items;

        public Variable this[string index]
        {
            get 
            {
                if (index == null) throw new ArgumentNullException();
                if (index == "") throw new ArgumentException();

                if (items.ContainsKey(index))
                    return items[index];
                else
                    return null;
            }
            set
            {
                if (index == null) throw new ArgumentNullException();
                if (index == "") throw new ArgumentException();

                if (items.ContainsKey(index))
                    items[index] = value;
                else
                    throw new ArgumentOutOfRangeException();
            }
        }

        public VariableCollection()
        {
            items = new Dictionary<string, Variable>();
        }

        public void Add(Variable item)
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
            return items.ContainsKey(name);
        }

        public bool Contains(Variable item)
        {
            return (items.ContainsKey(item.Name));
        }

        public void CopyTo(Variable[] array, int arrayIndex)
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

        public bool Remove(Variable item)
        {
            if (Contains(item))
            {
                items.Remove(item.Name);
                return true;
            }
            return false;
        }

        public IEnumerator<Variable> GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
