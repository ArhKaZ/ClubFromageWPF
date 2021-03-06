﻿
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Bcpg;
using System;
using System.IO;
using System.Globalization;
using CsvHelper;
using Model.data;
using Model.buisness;
using System.Collections.Generic;
using System.Data;

namespace Model.data
{

    public class DaoPays
    {
        private DBAL mydbal;
        public DaoPays(DBAL undbal)
        {
            mydbal = undbal;
        }

        public void InsertPays(Pays unPays)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values["id"] = unPays.Id.ToString();
            values["nom"] = "'" + unPays.Nom.Replace("'", "\\'") + "'";
            mydbal.Insert("pays", values);
        }

        public void UpdatePays(Pays unPays)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values["Nom"] = "'" + unPays.Nom.Replace("'", "\\'") + "'";
            mydbal.Update("pays", values, "Id = " + unPays.Id);
        }

        public void DeletePays(Pays unPays)
        {
            mydbal.Delete("pays", "Id = " + unPays.Id);
        }

        public void InsertFromCSV(string filename)
        {
            using (var reader = new StreamReader(filename))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<MapFromage>();
                csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower();

                var record = new Pays();
                IEnumerable<Pays> records = csv.EnumerateRecords(record);

                foreach (Pays r in records)
                {
                    Console.WriteLine(r.Id + "-" + r.Nom);
                    this.InsertPays(record);
                }
            }

        }

        public List<Pays> SelectAll()
        {
            List<Pays> lesPays = new List<Pays>();
            foreach (DataRow dr in mydbal.SelectAll("Pays").Rows)
            {
                lesPays.Add(new Pays((int)dr["id"], (string)dr["nom"]));
            }
            return lesPays;
        }
        public Pays SelectByName(string nom)
        {
            DataRow dr = mydbal.SelectByfield("Pays", "nom like '" + nom + "'").Rows[0];
            return new Pays((int)dr["id"], (string)dr["nom"]);
        }

        public Pays SelectById(int id)
        {
            DataRow dr = mydbal.DataRowSelectById("Pays", id);
            return new Pays((int)dr["id"], (string)dr["nom"]);
        }

    }
}