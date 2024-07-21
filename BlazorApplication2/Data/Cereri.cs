using Dapper;
using System.Data.SqlClient;
using System.Diagnostics;
namespace BlazorApplication2.Data
{
    public class Cereri
    {
        public int Id { get; set; } 
        public int Id_fluturas { get; set; }
        public string Tip_Cerere { get; set; }  
        public DateTime? Data1 { get; set; }
        public DateTime? Data2 { get; set; }
        public byte Status { get; set; }
    }
    public class CereriRepo
    {
        public List<String> CereriAfisate = new List<String>();
        public List<Cereri> data = new List<Cereri>();
        public string StringFinal;
        readonly string sqlCon = " ";
        public CereriRepo(IConfiguration configuration)
        {
            sqlCon = configuration["SqlConnection"];

        }
        public async Task<(bool added, Exception exx)> AddCerere (Cereri add)
        {
            bool succ = false;
            Exception exep = null;

            using (var con = new SqlConnection(sqlCon))
            {
                var sql = "Insert into Cereri (Id_fluturas, Tip_Cerere, Data1, Data2, Status)" + "values (@Id_fluturas, @Tip_Cerere, @Data1, @Data2, @Status) select @@Identity as Id";


                try
                {
                    var res = await con.ExecuteScalarAsync<int>(sql, add);
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
        public async Task<(List <Cereri> val, Exception exx)> SelectCereriByFluturasId (int Id_Fluturas)
        {
            Exception exep = null;
            List<Cereri> vall = null;
            using (var con = new SqlConnection(sqlCon))
            {
                string sql = "Select * from Cereri where Id_fluturas=@Id_fluturas";

                try
                {
                    vall = (await con.QueryAsync<Cereri>(sql, new { Id_fluturas = Id_Fluturas })).ToList();
                }

                catch (Exception ex)
                {
                    exep = ex;
                }
            }
            return (vall, exep);
        }

        public async Task<(List<Cereri> val, Exception exx)> SelectAllCereri()
        {
            Exception exep = null;
            List<Cereri> vall = null;
            using (var con = new SqlConnection(sqlCon))
            {
                string sql = "Select * from Cereri where Status = 0";

                try
                {
                    vall = (await con.QueryAsync<Cereri>(sql)).ToList();
                }

                catch (Exception ex)
                {
                    exep = ex;
                }
            }
            return (vall, exep);
        }
        public async Task<(bool succ, Exception exx)> EditStatusById(int Id, int daNu)
        {
            Exception exep = null;
            bool succ = false;
            using (var con = new SqlConnection(sqlCon))
            {
                string sql = "UPDATE Cereri Set Status=@daNu Where Id=@Id;";

                try
                {
                    var res = await con.ExecuteScalarAsync<int>(sql, new { daNu = daNu, Id = Id});
                    if (res != 0)
                        succ = true;
                }

                catch (Exception ex)
                {
                    exep = ex;
                    Debug.WriteLine(exep);
                }
            }
            return (succ, exep);
        }

    }
}
