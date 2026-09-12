using Microsoft.VisualStudio.TestTools.UnitTesting;

// TODO Problem 2 - Write and run test cases and fix the code to match requirements.

[TestClass]
public class PriorityQueueTests
{
    [TestMethod]
    // Scenario: Enqueue multiple items where the highest priority item is at the end of the queue.
    // Expected Result: The item with the highest priority ("Z", priority 5) should be dequeued first.
    // Defect(s) Found: The loop condition was 'index < _queue.Count - 1', which excluded the last item from evaluation.
    public void TestPriorityQueue_1()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("A", 1);
        priorityQueue.Enqueue("B", 2);
        priorityQueue.Enqueue("Z", 5);

        var result = priorityQueue.Dequeue();
        Assert.AreEqual("Z", result);
    }

    [TestMethod]
    // Scenario: Enqueue items where multiple items share the highest priority.
    // Expected Result: The item closest to the front (FIFO) should be dequeued first ("FirstHigh" before "SecondHigh").
    // Defect(s) Found: The comparison operator was '>=', which overwrote the first occurrence with subsequent items of equal priority.
    public void TestPriorityQueue_2()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("FirstHigh", 10);
        priorityQueue.Enqueue("Low", 1);
        priorityQueue.Enqueue("SecondHigh", 10);

        var firstDequeued = priorityQueue.Dequeue();
        Assert.AreEqual("FirstHigh", firstDequeued);

        var secondDequeued = priorityQueue.Dequeue();
        Assert.AreEqual("SecondHigh", secondDequeued);
    }

    [TestMethod]
    // Scenario: Enqueue multiple items and dequeue all of them sequentially.
    // Expected Result: Items should be returned in descending priority order and removed from the queue ("C", then "B", then "A").
    // Defect(s) Found: Dequeue retrieved the value but never called _queue.RemoveAt(highPriorityIndex), leaving the queue unchanged.
    public void TestPriorityQueue_ItemActuallyRemoved()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("A", 1);
        priorityQueue.Enqueue("B", 3);
        priorityQueue.Enqueue("C", 5);

        Assert.AreEqual("C", priorityQueue.Dequeue());
        Assert.AreEqual("B", priorityQueue.Dequeue());
        Assert.AreEqual("A", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: Call Dequeue on an empty queue.
    // Expected Result: InvalidOperationException should be thrown with message "The queue is empty."
    // Defect(s) Found: None. The queue already threw InvalidOperationException with the expected message.
    public void TestPriorityQueue_Empty()
    {
        var priorityQueue = new PriorityQueue();

        try
        {
            priorityQueue.Dequeue();
            Assert.Fail("Exception should have been thrown.");
        }
        catch (InvalidOperationException e)
        {
            Assert.AreEqual("The queue is empty.", e.Message);
        }
        catch (AssertFailedException)
        {
            throw;
        }
        catch (Exception e)
        {
            Assert.Fail(
                string.Format("Unexpected exception of type {0} caught: {1}",
                              e.GetType(), e.Message)
            );
        }
    }
}