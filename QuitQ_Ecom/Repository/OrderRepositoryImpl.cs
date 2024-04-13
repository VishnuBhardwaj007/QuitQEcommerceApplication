using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Models;

namespace QuitQ_Ecom.Repository
{
    public class OrderRepositoryImpl:IOrder
    {
        private readonly QuitQEcomContext _context;
        private readonly IMapper _mapper;
        private readonly IProduct _productRepo;
        private readonly IUserAddress _userAddressRepo;
        private readonly IPayment _paymentRepo;
        private readonly IOrderItem _ordererItemRepo;
        private readonly ICart _cartRepo;
        private readonly ILogger<OrderRepositoryImpl> _logger;

        public OrderRepositoryImpl(QuitQEcomContext quitQEcomContext, IMapper mapper,IProduct product, IUserAddress userAddress, IPayment paymentRepo,IOrderItem orderItem,ICart cart, ILogger<OrderRepositoryImpl> logger)
        {
            _context = quitQEcomContext;
            _mapper = mapper;
            _productRepo = product;
            _userAddressRepo = userAddress;
            _paymentRepo = paymentRepo;
            _ordererItemRepo = orderItem;
            _cartRepo = cart;

        }
        private List<CartDTO> GetCartItemsByUserId(int USerId)
        {
            var cartitems = _context.Carts.Where(x=> x.UserId == USerId).ToList();
            return _mapper.Map<List<CartDTO>>(cartitems);
        }

        public async Task<OrderDTO> Createorder(int userId,List<CartDTO> cartitemsList,string shippingAddress)
        {
            try
            {
                var cartObj = _mapper.Map<List<Cart>>(cartitemsList);


               
                decimal totalAmount = 0.0M;
                foreach (var cartitem in cartitemsList)
                {
                  
                    var productObj = _context.Products.FirstOrDefault(x => x.ProductId == cartitem.ProductId);
                    totalAmount += cartitem.Quantity * productObj.Price;
                }

                var OrderObj = new Order()
                {
                    UserId = userId,
                    TotalAmount = totalAmount,
                    OrderDate = DateTime.Now,
                    OrderStatus = "pending",
                    ShippingAddress = shippingAddress
                };
                await _context.Orders.AddAsync(OrderObj);
                await _context.SaveChangesAsync();
                int id = OrderObj.OrderId;
                return _mapper.Map<OrderDTO>(OrderObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating order: {Message}", ex.Message);
                return null;
            }

        }

        public async Task<bool> DeleteOrderById(int orderID)
        {
            try
            {
                var OrderObj =await _context.Orders.FindAsync(orderID);
                if(OrderObj == null)
                {
                    return false;
                }
                _context.Orders.Remove(OrderObj);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting order with ID {OrderId}: {Message}", orderID, ex.Message);
                return false;
            }
        }

        public async Task<Dictionary<bool,string>> PlaceOrder(int UserId, string paymentType)
        {
            //this method should get the userid as the input

            //get all cart items and send to checkExistsProducts of ProductReposiry to check if all products are available this returns true(null) or false(Products Which are not available)
            //call the Useraddress and select the address which is active 
            //if the /payment in the frontend the user selects the cod and hits enter 
            //you write a method in the PaymentController PaymentWithCod and call this OrderRepository PlaceOrder
            //if it is false return SomeProducts are out of Stock and send that to repsonse and say these are not availble
            //if true then pass this to CreateOrder method of Order Repository with the cart items and the user id this will return you OrderDto with the id

            //if it is null retuen could not place order then you pass this to the /payment method of the paymet controller and say could not place order


            //and if it is succefull call the AddNewPaymet method of the PaymentController and and add a new record with this order id,date,amout,paymentstatus pending,payment method cod
            //      and also call the updatequantityofproctbyproductId and pass this cartitems and update the products and also create a entry in the shippertable(create this)

            //after placing the order and payment is entered and productquantitiesareupdated, send this cartitems and the orderid and create productsobj and send  to the CreateOrderItems method of the OrderRepository
            //you iwll enter all theses records in the OrderItems table
            var result = new Dictionary<bool, string>();
            var cartItems = GetCartItemsByUserId(UserId); 
            var ProductsStatus =await _productRepo.CheckQuantityOfProducts(cartItems);
            if (ProductsStatus.Count() > 0)
            {
                result[false] = "Could not place order. Some products are out of stock.";
                return result;
                //return "Could Not Place order. Some Products Are Having Less Quantity";
            }
            var userShippingAddress = await _userAddressRepo.GetActiveUserAddressByUserId(UserId);


            string shippingAddress = userShippingAddress.ToString();
            if (shippingAddress==null)
            {
                result[false] = "Could not place order. Please select the delivery address.";
                return result;
                //return "Could Not Place order. Some Products Are Having Less Quantity";
            }
            else
            {
                try { var OrderDtoObj =await Createorder(UserId, cartItems, shippingAddress);
                    if(OrderDtoObj == null)
                    {
                        result[false] = "Could Not place The Order Try again later";
                        return result;
                        //return "Could Not place The Order Try again later";
                        
                    }
                    else
                    {
                        //create here one record in shipper table call the function here
                        var ShipperObj = new Shipper()
                        {
                            OrderId = OrderDtoObj.OrderId
                        };
                        _context.Shippers.Add(ShipperObj);
                        await _context.SaveChangesAsync();
                        PaymentDTO PaymentDtoObj;
                        if (paymentType == "cod")
                        {
                            PaymentDtoObj =await _paymentRepo.AddNewPayment(OrderDtoObj,paymentType);
                        }
                        else
                        {
                            PaymentDtoObj =await _paymentRepo.AddNewPayment(OrderDtoObj, paymentType);
                        }

                        //var PaymentDtoObj = _paymentRepo.AddNewCodPayment(OrderDtoObj);
                        
                        if (PaymentDtoObj == null)
                        {
                            //call the orderRepository to invoke the delete order by id and send msg to the payment method that invoke place order and say could not place order
                            var status = await DeleteOrderById(OrderDtoObj.OrderId);
                            result[false] = "Could not process the If amount debited will be refunded .Could Not place the order";
                            return result;
                            //return "Could Not pLace order";
                        }
                        else
                        {
                            //here delete cart itms of this user. 
                            var status = await _productRepo.UpdateQuantitiesOfProducts(cartItems);
                            var OrderItemsAddedStatus =await _ordererItemRepo.AddNewOrderItem(cartItems, OrderDtoObj);
                            var RemoveCartItemsStatus =await _cartRepo.RemoveCartItemsOfUser(UserId);
                            result[true] = "SuuccessFully placed order";
                            return result;
                            //return "SuuccessFully placed order";
                                           
                        }
                    }
                
                }
                catch (Exception ex) { _logger.LogError(ex, "Failed to place order for user ID {UserId}.", UserId); throw; }
                
            }


            
        }
        //if the user is seller and wants to see the orders placed from his store the users
        //from the order items get the store id and merge them with  userdid from the orders table and show

        public async Task<List<OrderDTO>> ViewAllOrdersByUserId(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems) // Eager loading OrderItems
                .Where(o => o.UserId == userId)
                .ToListAsync();

            // Mapping Order entities to OrderDTOs
            var orderDTOs = orders.Select(o => new OrderDTO
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                OrderStatus = o.OrderStatus,
                ShippingAddress = o.ShippingAddress,
                orderItemListDTOs = o.OrderItems.Select(oi => new OrderItemDTO
                {
                    // Map properties from OrderItem entity to OrderItemDTO
                    OrderItemId = oi.OrderItemId,
                    OrderId = oi.OrderId,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                }).ToList()
            }).ToList();

            return orderDTOs;
        }
        public async Task<OrderDTO> ViewOrderByOrderId(int orderId)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems) // Eager load OrderItems
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                {
                    return null; // Order not found
                }

                var orderDto = _mapper.Map<OrderDTO>(order); // Map Order to OrderDTO

                return orderDto;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                _logger.LogError(ex, $"Error retrieving order with ID {orderId}: {ex.Message}");
                return null; // Rethrow the exception to the caller
            }
        }

        public async Task<List<OrderDTO>> ViewOrdersBySellerId(int sellerId)
        {
            try
            {
                // Retrieve orders and their associated order items for the given seller
                var orders = await _context.Orders
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                    .Where(o => o.OrderItems.Any(oi => oi.Product.StoreId == sellerId)) // Filter orders by seller's store ID
                    .ToListAsync();

                // Map the retrieved orders to DTOs
                var orderDTOs = orders.Select(order => new OrderDTO
                {
                    OrderId = order.OrderId,
                    UserId = order.UserId,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    OrderStatus = order.OrderStatus,
                    ShippingAddress = order.ShippingAddress,
                    orderItemListDTOs = order.OrderItems
                        .Where(oi => oi.Product.StoreId == sellerId) // Filter order items by seller's store ID
                        .Select(oi => new OrderItemDTO
                        {
                            OrderItemId = oi.OrderItemId,
                            ProductId = oi.ProductId,
                            Quantity = oi.Quantity,


                        })
                        .ToList()
                })
                .ToList();

                return orderDTOs;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                _logger.LogError(ex, $"Error retrieving orders for seller ID {sellerId}: {ex.Message}");
                throw; // Rethrow the exception to the caller
            }
        }
    }
}
