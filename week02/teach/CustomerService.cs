/// <summary>
/// Maintain a Customer Service Queue.  Allows new customers to be 
/// added and allows customers to be serviced.
/// </summary>
public class CustomerService {
    public static void Run() {
        // Example code to see what's in the customer service queue:
        // var cs = new CustomerService(10);
        // Console.WriteLine(cs);

        // Test Cases

        // Test 1
        // Scenario: Create a queue with an invalid size (less than or equal to 0)
        // Expected Result: The maximum size should default to 10 (max_size=10)
        Console.WriteLine("Test 1");
        var cs1 = new CustomerService(0);
        Console.WriteLine(cs1);
        // Defect(s) Found: None in the constructor; it correctly assigns 10.

        Console.WriteLine("=================");

        // Test 2
        // Scenario: Create a queue with a capacity of 2, add 2 customers, and try to add a third
        // Expected Result: The third attempt should display “Maximum Number of Customers in Queue.”
        Console.WriteLine("Test 2");
        var cs2 = new CustomerService(2);
        cs2.AddNewCustomer(); // Input data for customer 1
        cs2.AddNewCustomer(); // Input data for customer 2
        cs2.AddNewCustomer(); // This should display the error message
        Console.WriteLine(cs2);
        // Defect(s) Found: It used to have `if (_queue.Count > _maxSize)` to allow an extra client. This was changed to `>=`.

        Console.WriteLine("=================");

        // Test 3
        // Scenario: Serving a customer when the queue is empty
        // Expected result: It should display an error message and not hang
        Console.WriteLine("Test 3: Serving with an empty queue");
        var cs3 = new CustomerService(5);
        cs3.ServeCustomer();
         // Defect(s) Found: It did not check whether the queue was empty and threw an unhandled exception.

        Console.WriteLine("=================");

        // Test 4
        // Scenario: Add a customer and then serve them
        // Expected result: The customer should be served and the queue should be empty afterward
        Console.WriteLine("Test 4: Add and serve a customer");
        var cs4 = new CustomerService(5);
        cs4.AddNewCustomer(); // Client 1
        cs4.AddNewCustomer(); // Client 2
        Console.WriteLine("First service: " + cs4);
        cs4.ServeCustomer(); // Must serve client 1
        Console.WriteLine("After serving: " + cs4);
        // Defect(s) Found: It was being removed from index 0 before the client was saved, causing the wrong client to be displayed or the program to crash.

        Console.WriteLine("=================");
    }

    private readonly List<Customer> _queue = new();
    private readonly int _maxSize;

    public CustomerService(int maxSize) {
        if (maxSize <= 0)
            _maxSize = 10;
        else
            _maxSize = maxSize;
    }

    /// <summary>
    /// Defines a Customer record for the service queue.
    /// This is an inner class.  Its real name is CustomerService.Customer
    /// </summary>
    private class Customer {
        public Customer(string name, string accountId, string problem) {
            Name = name;
            AccountId = accountId;
            Problem = problem;
        }

        private string Name { get; }
        private string AccountId { get; }
        private string Problem { get; }

        public override string ToString() {
            return $"{Name} ({AccountId})  : {Problem}";
        }
    }

    /// <summary>
    /// Prompt the user for the customer and problem information.  Put the 
    /// new record into the queue.
    /// </summary>
    private void AddNewCustomer() {
        // Verify there is room in the service queue
        // Fix 1: Changed the condition to >= to prevent adding a customer when the queue is full
        if (_queue.Count >= _maxSize) {
            Console.WriteLine("Maximum Number of Customers in Queue.");
            return;
        }

        Console.Write("Customer Name: ");
        var name = Console.ReadLine()!.Trim();
        Console.Write("Account Id: ");
        var accountId = Console.ReadLine()!.Trim();
        Console.Write("Problem: ");
        var problem = Console.ReadLine()!.Trim();

        // Create the customer object and add it to the queue
        var customer = new Customer(name, accountId, problem);
        _queue.Add(customer);
    }

    /// <summary>
    /// Dequeue the next customer and display the information.
    /// </summary>
    private void ServeCustomer() {
        // Fix 2: Validate if the queue is empty before trying to serve a customer
        if (_queue.Count <= 0) {
            Console.WriteLine("Error: The queue is empty. No customers to serve.");
            return;
        }

        // Fix 3: Get the customer from the front of the queue before removing it
        var customer = _queue[0];
        _queue.RemoveAt(0);
        Console.WriteLine(customer);
    }


    /// <summary>
    /// Support the WriteLine function to provide a string representation of the
    /// customer service queue object. This is useful for debugging. If you have a 
    /// CustomerService object called cs, then you run Console.WriteLine(cs) to
    /// see the contents.
    /// </summary>
    /// <returns>A string representation of the queue</returns>
    public override string ToString() {
        return $"[size={_queue.Count} max_size={_maxSize} => " + string.Join(", ", _queue) + "]";
    }
}