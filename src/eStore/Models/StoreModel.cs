using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace eStore.Models
{
    public class StoreModel
    {
        private AppDbContext _db;
        public StoreModel(AppDbContext context)
        {
            _db = context;
        }
        public List<Store> GetThreeClosetStores(float? lat, float? lng)
        {
            List<Store> storeDetails = null;
            try
            {
                var latParam = new SqlParameter("@lat", lat);
                var lngParam = new SqlParameter("@lng", lng);
                var query = _db.Stores.FromSql("dbo.pGetThreeClosestStores @lat = {0}, @lng = {1}", lat, lng);
                storeDetails = query.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return storeDetails;
        }
    }
}