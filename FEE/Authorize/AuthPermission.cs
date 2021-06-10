using FEE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FEE.Authorize
{
    public static class AuthPermission
    {
        public static List<string> GetProfileService(int userId)
        {
            FEEDbContext _db = new FEEDbContext();
            var query = from u in _db.Users
                        join r in _db.Roles on u.RoleId equals r.RoleId
                        join p in _db.Permissions on r.RoleId equals p.RoleId
                        join f in _db.Functions on p.FunctionId equals f.FunctionId
                        join c in _db.Commands on p.CommandId equals c.CommandId
                        where u.Id == userId
                        select f.FunctionId + "_" + c.CommandId;
            var permissions = query.Distinct().ToList();
            return permissions;
        }
    }
}