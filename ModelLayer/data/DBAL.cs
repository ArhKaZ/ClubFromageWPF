using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.IO;
using System.Data;

namespace Model.data
{



    public class DBAL
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public DBAL()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "Clubfromages";
            uid = "root";
            password = "5MichelAnnecy";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //Déclaration des erreurs les plus fréquentes : 
                //0: Ne peut pas se connection au serveur 
                //1045: Username et PSWD invalide
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Impossible de se connecter au serveur. Contacter l'administrateur");
                        break;

                    case 1045:
                        Console.WriteLine("Nom d'utilisateur ou Mot de passe invalide, Veuillez recommencez");
                        break;
                }
                return false;
            }
        }

        //Close connection 
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //ExecQuery
        public void ExecQuery(string query)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Insert statement 
        public void Insert(string table, Dictionary<string, string> values)
        {
            string insert = "INSERT INTO " + table + " Values (";
            foreach (var val in values)
            {
                insert += val.Value + ",";
            }
            insert = insert.Substring(0, insert.Length - 1); //Enlève la dernière virgule 
            insert += ")";
            Console.WriteLine(insert);
            //open connection 
            if (this.OpenConnection() == true)
            {
                //créer la commande et assigne la requête et la connection au constructeur
                MySqlCommand cmd = new MySqlCommand(insert, connection);

                try
                {


                    //Executer la commande
                    cmd.ExecuteNonQuery();

                    //Fermer la connection 
                    this.CloseConnection();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        //Update statement
        public void Update(string table, Dictionary<string, string> values, string where)
        {
            string update = "UPDATE " + table + "SET";
            foreach (var val in values)
            {
                update += val.Key + "=" + val.Value + ",";
            }
            update = update.Substring(0, update.Length - 1);
            update += " Where " + where;

            System.Console.WriteLine(update);
            //open connection 
            if (this.OpenConnection() == true)
            {
                //créer la commande
                MySqlCommand cmd = new MySqlCommand();
                //Assigner la requête en utilisant commandtext
                cmd.CommandText = update;
                //Assigner la connection 
                cmd.Connection = connection;

                //executer la commande 
                cmd.ExecuteNonQuery();

                //ferme la connection 
                this.CloseConnection();
            }
        }

        //Delete statement
        public void Delete(string table, string where)
        {
            string delete = "DELETE FROM " + table + " WHERE " + where;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(delete, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select statement 
        public List<string>[] Select(string query)
        {
            string select = "SELECT " + query;

            //Créer la liste pour stocker les résulats
            List<string>[] resultat = new List<string>[2];
            resultat[0] = new List<string>();
            resultat[1] = new List<string>();

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(select, connection);
                //Créer un data reader et execute la commande
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Lire les données et les stocker dans la liste
                while (dataReader.Read())
                {
                    resultat[0].Add(dataReader["id"] + "");
                    resultat[1].Add(dataReader["nom"] + "");
                }

                //close Data Reader
                dataReader.Close();

                this.CloseConnection();

                //returner la liste à afficher
                Console.WriteLine(resultat[0]);
                return resultat;
            }
            else
            {
                return resultat;
            }
        }

        //Count statement 
        public int Count(string query)
        {
            string count = "SELECT Count" + query;
            int Count = -1;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(count, connection);

                //ExecuteScalar return une valeur
                Count = int.Parse(cmd.ExecuteScalar() + "");

                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }

        private DataSet SelectQuery(string query)
        {
            DataSet mydataset = new DataSet();
            MySqlDataAdapter laquery = new MySqlDataAdapter(query, connection);
            laquery.Fill(mydataset);
            return mydataset;
        }

        public DataTable SelectAll(string table)
        {
            return this.SelectQuery("Select * From" + table).Tables[0];
        }

        public DataTable SelectByfield(string table, string fieldTestCondition)
        {
            return this.SelectQuery("select * from " + table + " where " + fieldTestCondition).Tables[0];
        }

        public DataRow DataRowSelectById(string table, int id)
        {
            return this.SelectQuery("select * from " + table + "where id = " + id).Tables[0].Rows[0];
        }
        

      

        //Backup
        public void Backup()
        {
            try
            {
                DateTime Time = DateTime.Now;
                int year = Time.Year;
                int month = Time.Month;
                int day = Time.Day;
                int hour = Time.Hour;
                int minute = Time.Minute;
                int second = Time.Second;
                int millisecond = Time.Millisecond;

                //Save file to C:\ with the current date as a filename
                string path;
                path = "C:\\MySqlBackup" + year + "-" + month + "-" + day +
            "-" + hour + "-" + minute + "-" + second + "-" + millisecond + ".sql";
                StreamWriter file = new StreamWriter(path);


                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysqldump";
                psi.RedirectStandardInput = false;
                psi.RedirectStandardOutput = true;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
                    uid, password, server, database);
                psi.UseShellExecute = false;

                Process process = Process.Start(psi);

                string output;
                output = process.StandardOutput.ReadToEnd();
                file.WriteLine(output);
                process.WaitForExit();
                file.Close();
                process.Close();
            }
            catch (IOException ex)
            {
                Console.WriteLine(); Console.WriteLine("Error , unable to backup!");
            }
        }

        //Restore
        public void Restore()
        {
            try
            {
                //Read file from C:\
                string path;
                path = "C:\\MySqlBackup.sql";
                StreamReader file = new StreamReader(path);
                string input = file.ReadToEnd();
                file.Close();

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysql";
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = false;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
                    uid, password, server, database);
                psi.UseShellExecute = false;


                Process process = Process.Start(psi);
                process.StandardInput.WriteLine(input);
                process.StandardInput.Close();
                process.WaitForExit();
                process.Close();
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error , unable to Restore!");
            }
        }



        private DataSet RQuery(string query)
        {
            DataSet dataset = new DataSet();
            if (this.OpenConnection() == true)
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                adapter.Fill(dataset);
                connection.Close();


            }
            return dataset;
        }

    }

}
