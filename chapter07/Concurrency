using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap7.Concurrency.Model
{
    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Version { get; set; }
    }

    public interface IPersonRepository
    {
        void Add(Person person);
        void Save(Person person);
        Person FindBy(Guid guid);
    }
}
namespace ASPPatterns.Chap7.Concurrency.Repository
{
    using ASPPatterns.Chap7.Concurrency.Model;
    using System.Data.SqlClient;
    public class PersonRepository : IPersonRepository
    {
        private string _connectionString;
        private string _findByIdSQL = "SELECT* FROM People WHERE PersonId = @PersonId";
        private string _insertSQL = "INSERT People(FirstName, LastName, PersonId, Version) VALUES " +
            "(@FirstName, @LastName, @PersonId, @Version)";
        private string _updateSQL = "UPDATE People SET FirstName = "
            + "@FirstName, LastName = @LastName, Version = " +
            "@Version + 1 WHERE PersonId = @PersonId AND Version = @Version;";
        public PersonRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Add(Person person)
        {
            using (SqlConnection connection =
            new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = _insertSQL;
                command.Parameters.Add
                (new SqlParameter("@PersonId", person.Id));
                command.Parameters.Add
                (new SqlParameter("@Version", person.Version));
                command.Parameters.Add
                (new SqlParameter("@FirstName", person.FirstName));
                command.Parameters.Add
                (new SqlParameter("@LastName", person.LastName));
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void Save(Person person)
        {
            int numberOfRecordsAffected = 0;
            using (SqlConnection connection =
            new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = _updateSQL;
                command.Parameters.Add
                (new SqlParameter("@PersonId", person.Id));
                command.Parameters.Add
                (new SqlParameter("@Version", person.Version));
                command.Parameters.Add
                (new SqlParameter("@FirstName", person.FirstName));
                command.Parameters.Add
                (new SqlParameter("@LastName", person.LastName));
                connection.Open();
                numberOfRecordsAffected = command.ExecuteNonQuery();
            }
            if (numberOfRecordsAffected == 0)
                throw new ApplicationException(
                    @"No changes were made to Person Id(" + person.Id + "), this was "
                    + "due to another process updating the data.");
            else
                person.Version++;
        }
        public Person FindBy(Guid Id)
        {
            Person person = default(Person);
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();

                command.CommandText = _findByIdSQL;
                command.Parameters.Add(new SqlParameter("@PersonId", Id));
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        person = new Person
                        {
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Id = new Guid(reader["PersonId"].ToString()),
                            Version = int.Parse(reader["Version"].ToString())
                        };
                    }
                }
            }
            return person;
        }
    }
}

namespace ASPPattern07
{
    using ASPPatterns.Chap7.Concurrency.Model;
    using ASPPatterns.Chap7.Concurrency.Repository;
    class Concurrency
    {
        static void Main(string[] args)
        {
            string connectionString = "";
            PersonRepository repo = new PersonRepository(connectionString);
            Guid id = Guid.NewGuid();
            Person p = new Person { Id = id, FirstName = "first", LastName = "second", Version = 1 };
            repo.Add(p);

            var p1 = repo.FindBy(id);
            var p2 = repo.FindBy(id);
            p1.FirstName = "first1";
            p2.FirstName = "first2";

            repo.Save(p1);

            try
            {
                repo.Save(p2);
            }
            catch (Exception e)
            {
                Console.WriteLine("this is supposed to fail with message:" + e.Message);
            }



            Console.ReadLine();
        }
    }
}
