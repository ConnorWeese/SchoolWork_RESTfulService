using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
public class Service : IService
{
    //service used to get the minimum kW output for a solar system given the average solar intensity and monthly power usage
    public String getSolarSystemRequirements(String solarIntensity, String kWH)
    {
        String message = "";
        double solarIntensityResult = 0.0;
        double kWHResult = 0.0;
        double calc = 0.0;

        //if the given solar intensity can be parsed to a double
        if (Double.TryParse(solarIntensity, out solarIntensityResult))
        {
            //if that result is greater than 0
            if (solarIntensityResult <= 0)
            {
                return "Please input a valid number";
            }
            //if the given kWH can be parsed to a double
            if (Double.TryParse(kWH, out kWHResult))
            {
                //if that result is greater than 0
                if (kWHResult <= 0)
                {
                    return "Please input a valid number";
                }
                //calculate the amount of kW that a solar system would need to produce
                calc = (kWHResult) / ((8 * (solarIntensityResult / 100)) * 30);
                message = "" + calc;
            }
            else
            {
                message = "Please input a valid number";
            }
        }
        else
        {
            message = "Please input a valid number";
        }

        return message;
    }

    //service used to get the annual energy output of a wind turbine given the average annual wind speed and the diameter in feet of the turbine blades
    public String annualWindEnergy(String windSpeed, String diameterInFeet)
    {
        String message = "";
        double windSpeedResult = 0.0;
        double diameterResult = 0.0;
        double calc = 0.0;
        //if the given wind speed can be parsed to a double
        if (Double.TryParse(windSpeed, out windSpeedResult))
        {
            //if that result is greater than 0
            if(windSpeedResult <= 0)
            {
                return "Please input a valid number";
            }
            //if the given diameter can be parsed to a double
            if (Double.TryParse(diameterInFeet, out diameterResult))
            {
                //if that result is greater than 0
                if (diameterResult <= 0)
                {
                    return "Please input a valid number";
                }
                //calculate the annual eneergy output of that turbine system
                calc = (0.01382) + (diameterResult * diameterResult) * (windSpeedResult * windSpeedResult * windSpeedResult);
                message = "" + calc;
            }
            else
            {
                message = "Please input a valid number";
            }
        }
        else
        {
            message = "Please input a valid number";
        }

        return message;
    }

    //service used to create an account to hold the information given by my other services
    //note that there is no way to update the account after the fact, so please provide all the data the first time
    public String createAccount(String username, String password, String powerUsage, String solarRec, String diameter, String windPower)
    {
        //if the given username does not have at least 1 character
        if (username.Length < 1)
        {
            return "Please enter a valid username";
        }
        //if the given password does not have at least 1 character 
        else if (password.Length < 1)
        {
            return "Please enter a valid password";
        }
        //create the path to the file location of the accounts
        String path = Path.Combine(HttpRuntime.AppDomainAppPath, @"App_Data");
        path = Path.Combine(path, "accounts.txt");
        String[] fileLines;
        //if that file does not exist, display an error message
        if (!File.Exists(path))
        {
            return "File does not exist";
        }
        //else read all the lines into an array of strings
        else
        {
            fileLines = File.ReadAllLines(path);
        }

        /* fileLines will hold data in this order:
         * username
         * password
         * estimated monthly power usage
         * solar energy requirements
         * wind turbine diameter
         * annual wind power
         */

        //iterate through the array to see if the given username already exists
        for (int i=0; i<fileLines.Length; i=i+6)
        {
            //if the username already exists in the file
            if (fileLines[i] == username)
            {
                return "Username already exists";
            }
        }

        //create the new account by appending the text to the file
        using(StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine(username);
            sw.WriteLine(password);
            sw.WriteLine("Estimated monthly power usage: {0}", powerUsage);
            sw.WriteLine("Solar energy requirements: {0}", solarRec);
            sw.WriteLine("Wind turbine diameter (in feet): {0}", diameter);
            sw.WriteLine("Annual power generated by wind: {0}", windPower);
            //close the file
            sw.Close();

            
        }

        return "Account successfully created!";
    }

    //service used to get the information stored in a users account
    public String getAccount(String username, String password)
    {
        //if the given username does not have at least 1 character
        if (username.Length < 1)
        {
            return "Please enter a valid username";
        }
        //if the given password does not have at least 1 character 
        else if (password.Length < 1)
        {
            return "Please enter a valid password";
        }

        //build the path to the file
        String accountInfo = "Account not found, please enter a valid username and password";
        String path = Path.Combine(HttpRuntime.AppDomainAppPath, @"App_Data");
        path = Path.Combine(path, "accounts.txt");
        String[] fileLines;
        //of the file does not exist, display an error message
        if (!File.Exists(path))
        {
            return "File does not exist";
        }
        //else read the lines of the file into an array of strings
        else
        {
            fileLines = File.ReadAllLines(path);
        }

        /* fileLines will hold data in this order:
         * username
         * password
         * estimated monthly power usage
         * solar energy requirements
         * wind turbine diameter
         * annual wind power
         */

        //iterate through the array to find the given username
        for (int i = 0; i < fileLines.Length; i = i + 6)
        {
            //if the username exists in the file
            if (fileLines[i] == username)
            {
                //if the given password matches what is in the file
                if (fileLines[i+1] == password)
                {
                    //get the account info
                    accountInfo = fileLines[i + 2] + "\n" + fileLines[i + 3] + "\n" + fileLines[i + 4] + "\n" + fileLines[i + 5];
                    return accountInfo;
                }
                else
                {
                    return "Password is invalid";
                }
            }
        }

        return accountInfo;
    }
}
