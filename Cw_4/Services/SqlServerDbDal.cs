using Cw_4.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cw_4.Services
{
    public class SqlServerDbDal: IStudentsDal
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s16449;Integrated Security=True";


        public IEnumerable<Student> GetStudents()
        {           
            var list = new List<Student>();


            using (SqlConnection connectionSql = new SqlConnection(ConString))
            using (SqlCommand commandSql = new SqlCommand())
            {
                commandSql.Connection = connectionSql;
                commandSql.CommandText = "select * from student";

                connectionSql.Open();
                SqlDataReader dataRead = commandSql.ExecuteReader();
                while (dataRead.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dataRead["IndexNumber"].ToString();
                    st.FirstName = dataRead["FirstName"].ToString();
                    st.LastName = dataRead["LastName"].ToString();
                    st.BirthDate = dataRead["BirthDate"].ToString().Substring(0, 8);
                    st.idEnrollment = Int32.Parse(dataRead["IdEnrollment"].ToString());
                    list.Add(st);
                }
                connectionSql.Dispose();

            }
            return list;
        }

        public IEnumerable<Enrollment> GetEnrollment(string id)
        {


            var list = new List<Enrollment>();
            using (SqlConnection connectionSql = new SqlConnection(ConString))
            using (SqlCommand commandSql = new SqlCommand())
            {
                commandSql.Connection = connectionSql;
                commandSql.CommandText = "select * from student as s inner join enrollment as e on s.idenrollment = e.idenrollment where s.indexnumber = @id ";
                commandSql.Parameters.AddWithValue("id", id);

                connectionSql.Open();
                SqlDataReader dataRead = commandSql.ExecuteReader();
                if (dataRead.Read())
                {
                    var en = new Enrollment();
                    en.IdEnrollment = Int32.Parse(dataRead["IdEnrollment"].ToString());
                    en.Semester = Int32.Parse(dataRead["Semester"].ToString());
                    en.IdStudy = Int32.Parse(dataRead["IdStudy"].ToString());
                    en.StartDate = dataRead["StartDate"].ToString().Substring(0, 8);
                    list.Add(en);
                }
                connectionSql.Dispose();
            }

            return list;
        }

    }
}
