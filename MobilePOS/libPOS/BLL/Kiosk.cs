﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using libPOS.DAL;

namespace libPOS.BLL
{
    public class Kiosk
    {
        public int KioskID { get; set; }
        public string KioskCode { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int ClassID { get; set; }
        public string Remarks { get; set; }

        public String _Supervisor { get; set; }

        public Kiosk()
        {
            this.KioskID = 0;
            this.KioskCode = "";
            this.Name = "";
            this.Location = "";
            this.ClassID = 0;
            this.Remarks = "";

            this._Supervisor = "";
        }

        public void Bind(DataRow row)
        {
            if (row != null)
            {
                this.KioskID = Utils.convInt("KioskID", row);
                this.KioskCode = Utils.convString("KioskCode", row);
                this.Name = Utils.convString("KioskName", row);
                this.Location = Utils.convString("Location", row);
                this.Remarks = Utils.convString("Remarks", row);
                this.ClassID = Utils.convInt("ClassID", row);
                this.KioskCode = Utils.convString("KioskCode", row);

                this._Supervisor = Utils.convString("Supervisor", row);
            }
        }

        public static List<Kiosk> GetAllKiosk()
        {
            var dal = new KioskDAL();
            var collection = new List<Kiosk>();

            foreach(DataRow row in dal.GetAllKiosk().Rows){
                //
                var instance = new Kiosk();
                instance.Bind(row);

                collection.Add(instance);
            }

            return collection;
        }

        public static Kiosk GetKioskByID(int id)
        {
            List<Kiosk> col = GetAllKiosk();

            var ins = col.Where(o=> o.KioskID == id).ToList();

            return ins[0];
        }

        public static void InsertKiosk(string name, string location, string remarks, int classid, string kioskcode, string admin)
        {
            var dal = new KioskDAL();
            dal.InsertKiosk(name, location, remarks, classid, kioskcode, admin);
        }

        public static Kiosk SelectKioskByID(int id)
        {
            var dal = new KioskDAL();
            DataRow row = dal.SelectKioskByID(id);
            //
            var ins = new Kiosk();
            ins.Bind(row);

            return ins;
        }

        public static void UpdateKiosk(int kioskid, string name, string location, string remarks, int classid, string admin)
        {
            var dal = new KioskDAL();
            dal.UpdateKiosk(kioskid, name, location, remarks, classid, admin);
        }
    }
}
