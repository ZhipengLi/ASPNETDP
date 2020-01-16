using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap7.UnitOfWork.Model
{
    public interface IAggregateRoot
    { }
    public class Account :IAggregateRoot
    {
        public decimal Balance { get; set; }
    }
    public interface IAccountRepository
    {
        void Add(Account account);
        void Remove(Account account);
        void Save(Account account);
    }
    public class AccountService
    {
        private IAccountRepository _repository;
        private Infrastructure.IUnitOfWork _unitOfWork;
        public AccountService(IAccountRepository repo, Infrastructure.IUnitOfWork unitOfWork)
        {
            this._repository = repo;
            this._unitOfWork = unitOfWork;
        }
        public void Transfer(Account from, Account to, decimal amount)
        {
            if (from.Balance >= amount)
            {
                from.Balance -= amount;
                to.Balance += amount;
                this._repository.Save(from);
                this._repository.Save(to);
                this._unitOfWork.Commit();
            }
        }
    }
}
namespace ASPPatterns.Chap7.UnitOfWork.Repository
{
    public interface IUnitOfWorkRepository
    {
        void PersistCreationOf(Model.IAggregateRoot entity);
        void PersistUpdateOf(Model.IAggregateRoot entity);
        void PersistDeletionOf(Model.IAggregateRoot entity);
    }
    public class AccountRepository : Model.IAccountRepository, Repository.IUnitOfWorkRepository
    {
        private Infrastructure.IUnitOfWork _unitOfWork;
        public AccountRepository(Infrastructure.IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public void Add(Model.Account account)
        {
            _unitOfWork.RegisterNew(account, this);
        }
        public void Remove(Model.Account account)
        {
            this._unitOfWork.RegisterRemoved(account, this);
        }
        public void Save(Model.Account account)
        {
            this._unitOfWork.RegisterAmended(account, this);
        }

        public void PersistCreationOf(Model.IAggregateRoot entity)
        {
            Console.WriteLine(" the creation job in DB");
        }
        public void PersistUpdateOf(Model.IAggregateRoot entity)
        {
            Console.WriteLine(" the update job in DB");
        }
        public void PersistDeletionOf(Model.IAggregateRoot entity)
        {
            Console.WriteLine(" the deletion job in DB");
        }
    }
}
namespace ASPPatterns.Chap7.UnitOfWork.Infrastructure
{
    using System.Transactions;
    public interface IUnitOfWork
    {
        void RegisterAmended(Model.IAggregateRoot entity, Repository.IUnitOfWorkRepository repository);
        void RegisterNew(Model.IAggregateRoot entity, Repository.IUnitOfWorkRepository repository);
        void RegisterRemoved(Model.IAggregateRoot entity, Repository.IUnitOfWorkRepository repository);
        void Commit();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private Dictionary<Model.IAggregateRoot, Repository.IUnitOfWorkRepository> _toBeAdded;
        private Dictionary<Model.IAggregateRoot, Repository.IUnitOfWorkRepository> _toBeAmended;
        private Dictionary<Model.IAggregateRoot, Repository.IUnitOfWorkRepository> _toBeRemoved;

        public UnitOfWork()
        {
            this._toBeAdded = new Dictionary<Model.IAggregateRoot, Repository.IUnitOfWorkRepository>();
            this._toBeAmended = new Dictionary<Model.IAggregateRoot, Repository.IUnitOfWorkRepository>();
            this._toBeRemoved = new Dictionary<Model.IAggregateRoot, Repository.IUnitOfWorkRepository>();
        }
        public void RegisterAmended(Model.IAggregateRoot entity, Repository.IUnitOfWorkRepository repository)
        {
            if (!this._toBeAmended.ContainsKey(entity))
            {
                this._toBeAmended.Add(entity, repository);
            }
        }
        public void RegisterNew(Model.IAggregateRoot entity, Repository.IUnitOfWorkRepository repository)
        {
            if (!this._toBeAdded.ContainsKey(entity))
            {
                this._toBeAdded.Add(entity, repository);
            }
        }
        public void RegisterRemoved(Model.IAggregateRoot entity, Repository.IUnitOfWorkRepository repository)
        {
            if (!this._toBeRemoved.ContainsKey(entity))
            {
                this._toBeRemoved.Add(entity, repository);
            }
        }
        public void Commit()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (Model.IAggregateRoot entity in this._toBeAdded.Keys)
                {
                    this._toBeAdded[entity].PersistCreationOf(entity);
                }
                foreach (Model.IAggregateRoot entity in this._toBeRemoved.Keys)
                {
                    this._toBeRemoved[entity].PersistDeletionOf(entity);
                }
                foreach (Model.IAggregateRoot entity in this._toBeAmended.Keys)
                {
                    this._toBeAmended[entity].PersistUpdateOf(entity);
                }
                scope.Complete();
            }
        }
    }
}

namespace ASPPattern07
{
    using ASPPatterns.Chap7.UnitOfWork.Model;
    using ASPPatterns.Chap7.UnitOfWork.Repository;
    using ASPPatterns.Chap7.UnitOfWork.Infrastructure;
    class Program
    {
        static void Main(string[] args)
        {
            ASPPatterns.Chap7.UnitOfWork.Model.Account account1 = new Account { Balance = 100.0m};
            ASPPatterns.Chap7.UnitOfWork.Model.Account account2 = new Account {  Balance = 0.0m};
            ASPPatterns.Chap7.UnitOfWork.Infrastructure.UnitOfWork unitOfWork = new UnitOfWork();
            ASPPatterns.Chap7.UnitOfWork.Repository.AccountRepository repo = new AccountRepository(unitOfWork);

            ASPPatterns.Chap7.UnitOfWork.Model.AccountService service = new AccountService(repo, unitOfWork);

            Console.WriteLine("before transaction account1:"+account1.Balance);
            Console.WriteLine("before transaction account2:" + account2.Balance);

            service.Transfer(account1, account2, 51.9m);
            Console.WriteLine("after transaction account1:" + account1.Balance);
            Console.WriteLine("after transaction account2:" + account2.Balance);

            Console.ReadLine();
        }
    }
}
