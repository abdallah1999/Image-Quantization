using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PriorityQueue
{


    protected Hashtable index; //O(1)
    Node[] array;  //O(1)
    int count;   //O(1)

    public PriorityQueue(int size)
    {
        //Initialize the array that will hold the values

        index = new Hashtable();  //O(1)

        array = new Node[size]; //O(1)
        count = 1; //O(1)
        //Fill the first cell in the array with an empty value
        array[0] = default(Node); //O(1)
        index.Add(-1, -1);  //O(1)
    }




    /// Gets the number of values stored within the Priority Queue
    public virtual int Count
    {
        get { return count - 1; } //o(1)
    }




    /// Returns the value at the head of the Priority Queue without removing it. O(1)
    public virtual Node Peek()
    {
        if (count == 0)
            return default(Node); //Priority Queue empty O(1)
        else
            return array[1]; //head of the queue O(1)

    }



    //Update value of Vertix O(Log V)
    public void ChangeKey(Node n, double w)
    {
        //Update Value O(1)
        int indexElement = (int)index[n.Vertex];
        array[indexElement].Value = w; //O(1)

        //O(Log V)
        this.BubbleUp(indexElement);
    }



    /// Adds a value to the Priority Queue O(Log V)
    public virtual void Enqueue(Node value)
    {
        //Add the value to the internal array       O(1)  
        array[count] = value;  //O(1)
        count++; //O(1)
        index.Add(value.Vertex, count - 1);  //O(1)

        //Bubble up to preserve the heap property, O(Log V)
        //starting at the inserted value 
        this.BubbleUp(count - 1);
    }

    /// Returns the minimum value inside the Priority Queue O(Log V)
    public virtual Node Dequeue()
    {
        if (count == 0) //O(1)
            return default(Node); //queue is empty
        else
        {
            //The smallest value in the Priority Queue is the first item in the array O(1)
            Node minValue = array[1]; //O(1)
            int index_minValue = (int)index[minValue.Vertex]; //O(1)
            //If there's more than one item, replace the first item in the array with the last one 
            if (count > 2)
            {
                Node lastValue = array[count - 1];//O(1)
                int index_lastValue = (int)index[lastValue.Vertex]; //O(1)
                //Move last node to the head O(1)
                count--; //O(1)
                int remove_v = array[count].Vertex; //O(1)

                index.Remove(remove_v); //O(1)

                array[1] = lastValue; //O(1)
                index[lastValue.Vertex] = 1; //O(1)
                //Bubble down O(Log V)
                this.BubbleDown(1);
            }
            else
            {
                //Remove the only value stored in the queue O(1)
                count--; //O(1)
                index.Remove(1); //O(1)

            }

            return minValue;
        }
    }


    /// Restores the heap-order property between child and parent values going up towards the head O(Log V)
    protected virtual void BubbleUp(int startCell)
    {
        int cell = startCell; //O(1)

        //Bubble up as long as the parent is greater
        while (this.IsParentBigger(cell)) //O(Log V)
        {
            //Get values of parent and child O(1)
            Node parentValue = array[cell / 2]; //O(1)
            Node childValue = array[cell]; //O(1)


            //Swap the values O(1)
            array[cell / 2] = childValue; //O(1)
            array[cell] = parentValue; //O(1)
            index[childValue.Vertex] = cell / 2; //O(1)
            index[parentValue.Vertex] = cell; //O(1)
            cell /= 2; //go up parents
        }
    }

    /// Restores the heap-order property between child and parent values going down towards the bottom O(Log V )
    protected virtual void BubbleDown(int startCell)
    {
        int cell = startCell; //O(1)

        //Bubble down as long as either child is smaller O(Log V)
        while (this.IsLeftChildSmaller(cell) || this.IsRightChildSmaller(cell))
        {
            int child = this.CompareChild(cell); //O(1)

            if (child == -1) //Left Child O(1)
            {
                //Swap values O(1)
                Node parentValue = array[cell]; //O(1)
                Node leftChildValue = array[2 * cell];  //O(1)


                array[cell] = leftChildValue;  //O(1)
                array[2 * cell] = parentValue;  //O(1)
                index[leftChildValue.Vertex] = cell;  //O(1)
                index[parentValue.Vertex] = 2 * cell;  //O(1)

                cell = 2 * cell; //move down to left child O(1)
            }
            else if (child == 1) //Right Child O(1)
            {
                //Swap values O(1)
                Node parentValue = array[cell];  //O(1)
                Node rightChildValue = array[2 * cell + 1]; //O(1)

                array[cell] = rightChildValue; //O(1)
                array[2 * cell + 1] = parentValue; //O(1)
                index[rightChildValue.Vertex] = cell; //O(1)
                index[parentValue.Vertex] = 2 * cell + 1; //O(1)

                cell = 2 * cell + 1; //move down to right child O(1)
            }
        }
    }


    /// Returns if the value of a parent is greater than its child O(1)
    protected virtual bool IsParentBigger(int childCell)
    {
        if (childCell == 1)
            return false; //top of heap, no parent O(1)
        else
            return array[childCell / 2].Value.CompareTo(array[childCell].Value) > 0;

    }

    /// Returns whether the left child cell is smaller than the parent cell. O(1)
    /// Returns false if a left child does not exist.
    protected virtual bool IsLeftChildSmaller(int parentCell)
    {
        if (2 * parentCell >= count)
            return false; //out of bounds O(1)
        else
            return array[2 * parentCell].Value.CompareTo(array[parentCell].Value) < 0; //O(1)
    }


    /// Returns whether the right child cell is smaller than the parent cell.O(1)
    /// Returns false if a right child does not exist. 
    protected virtual bool IsRightChildSmaller(int parentCell)
    {
        if (2 * parentCell + 1 >= count)
            return false; //out of bounds O(1)
        else
            return array[2 * parentCell + 1].Value.CompareTo(array[parentCell].Value) < 0;    //O(1)      
    }

    /// Compares the children cells of a parent cell. -1 indicates the left child is the smaller of the two,
    /// 1 indicates the right child is the smaller of the two, 0 inidicates that neither child is smaller than the parent. O(1)
    protected virtual int CompareChild(int parentCell)
    {
        bool leftChildSmaller = this.IsLeftChildSmaller(parentCell); //O(1)
        bool rightChildSmaller = this.IsRightChildSmaller(parentCell);//O(1)

        if (leftChildSmaller || rightChildSmaller)//O(1)
        {
            if (leftChildSmaller && rightChildSmaller) //O(1)
            {
                //Figure out which of the two is smaller O(1)
                int leftChild = 2 * parentCell;//O(1)
                int rightChild = 2 * parentCell + 1;//O(1)

                Node leftValue = this.array[leftChild]; //O(1)
                Node rightValue = this.array[rightChild];//O(1)

                //Compare the values of the children O(1)
                if (leftValue.Value.CompareTo(rightValue.Value) <= 0) //O(1)
                    return -1; //left child is smaller
                else
                    return 1; //right child is smaller O(1)
            }
            else if (leftChildSmaller)
                return -1; //left child is smaller O(1)
            else
                return 1; //right child smaller
        }
        else
            return 0; //both children are bigger or don't exist O(1)
    }




}








