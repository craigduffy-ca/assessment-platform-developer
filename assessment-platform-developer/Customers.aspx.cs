﻿using assessment_platform_developer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using assessment_platform_developer.Services;
using Container = SimpleInjector.Container;
using System.Text.RegularExpressions;

namespace assessment_platform_developer
{
    public partial class Customers : Page
    {
        private static List<Customer> customers = new List<Customer>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var testContainer = (Container)HttpContext.Current.Application["DIContainer"];
                var customerService = testContainer.GetInstance<ICustomerService>();

                var allCustomers = customerService.GetAllCustomers();
                ViewState["Customers"] = allCustomers;
            }
            else
            {
                customers = (List<Customer>)ViewState["Customers"];
            }

            PopulateCustomerListBox();
            PopulateCustomerDropDownLists();
        }

        private void PopulateCustomerDropDownLists()
        {
            var countryList = Enum.GetValues(typeof(Countries))
                .Cast<Countries>()
                .Select(c => new ListItem
                {
                    Text = c.ToString(),
                    Value = ((int)c).ToString()
                })
                .ToArray();

            CountryDropDownList.Items.AddRange(countryList);
            CountryDropDownList.SelectedValue = ((int)Countries.Canada).ToString();

            var provinceList = Enum.GetValues(typeof(CanadianProvinces))
                .Cast<CanadianProvinces>()
                .Select(p => new ListItem
                {
                    Text = p.ToString(),
                    Value = ((int)p).ToString()
                })
                .ToArray();

            StateDropDownList.Items.Add(new ListItem(""));
            StateDropDownList.Items.AddRange(provinceList);
        }

        protected void PopulateCustomerListBox()
        {
            CustomersDDL.Items.Clear();
            var storedCustomers = customers.Select(c => new ListItem(c.Name)).ToArray();
            if (storedCustomers.Length != 0)
            {
                CustomersDDL.Items.AddRange(storedCustomers);
                CustomersDDL.SelectedIndex = 0;
                return;
            }

            CustomersDDL.Items.Add(new ListItem("Add new customer"));
        }

        protected void AddButton_Click(object sender, EventArgs e)
        {
            var customer = new Customer
            {
                Name = CustomerName.Text,
                Address = CustomerAddress.Text,
                City = CustomerCity.Text,
                State = StateDropDownList.SelectedValue,
                Zip = CustomerZip.Text,
                Country = CountryDropDownList.SelectedValue,
                Email = CustomerEmail.Text,
                Phone = CustomerPhone.Text,
                Notes = CustomerNotes.Text,
                ContactName = ContactName.Text,
                ContactPhone = CustomerPhone.Text,
                ContactEmail = CustomerEmail.Text
            };

            var testContainer = (Container)HttpContext.Current.Application["DIContainer"];
            var customerService = testContainer.GetInstance<ICustomerService>();
            customerService.AddCustomer(customer);
            customers.Add(customer);

            CustomersDDL.Items.Add(new ListItem(customer.Name));

            ClearCustomerFields();
        }

        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            var selectedCustomer = customers.FirstOrDefault(c => c.Name == CustomersDDL.SelectedValue);
            if (selectedCustomer != null)
            {
                selectedCustomer.Name = CustomerName.Text;
                selectedCustomer.Address = CustomerAddress.Text;
                selectedCustomer.City = CustomerCity.Text;
                selectedCustomer.State = StateDropDownList.SelectedValue;
                selectedCustomer.Zip = CustomerZip.Text;
                selectedCustomer.Country = CountryDropDownList.SelectedValue;
                selectedCustomer.Email = CustomerEmail.Text;
                selectedCustomer.Phone = CustomerPhone.Text;
                selectedCustomer.Notes = CustomerNotes.Text;
                selectedCustomer.ContactName = ContactName.Text;
                selectedCustomer.ContactPhone = CustomerPhone.Text;
                selectedCustomer.ContactEmail = CustomerEmail.Text;

                ClearCustomerFields();
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            var selectedCustomer = customers.FirstOrDefault(c => c.Name == CustomersDDL.SelectedValue);
            if (selectedCustomer != null)
            {
                customers.Remove(selectedCustomer);
                CustomersDDL.Items.Remove(CustomersDDL.SelectedItem);

                ClearCustomerFields();
            }
        }

        private void ClearCustomerFields()
        {
            CustomerName.Text = string.Empty;
            CustomerAddress.Text = string.Empty;
            CustomerEmail.Text = string.Empty;
            CustomerPhone.Text = string.Empty;
            CustomerCity.Text = string.Empty;
            StateDropDownList.SelectedIndex = 0;
            CustomerZip.Text = string.Empty;
            CountryDropDownList.SelectedIndex = 0;
            CustomerNotes.Text = string.Empty;
            ContactName.Text = string.Empty;
            ContactPhone.Text = string.Empty;
            ContactEmail.Text = string.Empty;
        }

        protected void ZipCodeValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (CountryDropDownList.SelectedValue == ((int)Countries.UnitedStates).ToString())
            {
                // Validate US ZIP code format
                args.IsValid = Regex.IsMatch(args.Value, @"^\d{5}(?:[-\s]\d{4})?$");
            }
            else if (CountryDropDownList.SelectedValue == ((int)Countries.Canada).ToString())
            {
                // Validate Canadian postal code format
                args.IsValid = Regex.IsMatch(args.Value, @"^[ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ]( )?\d[ABCEGHJKLMNPRSTVWXYZ]\d$");
            }
        }
        protected void PhoneValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, @"^\d{10}$");
        }

        protected void EmailValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Regex.IsMatch(args.Value, @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$");
        }
        protected void CustomersDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearCustomerFields();

            var selectedCustomer = customers.FirstOrDefault(c => c.Name == CustomersDDL.SelectedValue);
            if (selectedCustomer != null)
            {
                CustomerName.Text = selectedCustomer.Name;
                CustomerAddress.Text = selectedCustomer.Address;
                CustomerCity.Text = selectedCustomer.City;
                StateDropDownList.SelectedValue = selectedCustomer.State;
                CustomerZip.Text = selectedCustomer.Zip;
                CountryDropDownList.SelectedValue = selectedCustomer.Country;
                CustomerEmail.Text = selectedCustomer.Email;
                CustomerPhone.Text = selectedCustomer.Phone;
                CustomerNotes.Text = selectedCustomer.Notes;
                ContactName.Text = selectedCustomer.ContactName;
                ContactPhone.Text = selectedCustomer.ContactPhone;
                ContactEmail.Text = selectedCustomer.ContactEmail;
            }
        }
    }
}