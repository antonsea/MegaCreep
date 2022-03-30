using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MegaCreep
{
    public class PriorityQueue<T> //where T : IComparable<T>
    {

        private List<Tuple<T, float>> data;

        public int Count
        {
            get { return data.Count; }
        }

        public PriorityQueue()
        {
            this.data = new List<Tuple<T, float>>();

            //Priority queue will return the value with lowest number for priority, priority of 1 > 5
        }

        public void Enqueue(T item, float priority)
        {
            //we add a new item at the end of the list. a new child at the end of a "binary heap"
            data.Add(Tuple.Create(item, priority));

            //We start at the end of the "binary" heap and work our way up
            int childIndex = data.Count - 1;

            //if the child index is 0, then its the highest parent so stop.
            //We are sorting the list so that the parent at index = 0 has the smallest value for priority, therefore will be removed first when needed.
            while (childIndex > 0)
            {
                //The parent of the child in the binary heap
                int parentIndex = (childIndex - 1) / 2;

                //If the child has a bigger value than its parent, then its sorted, we wont be moving it up
                if (data[childIndex].Item2 >= data[parentIndex].Item2)
                    break;

                //But if the top line is false, then the child has a smaller value and the order needs to be fixed (i.e. swapping the positon of child and parent, and go up the chain
                Tuple<T, float> temp = data[childIndex];
                data[childIndex] = data[parentIndex];
                data[parentIndex] = temp;

                //We just moved up the chain, so update the child index.
                childIndex = parentIndex;

            }
        }

        public T Dequeue()
        {
            //Get the index for the last item, see below why.
            int lineIndex = data.Count - 1;

            //Take the first item, and then put the last one at front, can't just change all the indexs by 1 because it will mess up all the parent/child relationships
            Tuple<T, float> frontItem = data[0];
            data[0] = data[lineIndex];
            data.RemoveAt(lineIndex);

            //Now we need to fix the stack, bringing the new front item all the way down the stack
            lineIndex--;
            int parentIndex = 0;

            while (true)
            {
                //The new front item is a parent and has two children. We will compare between these two children
                //this childIndex is the left child of the binary heap
                int childIndex = parentIndex * 2 + 1;

                //if we are at the end of the of the list, stop
                if (childIndex > lineIndex)
                    break;

                int rightChild = childIndex + 1;
                //Now compare the two children and see which one has the lowest priority value. We will compare this new parent (the previous last item of the list). If the right child has a lower priority value than the left index, we will be comparing the parent and the right child
                if (rightChild <= lineIndex && data[rightChild].Item2 < data[childIndex].Item2)
                    childIndex = rightChild;

                //Now compare the current parent to the child
                //If the parent is smaller than the child, than all is good. That is why we check to see which child is smaller above. If we are smaller than the smallest no need to compare to the other child
                if (data[parentIndex].Item2 <= data[childIndex].Item2)
                    break;

                //If the order is wrong, fix it
                Tuple<T, float> temp = data[parentIndex];
                data[parentIndex] = data[childIndex];
                data[childIndex] = temp;

                //Now continue going down the stack
                parentIndex = childIndex;

            }

            return frontItem.Item1;

        }

        public T Peek()
        {
            return data[0].Item1;
        }
    }
}
