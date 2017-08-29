using System.Threading.Tasks;
using InfHelper;

namespace DriversBackup.Models
{
    public class DriverInformations
    {
        public DriverInformation FromInfFile(string path)
        {
            var helper = new InfUtil();
            var data = helper.ParseFile(path);
            var version = data["Version"];
            //TODO extract driver info from the inf file
            var result =
                new DriverInformation(version["Provider"].PrimitiveValue, "empty description", version["ClassGuid"].PrimitiveValue, "empty driver id")
                {
                    InfPath = path
                };
            return result;
        }

        public async Task<DriverInformation> FromInfFileAsync(string path)
        {
            return await Task.Run(() => FromInfFile(path));
        }
    }
}