using Dapper;
using System.Data.SqlClient;

namespace BlazorApplication2.Data
{
    public class Users
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }

        public string Functie { get; set; }

        public int Salariu { get; set; }


    }

    public class UsersRepo
    {
        readonly string sqlCon = "";

        public int Logged = 0;
        public UsersRepo(IConfiguration configuration)
        {
            sqlCon = configuration["SqlConnection"];
        }

        public async Task<(Users data, Exception err)> SelectByUsername(string Username)
        {
            Exception exep = null;
            Users user = null;

            using (var con = new SqlConnection(sqlCon))
            {
                //string de sql - selectare/insert din baza de date
                string sql = "Select * from Users_Login where Username=@Username"; //@ variabila de mai sus
                try
                {
                    user = await con.QueryFirstAsync<Users>(sql, new { Username = Username });    //prima valoare o ia query..
                }
                catch (Exception ex)
                {
                    exep = ex;
                    user = null;
                }
            }
            return (user, exep);
        }

        public async Task<(bool added, Exception err)> AddUserAsync(Users data)      //daca o ajuns in baza de date, true sau false
        {
            bool succ = false;
            Exception exep = null;
            using (var con = new SqlConnection(sqlCon))
            {
                string sql = "insert into Users_Login(Username, Password)" + "values (@Username,@Password) select @@Identity as Id";
                try
                {
                    var res = await con.ExecuteScalarAsync<int>(sql, data);
                    if (res > 0)
                        succ = true;
                }
                catch (Exception ex)
                {
                    exep = ex;

                }
            }
            return (succ, exep);
        }
        public async Task<(Users data, Exception err)> SelectByLoginId(int LoginId)
        {
            Exception exep = null;
            Users fluturas = null;

            using (var con = new SqlConnection(sqlCon))
            {
                string sql = "Select * from Users_Login where Id = @Id_Login";

                try
                {
                    fluturas = await con.QueryFirstAsync<Users>(sql, new { Id_Login = LoginId });
                }

                catch (Exception ex)
                {
                    exep = ex;
                }
            }
            return (fluturas, exep);
        }
        public async Task<(List<Users> data, Exception err)> SelectForAdd()
        {
            Exception exep = null;
            List<Users> fluturas = null;

            using (var con = new SqlConnection(sqlCon))
            {
                string sql = "Select * from Users_Login where Salariu IS NULL;";

                try
                {
                    fluturas = (await con.QueryAsync<Users>(sql)).ToList();
                }

                catch (Exception ex)
                {
                    exep = ex;
                }
            }
            return (fluturas, exep);
        }

        public async Task<(bool added, Exception err)> AddAllByAdmin(Users val, int Id)
        {
            bool succ = false;
            Exception exep = null;

            using (var con = new SqlConnection(sqlCon))
            {
                var sql = "Update Users_Login SET Nume=@Nume,Prenume=@Prenume,Functie=@Functie, Salariu=@Salariu  Where Id=@Id;";


                try
                {
                    var res = await con.ExecuteScalarAsync<int>(sql, new { Id = Id , Nume = val.Nume, Prenume = val.Prenume, Functie = val.Functie, Salariu = val.Salariu});
                    if (res != 0)
                        succ = true;
                }
                catch (Exception ex)
                {
                    exep = ex;
                }
            }

            return (succ, exep);
        }
    }
}
 
