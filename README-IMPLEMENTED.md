# Changes to original solution

To enforce the Command Query Responsibility Segregation (CQRS) pattern: ICustomerRepository was modified to separate ICustomerQuery and ICustomerCommand, along with their corresponding classes. CustomerService was also modified to orchestrate these.

Server side validation added to the Customer fields.

Add and Delete functionality added to the form.
