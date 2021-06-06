using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolarCoffee.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly SolarDbContext _db;

        public CustomerService(SolarDbContext dbContext)
        {
            _db = dbContext;
        }

        /// <summary>
        /// Returns a list of Customers from the database
        /// </summary>
        /// <returns></returns>
        public List<Data.Models.Customer> GetAllCustomers()
        {
            return _db.Customers.
                Include(customer => customer.PrimaryAddress).
                OrderBy(customer => customer.LastName).
                ToList();
        }

        /// <summary>
        /// Add a new Customer record
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <returns>ServiceResponse</returns>
        public ServiceResponse<Data.Models.Customer> CreateCustomer(Data.Models.Customer customer)
        {
            try
            {
                _db.Customers.Add(customer);
                _db.SaveChanges();
                return new ServiceResponse<Data.Models.Customer>
                {
                    IsSuccess = true,
                    Message = "New customer added",
                    Time = DateTime.UtcNow,
                    Data = customer
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Customer>
                {
                    IsSuccess = false,
                    Message = e.StackTrace,
                    Time = DateTime.UtcNow,
                    Data = customer
                };
            }
        }

        /// <summary>
        /// Delete a customer record
        /// </summary>
        /// <param name="id">int customer primary key</param>
        /// <returns>ServiceResponse</returns>
        public ServiceResponse<bool> DeleteCustomer(int id)
        {
            var customer = _db.Customers.Find(id);

            if (customer == null)
            {
                return new ServiceResponse<bool>
                {
                    Time = DateTime.UtcNow,
                    IsSuccess = false,
                    Message = "Customer to delete not found!",
                    Data = false
                };
            }

            try
            {
                _db.Customers.Remove(customer);
                _db.SaveChanges();
                return new ServiceResponse<bool>
                {
                    Time = DateTime.UtcNow,
                    IsSuccess = true,
                    Message = "Customer was deleted!",
                    Data = true
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<bool>
                {
                    Time = DateTime.UtcNow,
                    IsSuccess = false,
                    Message = e.StackTrace,
                    Data = false
                };
            }
        }


        /// <summary>
        /// Gets a customer record by primary key
        /// </summary>
        /// <param name="id">int customer primary key</param>
        /// <returns>Customer</returns>
        public Data.Models.Customer GetById(int id)
        {
            return _db.Customers.Find(id);
        }
    }
}