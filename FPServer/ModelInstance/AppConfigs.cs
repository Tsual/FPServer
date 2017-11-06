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

        }

        private static AppConfigs _Current = new AppConfigs();
        public static AppConfigs Current { get => _Current; set => _Current = value; }

        public string this[string key]
        {
            get
            {
                return _QueryConfig(key);
            }
            set
            {
                _SaveConfig(key, value);
            }
        }

        private string _QueryConfig(string key)
        {
            if (key == "" || key == null) return "";
            Models.AppDbContext db = new Models.AppDbContext();
            db.Database.EnsureCreated();
            var items = from t in db.M_AppConfigModels
                        where t.Key == key
                        select t;
            return items.Count() > 0 ? items.ElementAt(0).Value : "";
        }

        private void _SaveConfig(string key, string value)
        {
            if (key == "" || key == null) return;
            Models.AppDbContext db = new Models.AppDbContext();
            db.Database.EnsureCreated();
            var items = from t in db.M_AppConfigModels
                        where t.Key == key
                        select t;
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

    }
}
