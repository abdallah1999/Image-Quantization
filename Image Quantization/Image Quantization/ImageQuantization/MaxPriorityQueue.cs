using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class MaxPriorityQueue
      
    {

        //store Nodes of vertix and weight
        protected List<Node> storedValues;
        //store vertex and index
   
        public MaxPriorityQueue()
        {
            //Initialize the array that will hold the values
          
            storedValues = new List<Node>();
            //Fill the first cell in the array with an empty value
            storedValues.Add(default(Node));
         
        }

        /// Gets the number of values stored within the Priority Queue
        public virtual int Count
        {
            get { return storedValues.Count - 1; } //o(1)
        }


        /// Returns the value at the head of the Priority Queue without removing it. O(1)
        public virtual Node Peek()
        {
            if (this.Count == 0)
                return default(Node); //Priority Queue empty O(1)
            else
                return storedValues[1]; //head of the queue O(1)
           
        }

        /// Adds a value to the Priority Queue O(Log V)
        public virtual void Enqueue(Node value)
        {
            //Add the value to the internal array       O(1)  
            storedValues.Add(value);

            //Bubble up to preserve the heap property, O(Log V)
            //starting at the inserted value 
            this.BubbleUp(storedValues.Count - 1);
        }

        /// Returns the minimum value inside the Priority Queue O(Log V)
        public virtual Node Dequeue()
        {
            if (this.Count == 0)
                return default(Node); //queue is empty
            else
            {
                //The smallest value in the Priority Queue is the first item in the array O(1)
                Node minValue = this.storedValues[1];
                //If there's more than one item, replace the first item in the array with the last one 
                if (this.storedValues.Count > 2)
                {
                    Node lastValue = this.storedValues[storedValues.Count - 1];
                    //Move last node to the head O(1)
                    int remove_v = storedValues[storedValues.Count - 1].Vertex;
                    this.storedValues.RemoveAt(storedValues.Count - 1);
                   
                    this.storedValues[1] = lastValue;
                    //Bubble down O(Log V)
                    this.BubbleDown(1);
                }
                else
                {
                    //Remove the only value stored in the queue O(1)
                    storedValues.RemoveAt(1);
                }

                return minValue;
            }
        }

        /// Restores the heap-order property between child and parent values going up towards the head O(Log V)
        protected virtual void BubbleUp(int startCell)
        {
            int cell = startCell;

            //Bubble up as long as the parent is greater
            while (this.IsParentSmaller(cell)) //O(Log V)
            {
                //Get values of parent and child O(1)
                Node parentValue = this.storedValues[cell / 2];
                Node childValue = this.storedValues[cell];
             

                //Swap the values O(1)
                this.storedValues[cell / 2] = childValue;
                this.storedValues[cell] = parentValue;
                cell /= 2; //go up parents
            }
        }

        /// Restores the heap-order property between child and parent values going down towards the bottom O(Log V )
        protected virtual void BubbleDown(int startCell)
        {
            int cell = startCell;

            //Bubble down as long as either child is smaller O(Log V)
            while (this.IsLeftChildBigger(cell) || this.IsRightChildBigger(cell))
            {
                int child = this.CompareChild(cell);

                if (child == -1) //Left Child O(1)
                {
                    //Swap values O(1)
                    Node parentValue = storedValues[cell];
                    Node leftChildValue = storedValues[2 * cell];
                 

                    storedValues[cell] = leftChildValue;
                    storedValues[2 * cell] = parentValue;
                    cell = 2 * cell; //move down to left child O(1)
                }
                else if (child == 1) //Right Child O(1)
                {
                    //Swap values O(1)
                    Node parentValue = storedValues[cell];
                    Node rightChildValue = storedValues[2 * cell + 1];
                    
                    storedValues[cell] = rightChildValue;
                    storedValues[2 * cell + 1] = parentValue;
                    cell = 2 * cell + 1; //move down to right child O(1)
                }
            }
        }


        /// Returns if the value of a parent is greater than its child O(1)
        protected virtual bool IsParentSmaller(int childCell)
        {
            if (childCell == 1)
                return false; //top of heap, no parent O(1)
            else
                return storedValues[childCell / 2].Value.CompareTo(storedValues[childCell].Value) < 0;
            
        }

        /// Returns whether the left child cell is smaller than the parent cell. O(1)
        /// Returns false if a left child does not exist.
        protected virtual bool IsLeftChildBigger(int parentCell)
        {
            if (2 * parentCell >= storedValues.Count)
                return false; //out of bounds O(1)
            else
                return storedValues[2 * parentCell].Value.CompareTo(storedValues[parentCell].Value) > 0; //O(1)
        }


        /// Returns whether the right child cell is smaller than the parent cell.O(1)
        /// Returns false if a right child does not exist. 
        protected virtual bool IsRightChildBigger(int parentCell)
        {
            if (2 * parentCell + 1 >= storedValues.Count)
                return false; //out of bounds O(1)
            else
                return storedValues[2 * parentCell + 1].Value.CompareTo(storedValues[parentCell].Value) > 0;    //O(1)      
        }

        /// Compares the children cells of a parent cell. -1 indicates the left child is the smaller of the two,
        /// 1 indicates the right child is the smaller of the two, 0 inidicates that neither child is smaller than the parent. O(1)
        protected virtual int CompareChild(int parentCell)
        {
            bool leftChildSmaller = this.IsLeftChildBigger(parentCell);
            bool rightChildSmaller = this.IsRightChildBigger(parentCell);

            if (leftChildSmaller || rightChildSmaller)
            {
                if (leftChildSmaller && rightChildSmaller)
                {
                    //Figure out which of the two is smaller O(1)
                    int leftChild = 2 * parentCell;
                    int rightChild = 2 * parentCell + 1;

                    Node leftValue = this.storedValues[leftChild];
                    Node rightValue = this.storedValues[rightChild];

                    //Compare the values of the children O(1)
                    if (leftValue.Value.CompareTo(rightValue.Value) > 0)
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





    


