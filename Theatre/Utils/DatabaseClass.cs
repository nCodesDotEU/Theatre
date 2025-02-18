﻿using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theatre.Instances;

namespace Theatre.Utils
{
    class DatabaseClass
    {

        public static bool OpenConnection()
        {
            try
            {
                ProgramVariables.Connection.Open();
                Debug.WriteLine("MySQL connection established!");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public static void LoadDataIntoCache()
        {

            var command = new MySqlCommand($"SELECT * FROM Actor;", ProgramVariables.Connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ActorInstance actor = new ActorInstance(reader.GetInt32(0), reader.GetString(1), reader.GetChar(2), reader.GetString(3), reader.GetString(4), reader.GetDouble(5));
                ProgramVariables.Actors.Add(actor);
            }
            reader.Close();

            command = new MySqlCommand("SELECT * FROM Play;", ProgramVariables.Connection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                PlayInstance play = new PlayInstance(reader.GetInt32(0), reader.GetDateTime(1), reader.GetInt32(2), reader.GetInt32(3));
                ProgramVariables.Plays.Add(play);
            }
            reader.Close();

            command = new MySqlCommand("SELECT * FROM Production;", ProgramVariables.Connection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                ProductionInstance production = new ProductionInstance(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetDateTime(3), reader.GetDateTime(4), reader.GetString(5));
                ProgramVariables.Productions.Add(production);
            }
            reader.Close();

        }

        public static int AddActor(string fullname, char sex, string email, string phone, double salary)
        {
            var command = new MySqlCommand($"INSERT INTO Actor(Fullname, Sex, Email, Phone, Salary) VALUES('{fullname}', '{sex}', '{email}', '{phone}', {salary});", ProgramVariables.Connection);
            command.ExecuteNonQuery();
            command = new MySqlCommand($"SELECT ID FROM Actor WHERE Fullname='{fullname}' AND Sex='{sex}' AND Email='{email}' AND Phone='{phone}' AND Salary={salary};", ProgramVariables.Connection);
            var reader = command.ExecuteReader(); 
            reader.Read();
            int id = reader.GetInt32(0);
            reader.Close();
            return id;
        }

        public static int AddProduction(string name, string author, DateTime premier, DateTime denier, string description)
        {
            var command = new MySqlCommand($"INSERT INTO Production(Name, Author, PremierDate, DenierDate, Description) VALUES('{name}', '{author}', '{premier.ToString("yyyy-MM-dd HH:mm:ss")}', '{denier.ToString("yyyy-MM-dd HH:mm:ss")}', '{description}');", ProgramVariables.Connection);
            command.ExecuteNonQuery();
            command = new MySqlCommand($"SELECT ID FROM Production WHERE Name='{name}' AND Author='{author}' AND PremierDate='{premier.ToString("yyyy-MM-dd HH:mm:ss")}' AND DenierDate='{denier.ToString("yyyy-MM-dd HH:mm:ss")}' AND Description='{description}';", ProgramVariables.Connection);
            var reader = command.ExecuteReader();
            reader.Read();
            int id = reader.GetInt32(0);
            reader.Close();
            return id;
        }

        public static int AddPlay(DateTime playDate, int participate, int production_id)
        {
            var command = new MySqlCommand($"INSERT INTO Play(PlayDate, Participate, Production_ID) VALUES('{playDate.ToString("yyyy-MM-dd HH:mm:ss")}', {participate}, {production_id});", ProgramVariables.Connection);
            command.ExecuteNonQuery();
            command = new MySqlCommand($"SELECT ID FROM Play WHERE PlayDate='{playDate.ToString("yyyy-MM-dd HH:mm:ss")}' AND Participate={participate} AND Production_ID={production_id};", ProgramVariables.Connection);
            var reader = command.ExecuteReader();
            reader.Read();
            int id = reader.GetInt32(0);
            reader.Close();
            return id;
        }

        public static void RemoveActor(int id)
        {
            var command = new MySqlCommand($"DELETE FROM Actor WHERE ID={id};", ProgramVariables.Connection);
            command.ExecuteNonQuery();
        }

        public static void RemoveProduction(int id)
        {
            var command = new MySqlCommand($"DELETE FROM Production WHERE ID={id};", ProgramVariables.Connection);
            command.ExecuteNonQuery();
        }

        public static void RemovePlay(int id)
        {
            var command = new MySqlCommand($"DELETE FROM Play WHERE ID={id};", ProgramVariables.Connection);
            command.ExecuteNonQuery();
        }

        public static void UpdateActor(int id, string fullname, char sex, string email, string phone, double salary)
        {
            var command = new MySqlCommand($"UPDATE Actor SET Fullname='{fullname}', Sex='{sex}', Email='{email}', Phone='{phone}', Salary={salary} WHERE ID={id};", ProgramVariables.Connection);
            command.ExecuteNonQuery();
        }

        public static void UpdateProduction(int id, string name, string author, DateTime premier, DateTime denier, string description)
        {
            var command = new MySqlCommand($"UPDATE Production SET Name='{name}', Author='{author}', PremierDate='{premier.ToString("yyyy-MM-dd HH:mm:ss")}', DenierDate='{denier.ToString("yyyy-MM-dd HH:mm:ss")}', Description='{description}' WHERE ID={id};", ProgramVariables.Connection);
            command.ExecuteNonQuery();
        }

        public static void UpdatePlay(int id, DateTime playTime, int participate, int production_id)
        {
            var command = new MySqlCommand($"UPDATE Play SET PlayDate='{playTime.ToString("yyyy-MM-dd HH:mm:ss")}', Participate={participate}, Production_ID={production_id} WHERE ID={id}", ProgramVariables.Connection);
            command.ExecuteNonQuery();
        }

    }
}