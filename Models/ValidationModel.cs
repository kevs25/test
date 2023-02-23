using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;

namespace Webapp.Models
{

    public class ValidationModel
    {
        static Dictionary<string, string[]> userDetails = new Dictionary<string, string[]>();
        static ValidationModel()
        {
            try
            {
                using(SqlConnection connection = new SqlConnection("Data source =ASPIRE1890\\SQLEXPRESS; Database = signupDetails; Integrated security=SSPI"))
                {
                    connection.Open();
                    
                    Console.WriteLine("Successfull");
                    SqlCommand selectCommand = new SqlCommand("Select * from signupData",connection);
                    SqlDataReader sqlReader = selectCommand.ExecuteReader();
                    while(sqlReader.Read())
                    {
                        string[] userdata = new string[2];
                        userdata[0] = sqlReader["password"].ToString();
                        userdata[1] = sqlReader["emailId"].ToString();
                        userDetails.Add(sqlReader["username"].ToString(),userdata);


                    }
                
                    
                
                }
            }
            catch (SqlException sqlException)
            {
                Console.WriteLine(sqlException);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            
            
        }
        public static int signupValidation(SignUpModel signUpModel)
        {
            Regex usernameValidation = new Regex(@"^[a-z]*$");
            Regex passwordValidation = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");

            
            string? username = signUpModel.username;
            string? emailId = signUpModel.emailId;
            string? password = signUpModel.password;
            string? confirmPassword = signUpModel.ConfirmPassword;

            if(username.Length >= 10 && usernameValidation.IsMatch(username) && password.Length >= 10 && passwordValidation.IsMatch(password))
            {
                 if(String.Equals(password,confirmPassword))
                {
                    foreach (KeyValuePair<string,string[]> checkUserData in userDetails)
                    { 
                        if(string.Equals(username,checkUserData.Key) || string.Equals(checkUserData.Value[1],emailId))
                        {
                            Console.WriteLine("User Exists");
                            return 4;
                        }
                    }
                     using(SqlConnection connection = new SqlConnection("Data source =ASPIRE1890\\SQLEXPRESS; Database = signupDetails; Integrated security=SSPI"))
                     {
                        connection.Open();
                        SqlCommand insertCommand = new SqlCommand("Insert into signupData values(@value1, @value2, @value3, @value4)",connection);
                        insertCommand.Parameters.Add("@value1", SqlDbType.VarChar,50,"username").Value=signUpModel.username;
                        insertCommand.Parameters.Add("@value2", SqlDbType.VarChar,50,"password").Value=signUpModel.password;
                        insertCommand.Parameters.Add("@value3", SqlDbType.VarChar,50,"emailId").Value=signUpModel.emailId;
                        insertCommand.Parameters.Add("@value4", SqlDbType.VarChar,50,"choice").Value=signUpModel.choice;
                        insertCommand.ExecuteNonQuery();
                     }
                    Console.WriteLine("password matched");
                    return 1;
                    
                }
                else
                {
                    // throw new PasswordNotMatchException("password not matched");
                    // Console.WriteLine("password not matched");
                    return 2;
                }
                  
            }         
            else
                Console.WriteLine("wrong validation");
                Console.WriteLine(password);
                Console.WriteLine(confirmPassword);
                return 3;
                
        }
        public static int LoginValidation(LoginModel loginModel)
        {
            string? username = loginModel.username;
            string? password = loginModel.password;
            foreach(KeyValuePair<string,string[]> checkUserData in userDetails)
            {
                if(String.Equals(username,checkUserData.Key))
                {
                    if(String.Equals(password,checkUserData.Value[0]))
                        return 1;
                    else
                        return 2;
                }
                 
            }
            return 3;
            
            
        }
        public static int ForgotPassword(ForgotPasswordModel forgotPassword)
        {
            string? emailId = forgotPassword.emailId;
            string? password = forgotPassword.password;
            string? confirmPassword = forgotPassword.ConfirmPassword;

            Regex passwordValidation = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");

            foreach(KeyValuePair<string,string[]> checkUserData in userDetails)
            {
                if(String.Equals(emailId,checkUserData.Value[1]))
                {
                    if(String.Equals(password,confirmPassword))
                    {
                        if(passwordValidation.IsMatch(password) && passwordValidation.IsMatch(confirmPassword))
                        {
                            using(SqlConnection connection = new SqlConnection("Data source =ASPIRE1890\\SQLEXPRESS; Database = signupDetails; Integrated security=SSPI"))
                            {
                                connection.Open();
                                SqlCommand updateCommand = new SqlCommand("Update signupData set password =@pass where emailId=@email",connection);
                                updateCommand.Parameters.Add("@email",SqlDbType.VarChar,50,"emailId").Value=emailId;
                                updateCommand.Parameters.Add("@pass",SqlDbType.VarChar,50,"password").Value=password;
                                updateCommand.ExecuteNonQuery(); 
                                Console.WriteLine("Password Changed Successfully");
                                
                            }
                     
                        // userDetails[username] = password;
                            return 1;
                        }
                        Console.WriteLine("Wrong Validation.");
                        return 4;
                            
                    }
                    Console.WriteLine("Password Does Not Match");
                    return 3;
                    
                }
            }  
        Console.WriteLine("User does not exist");
        return 2;
        }
    }
    // class PasswordNotMatchException : Exception
    // {
    //     public PasswordNotMatchException(string Message): base(Message){}
        

    // }
}