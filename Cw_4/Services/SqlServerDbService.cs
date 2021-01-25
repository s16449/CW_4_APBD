using Cw_4.DTOs.Request;
using Cw_4.DTOs.Response;
using Cw_4.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cw_4.Services
{
    public class SqlServerDbService : IStudentDbService
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s16449;Integrated Security=True";

        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest request)
        {
            EnrollStudentResponse response = new EnrollStudentResponse();
            Enrollment enrollment = new Enrollment();
            DateTime startDateTime;

            using (SqlConnection connectionSql = new SqlConnection(ConString))
            using (SqlCommand commandSql = new SqlCommand())
            {
                commandSql.Connection = connectionSql;
                connectionSql.Open();

                var transaction = connectionSql.BeginTransaction();

                try
                {
                    commandSql.CommandText = "select idstudy from studies where name=@name";
                    commandSql.Parameters.AddWithValue("name", request.Name);
                    commandSql.Transaction = transaction;

                    var readStudiesName = commandSql.ExecuteReader();
                    if (!readStudiesName.Read())
                    {

                        response.answer = "Brak studiow";
                        return response;
                    }
                    int idStudy = (int)readStudiesName["IdStudy"];
                    readStudiesName.Close();

                    commandSql.CommandText = "select idenrollment from enrollment where idenrollment >= (select max(idenrollment) from enrollment)";
                    var readIdEnrollment = commandSql.ExecuteReader();
                    if (!readIdEnrollment.Read()) { }
                    int idEnrollment = (int)readIdEnrollment["IdEnrollment"] + 1;
                    readIdEnrollment.Close();

                    commandSql.CommandText = "select idEnrollment, StartDate from Enrollment where idStudy=@idStudy and Semester=1" +
                        "ORDER BY StartDate";
                    commandSql.Parameters.AddWithValue("idStudy", idStudy);



                    var readEnrollment = commandSql.ExecuteReader();
                    if (!readEnrollment.Read())
                    {
                        response.answer = "Brak rekrutacji";
                        startDateTime = DateTime.Now;
                        commandSql.CommandText = "insert into Enrollment VALUES(@id, @Semester, @IdStud, @StartDate)";
                        commandSql.Parameters.AddWithValue("id", idEnrollment);
                        commandSql.Parameters.AddWithValue("Semester", 1);
                        commandSql.Parameters.AddWithValue("IdStud", idStudy);
                        commandSql.Parameters.AddWithValue("StartDate", startDateTime);
                        readEnrollment.Close();
                        commandSql.ExecuteNonQuery();
                    }
                    else
                    {
                        idEnrollment = (int)readEnrollment["IdEnrollment"];
                        startDateTime = (DateTime)readEnrollment["StartDate"];
                        readEnrollment.Close();
                    }

                    enrollment.IdEnrollment = idEnrollment;
                    enrollment.Semester = 1;
                    enrollment.IdStudy = idStudy;
                    enrollment.StartDate = startDateTime;

                    response.enrollment = enrollment;

                    commandSql.CommandText = "select IndexNumber from Student where IndexNumber=@indexNum";
                    commandSql.Parameters.AddWithValue("indexNum", request.IndexNumber);

                    //DateTime dT = Convert.ToDateTime(request.BirthDate);
                    string converted = Convert.ToDateTime(request.BirthDate).ToString("yyyy-MM-dd");

                    try
                    {
                        commandSql.CommandText = "insert into Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) values " +
                        "(@indexNumber, @firstName, @lastName, @birthDate, @idEnrollment)";

                        commandSql.Parameters.AddWithValue("indexNumber", request.IndexNumber);
                        commandSql.Parameters.AddWithValue("firstName", request.FirstName);
                        commandSql.Parameters.AddWithValue("lastName", request.LastName);
                        commandSql.Parameters.AddWithValue("BirthDate", converted);
                        commandSql.Parameters.AddWithValue("idEnrollment", idEnrollment);

                        response.answer = "OK";

                        commandSql.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (SqlException e)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Studia dodane");
                    }
                }
                catch (SqlException e)
                {
                    transaction.Rollback();
                    response.answer = e.Message;
                }
                return response;
            }
        }

        public StudentPromotionResponse StudentPromotion(StudentPromotionRequest request)
        {
            StudentPromotionResponse response = new StudentPromotionResponse();
            Enrollment enrollment = new Enrollment();

            using (SqlConnection connectionSql = new SqlConnection(ConString))
            using (SqlCommand commandSql = new SqlCommand("PromoteStudents", connectionSql))
            {
                commandSql.CommandType = CommandType.StoredProcedure;
                commandSql.Parameters.AddWithValue("@name", request.Name);
                commandSql.Parameters.AddWithValue("@semester", request.Semester);

                response.answer = "OK";

                SqlCommand command = new SqlCommand();

                command.Connection = connectionSql;
                connectionSql.Open();


                command.CommandText = "select IdStudy from Studies where Name=@name";
                command.Parameters.AddWithValue("name", request.Name);

                var readStudiesName = command.ExecuteReader();
                if (!readStudiesName.Read())
                {
                    response.answer = "Brak studiów w bazie";
                    return response;
                }
                int idstudies = (int)readStudiesName["IdStudy"];
                readStudiesName.Close();

                command.CommandText = "select idEnrollment,semester,idstudy,StartDate from Enrollment where idStudy=@idStudy and Semester=@Semester";
                command.Parameters.AddWithValue("idStudy", idstudies);
                command.Parameters.AddWithValue("Semester", request.Semester + 1);

                var readEnrollment = command.ExecuteReader();
                if (readEnrollment.Read())
                {
                    enrollment.IdEnrollment = (int)readEnrollment["IdEnrollment"];
                    enrollment.Semester = (int)readEnrollment["Semester"];
                    enrollment.IdStudy = (int)readEnrollment["IdStudy"];
                    enrollment.StartDate = (DateTime)readEnrollment["StartDate"];
                }
                readEnrollment.Close();
                response.enrollment = enrollment;

                return response;
            }
        }


        public Student GetStudent(string indexNumber)
        {
            using (SqlConnection connectionSql = new SqlConnection(ConString))
            using (SqlCommand commandSql = new SqlCommand())
            {
                commandSql.Connection = connectionSql;
                commandSql.CommandText = "select * from student where IndexNumber = @indexNumber";
                commandSql.Parameters.AddWithValue("@indexNumber", indexNumber);

                connectionSql.Open();
                SqlDataReader dataRead = commandSql.ExecuteReader();
                if (dataRead.Read())
                {
                    var st = new Student();
                    st.IndexNumber = indexNumber;
                    st.FirstName = dataRead["FirstName"].ToString();
                    st.LastName = dataRead["LastName"].ToString();
                    st.BirthDate = Convert.ToDateTime(dataRead["BirthDate"].ToString().Substring(0, 8));
                    st.idEnrollment = Int32.Parse(dataRead["IdEnrollment"].ToString());
                    connectionSql.Close();
                    return st;
                   
                }
                return null;
            }
        }
    }
}
