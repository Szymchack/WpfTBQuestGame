using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTBQuestGame.Models;

namespace WpfTBQuestGame.DataLayer
{
    public static class GameData
    {
        public static Player PlayerData()
        {
            return new Player()
            {
                Id = 1,
                Name = "Annette",
                Age = 43,
                JobTitle = Player.JobTitleName.Explorer,
                Race = Character.RaceType.Human,
                Health = 100,
                Lives = 3,
                ExperiencePoints = 10,
                SkillLevel = 5,
                Inventory = new ObservableCollection<GameItemQuantity>()
                {
                    new GameItemQuantity(GameItemById(1002), 1),
                    new GameItemQuantity(GameItemById(2001), 5),
                }
            };
        }

        private static GameItem GameItemById(int id)
        {
            return StandardGameItems().FirstOrDefault(i => i.Id == id);
        }

        private static Npc NpcById(int id)
        {
            return Npcs().FirstOrDefault(i => i.Id == id);
        }

        public static GameMapCoordinates InitialGameMapLocation()
        {
            return new GameMapCoordinates() { Row = 0, Column = 0 };
        }

        public static Map GameMap()
        {
            int rows = 3;
            int columns = 4;

            Map gameMap = new Map(rows, columns);

            gameMap.StandardGameItems = StandardGameItems();

            //
            // row 1
            //
            gameMap.MapLocations[0, 0] = new Location()
            {
                Id = 1,
                Name = "NMC Base Camp",
                Description = "The Base Camp for the Archeological Institute In Traverse City Michigan " +
                "The ongoing NMC Project is a privately funded dig in what remains of the long lost NMC College" +
                "The project is headed by Dr. Mark Holley.",
                Accessible = true,
                ModifiyExperiencePoints = 10,
                Message = "\tYou have been hired by the The National Archeology Foundation to participate in its latest endeavor, the " +
                "NMC Project. Your mission is to search the ruins of the newly discovered Northwestern Michigan College " +
                "You must start at the beginning and follow the clues that will lead you to ground zero.  The place where Artificial " +
                "Intelligence took over and ultimately distroyed this institute of higher education."
            };
            gameMap.MapLocations[0, 1] = new Location()
            {
                Id = 2,
                Name = "Scholors Hall",
                Description = "The Scholors Hall was the area in which the primative language of the time was taught and refined" +
                "On the west end of the campus it is the building that is yielding the most intact artifacts because it was the furthest from the, " + "" +
                "detonation site.  " +
                "Here you must find the manuscript that will unlock the language barrier and help find the lab of the Great John Velis",
                Accessible = true,
                ModifiyExperiencePoints = 10,
                GameItems = new ObservableCollection<GameItemQuantity>
                {
                    new GameItemQuantity(GameItemById(4002), 1)
                },
                Npcs = new ObservableCollection<Npc>()
                {
                    NpcById(1001),
                }
            };

            gameMap.MapLocations[1, 1] = new Location()
            {
                Id = 3,
                Name = "Tanis Building",
                Description = "The Tanis building at one time was a major site for mathmatics and astrology.  It is now been added on to" +
                "and down a short hallway will connect with another building.  You must find the Dobek Conjecture to follow the stars to " +
                "the location of that hallway as well as a chart to find the office of the Great John Velis " +
                "You must find the Dobek Conjecture if you have any hope of making it any further on this project.  Pick up anything else you " +
                "find that you might need."
                ,
                Accessible = true,
                ModifiyExperiencePoints = 10,
                GameItems = new ObservableCollection<GameItemQuantity>
                {
                    new GameItemQuantity(GameItemById(3001), 1),
                    new GameItemQuantity(GameItemById(1002), 1),
                    new GameItemQuantity(GameItemById(4001), 1)
                },
                Npcs = new ObservableCollection<Npc>()
                {
                    NpcById(1002),
                    NpcById(2001)
                }
            };
            gameMap.MapLocations[1, 2] = new Location()
            {
                Id = 4,
                Name = "Math and Science Building",
                Description = "The addition to the Math and Science building added great prestige to the college " +
                "here is where a distant relative of our head Anthropologist Dr. Holley was said to have worked. " +
                "Here we will find many artifacts that were indeed artifacts when the building was in use.  We must" +
                "sift through the artifacts of the age of the college as well as the artifacts that were owned by the college." +
                "Here you must find the machine built by the late Dr. Holley that helped divers recover from deep water dives more quickly" +
                "if you hope to go any further.",
                Accessible = false,
                RequiredRelicId = 4001,
                ModifiyExperiencePoints = 50,
                ModifyLives = -1,
                RequiredExperiencePoints = 40
            };

            //
            // row 3
            //
            gameMap.MapLocations[2, 0] = new Location()
            {
                Id = 5,
                Name = "The Clock Tower",
                Description = "The Clock Tower was said to divide the campus exactly in two.  No one knows if that is true or not " +
                "This is an area where all ages and nationalities would meet and have dances as well as hang out and talk." +
                "You purchase a blue potion in a thin, clear flask, drink it and receive 50 points of health.",
                Accessible = false,
                ModifiyExperiencePoints = 20,
                ModifyHealth = 50,
                Message = "Traveler, our telemetry places you at the Xantoria Market. We have reports of local health potions."
            };
            gameMap.MapLocations[2, 1] = new Location()
            {
                Id = 6,
                Name = "The James Becket Building",
                Description = "The James Becket building was the site of the massive explosion that distroyed the college. " +
                "Here is where we are hoping to find the reason the Artifical Intelligence Project went off course and the" +
                "robots turned against their creators and ultimately distroyed themselves and the college..",
                Accessible = true,
                ModifiyExperiencePoints = 10,
                GameItems = new ObservableCollection<GameItemQuantity>()
                {
                    new GameItemQuantity(GameItemById(2001), 10)
                }
            };
            return gameMap;
        }

        public static List<GameItem> StandardGameItems()
        {
            return new List<GameItem>()
            {
                new Weapon(1001, "Lazer Pointer", 75, 1, 4, "The Lazer Pointer is a long cylindrical form that has a strong red light when you depress the button that will blind your foe.", 10),
                new Weapon(1002, "Phaser", 250, 1, 9, "Phasers are common and versatile phased array pulsed energy projectile weapons.", 10),
                new Treasure(2001, "Gold Coin", 10, Treasure.TreasureType.Coin, "24 karat gold coin", 1),
                new Treasure(2020, "Small Diamond", 50, Treasure.TreasureType.Jewel, "A small pea-sized diamond of various colors.", 1),
                new Treasure(2030, "Kalzonian Manuscript", 10, Treasure.TreasureType.Manuscript, "Reportedly stolen during the Zantorian raids of of the 4th dynasty, it is said to contain information about early galactic technologies.", 5),
                new Potion(3001, "Sodium Chloride", 5, 40, 1, "Rare potion due to the dangers of procurement. Add 40 points of health.", 5),
                new Relic(4001, "Dobek Conjecture", 5, "Conjured by the Maji Dobek, it opens many doors.", 5, "You have opened the hallway to the Math building.", Relic.UseActionType.OPENLOCATION),
                new Relic(4002, "Manuscript of Chu", 5, "A document that will unlock the language and let you out of the building..", 5, "Sliding the silver ribbons, you feel a sharp pain in your left temple and quickly die.", Relic.UseActionType.KILLPLAYER)
            };
        }

        public static List<Npc> Npcs()
        {
            return new List<Npc>()
            {
                new Proctor()
                {
                    Id = 2001,
                    Name = "Proctor Mark Talismen",
                    Race = Character.RaceType.Human,
                    Description = "A short, stocky man who had a strong look of determination and a disposition to match.",
                    Messages = new List<string>()
                    {
                        "Stop and state your purpose.",
                        "I have been ordered to kill all who enter.",
                        "Leave now or bear the consequences."
                    },
                   SkillLevel = 3,
                   CurrentWeapon = GameItemById(1001) as Weapon
                },

                new Civilian()
                {
                    Id = 1001,
                    Name = "Judy Chu",
                    Race = Character.RaceType.Human,
                    Description = "A tall women of respectable stature.",
                    Messages = new List<string>()
                    {
                        "Hello, my name is Ms Smith. I noticed you when you arrived.",
                        "Excuse me, but are you looking for something."
                    }
                },

                new Civilian()
                {
                    Id = 1002,
                    Name = "Astoria Mantisa",
                    Race = Character.RaceType.Xantorian,
                    Description = "A tall women of respectable stature.",
                    Messages = new List<string>()
                    {
                        "Excuse me, but my kind does not speak with your kind."
                    }
                }
            };
        }
    }
}