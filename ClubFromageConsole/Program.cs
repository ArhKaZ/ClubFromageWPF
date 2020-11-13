using MySql.Data.MySqlClient;
using System.IO;
using System;
using Model.buisness;
using Model.data;
using System.ComponentModel.Design;
using System.Data;


namespace ClubFromageConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            DBAL MaBDD = new DBAL();//nouvel instance DBAL
            Console.WriteLine("Bonjour !");
            DaoPays daoP = new DaoPays(MaBDD); // nouvel instance dao avec un DBAL
            DaoFromage daoF = new DaoFromage(MaBDD, daoP);
            daoP.InsertFromCSV()
           // daoF.MainCSV();

            //------------------------------------------------------------
            //selectbyID dao fromage
            //Console.WriteLine(daoF.SelectByID(1).Nom);
            //-----------------------------------------------------------
            //SelectByName dao fromage
            //Console.WriteLine(daoF.SelectByName("fromage de chèvre").Id);
            //-----------------------------------------------------------
            //list Fromage selectALl
            daoF.SelectAll();
            foreach (Fromage item in daoF.SelectAll())
            {
                Console.WriteLine(item.Nom);
            }
            //----------------------------------------------------
            //selectByID
            //Console.WriteLine(daoP.selectByID(110).Nom);
            //---------------------------------------------------------------------------
            //selectbyname
            //Console.WriteLine(daoP.selectByName("vietnam").Nom);
            //-----------------------------------------------------------------------
            //list Pays selectALL
            //foreach (Pays unpays in daoP.SelectAll())
            //{
            // Console.WriteLine(unpays.Nom);
            //}

            //Datarow SelectbyID
            // DataRow DataR = MaBDD.SelectByID("pays", 165);
            //  Console.WriteLine(DataR["nom"]);

            //--------------------------------------------------------------------------
            //DataTable Selectbyfield
            //foreach (DataRow dataR in MaBDD.SelectByField("pays", "id = 10").Rows)
            //{
            //    Console.WriteLine(dataR["nom"] + " " + dataR["id"]);
            //}
            //--------------------------------------------------------------------------
            //DataTable selectAll
            //foreach  (DataRow dataR in MaBDD.SelectALL("pays").Rows)
            //{
            //    Console.WriteLine(dataR["nom"]+" "+ dataR["id"]);
            //}
            //--------------------------------------------------------------------------
            //DataSet RQUERY
            //DataSet dataS = MaBDD.RQuery("Select * from pays ;"); //instancie un objet dataset (bdd) grace a ma MaBDD.query
            //foreach (DataRow Undata in dataS.Tables[0].Rows)
            //{
            //    //pour un objet datarow qui sera dans Undata dans la tables dataS a chaque ligne

            //        Console.WriteLine(Undata["id"]+ " "+ Undata["nom"]);



            //}





            //daoP.MainCSV();


            // Pays p3 = new Pays(3,"Chine");
            // Fromage fromage1 = new Fromage(1, p3, "tome", "2010-03-19", "ttt");

            //daoF.Insert(fromage1);

            //daoP.Insert(p3);
            //daoP.Update(p3);
            //daoP.Delete(p3);

            Console.WriteLine("------- FIN -------");
        }

    }
}
