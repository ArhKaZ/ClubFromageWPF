using CsvHelper.Configuration;
using Model.buisness;
using System;
using System.Collections.Generic;
using System.Text;
using Model.data;

namespace Model.data
{
    class MapFromage : ClassMap<Fromage>
    {
        public MapFromage()
        {

            Map(m => m.Id);
            Map(m => m.Nom);
            Map(m => m.Pays_origine_id).TypeConverter<Paysconverter<Pays>>();
        }
    }
}
