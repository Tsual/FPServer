﻿using FPServer.Attribute;
using FPServer.Enums;
using FPServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPServer.ModelInstance
{

    public class AppConfigs : IAppConfigs
    {
        private AppConfigs()
        {
            foreach (var t in Enum.GetValues(typeof(Enums.AppConfigEnum)))
            {
                foreach (var value in t.GetType().GetField(t.ToString()).GetCustomAttributes(typeof(AppConfigDefaultAttribute), false))
                {
                    if (!this.ContainsKey((Enums.AppConfigEnum)t))
                        this[(Enums.AppConfigEnum)t] = ((AppConfigDefaultAttribute)value).DefaultValue;
                }
            }
        }

        private static AppConfigs _Current = new AppConfigs();
        public static AppConfigs Current { get => _Current; set => _Current = value; }

        public string this[AppConfigEnum key]
        {
            get
            {
                return _QueryConfig(key.ToString());
            }
            set
            {
                _SaveConfig(key.ToString(), value);
            }
        }

        private string _QueryConfig(string key)
        {
            if (key == "" || key == null) return "";
            Models.AppDbContext db = new Models.AppDbContext();
            db.Database.EnsureCreated();
            var items = (from t in db.M_AppConfigModels
                        where t.Key == key
                        select t).ToList();
            return items.Count() > 0 ? items.ElementAt(0).Value : "";
        }

        private void _SaveConfig(string key, string value)
        {
            if (key == "" || key == null) return;
            Models.AppDbContext db = new Models.AppDbContext();
            db.Database.EnsureCreated();
            var items = (from t in db.M_AppConfigModels
                        where t.Key == key
                        select t).ToList();
            if (items.Count() > 0)
            {
                items.ElementAt(0).Value = value;
                db.Entry(items.ElementAt(0)).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                var t = new Models.AppConfigModel()
                {
                    Key = key,
                    Value = value
                };
                db.Entry(t).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            }
            db.SaveChanges();
        }

        public bool ContainsKey(AppConfigEnum Key)
        {
            string key = Key.ToString();
            if (key == "" || key == null) return false;
            Models.AppDbContext db = new Models.AppDbContext();
            db.Database.EnsureCreated();
            var items = (from t in db.M_AppConfigModels
                         where t.Key == key
                         select t).ToList();
            return items.Count() > 0;
        }

    }
}
