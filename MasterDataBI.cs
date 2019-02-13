using SusheelRawat.DA;
using SusheelRawat.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace SusheelRawat.BI
{
    public class MasterDataBI:IMasterDataBI
    {
        IMasterDataDA IMasterDataDA;
        public MasterDataBI(IMasterDataDA _IMasterDataDA)
        {
            this.IMasterDataDA = _IMasterDataDA;
        }

        public List<VMHobbies> GetHobbiesBI()
        {
            return IMasterDataDA.GetHobbies().Select(obj => new VMHobbies()
            {
<<<<<<< HEAD
                HobbyID=obj.HobbyID,
                Hobby=obj.Hobby,
                x=9,
                y=4
=======
                HobbyID=1,
                //Hobby=obj.Hobby
>>>>>>> 21029030294f719c2a42324e792505a428b4fcfb
            }).ToList();
        }


        public List<VMCountry> GetCountryBI()
        {
            return IMasterDataDA.GetCountry().Select(obj => new VMCountry()
            {
                CountryId=obj.CountryID,
                CountryName=obj.CountryName
            }).ToList();
        }


        public List<VMSate> GetSateBI(int CountryID)
        {
            return IMasterDataDA.GetState(CountryID).Select(obj => new VMSate()
            {
                StateID=obj.StateID,
                StateName=obj.StateName
            }).ToList();
        }

        public List<VMCity> GetCityBI(int SateID)
        {
            return IMasterDataDA.GetCity(SateID).Select(obj => new VMCity()
            {
                CityID=obj.CityID,
                CityName=obj.CityName
            }).ToList();
        }
    }
}
