

Declare @XML XML
SET @XML ='<DotNetNancy>
  <initialElement displayName="If" />
  <fields>
    <field propertyName="AirDepartureCity" displayName="departure city" maxLength="3" dataType="string" />
    <field propertyName="AirDestinationCity" displayName="destination city" maxLength="3" dataType="string" />
    <field propertyName="HasAirline" displayName="airline" maxLength="2" dataType="string" invocationType="AsMethodPassDefinedValueAsParameter" />
    <field propertyName="AirArrivalDate" displayName="arrival date" dataType="date" />
    <field propertyName="AirArrivalTime" displayName="arrival time" dataType="time" />
    <field propertyName="AirDepartureDate" displayName="departure date" dataType="date" />
    <field propertyName="AirDepartureTime" displayName="departure time" dataType="time" />
    <field propertyName="HasClassOfService" displayName="class of service" dataType="string" invocationType="AsMethodPassDefinedValueAsParameter" />
    <field propertyName="OperatingFlight.FlightNo" displayName="flight number" dataType="string" memberOfCollection="FlightSegments" invocationType="AsAnyOneDynamic" />
    <field propertyName="HasFlightSegments" displayName="has air segment" dataType="bool" />
    <field propertyName="AirStatusCode" displayName="air status code" dataType="string" />
    <field propertyName="AirTripLength" displayName="trip length" dataType="string" />
    <field propertyName="RoomRate.Amount" displayName="AnyOneHotelSegmentRoomRateAmount" dataType="numeric" memberOfCollection="HotelSegments" invocationType="AsAnyOneDynamic" />
    <field propertyName="HotelChainCode" displayName="hotel chain code" maxLength="2" dataType="string" />
    <field propertyName="HotelCheckInDate" displayName="hotel check-in date" dataType="date" />
    <field propertyName="HotelCheckOutDate" displayName="hotel check-out date" dataType="date" />
    <field propertyName="HotelPropertyNumber" displayName="hotel property number" dataType="string" />
    <field propertyName="HasHotelSegments" displayName="has hotel segments" dataType="bool" />
    <field propertyName="HotelStatusCode" displayName="hotel status code" dataType="string" />
    <field propertyName="AirCost" displayName="air cost" dataType="numeric" />
    <field propertyName="HotelTotalCost" displayName="hotel total cost" dataType="numeric" />
    <field propertyName="TotalCost" displayName="total cost" dataType="numeric" />
    <field propertyName="Fare.TotalFare.Amount" displayName="AnyOneVehicleSegmentTotalFareAmount" dataType="numeric" memberOfCollection="VehicleSegments" invocationType="AsAnyOneDynamic" />
    <field propertyName="VehicleCost" displayName="vehicle cost" dataType="numeric" />
    <field propertyName="HotelDailyRoomRate" displayName="hotel daily room rate" dataType="numeric" />
    <field propertyName="HotelCityCode" displayName="hotel city code" dataType="string" />
    <field propertyName="CarChainCode" displayName="car chain code" maxLength="2" dataType="string" />
    <field propertyName="CarDropOffDate" displayName="car drop-off date" dataType="date" />
    <field propertyName="CarPickUpDate" displayName="car pick-up date" dataType="date" />
    <field propertyName="HasVehicleSegments" displayName="has vehicle segments" dataType="bool" />
    <field propertyName="CarStatusCode" displayName="car status code" dataType="string" />
    <field propertyName="CarProfileCreditCardType" displayName="car credit card type" dataType="enum">
      <item displayName="Unspecified" value="0" />
      <item displayName="MasterCard" value="1" />
      <item displayName="Visa" value="2" />
      <item displayName="AmEx" value="3" />
      <item displayName="Discover" value="4" />
      <item displayName="DinersClub" value="5" />
    </field>
    <field propertyName="ProfileEmail" displayName="email address" dataType="string" />
    <field propertyName="ProfileName" displayName="name (FIRST, LAST)" dataType="string" />
    <field propertyName="ProfilePhoneNumber" displayName="phone number" maxLength="12" dataType="string" />
    <field propertyName="ProfileFullName" displayName="full name (LAST,FIRST MI)" dataType="string" />
    <field propertyName="ProfileDKNumber" displayName="DK number" maxLength="10" dataType="string" />
    <field propertyName="ProfileCreditCardType" displayName="profile credit card type" dataType="enum">
      <item displayName="Unspecified" value="0" />
      <item displayName="MasterCard" value="1" />
      <item displayName="Visa" value="2" />
      <item displayName="AmEx" value="3" />
      <item displayName="Discover" value="4" />
      <item displayName="DinersClub" value="5" />
    </field>
    <field propertyName="RemarksAllRemarks" displayName="remarks all" dataType="string" />
    <field propertyName="RemarksGovFields" displayName="remarks government fields" dataType="string" collectionType="IDictionary" collectionName="RemarksGovFields" invocationType="AsAnyOneDynamic" />
    <field propertyName="RemarksGovField1" displayName="remarks government field 1" dataType="string" memberOfCollection="RemarksGovFields" key="GF1" />
    <field propertyName="RemarksGovField2" displayName="remarks government field 2" dataType="string" memberOfCollection="RemarksGovFields" key="GF2" />
    <field propertyName="RemarksGovField3" displayName="remarks government field 3" dataType="string" memberOfCollection="RemarksGovFields" key="GF3" />
    <field propertyName="RemarksGovField4" displayName="remarks government field 4" dataType="string" memberOfCollection="RemarksGovFields" key="GF4" />
    <field propertyName="RemarksGovField5" displayName="remarks government field 5" dataType="string" memberOfCollection="RemarksGovFields" key="GF5" />
    <field propertyName="RemarksGovField6" displayName="remarks government field 6" dataType="string" memberOfCollection="RemarksGovFields" key="GF6" />
    <field propertyName="RemarksGovField7" displayName="remarks government field 7" dataType="string" memberOfCollection="RemarksGovFields" key="GF7" />
    <field propertyName="RemarksGovField8" displayName="remarks government field 8" dataType="string" memberOfCollection="RemarksGovFields" key="GF8" />
    <field propertyName="RemarksGovField9" displayName="remarks government field 9" dataType="string" memberOfCollection="RemarksGovFields" key="GF9" />
    <field propertyName="RemarksSortFields" displayName="remarks sort fields" dataType="string" collectionType="IDictionary" collectionName="RemarksSortFields" invocationType="AsAnyOneDynamic" />
    <field propertyName="RemarksSortField1" displayName="remarks sort field 1" dataType="string" memberOfCollection="RemarksSortFields" key="SF1" />
    <field propertyName="RemarksSortField2" displayName="remarks sort field 2" dataType="string" memberOfCollection="RemarksSortFields" key="SF2" />
    <field propertyName="RemarksSortField3" displayName="remarks sort field 3" dataType="string" memberOfCollection="RemarksSortFields" key="SF3" />
    <field propertyName="RemarksSortField4" displayName="remarks sort field 4" dataType="string" memberOfCollection="RemarksSortFields" key="SF4" />
    <field propertyName="RemarksSortField5" displayName="remarks sort field 5" dataType="string" memberOfCollection="RemarksSortFields" key="SF5" />
    <field propertyName="RemarksSortField6" displayName="remarks sort field 6" dataType="string" memberOfCollection="RemarksSortFields" key="SF6" />
    <field propertyName="RemarksSortField7" displayName="remarks sort field 7" dataType="string" memberOfCollection="RemarksSortFields" key="SF7" />
    <field propertyName="RemarksSortField8" displayName="remarks sort field 8" dataType="string" memberOfCollection="RemarksSortFields" key="SF8" />
    <field propertyName="RemarksSortField9" displayName="remarks sort field 9" dataType="string" memberOfCollection="RemarksSortFields" key="SF9" />
    <field propertyName="RemarksUdidFields" displayName="remarks udid fields" dataType="string" collectionType="IDictionary" collectionName="RemarksUdidFields" invocationType="AsAnyOneDynamic" />
    <field propertyName="RemarksUdidField1" displayName="remarks udid field 1" dataType="string" memberOfCollection="RemarksUdidFields" key="UF1" />
    <field propertyName="RemarksUdidField2" displayName="remarks udid field 2" dataType="string" memberOfCollection="RemarksUdidFields" key="UF2" />
    <field propertyName="RemarksUdidField3" displayName="remarks udid field 3" dataType="string" memberOfCollection="RemarksUdidFields" key="UF3" />
    <field propertyName="RemarksUdidField4" displayName="remarks udid field 4" dataType="string" memberOfCollection="RemarksUdidFields" key="UF4" />
    <field propertyName="RemarksUdidField5" displayName="remarks udid field 5" dataType="string" memberOfCollection="RemarksUdidFields" key="UF5" />
    <field propertyName="RemarksUdidField6" displayName="remarks udid field 6" dataType="string" memberOfCollection="RemarksUdidFields" key="UF6" />
    <field propertyName="RemarksUdidField7" displayName="remarks udid field 7" dataType="string" memberOfCollection="RemarksUdidFields" key="UF7" />
    <field propertyName="RemarksUdidField8" displayName="remarks udid field 8" dataType="string" memberOfCollection="RemarksUdidFields" key="UF8" />
    <field propertyName="RemarksUdidField9" displayName="remarks udid field 9" dataType="string" memberOfCollection="RemarksUdidFields" key="UF9" />
    <field propertyName="RemarksUdidField10" displayName="remarks udid field 10" dataType="string" memberOfCollection="RemarksUdidFields" key="UF10" />
    <field propertyName="RemarksUdidField11" displayName="remarks udid field 11" dataType="string" memberOfCollection="RemarksUdidFields" key="UF11" />
    <field propertyName="RemarksUdidField12" displayName="remarks udid field 12" dataType="string" memberOfCollection="RemarksUdidFields" key="UF12" />
    <field propertyName="RemarksUdidField13" displayName="remarks udid field 13" dataType="string" memberOfCollection="RemarksUdidFields" key="UF13" />
    <field propertyName="RemarksUdidField14" displayName="remarks udid field 14" dataType="string" memberOfCollection="RemarksUdidFields" key="UF14" />
    <field propertyName="RemarksUdidField15" displayName="remarks udid field 15" dataType="string" memberOfCollection="RemarksUdidFields" key="UF15" />
    <field propertyName="RemarksUdidField16" displayName="remarks udid field 16" dataType="string" memberOfCollection="RemarksUdidFields" key="UF16" />
    <field propertyName="RemarksUdidField17" displayName="remarks udid field 17" dataType="string" memberOfCollection="RemarksUdidFields" key="UF17" />
    <field propertyName="RemarksUdidField18" displayName="remarks udid field 18" dataType="string" memberOfCollection="RemarksUdidFields" key="UF18" />
    <field propertyName="RemarksUdidField19" displayName="remarks udid field 19" dataType="string" memberOfCollection="RemarksUdidFields" key="UF19" />
    <field propertyName="RemarksUdidField20" displayName="remarks udid field 20" dataType="string" memberOfCollection="RemarksUdidFields" key="UF20" />
  </fields>
  <actions>
    <action methodName="PreAuthorize" displayName="pre-authorize" />
    <action methodName="SANotification" displayName="SA notification" />
    <action methodName="Report" displayName="report" />
  </actions>
  <nonDisplayedFields>
    <field propertyName="FlightSegments" displayName="not displayed" dataType="string" collectionType="IEnumerable" collectionName="FlightSegments" />
    <field propertyName="HotelSegments" displayName="not displayed" dataType="string" collectionType="IEnumerable" collectionName="HotelSegments" />
    <field propertyName="VehicleSegments" displayName="not displayed" dataType="string" collectionType="IEnumerable" collectionName="VehicleSegments" />
  </nonDisplayedFields>
</DotNetNancy>'
UPDATE [dbo].[ConfigurationFile]
	Set [ConfigurationFile] = @XML
	Where  ApplicationID = '3428a138-4738-4324-bd1f-134732b26ce0'
	and TypeID = '9b6df0f2-c552-4e32-a7ec-684613c9985d'
	and ConfigurationTypeID = 1
GO

