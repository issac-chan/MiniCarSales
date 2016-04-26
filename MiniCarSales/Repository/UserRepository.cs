using MiniCarSales.Models;
using System;
using System.Collections.Generic;

namespace MiniCarSales.Repository
{
    public class UserRepository : BaseRepository,IUserRepository
    {
        public UserRepository(FileConnection fileConnection)
        {
            Connection = fileConnection;
        }

        public bool ValidateUser(string username, string password)
        {
            var lstUser = FileRepository<List<User>>.ReadDataFromFile(TableType.user, Connection.FilePath);

            return lstUser.Exists(x => x.username.Equals(username) && x.password.Equals(password));
        }
    }
}