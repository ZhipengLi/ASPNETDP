using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap7.IdentityMap.Model
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
    }

    public interface IEmployeeRepository
    {
        Employee FindBy(Guid id);
    }
}
namespace ASPPatterns.Chap7.IdentityMap.Repository
{
    public class IdentityMap<T>
    {
        private System.Collections.Hashtable entities = new System.Collections.Hashtable();
        public T GetById(Guid id)
        {
            if (entities.ContainsKey(id))
                return (T)entities[id];
            else
                return default(T);
        }
        public void Store(Guid key, T entity)
        {
            if (!entities.ContainsKey(key))
            {
                entities.Add(key, entity);
            }
        }
    }

    public class EmployeeRepository : Model.IEmployeeRepository
    {
        private IdentityMap<Model.Employee> _employeeMap = new IdentityMap<Model.Employee>();
        public Model.Employee FindBy(Guid id)
        {
            var employee = _employeeMap.GetById(id);
            if (employee == null)
            {
                employee = DataStoreFindBy(id);
                if(employee != null)
                    this._employeeMap.Store(id, employee);
            }
            return employee;
        }
        public Model.Employee DataStoreFindBy(Guid id)
        {
            Console.WriteLine("hit datastore");
            var employee = new Model.Employee() { Id = id, FirstName = "first", SecondName = "second" };
            return employee;
        }
    }

}

namespace ASPPattern07
{
    using ASPPatterns.Chap7.IdentityMap.Model;
    using ASPPatterns.Chap7.IdentityMap.Repository;
    class Concurrency
    {
        static void Main(string[] args)
        {
            var id = Guid.NewGuid();
            var repo = new EmployeeRepository();
            Console.WriteLine("fetch employee from store for the first time. id:" + id.ToString());
            var employee = repo.FindBy(id);

            Console.WriteLine("fetch employee from store for the second time. id:" + id.ToString());
            employee = repo.FindBy(id);



            Console.ReadLine();
        }
    }
}
