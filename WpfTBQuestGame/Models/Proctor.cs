﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTBQuestGame.Models
{
    public class Proctor : Npc, ISpeak, IBattle
    {
        Random r = new Random();

        private const int DEFENDER_DAMAGE_ADJUSTMENT = 10;
        private const int MAXIMUM_RETREAT_DAMAGE = 10;

        public List<string> Messages { get; set; }
        public int SkillLevel { get; set; }
        public BattleModeName BattleMode { get; set; }
        public Weapon CurrentWeapon { get; set; }

        protected override string InformationText()
        {
            return $"{Name} - {Description}";
        }

        public Proctor()
        {

        }

        public Proctor(
            int id,
            string name,
            RaceType race,
            string description,
            List<string> messages,
            int skillLevel,
            Weapon currentWeapon)
            : base(id, name, race, description)
        {
            Messages = messages;
            SkillLevel = skillLevel;
            CurrentWeapon = currentWeapon;
        }

        
        public string Speak()
        {
            if (this.Messages != null)
            {
                return GetMessage();
            }
            else
            {
                return "";
            }
        }

       
        private string GetMessage()
        {
            int messageIndex = r.Next(0, Messages.Count());
            return Messages[messageIndex];
        }

        #region BATTLE METHODS

      
        public int Attack()
        {
            int hitPoints = random.Next(CurrentWeapon.MinimumDamage, CurrentWeapon.MaximumDamage) * SkillLevel;

            if (hitPoints <= 100)
            {
                return hitPoints;
            }
            else
            {
                return 100;
            }
        }

        public int Defend()
        {
            int hitPoints = (random.Next(CurrentWeapon.MinimumDamage, CurrentWeapon.MaximumDamage) * SkillLevel) - DEFENDER_DAMAGE_ADJUSTMENT;

            if (hitPoints >= 0 && hitPoints <= 100)
            {
                return hitPoints;
            }
            else if (hitPoints > 100)
            {
                return 100;
            }
            else
            {
                return 0;
            }
        }

        public int Retreat()
        {
            int hitPoints = SkillLevel * MAXIMUM_RETREAT_DAMAGE;

            if (hitPoints <= 100)
            {
                return hitPoints;
            }
            else
            {
                return 100;
            }
        }

        #endregion

    }
}
