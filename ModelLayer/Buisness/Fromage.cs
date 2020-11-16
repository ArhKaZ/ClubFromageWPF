using System;

namespace Model.buisness
{
    public class Fromage
    {
        private int _id;
        private string _nom;
        private Pays _pays_origine_id;
        private DateTime _creation;
        private string _image;

        public Fromage(int id, string nom, Pays pays_origine_id, DateTime creation, string image)
        {
            this._id = id;
            this._nom = nom;
            this._pays_origine_id = pays_origine_id;
            this._creation = creation;
            this._image = image;
        }
        public Fromage()
        {


        }

        public int Id { get => _id; set => _id = value; }
        public Pays Pays_origine_id { get => _pays_origine_id; set => _pays_origine_id = value; }
        public string Nom { get => _nom; set => _nom = value; }
        public DateTime Creation { get => _creation; set => _creation = value; }
        public string Image { get => _image; set => _image = value; }
    }
}
