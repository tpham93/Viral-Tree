using SFML.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree
{
    /// <summary>
    /// Dynamic array. Reserves more capacity if the capacity is too small when adding items. No moving up when removing items -> but the indices are not safe, they may change during the use of the list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class MyList<T>
    {
        const int START_SIZE = 10;

        public double ResizeFactor
        {
            get;
            set;
        }

        T[] internArray;
        int count;

        /// <summary>
        /// Creates MyList with a specific intern start capacity.
        /// </summary>
        public MyList()
        {
            internArray = new T[START_SIZE];
            ResizeFactor = 1.75;
            count = 0;
        }

        /// <summary>
        /// Creates MyList with a given extern starting capacity.
        /// </summary>
        /// <param name="startCapacity">How big the list should be at initializing.</param>
        public MyList(int startCapacity)
        {
            internArray = new T[startCapacity];
            ResizeFactor = 1.75;
            count = 0;
        }

        /// <summary>
        /// How many elements are acutally inside the list.
        /// </summary>
        public int Count
        {
            get { return count; }
        }

        /// <summary>
        /// How many elements can the list carry before resizing.
        /// </summary>
        public int Capacity
        {
            get { return internArray.Length; }
            set { reserve(value); }
        }

        /// <summary>
        /// Adds a new element at the end of the list. O(1) if capacity is big enough.
        /// </summary>
        /// <param name="obj">Which object to add.</param>
        public void pushBack(T obj)
        {
            if (count == internArray.Length)
                reserve((int)(internArray.Length * ResizeFactor));

            internArray[count++] = obj;
        }

        /// <summary>
        /// Searches for the given object and removes it, if the list contains it. Swaps the object to be deleted to the end of the list to remove in O(1). Overall: O(n)
        /// </summary>
        /// <param name="obj">Which object to be deleted.</param>
        /// <returns>If the object could be deleted.</returns>
        public bool remove(T obj)
        {
            for (int i = 0; i < count; i++)
            {
                if (internArray[i].Equals(obj))
                {
                    removeAt(i);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes the element at the given index. O(1). Swaps element[i] with element[count - 1], thus no element moving.
        /// </summary>
        /// <param name="index">Where to delete.</param>
        public void removeAt(int index)
        {
            MathUtil.Swap<T>(ref internArray[index], ref internArray[count - 1]);
            removeLast();
        }

        /// <summary>
        /// Removes the last element of the List. O(1).
        /// </summary>
        public void removeLast()
        {
            internArray[count - 1] = default(T);
            count--;
        }

        /// <summary>
        /// Reservese the list to the given capacity if its bigger than before. O(n).
        /// </summary>
        /// <param name="desiredCap">Targeted capacity.</param>
        public void reserve(int desiredCap)
        {
            if (desiredCap > internArray.Length)
            {
                T[] newArray = new T[desiredCap];

                for (int i = 0; i < count; i++)
                    newArray[i] = internArray[i];

                internArray = newArray;
            }
        }

        /// <summary>
        /// Indicates if the given object is inside the list. O(n).
        /// </summary>
        /// <param name="toTest">Which object to test.</param>
        /// <returns>True, if the object is inside the list. False else.</returns>
        public bool contains(T toTest)
        {
            for (int i = 0; i < count; i++)
            {
                if (internArray[i].Equals(toTest))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the index of the given object, if its inside the list. O(n).
        /// </summary>
        /// <param name="obj">The searched object.</param>
        /// <returns>Index of the object, if its inside, -1 otherwise.</returns>
        public int getIndexOf(object obj)
        {
            for (int i = 0; i < count; i++)
            {
                if (internArray[i].Equals(obj))
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Removes all elements of the list but dont destroys them. O(n).
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < count; i++)
                internArray[i] = default(T);

            count = 0;
        }

        /// <summary>
        /// Operator overloading for []. So it is used as an array. O(1).
        /// </summary>
        /// <param name="index">Index of the array.</param>
        /// <returns>elementAt(index)</returns>
        public T this[int index]
        {
            get { return internArray[index]; }
            set { internArray[index] = value; }
        }

        /// <summary>
        /// Getter for the object at index. O(1).
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T GetElementAt(int index)
        {
            return internArray[index];
        }

        public T Last()
        {
            return internArray[count - 1];
        }

        public T First()
        {
            return internArray[0];
        }

        public bool IsEmpty()
        {
            return count == 0;
        }

        

    }

}
