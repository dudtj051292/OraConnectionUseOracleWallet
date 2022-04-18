using Oracle.ManagedDataAccess.Client;
using PInvoke;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OraConnectionUseOracleWallet
{
    class Program
    {
        static void Main(string[] args)
        {

            // This sample demonstrates how to use ODP.NET Core Configuration API

            // Add connect descriptors and net service names entries.
            OracleConfiguration.OracleDataSources.Add("orcl_high", "(description= (retry_count=20)(retry_delay=3)(address=(protocol=tcps)(port=1522)(host=adb.ap-seoul-1.oraclecloud.com))(connect_data=(service_name=g21d97f7712585e_orcl_high.adb.oraclecloud.com))(security=(ssl_server_cert_dn=\"CN = adb.ap - seoul - 1.oraclecloud.com, OU = Oracle ADB SEOUL, O = Oracle Corporation, L = Redwood City, ST = California, C = US\")))");

            // Set default statement cache size to be used by all connections.
            OracleConfiguration.StatementCacheSize = 25;

            // Disable self tuning by default.
            OracleConfiguration.SelfTuning = false;

            // Bind all parameters by name.
            OracleConfiguration.BindByName = true;

            // Set default timeout to 60 seconds.
            OracleConfiguration.CommandTimeout = 60;

            // Set default fetch size as 1 MB.
            OracleConfiguration.FetchSize = 1024 * 1024;


            // Set network properties

            OracleConnection orclCon = null;
            try
            {
                // Open a connection
                orclCon = new OracleConnection("user id=PROJ_BLD; password=aldwlTkfkd1!; data source=orcl_high");
                orclCon.WalletLocation = @"C:\Users\ussoft\OneDrive\OracleCloud\Wallet_orcl";
                orclCon.Open();

                // Execute simple select statement that returns first 10 names from EMPLOYEES table
                OracleCommand orclCmd = orclCon.CreateCommand();
                orclCmd.CommandText = "select ST_ID, ST_PASSWORD from BLD_USER WHERE ST_ID = 'ADMIN1'";
                OracleDataReader rdr = orclCmd.ExecuteReader();
                string myPassword = "";
                while (rdr.Read())
                {
                     myPassword = rdr.GetString(1);
                }
                string mySalt = BCrypt.Net.BCrypt.GenerateSalt();
                string myHash = BCrypt.Net.BCrypt.HashPassword(myPassword, mySalt);
                bool isMatch = false;

                while (!isMatch)
                {
                    string str = Console.ReadLine();
                    isMatch = BCrypt.Net.BCrypt.Verify(str, myHash);
                    Console.WriteLine("isMatch = " + isMatch.ToString());

                }
                
                Console.ReadLine();

                rdr.Dispose();
                orclCmd.Dispose();
            }
            finally
            {
                // Close the connection
                if (null != orclCon)
                    orclCon.Close();
            }

            //try
            //{
            //    Console.WriteLine("try Connect...");
            //    conn.Open();
            //    if (conn.State == ConnectionState.Open)
            //    {
            //        Console.WriteLine("Oracle Connected...");
            //        string sqlString = "SELECT * FROM BLD_USER ";
            //        OracleDataAdapter adapter = new OracleDataAdapter(sqlString, conn);
            //        DataTable dt = new DataTable();
            //        adapter.Fill(dt);

            //        conn.Dispose();
            //        conn.Close();
            //    }
            //    else
            //    {
            //        Console.WriteLine("Oracle Connect failure");
            //    }
            //}
            //catch (Exception ex) {
            //    Console.WriteLine(ex.Message);
            //    Console.WriteLine("conn.State : " + conn.State);
            //}

            Console.WriteLine("아무키나 입력시 꺼짐");
            Console.ReadLine();
        }

    }
}
