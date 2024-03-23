namespace OOPSample.Sales.Domain.Model.Aggregates;

public class SalesOrderItem
{
    public SalesOrderItem(int salesOrderId, int productId, int quantity, double unitPrice)
    {
        if (salesOrderId <= 0)
        {
            throw new ArgumentException("Sales order id must be greater than zero.");
        }
        
        if (productId <= 0)
        {
            throw new ArgumentException("Product id must be greater than zero.");
        }
        
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.");
        }
        
        if (unitPrice <= 0)
        {
            throw new ArgumentException("Unit price must be greater than zero.");
        }
        
        SalesOrderId = salesOrderId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public Guid Id { get;  } = GenerateOrderItemId();
    public int SalesOrderId { get; }
    public int ProductId { get; }
    public int Quantity { get; }
    public double UnitPrice { get; }

    private static Guid GenerateOrderItemId()
    {
        return Guid.NewGuid();
    }
    
    public double CalculateItemPrice() => Quantity * UnitPrice;
}