using System.Linq;
using System.Threading.Tasks;
using InfHelper;
using InfHelper.Models;

namespace DriversBackup.Models
{
    public class DriverInformations
    {
        public DriverInformation FromInfFile(string path)
        {
            var helper = new InfUtil();
            var data = helper.ParseFile(path);
            var version = data["Version"];
            //extract driver info from the inf file
            var providerKey = version["Provider"];
            var classGuidKey = version["ClassGuid"];
            var provider = GetPrimitiveValueForKey(data, providerKey);
            var classGuid = GetPrimitiveValueForKey(data, classGuidKey);

            string description = "...";
            var descritpionKey = data["Strings"].Keys.FirstOrDefault(x => x.Id.ToLower().Contains("desc"));
            if (descritpionKey != null)
            {
                description = descritpionKey.KeyValues.First().Value;
            }
            
            var result =
                new DriverInformation(provider, description, classGuid, "empty driver id")
                {
                    InfPath = path
                };
            return result;
        }

        public async Task<DriverInformation> FromInfFileAsync(string path)
        {
            return await Task.Run(() => FromInfFile(path));
        }

        private string GetPrimitiveValueForKey(InfData data, Key key)
        {
            if (key.KeyValues.Any())
            {
                var first = key.KeyValues.First();
                //dynamic
                if (first.IsDynamic)
                {
                    return data.FindKeyById(first.DynamicKeyId) //find dynamic key
                        .First(x => x.KeyValues.All(v => !v.IsDynamic)) // that has not a dynamic value
                        .KeyValues.First().Value; //return the first text value
                }
                //static
                else
                {
                    return key.PrimitiveValue;
                }
            }
            else
            {
                return "";
            }
        }
    }
}