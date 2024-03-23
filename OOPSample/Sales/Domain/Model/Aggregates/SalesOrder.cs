using OOPSample.Shared.Domain.Model.ValueObjects;

namespace OOPSample.Sales.Domain.Model.Aggregates;

public class SalesOrder(int id, int customerId)
{
    public int Id { get; } = id;
    public int CustomerId { get; } = customerId;
    
    private readonly List<SalesOrderItem> _items = [];

    public SalesOrderStatus Status { get; private set; } = SalesOrderStatus.PendingPayment;

    public Address ShippingAddress { get; private set; }
    public void AddItem(int productId, int quantity, double unitPrice)
    {
        _items.Add(new SalesOrderItem(Id, productId, quantity, unitPrice));
    }
    
    public void Dispatch(string street, string city, string state, string zipCode)
    {
        ShippingAddress = new Address(street, city, state, zipCode);
        Status = SalesOrderStatus.Shipped;
    }

    public void Cancel()
    {
        Status = SalesOrderStatus.Cancelled;
    }
    
    public void Complete()
    {
        Status = SalesOrderStatus.Completed;
    }
    public double CalculateTotalPrice() => _items.Sum(x => x.CalculateItemPrice());
}