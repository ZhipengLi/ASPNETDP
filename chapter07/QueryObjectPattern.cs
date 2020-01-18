using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap7.QueryObject.Infrastructure
{
    public enum CriteriaOperator
    {
        Equal,
        LessThanOrEqual,
        NotApplicable
    }
    public class Criterion
    {
        private string _propertyName;
        private Object _value;
        private CriteriaOperator _operator;
        public Criterion(string property, Object obj, CriteriaOperator op)
        {
            this._propertyName = property;
            this._value = obj;
            this._operator = op;
        }

        public string PropertyName
        {
            get
            {
                return _propertyName;
            }
        }

        public Object Value
        {
            get
            {
                return _value;
            }
        }

        public CriteriaOperator criteriaOperator
        {
            get
            {
                return _operator;
            }
        }
    }

    public class OrderByClause
    {
        public string PropertyName { get; set; }
        public bool Desc { get; set; }
    }

    public enum QueryOperator
    {
        And,
        Or
    }

    public enum QueryName
    {
        Dynamic = 0,
        RetrieveOrdersInAComplexQuery =1
    }

    public class Query
    {
        private QueryName _name;
        private IList<Criterion> _criteria;
        public QueryOperator QueryOperator { get; set; }
        public OrderByClause OrderByClause { get; set; }
        public Query(QueryName name, IList<Criterion> criteria) {
            this._name = name;
            this._criteria = criteria;
        }
        public Query() : this(QueryName.Dynamic, new List<Criterion>())
        { }
        public QueryName Name
        {
            get
            {
                return _name;
            }
        }
        public IEnumerable<Criterion> Criteria
        {
            get
            {
                return _criteria;
            }
        }

        public bool IsNamedQuery()
        {
            return _name != QueryName.Dynamic;
        }

        public void Add(Criterion criterion)
        {
            if (IsNamedQuery())
            {
                throw new Exception("you cannot add criterion to a named query");
            }
            else
            {
                this._criteria.Add(criterion);
            }
        }
    }
    public static class NamedQueryFactory
    {
        public static Query CreateRetrieveOrdersUsingAComplexQuery(Guid customerId)
        {
            IList<Criterion> criteria = new List<Criterion>();
            Query query = new Query(QueryName.RetrieveOrdersInAComplexQuery, criteria);
            criteria.Add(new Infrastructure.Criterion("CustomerId", customerId, CriteriaOperator.NotApplicable));
            return query;
        }
    }
}
namespace ASPPatterns.Chap7.QueryObject.Model
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsShipped { get; set; }
    }
    public interface IOrderRepository
    {
        IEnumerable<Order> FindBy(Infrastructure.Query query);
    }
    public class OrderService
    {
        private IOrderRepository _repo;
        public OrderService(IOrderRepository repo)
        {
            this._repo = repo;
        }
        public IEnumerable<Order> FindAllCustomerOrdersBy(Guid customerId)
        {
            Infrastructure.Query query = new Infrastructure.Query();
            query.Add(new Infrastructure.Criterion("CustomerId", customerId, Infrastructure.CriteriaOperator.Equal));
            query.OrderByClause = new Infrastructure.OrderByClause() { PropertyName="CustomerId", Desc = true };
            var res = this._repo.FindBy(query);
            return res;
        }

        public IEnumerable<Order> FindAllCustomerOrdersWithinDateBy(Guid customerId, DateTime dateTime)
        {
            Infrastructure.Query query = new Infrastructure.Query();
            query.Add(new Infrastructure.Criterion("CustomerId", customerId, Infrastructure.CriteriaOperator.Equal));
            query.QueryOperator = Infrastructure.QueryOperator.And;
            query.Add(new Infrastructure.Criterion("OrderDate", dateTime, Infrastructure.CriteriaOperator.LessThanOrEqual));
            query.OrderByClause = new Infrastructure.OrderByClause() { PropertyName = "OrderDate", Desc = true };
            var res = this._repo.FindBy(query);
            return res;
        }
        public IEnumerable<Order> FindAllCustomerOrdersUsingAComplexQueryWith(Guid customerId)
        {
            Infrastructure.Query query = Infrastructure.NamedQueryFactory.CreateRetrieveOrdersUsingAComplexQuery(customerId);
            var orders = this._repo.FindBy(query);
            return orders;
        }
    }
}
namespace ASPPatterns.Chap7.QueryObject.Repository
{
    using System.Data.SqlClient;
    public static class OrderQueryTranslator
    {
        private static string baseQuery = "SELECT * FROM  Orders ";
        public static void TranslateTo(this Infrastructure.Query query, SqlCommand command)
        {
            if (query.IsNamedQuery())
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = query.Name.ToString();
                foreach (var cri in query.Criteria)
                {
                    command.Parameters.AddWithValue("@" + cri.PropertyName, cri.Value);
                }
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(baseQuery);
                bool isFirstCriterion = true;
                foreach (var cri in query.Criteria)
                {
                    if (isFirstCriterion)
                    {
                        isFirstCriterion = false;
                        sb.Append("WHERE ");
                    }
                    sb.Append(GenerateFilterClauseFrom(cri));
                }

                sb.Append(" ");
                sb.Append(GenerateOrderByClauseFrom(query.OrderByClause));

                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = sb.ToString();
            }
        }
        private static string GenerateOrderByClauseFrom(Infrastructure.OrderByClause orderByClause)
        {
            return " ORDERBY " + orderByClause.PropertyName + " " + (orderByClause.Desc ? "DESC" : "ASC");
        }
        private static string GenerateFilterClauseFrom(Infrastructure.Criterion criterion)
        {
            return "@" + FindTableColumnName(criterion.PropertyName) +
                        GenerateSQLOperatorFrom(criterion.criteriaOperator) +
                        criterion.Value;
        }
        private static string GenerateQueryOperatorFrom(Infrastructure.Query query)
        {
            switch (query.QueryOperator)
            {
                case Infrastructure.QueryOperator.And:
                    return "AND";
                case Infrastructure.QueryOperator.Or:
                    return "OR";
                default:
                    throw new Exception("No such operator");
            }
        }
        private static string FindTableColumnName(string name)
        {
            switch (name)
            {
                case "CustomerId":
                    return "CustomerId";
                case "OrderDate":
                    return "OrderDate";
                default:
                    throw new Exception("No such column name");
            }
        }
        private static string GenerateSQLOperatorFrom(Infrastructure.CriteriaOperator op)
        {
            switch (op)
            {
                case Infrastructure.CriteriaOperator.Equal:
                    return "=";
                case Infrastructure.CriteriaOperator.LessThanOrEqual:
                    return "<=";
                case Infrastructure.CriteriaOperator.NotApplicable:
                    throw new Exception("no such SQL operator");
                default:
                    throw new Exception("no such SQL operator");
            }
        }
    }
}

namespace ASPPattern07
{
    using ASPPatterns.Chap7.QueryObject.Infrastructure;
    using ASPPatterns.Chap7.QueryObject.Model;
    using ASPPatterns.Chap7.QueryObject.Repository;

    public class FakeOrderRepo : IOrderRepository
    {
        public IEnumerable<Order> FindBy(Query query)
        {
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            OrderQueryTranslator.TranslateTo(query, cmd);
            Console.WriteLine(cmd.CommandText);

            return null;
        }
    }
    class Concurrency
    {
        static void Main(string[] args)
        {
            OrderService os = new OrderService(new FakeOrderRepo());
            os.FindAllCustomerOrdersBy(new Guid());
            os.FindAllCustomerOrdersUsingAComplexQueryWith(new Guid());
            os.FindAllCustomerOrdersWithinDateBy(new Guid(), DateTime.Now);
            Console.ReadLine();
        }
    }
}
