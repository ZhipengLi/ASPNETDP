
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap5.LayerSupertypePattern.Model
{
    abstract class EntityBase<T>
    {
        private T _id;
        private bool _idHasBeenSet = false;
        private IList<string> _brokenRules = new List<String>();
        public void Add(string rule)
        {
            this._brokenRules.Add(rule);
        }
        protected abstract void CheckForBrokenRules();
        private void ClearCollectionOfBrokenRules()
        {
            this._brokenRules.Clear();
        }
        public EntityBase()
        { }
        public EntityBase(T id)
        {
            this.Id = id;
            this._idHasBeenSet = true;
        }
        public T Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (this._idHasBeenSet)
                    ThrowExceptionIfValueIsNull();
                _id = value;
            }
        }

        public IList<String> GetBrokenRules()
        {
            return this._brokenRules;
        }
        public bool IsValid()
        {
            this.ClearCollectionOfBrokenRules();
            this.CheckForBrokenRules();
            return this._brokenRules.Count() == 0;
        }
        private void ThrowExceptionIfValueIsNull()
        {
            throw new ApplicationException("You cannot override an ID");
        }
    }

    class Customer : EntityBase<Customer>
    {
        private string _firstname;
        private string _secondname;
        public Customer(string firstname, string secondname)
        {
            this._firstname = firstname;
            this._secondname = secondname;
        }
        protected override void CheckForBrokenRules()
        {
            if (_firstname.Length < 3)
                this.Add("first name is too short");
            if (_secondname.Length < 3)
                this.Add("second name is too short");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Customer c = new Customer("te", "stname");
            Console.WriteLine(c.IsValid());

            Customer c1 = new Customer("te1", "stname");
            Console.WriteLine(c1.IsValid());


            Console.ReadLine();
        }
    }
}

