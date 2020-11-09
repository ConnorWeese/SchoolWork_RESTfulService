using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService" in both code and config file together.
[ServiceContract]
public interface IService
{
    [OperationContract]
    [WebGet(UriTemplate = "getSolarSystemRequirements?solarIntensity={solarIntensity}&kWH={kWH}")]
    String getSolarSystemRequirements(String solarIntensity, String kWH);

    [OperationContract]
    [WebGet(UriTemplate = "annualWindEnergy?windSpeed={windSpeed}&diameterInFeet={diameterInFeet}")]
    String annualWindEnergy(String windSpeed, String diameterInFeet);

    [OperationContract]
    [WebGet(UriTemplate = "createAccount?username={username}&password={password}&powerUsage={powerUsage}&solarRec={solarRec}&diameter={diameter}&windPower={windPower}")]
    String createAccount(String username, String password, String powerUsage, String solarRec, String diameter, String windPower);

    [OperationContract]
    [WebGet(UriTemplate = "getAccount?username={username}&password={password}")]
    String getAccount(String username, String password);

    // TODO: Add your service operations here
}

// Use a data contract as illustrated in the sample below to add composite types to service operations.
[DataContract]
public class CompositeType
{
	bool boolValue = true;
	string stringValue = "Hello ";

	[DataMember]
	public bool BoolValue
	{
		get { return boolValue; }
		set { boolValue = value; }
	}

	[DataMember]
	public string StringValue
	{
		get { return stringValue; }
		set { stringValue = value; }
	}
}
