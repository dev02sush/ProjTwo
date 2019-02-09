using SusheelRawat.DomainModal;
using System;
using System.Collections.Generic;
using System.Text;
using SusheelRawat.DA;
using SusheelRawat.Models;
using System.Linq;
using SusheelRawat.Models.CustomDominModel;

namespace SusheelRawat.BI
{
    public class UserProfileBI : IUserProfileBI
    {
        readonly IUserProfile IUserProfile;
        public UserProfileBI(IUserProfile _IUserProfile)
        {
            this.IUserProfile = _IUserProfile;
        }

        /// <summary>
        /// add edit user profile
        /// </summary>
        /// <param name="MyProfile"></param>
        /// <returns></returns>
        public long AddEditUserProfile(VMProfile MyProfile)
        {
            UserProfileDomainModel objUserModel = new UserProfileDomainModel();
            /**/
            objUserModel.UserID = MyProfile.UserID;
            objUserModel.UserName = MyProfile.UserName;
            objUserModel.Email = MyProfile.Email;
            objUserModel.PhoneNumber = MyProfile.PhoneNumber;
            objUserModel.Gender = MyProfile.Gender;
            objUserModel.HavePassport = MyProfile.HavePassport;
            objUserModel.Married = MyProfile.Married;
            objUserModel.Description = MyProfile.Description;
            objUserModel.Status = MyProfile.Status;
            //user hobby
            if (MyProfile.UserHobby != null && MyProfile.UserHobby.Count > 0)
            {
                objUserModel.UserHobby = MyProfile.UserHobby.Select(obj => new UserHobbiesDomainModel()
                {
                    UserHobbyID = obj.UserHobbyID,
                    UserID = objUserModel.UserID,
                    HobbyId = obj.HobbyId,
                    IsDeleted = obj.UserHobbyID > 0 && obj.IsSelected == false
                }).ToList();
            }
            //user address
            if (MyProfile.UserAddress != null && MyProfile.UserAddress.Count > 0)
            {
                objUserModel.UserAddress = MyProfile.UserAddress.Select(obj => new AddressesDomainModel()
                {
                    AddressID = obj.AddressID,
                    AddressType = obj.AddressType,
                    UserID = objUserModel.UserID,
                    Address = obj.Address,
                    CityID = obj.CityID,
                    AreaCode = obj.AreaCode
                }).ToList();
            }
            /**/
            if (objUserModel.UserID > 0)
            {
                IUserProfile.UpdateEntity(objUserModel);
                if (objUserModel.UserAddress != null)
                {
                    var addressToUpdate = objUserModel.UserAddress.Where(x => x.AddressID > 0).ToList();
                    if (addressToUpdate != null && addressToUpdate.Count > 0)
                    {
                        IUserProfile.UpdateListOfEntity<AddressesDomainModel>(addressToUpdate);
                        //foreach (var add in addressToUpdate)
                        //{
                        //    IUserProfile.UpdatedChildEntity<AddressesDomainModel>(add);
                        //}
                    }
                    //var newAddress = objUserModel.UserAddress.Where(x => x.AddressID == 0).ToList();
                    //if (newAddress != null && newAddress.Count > 0)
                    //{
                    //    IUserProfile.AddChildEntityList<AddressesDomainModel>(newAddress);
                    //}
                }
                if (objUserModel.UserHobby != null)
                {
                    var hobbyToUpdate = objUserModel.UserHobby.Where(x => x.UserHobbyID > 0).ToList();
                    if (hobbyToUpdate != null && hobbyToUpdate.Count > 0)
                    {
                        IUserProfile.UpdateListOfEntity<UserHobbiesDomainModel>(hobbyToUpdate);
                    }
                    //IUserProfile.UpdateListOfEntity<UserHobbiesDomainModel>(objUserModel.UserHobby);
                    //IUserProfile.UpdateListOfEntity<AddressesDomainModel>(objUserModel.UserAddress);
                }
            }
            else
            {
                //return IUserProfile.AddEntity(objUserModel);
                IUserProfile.AddEntity(objUserModel);
            }
            IUserProfile.SaveChanges();
            return objUserModel.UserID;

        }

        /// <summary>
        /// get list of all users
        /// </summary>
        /// <returns></returns>
        public List<VMProfile> GetAllUsers()
        {
            List<UserProfileCDM> liUser = IUserProfile.GetAllUsers();
            List<VMProfile> users = liUser.GroupBy(x => x.UserID).Select(obj => new VMProfile
            {
                //UserID = obj.UserID,
                //UserName = obj.UserName,
                //Email = obj.Email,
                //PhoneNumber = obj.PhoneNumber,
                //Gender = obj.Gender
                UserID = obj.Key,
                UserName = obj.FirstOrDefault().UserName,
                Email = obj.FirstOrDefault().Email,
                PhoneNumber = obj.FirstOrDefault().PhoneNumber,
                Gender = obj.FirstOrDefault().Gender,
            }).ToList().ToList();
            //users = users.Distinct().ToList();

            //foreach (var usr in users)
            //{
            //    var us = liUser.Where(x => x.UserID == usr.UserID);
            //    if (us.FirstOrDefault().Hobbies != null)
            //    {
            //        usr.UserHobby = us.Select(obj => new VMProfileHobbies
            //        {
            //            HobbyId = obj.Hobbies.FirstOrDefault().HobbyID,
            //            HobbyName = obj.Hobbies.FirstOrDefault().HobbyName,
            //            UserHobbyID = obj.Hobbies.FirstOrDefault().UserHobbyID,
            //        }).ToList();
            //    }
            //    if(us.FirstOrDefault().UserAddress!=null)
            //    {
            //        usr.UserAddress = us.Select(obj => new VMProfileAddress
            //        {
            //            AddressID=obj.UserAddress.FirstOrDefault().AddressID,
            //            AddressType = obj.UserAddress.FirstOrDefault().AddressType,
            //            CityName = obj.UserAddress.FirstOrDefault().City
            //        }).ToList();
            //    }
            //}

            //return liUser.Select(obj => new VMProfile()
            //{
            //    UserID=obj.UserID,
            //    UserName=obj.UserName,
            //    Email=obj.Email,
            //    PhoneNumber=obj.PhoneNumber,
            //    Gender=obj.Gender,
            //    HavePassport=obj.HavePassport,
            //    Description=obj.Description,
            //    Status=obj.Status,
            //    UserHobby=obj.Hobbies!=null && obj.Hobbies.Count>0?obj.Hobbies.Select(objHobbies=>new VMProfileHobbies() {
            //        HobbyName=objHobbies.HobbyName,                    
            //    }).ToList():null,
            //    UserAddress=obj.UserAddress!=null && obj.UserAddress.Count>0?obj.UserAddress.Select(objAddr=>new VMProfileAddress() {
            //        Address=objAddr.Address,
            //        AddressType=objAddr.AddressType,
            //        AreaCode=objAddr.AreaCode,
            //        CityName=objAddr.City,
            //        SateName=objAddr.State,
            //        CountryName=objAddr.Country
            //    }).ToList():null
            //}).ToList();
            return users;
        }

        /// <summary>
        /// get detail of particular user
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public VMProfile GetDetailOfParticularUser(long UserID)
        {
            UserProfileCDM objPro = IUserProfile.GetDetailOfParticularUser(UserID);
            VMProfile objProfile = new VMProfile();
            //var tempHobby = objPro.Select(objHb => new VMProfileHobbies()
            //{
            //    UserHobbyID=objHb.UserHobbyID,
            //    HobbyId=objHb.HobbyID
            //}).ToList();
            //var tempHobby = objPro.Where(x => x.UserHobbyID > 0).ToList();
            //var tempAdd = objPro.Where(x => x.AddressID > 0).ToList();
            /**/
            objProfile.UserID = objPro.UserID;
            objProfile.IsActive = objPro.IsActive;
            objProfile.UserName = objPro.UserName;
            objProfile.Email = objPro.Email;
            objProfile.PhoneNumber = objPro.PhoneNumber;
            objProfile.Gender = objPro.Gender;
            objProfile.HavePassport = objPro.HavePassport;
            objProfile.Description = objPro.Description;
            objProfile.Status = objPro.Status;
            objProfile.Married = objPro.Married;
            objProfile.UserHobby = new List<VMProfileHobbies>();
            //objProfile.UserHobby = objPro[0].Hobbies.Select(obj => new VMProfileHobbies()
            //{
            //    UserHobbyID = obj.UserHobbyID,
            //    HobbyId = obj.HobbyID
            //}).ToList();
            if (objPro.Hobbies != null && objPro.Hobbies.Count > 0)
            {
                objProfile.UserHobby = objPro.Hobbies.Select(obj => new VMProfileHobbies()
                {
                    UserHobbyID = obj.UserHobbyID,
                    HobbyId = obj.HobbyID
                }).ToList();
            }

            objProfile.UserAddress = new List<VMProfileAddress>();
            //objProfile.UserAddress = objPro[0].UserAddress.Select(obj => new VMProfileAddress()
            if (objPro.UserAddress != null && objPro.UserAddress.Count > 0)
            {
                objProfile.UserAddress = objPro.UserAddress.Select(obj => new VMProfileAddress()
                {
                    AddressID = obj.AddressID,
                    AddressType = obj.AddressType,
                    Address = obj.Address,
                    AreaCode = obj.AreaCode,
                    CityID = obj.CityID,
                    StateID = obj.SateID,
                    CountryId = obj.CountryID,
                    CountryName = obj.Country,
                    SateName = obj.State,
                    CityName = obj.City
                }).ToList();
            }
            /**/
            return objProfile;
        }

        //public long AddEditUserProfile(VMProfile MyProfile)
        //{
        //    UserProfileDomainModel objProfile = new UserProfileDomainModel();
        //    /**/

        //    /**/
        //    return IUserProfile.AddEditEntity(objProfile);
        //    //return 0;
        //}
    }
}
