using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebShop
{
    public class DatabaseProduct
    {
        public int Id { get; private set; }
        public string Model { get; private set; }
        public string Manufacturer { get; private set; }
        public string OS { get; private set; }
        public float ScreenSize { get; private set; }
        public int Memory { get; private set; }
        public int RAM { get; private set; }
        public string Processor { get; private set; }
        public int Cores { get; private set; }
        public float Clock { get; private set; }
        public int Camera { get; private set; }
        public bool SDCard{ get; private set; }
        public bool DualSIM { get; private set; }
        public float Price { get; private set; }

        public DatabaseProduct(DataRowView drv)
        {
            Id = Convert.ToInt32(drv["Id"]);
            Manufacturer = drv["Manufacturer"].ToString();
            Model = drv["Model"].ToString();
        }
    }
}