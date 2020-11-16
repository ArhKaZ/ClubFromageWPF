
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
using CsvHelper.TypeConversion;

namespace Model.data
{


    public class DaoFromage
    {
        private DBAL mydbal;
        private DaoPays _daoPays;
        public DaoFromage(DBAL undbal, DaoPays undaopays)
        {
            mydbal = undbal;
            _daoPays = undaopays;
        }

        public void InsertFromage(Fromage unFromage)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values["id"] = unFromage.Id.ToString();
            values["pays_origine_id"] = unFromage.Pays_origine_id.ToString();
            values["nom"] = "'" + unFromage.Nom.Replace("'", "\\'") + "'";
            values["creation"] = "'" + unFromage.Creation.ToString("yyyy'-'MM'-'dd") + "'";
            values["image"] = "'" + unFromage.Image.Replace("'", "\\'") + "'";
            mydbal.Insert("fromage", values);
        }

        public void UpdateFromage(Fromage unFromage)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values["pays_origine_id"] = unFromage.Pays_origine_id.ToString();
            values["nom"] = "'" + unFromage.Nom.Replace("'", "\\'") + "'";
            values["creation"] = "'" + unFromage.Creation.ToString("yyyy'-'MM'-'dd") + "'";
            values["image"] = "'" + unFromage.Image.Replace("'", "\\'") + "'";
            mydbal.Update("fromage", values, "id = " + unFromage.Id);
        }

        public void DeleteFromage(Fromage unFromage)
        {
            mydbal.Delete("Fromage", "id = " + unFromage.Id);
        }
        public void insertfile(string path, string delimiter)
        {
            using (var reader = new StreamReader("fromage.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = delimiter;
                csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower();

                Fromage record = new Fromage();
                var records = csv.EnumerateRecords(record);
                foreach (Fromage fr in records)
                {
                    this.InsertFromage(fr);
                }
            }
        }
        public List<Fromage> SelectAll()
        {
            List<Fromage> lesFro = new List<Fromage>();
            DataTable tableP = mydbal.SelectAll("Pays");
            DataTable tableF = mydbal.SelectAll("Fromage");
            foreach (DataRow dr in tableF.Rows)
            {
                lesFro.Add(new Fromage(
                    (int)dr["id"],
                     (string)dr["nom"],
                    _daoPays.SelectById((int)dr["pays_origine_id"]),                   
                    (DateTime)dr["Creation"],
                    (string)dr["image"]
                    ));
            }
            return lesFro;
        }
        public Fromage SelectByName(string nom)
        {
            DataRow dr = mydbal.SelectByfield("Fromage", "nom = '" + nom + "'").Rows[0];
            return new Fromage(
                (int)dr["id"],
                (string)dr["nom"],
                _daoPays.SelectById((int)dr["pays_origine_id"]),
                (DateTime)dr["creation"],
                (string)dr["image"])
                ;
        }

        public Fromage SelectById(int id)
        {
            DataRow dr = mydbal.DataRowSelectById("Pays", id);
            return new Fromage(
                (int)dr["id"],
                (string)dr["nom"],
                _daoPays.SelectById((int)dr["pays_origine_id"]),
                (DateTime)dr["creation"],
                (string)dr["image"]);
        }


        public void InsertFromCSV(string filename)
        {
            using (var reader = new StreamReader(filename))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<MapFromage>();
                csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower();

                var record = new Fromage();
                IEnumerable<Fromage> records = csv.EnumerateRecords(record);

                foreach (Fromage r in records)
                {
                    Console.WriteLine(r.Id + "-" + r.Nom + "-" + r.Pays_origine_id.Nom);
                    this.InsertFromage(record);
                }
            }

        }
    }
}
