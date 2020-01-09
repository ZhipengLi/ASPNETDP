using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap5.CompositePattern.Model
{
    interface ISpecification<T>
    {
        bool IsSatisfiedBy(T candidate);
        ISpecification<T> And(ISpecification<T> other);
        ISpecification<T> Not();
    }

    abstract class CompositeSpecification<T>: ISpecification<T>
    {
        public abstract bool IsSatisfiedBy(T candidate);
        public ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }
        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }

    class AndSpecification<T>: CompositeSpecification<T>
    {
        private ISpecification<T> _leftSpec;
        private ISpecification<T> _rightSpec;
        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this._leftSpec = left;
            this._rightSpec = right;
        }
        public override bool IsSatisfiedBy(T candidate)
        {
            return _leftSpec.IsSatisfiedBy(candidate) && _rightSpec.IsSatisfiedBy(candidate);
        }
    }

    class NotSpecification<T> : CompositeSpecification<T>
    {
        private ISpecification<T> _innserSpec;
        public NotSpecification(ISpecification<T> inner)
        {
            this._innserSpec = inner;
        }
        public override bool IsSatisfiedBy(T candidate)
        {
            return !_innserSpec.IsSatisfiedBy(candidate);
        }
    }

    class CustomerAccount
    {
        private ISpecification<CustomerAccount> _hasReachedRentalThreshold;
        private ISpecification<CustomerAccount> _hasLateFee;
        private ISpecification<CustomerAccount> _stillActive;
        public CustomerAccount()
        {
            this._hasReachedRentalThreshold = new HasReachedRentalThresholdSpecification();
            this._hasLateFee = new CustomerAccountLateFeeSpecification();
            this._stillActive = new CustomerAccountStillActiveSpecification();
        }

        public bool CanRent()
        {
            var composite = this._hasReachedRentalThreshold.And(this._hasLateFee.Not()).And(this._stillActive);
            return composite.IsSatisfiedBy(this);
        }
        public int NumberOfRentalsThisMonth { get; set; }
        public bool AccountActive { get; set; }
        public decimal LateFee { get; set; }
    }
    class HasReachedRentalThresholdSpecification : CompositeSpecification<CustomerAccount>
    {
        public override bool IsSatisfiedBy(CustomerAccount ca)
        {
            return ca.NumberOfRentalsThisMonth < 5;
        }
    }

    class CustomerAccountLateFeeSpecification : CompositeSpecification<CustomerAccount>
    {
        public override bool IsSatisfiedBy(CustomerAccount ca)
        {
            return ca.LateFee>0;
        }
    }

    class CustomerAccountStillActiveSpecification : CompositeSpecification<CustomerAccount>
    {
        public override bool IsSatisfiedBy(CustomerAccount ca)
        {
            return ca.AccountActive;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            CustomerAccount ca1 = new CustomerAccount();
            ca1.NumberOfRentalsThisMonth = 3;
            ca1.LateFee = 0;
            ca1.AccountActive = true;
            Console.WriteLine(ca1.CanRent());

            CustomerAccount ca2 = new CustomerAccount();
            ca2.NumberOfRentalsThisMonth = 3;
            ca2.LateFee = 1;
            ca2.AccountActive = true;
            Console.WriteLine(ca2.CanRent());

            Console.ReadLine();
        }
    }
}
