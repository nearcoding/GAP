using System;
using System.Collections.Generic;
using GAP.Helpers;

namespace GAP.BL
{
    public class Login
    {
        public bool Authenticate(string email, string password)
        {
            try
            {
                GAP.DAL.LoginModelDataContext loginmdc = new GAP.DAL.LoginModelDataContext();
                string salt = loginmdc.GetSalt(email);
                string dbpass = loginmdc.GetPassword(email);

                if (salt != string.Empty && dbpass != string.Empty)
                {

                    Helpers.Login_Security sec = new Helpers.Login_Security();

                    string hashedpassword = sec.hashPass(salt, password);

                    if (dbpass == hashedpassword)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {                
                return false;
            }
        }

        public bool ResetPassword(string email)
        {
            try
            {
                GAP.DAL.LoginModelDataContext resetpsw = new GAP.DAL.LoginModelDataContext();
                Login_Security sec = new Login_Security();
                EmailSender sender = new EmailSender();
                if (EmailExists(email))
                {
                    string salt = resetpsw.GetSalt(email);
                    string clearpass = sec.RandomString();

                    string hashedpassword = sec.hashPass(salt, clearpass);

                    if (resetpsw.UpdatePassword(email, hashedpassword, salt))
                    {
                        sender.ResetPassword(email, clearpass);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SaveUserInfo(string Email, int role, string FirstName, string LastName, string Country, string State, string Add1, string Add2, string serial, string Cellphone, string Phone1, string Phone2)
        {
            DAL.LoginModelDataContext createuser = new DAL.LoginModelDataContext();
            DAL.User user = new User();
            DAL.UsersDataContext userdata = new UsersDataContext();
            Helpers.Login_Security sec = new Helpers.Login_Security();
            Helpers.EmailSender sender = new Helpers.EmailSender();
            if (EmailExists(Email))
            {
                return false;
            }
            else
            {
                string salt = "TempSalt";
                string clearpass = sec.RandomString();
                string hashedpassword = sec.hashPass(salt, clearpass);
                user.Email = Email;
                user.FirstName = FirstName;
                user.LastName = LastName;
                user.Country = Country;
                user.State_Province = State;
                user.Address1 = Add1;
                user.Address2 = Add2;
                user.Salt = salt;
                user.RoleID = role;
                user.HashedPassword = hashedpassword;
                user.SerialNumber = serial;
                user.Cellphone = Cellphone;
                user.Phone1 = Phone1;
                user.Phone2 = Phone2;
                try
                {
                    createuser.Users.InsertOnSubmit(user);
                    createuser.SubmitChanges();
                    sender.UserCreate(Email, clearpass, serial);
                    return true;

                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public string GetUserRole(string Email)
        {
            LoginModelDataContext db = new LoginModelDataContext();

            return db.GetRole(Email);
        }

        public bool CreateRandomPassword(string email)
        {
            try
            {
                DAL.LoginModelDataContext resetpsw = new DAL.LoginModelDataContext();
                Helpers.Login_Security sec = new Helpers.Login_Security();
                Helpers.EmailSender sender = new Helpers.EmailSender();
                List<string> UserInfo = new List<string>();
                if (EmailExists(email))
                {
                    return false;
                }
                else
                {
                    string salt = "TempSalt";
                    string clearpass = sec.RandomString();

                    string hashedpassword = sec.hashPass(salt, clearpass);

                    if (resetpsw.UpdatePassword(email, hashedpassword, salt))
                    {
                        sender.ResetPassword(email, clearpass);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool NewPassword(string email, string Password)
        {
            try
            {
                DAL.LoginModelDataContext newpwd = new DAL.LoginModelDataContext();
                Helpers.Login_Security sec = new Helpers.Login_Security();
                Helpers.EmailSender sender = new Helpers.EmailSender();
                if (EmailExists(email))
                {
                    string salt = newpwd.GetSalt(email);
                    string clearpass = Password;

                    string hashedpassword = sec.hashPass(salt, clearpass);

                    if (newpwd.UpdatePassword(email, hashedpassword, salt))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        private bool EmailExists(string email)
        {
            DAL.LoginModelDataContext emailexists = new DAL.LoginModelDataContext();
            return emailexists.EmailExists(email);
        }
    }
}
