using Dapper;
using System.Data.SqlClient;


namespace BlazorApplication2.Data
{
    public class Tip_cerere
    {
        public int Id { get; set; }
        public string Tip_Cerere { get; set; }       

    }

    public class Tip_cerereRepo
    {
        readonly string sqlCon = " ";
        public Tip_cerereRepo(IConfiguration configuration)
        {
            sqlCon = configuration["SqlConnection"];

        }

        public async Task<(Tip_cerere data, Exception err)> SelectByLoginId(int CerereId)
        {
            Exception exep = null;
            Tip_cerere cerere = null;

            using (var con = new SqlConnection(sqlCon))
            {
                string sql = "Select * from Tip_Cerere where Id =@Id";

                try
                {
                    cerere = await con.QueryFirstAsync<Tip_cerere>(sql, new { Id = CerereId });
                }

                catch (Exception ex)
                {
                    exep = ex;
                }
            }
            return (cerere, exep);
        }


        public async Task<(bool added, Exception err)> AddAllByAdmin(Tip_cerere val)
        {
            bool succ = false;
            Exception exep = null;

            using (var con = new SqlConnection(sqlCon))
            {
                var sql = "Insert into Tip_Cerere (Tip_cerere) values (@Tip_Cerere) select @@Identity as Id";


                try
                {
                    var res = await con.ExecuteScalarAsync<int>(sql, val);
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
