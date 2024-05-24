using assessment_platform_developer.Models;
using assessment_platform_developer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace assessment_platform_developer.Services
{
	public interface ICustomerService
	{
		IEnumerable<Customer> GetAllCustomers();
		Customer GetCustomer(int id);
		void AddCustomer(Customer customer);
		void UpdateCustomer(Customer customer);
		void DeleteCustomer(int id);
	}

	public class CustomerService : ICustomerService
	{
		private readonly ICustomerQuery customerQuery;
        private readonly ICustomerCommand customerCommand;

        public CustomerService(ICustomerQuery customerQuery, ICustomerCommand customerCommand)
		{
			this.customerQuery = customerQuery;
			this.customerCommand = customerCommand;
		}

		public IEnumerable<Customer> GetAllCustomers()
		{
			return customerQuery.GetAll();
		}

		public Customer GetCustomer(int id)
		{
			return customerQuery.Get(id);
		}

		public void AddCustomer(Customer customer)
		{
			customerCommand.Add(customer);
		}

		public void UpdateCustomer(Customer customer)
		{
            customerCommand.Update(customer);
		}

		public void DeleteCustomer(int id)
		{
            customerCommand.Delete(id);
		}
	}

}