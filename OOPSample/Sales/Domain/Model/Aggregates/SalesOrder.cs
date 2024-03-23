using OOPSample.Shared.Domain.Model.ValueObjects;

namespace OOPSample.Sales.Domain.Model.Aggregates;

public class SalesOrder(int id, int customerId)
{
    public int Id { get; } = id;
    public int CustomerId { get; } = customerId;
    
    private readonly List<SalesOrderItem> _items = [];

    public SalesOrderStatus Status { get; private set; } = SalesOrderStatus.PendingPayment;

    public Address ShippingAddress { get; private set; }

    public double PaidAmount { get; private set; } = 0;
    
    public void AddItem(int productId, int quantity, double unitPrice)
    {
        if (Status != SalesOrderStatus.PendingPayment)
            throw new InvalidOperationException("Can't modify an order once payment is processed.");
        _items.Add(new SalesOrderItem(Id, productId, quantity, unitPrice));
    }
    
    public void Dispatch(string street, string city, string state, string zipCode)
    {
        if (Status == SalesOrderStatus.PendingPayment)
            throw new InvalidOperationException("Can't dispatch an order that is not paid yet.");
        
        if (_items.Count == 0)
            throw new InvalidOperationException("Can't dispatch an order without items.");
        
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

    public void AddPayment(double amount)
    {
        if (amount <= 0)
            throw new InvalidOperationException("Amount must be greater than zero.");
        
        if (amount > CalculateTotalPrice() - PaidAmount)
            throw new InvalidOperationException("Amount must be less than or equal to the total price.");
        PaidAmount += amount;
        VerifyIfReadyForShipment();
    }
    
    private void VerifyIfReadyForShipment()
    {
        if (PaidAmount == CalculateTotalPrice())
        {
            Status = SalesOrderStatus.ReadyForShipment;
        }
        
    }
}