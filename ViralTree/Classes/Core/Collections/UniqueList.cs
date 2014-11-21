using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree
{
    /// <summary>
    /// Init your objects with -1 in constructor, otherwise dont touch it outside of UniqueList
    /// </summary>
    public interface IUnique
    {
        int UniqueId
        {
            get;
            set;
        }
    }

    public sealed class UniqueList<T> where T : class, IUnique
    {
        private const int START_SIZE = 10;
        private const double RESIZE_FACTOR = 1.5;

        private T[] objects;
        private int[] indirectionList;
        private Queue<int> freeUniqueIds;

        public int Count
        {
            get;
            set;
        }

        public int Capacity
        {
            get;
            set;
        }

        public UniqueList()
        {
            objects = new T[START_SIZE];
            indirectionList = new int[START_SIZE];

            init();
        }

        public UniqueList(int startSize)
        {
            objects = new T[startSize];
            indirectionList = new int[startSize];

            init();
        }

        public bool AddLast(T obj)
        {
            if (obj.UniqueId == -1)
            {
                if (Count == Capacity)
                    Resize((int)(Capacity * RESIZE_FACTOR));

                obj.UniqueId = freeUniqueIds.Dequeue();

                objects[Count] = obj;

                indirectionList[obj.UniqueId] = Count;

                Count++;

                return true;
            }


            return false;
        }


        //tested
        public T Pop(T obj)
        {
            indirectionList[objects[Count - 1].UniqueId] = indirectionList[obj.UniqueId];

            MathUtil.Swap(ref objects[indirectionList[obj.UniqueId]], ref objects[Count - 1]);

            RemoveLast();

            return obj;
        }
        //tested
        public T PopAtIndex(int index)
        {
            return Pop(objects[index]);
        }
        //tested
        public T PopAtUniqueId(int uniqueId)
        {
            return Pop(objects[indirectionList[uniqueId]]);
        }
        //tested
        public T PopLast()
        {
            T tmp = objects[Count - 1];

            RemoveLast();

            return tmp;
        }


        //tested
        public bool Remove(T obj)
        {
            if (obj.UniqueId != -1)
            {
                Debug.Assert(objects[indirectionList[obj.UniqueId]] == obj, "The uniqueId of: " + obj + " doenst match the entry in the UniqueList!");
                int realIndex = indirectionList[obj.UniqueId];

                indirectionList[objects[Count - 1].UniqueId] = realIndex;

                MathUtil.Swap(ref objects[realIndex], ref objects[Count - 1]);

                RemoveLast();

                return true;
            }

            else
                return false;
        }
        //tested
        public void RemoveAtIndex(int index)
        {
            Remove(objects[index]);
        }
        //tested
        public void RemoveAtUniqueId(int uniqueId)
        {
            Remove(objects[indirectionList[uniqueId]]);
        }
        //tested
        public void RemoveLast()
        {
            indirectionList[objects[Count - 1].UniqueId] = -1;

            freeUniqueIds.Enqueue(objects[Count - 1].UniqueId);

            objects[Count - 1].UniqueId = -1;

            objects[Count - 1] = null;

            Count--;
        }


        //tested
        public bool Contains(T obj)
        {
            if (obj.UniqueId == -1)
                return false;

            else
            {
                Debug.Assert(objects[indirectionList[obj.UniqueId]] == obj, "Wrong index!");
                return true;
            }

        }

        //tested
        public T this[int index]
        {
            get { return objects[index]; }
        }

        //tested
        public int GetIndexOf(T obj)
        {
            return indirectionList[obj.UniqueId];
        }

        //tested
        public int GetIndexOf(int uniqueId)
        {
            return indirectionList[uniqueId];
        }

        //tested
        public T GetByIndex(int index)
        {
            return this[index];
        }

        //tested
        public T GetByUniqueId(int uniqueId)
        {
            return this[indirectionList[uniqueId]];
        }

        //tested
        private void init()
        {
            freeUniqueIds = new Queue<int>();

            Capacity = objects.Length;
            Count = 0;

            for (int i = 0; i < indirectionList.Length; i++)
            {
                indirectionList[i] = -1;
                freeUniqueIds.Enqueue(i);
            }
        }

        private void Resize(int newCap)
        {
            if (newCap > Capacity)
            {
                T[] tmpObjs = new T[newCap];
                int[] tmpIndirectionList = new int[newCap];

                //Copy all entries:
                for (int i = 0; i < objects.Length; i++)
                {
                    tmpObjs[i] = objects[i];
                    tmpIndirectionList[i] = indirectionList[i];
                }

                //set new references:
                indirectionList = tmpIndirectionList;
                objects = tmpObjs;

                Capacity = newCap;

                for (int i = Count; i < Capacity; i++)
                {
                    indirectionList[i] = -1;
                    freeUniqueIds.Enqueue(i);
                    //test();
                }

            }
        }

        [Conditional("DEBUG")]
        private void test() //tests if freeUniqueIds doenst have duplicates
        {
            for (int i = 0; i < freeUniqueIds.Count; i++)
            {
                for (int j = 0; j < freeUniqueIds.Count; j++)
                {
                    if (j != i)
                    {
                        Debug.Assert(freeUniqueIds.ElementAt(i) != freeUniqueIds.ElementAt(j));
                    }
                }

                for (int k = 0; k < Count; k++)
                {
                    Debug.Assert(objects[k].UniqueId != freeUniqueIds.ElementAt(i));
                }
            }



        }
    }
}
