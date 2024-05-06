using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Transactions;
using TestD.Data;
using TestD.Models;
using TestD.Models.Dto;
using TestD.Utility;

namespace TestD.Controllers
{
    [Route("api/Order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private ApiResponse _response;
        private readonly AppDbContext _db;

        public OrderController(AppDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult<ApiResponse>> CreateOrder([FromBody] OrderCreateDTO orderCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Order order = new()
                    {
                        //Or we can use AutoMapper

                        ApplicationUserId = orderCreateDTO.ApplicationUserId,
                        PickupEmail = orderCreateDTO.PickupEmail,
                        PickupName = orderCreateDTO.PickupName,
                        PickupPhoneNumber = orderCreateDTO.PickupPhoneNumber,
                        OredrTotal = orderCreateDTO.OredrTotal,
                        OrderDate = DateTime.Now,
                        PaymentId = orderCreateDTO.PaymentId,
                        TotalItems = orderCreateDTO.TotalItems,
                        Status = String.IsNullOrEmpty(orderCreateDTO.Status) ? SD.Status_Pending : orderCreateDTO.Status,
                        OrderId = orderCreateDTO.OrderId,
                        ItemName = orderCreateDTO.ItemName,
                        MenuItemId = orderCreateDTO.MenuItemId,
                        Price = orderCreateDTO.Price,
                        Quantity = orderCreateDTO.Quantity,
                    };

                    if (order.Status == SD.Status_Pending || order.Status == SD.Status_Cancelled)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                    }
                    else if (order.Status == SD.Status_Approved)
                    {
                        _db.Orders.Add(order);
                        await _db.SaveChangesAsync();  //It will save to Database before we go to Transaction
                        _response.Result = order;
                        _response.StatusCode = HttpStatusCode.Created;
                        order.Status = SD.Status_Approved;
                        return Ok(_response);
                    }
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    ModelState.AddModelError("customError", "Error occured");
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Error = new() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> PlaceOrder(string userId, [FromBody] CartItem cartItem)
        {
            //Fist we need to retrieve the ShoppingCart
            ShoppingCart? shoppingCart = await _db.ShoppingCarts.Include(x => x.CartItems)
                .ThenInclude(u => u.MenuItem).FirstOrDefaultAsync(x => x.UserId == userId);

            //Now we need to retrieve the Order
            Order? order = await _db.Orders.FirstOrDefaultAsync(x => x.ApplicationUserId == userId);

            if (shoppingCart == null || shoppingCart.CartItems == null || shoppingCart.CartItems.Count() == 0)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (order.Status == SD.Status_Approved)
            {
                #region create Payment 

                shoppingCart.CartTotal = shoppingCart.CartItems.Sum(u => u.Quantity * u.MenuItem.Price);

                using (TransactionScope scope = new TransactionScope())
                {
                    //We need to retrieve the MenuItems
                    MenuItem? menuItem = await _db.MenuItems.FirstOrDefaultAsync(x => x.Id == cartItem.MenuItemId);
                    if (menuItem != null && menuItem.Quantity >= 1)
                    {
                        menuItem.Quantity -= 1;
                        await _db.SaveChangesAsync();
                        _response.IsSuccess = true;
                        _response.Result = order;
                        _response.StatusCode = HttpStatusCode.Created;
                        order.Status = SD.Status_Completed;
                        scope.Complete();
                    }
                }
                #endregion
            }
            else
            {
                var timeZone = order.Status = SD.Status_Pending;
                if (timeZone == SD.Status_Pending)
                {
                    //Set the Status to cancelled after 15 min!
                }
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            _response.Result = shoppingCart;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
    }
}
