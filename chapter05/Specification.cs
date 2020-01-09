using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap5.SpecificationPattern.Model
{
    interface ISpecification<T>
    {
        bool IsSatisfiedBy(T candidate);
    }
    class CustomerAccount
    {
        private ISpecification<CustomerAccount> _hasReachedRentalThreshold;

        public CustomerAccount()
        {
            this._hasReachedRentalThreshold = new HasReachedRentalThresholdSpecification();
        }

        public bool CanRent()
        {
            return this._hasReachedRentalThreshold.IsSatisfiedBy(this);
        }
        public int NumberOfRentalsThisMonth { get; set; }
    }
    class HasReachedRentalThresholdSpecification : ISpecification<CustomerAccount>
    {
        public bool IsSatisfiedBy(CustomerAccount ca)
        {
            return ca.NumberOfRentalsThisMonth < 5;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            CustomerAccount ca1 = new CustomerAccount();
            ca1.NumberOfRentalsThisMonth = 3;

            CustomerAccount ca2 = new CustomerAccount();
            ca2.NumberOfRentalsThisMonth = 5;

            Console.WriteLine(ca1.CanRent());
            Console.WriteLine(ca2.CanRent());

            Console.ReadLine();
        }
    }
}
