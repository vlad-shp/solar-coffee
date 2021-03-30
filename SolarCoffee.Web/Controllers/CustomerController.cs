using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Customer;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Controllers
{
    public class CustomerController:ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [HttpPost("/api/customer")]
        public ActionResult CreateCustomer([FromBody] CustomerModel customer)
        {
            _logger.LogInformation("Creating customer");

            customer.CreatedOn=customer.UpdatedOn=DateTime.UtcNow;
            
            var customerData = CustomerMapper.SerializeCustomer(customer);
            var newCustomer = _customerService.CreateCustomer(customerData);

            return Ok(newCustomer);
        }

        [HttpGet("/api/customer")]
        public ActionResult GetCustomers()
        {
            _logger.LogInformation("Getting customers");
            var customer = _customerService.GetAllCustomers();

            var customerModels = customer
                .Select(CustomerMapper.SerializeCustomer)
                .OrderByDescending(customerModel=>customerModel.CreatedOn)
                .ToList();


            return Ok(customerModels);
        }

        [HttpDelete("/api/customer/{id}")]
        public ActionResult RemoveCustomer(int id)
        {
            _logger.LogInformation("Removing a customer");
            var response = _customerService.DeleteCustomer(id);

            return Ok(response);
        }

    }
}