﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTBQuestGame.Models
{
    public class Character : ObservableObject
    {
        #region ENUMERABLES

        public enum RaceType
        {
            Human,
            Thorian,
            Xantorian
        }

        #endregion

        #region FIELDS

        protected int _id;
        protected string _name;
        //protected int _locationId;
        protected int _age;
        protected RaceType _race;

        protected Random random = new Random();

        #endregion

        #region PROPERTIES

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        //public int LocationId
        //{
        //    get { return _locationId; }
        //    set { _locationId = value; }
        //}

        public int Age
        {
            get { return _age; }
            set { _age = value; }
        }

        public RaceType Race
        {
            get { return _race; }
            set { _race = value; }
        }

        #endregion

        #region CONSTRUCTORS

        public Character()
        {

        }

        public Character(int id, string name, RaceType race)
        {
            _name = name;
            _race = race;
            //_locationId = locationId;
        }

        #endregion

        #region METHODS

        public virtual string DefaultGreeting()
        {
            return $"Hello, my name is {_name} and I am a {_race}.";
        }

        #endregion
    }
}
