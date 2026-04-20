using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace BobMapper.Services
{
    internal static class InternalNameSevice
    {
        internal static string GetInternalName(string resourceName)
        {
            SqliteConnection textureConnection = new("Data Source=Data/TextureManifest.sqlite");
            textureConnection.Open();
            var selectTexturesCommand = textureConnection.CreateCommand();
            selectTexturesCommand.CommandText = $"SELECT InternalName FROM Textures WHERE ResourceName LIKE '%{resourceName}%'";
            string internalName = null;
            var reader = selectTexturesCommand.ExecuteReader();
            while (reader.Read())
            {
                internalName = reader.GetString(0);
            }
            if (internalName == null)
            {
                throw new Exception("Texture not found");
            }
            return internalName;
        }
    }
}
