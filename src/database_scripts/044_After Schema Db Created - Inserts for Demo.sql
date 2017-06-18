use [GenericDotNetRulesStore]
go

INSERT INTO [GenericDotNetRulesStore].[dbo].[Application]
           ([ApplicationID]
           ,[ApplicationName]
           ,[ApplicationDescription])
     VALUES
           ('3428a138-4738-4324-bd1f-134732b26ce0',
			'GenericDotNetRules',
			'Sample Implementation of Rules Processing')
go

INSERT INTO [GenericDotNetRulesStore].[dbo].[Type]
           ([TypeID]
           ,[TypeFullName])
     VALUES
           ('9b6df0f2-c552-4e32-a7ec-684613c9985d',
			'DotNetNancy.Rules.RuleSet.GenericDotNetRules.KnownTypesLibrary.RuleSetPNR')
go

INSERT INTO [GenericDotNetRulesStore].[dbo].[UserStore]
           ([UserID]
           ,[FirstName]
           ,[LastName]
           ,[Email]
           ,[Password]
           ,[LastLogin]
           ,[DateCreated]
           ,[CreatedBy]
           ,[IsSuperUser])
     VALUES
           ('eaad5439-08ae-4e12-8307-138afd0b6d9f',
			'super',
			'administrator',
			'a@a.com',
			'123456',
			'10/19/2009 12:00:00 AM',
			'10/19/2009 11:06:09 AM',
			'eaad5439-08ae-4e12-8307-138afd0b6d9f',
			1)
go

INSERT INTO [GenericDotNetRulesStore].[dbo].[ConfigurationType]
           ([ConfigurationTypeID]
           ,[ConfigurationTypeDescription])
     VALUES
           (1, 'FLDS')
go

INSERT INTO [GenericDotNetRulesStore].[dbo].[ConfigurationType]
           ([ConfigurationTypeID]
           ,[ConfigurationTypeDescription])
     VALUES
			(2,	'OPRS')
go

INSERT INTO [GenericDotNetRulesStore].[dbo].[ConfigurationFile]
           ([TypeID]
           ,[ApplicationID]
           ,[ConfigurationTypeID]
           ,[ConfigurationFile])
     VALUES
           ('9b6df0f2-c552-4e32-a7ec-684613c9985d',
			'3428a138-4738-4324-bd1f-134732b26ce0',
			1,
			'<DotNetNancy>
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
    <field propertyName="HotelChainCode" displayName="hotel chain code" maxLength="2" dataType="string" />
    <field propertyName="HotelCheckInDate" displayName="hotel check-in date" dataType="date" />
    <field propertyName="HotelCheckOutDate" displayName="hotel check-out date" dataType="date" />
    <field propertyName="HotelPropertyNumber" displayName="hotel property number" dataType="string" />
    <field propertyName="HasHotelSegments" displayName="has hotel segments" dataType="bool" />
    <field propertyName="HotelStatusCode" displayName="hotel status code" dataType="string" />
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
</DotNetNancy>')
go

INSERT INTO [GenericDotNetRulesStore].[dbo].[ConfigurationFile]
           ([TypeID]
           ,[ApplicationID]
           ,[ConfigurationTypeID]
           ,[ConfigurationFile])
     VALUES
           ('9b6df0f2-c552-4e32-a7ec-684613c9985d',
			'3428a138-4738-4324-bd1f-134732b26ce0',
			2,
			'<DotNetNancy>
  <clauses>
    <clause name="and" value="and" />
    <clause name="or" value="or" />
    <clause name="then" value="then" />
  </clauses>
  <operators>
    <operator name="in" value="in" set="string" />
    <operator name="not in" value="notin" set="string" />
    <operator name="is" value="is" set="string" />
    <operator name="is not" value="not" set="string" />
    <operator name="contains" value="contains" set="string" />
    <operator name="doesn''t contain" value="nocontains" set="string" />
    <operator name="starts with" value="starts" set="string" />
    <operator name="doesn''t start with" value="nostarts" set="string" />
    <operator name="ends with" value="ends" set="string" />
    <operator name="doesn''t end with" value="noends" set="string" />
    <operator name="is equal to" value="is" set="numeric" />
    <operator name="is not equal to" value="not" set="numeric" />
    <operator name="is less than" value="less" set="numeric" />
    <operator name="is less than or equal to" value="lessis" set="numeric" />
    <operator name="is greater than" value="more" set="numeric" />
    <operator name="is greater than or equal to" value="moreis" set="numeric" />
    <operator name="is equal to" value="is" set="date" />
    <operator name="is not equal to" value="not" set="date" />
    <operator name="is in the past of" value="less" set="date" />
    <operator name="is in the past of or equal to" value="lessis" set="date" />
    <operator name="is in the future of" value="more" set="date" />
    <operator name="is in the future of or equal to" value="moreis" set="date" />
    <operator name="is equal to" value="is" set="time" />
    <operator name="is not equal to" value="not" set="time" />
    <operator name="is before" value="less" set="time" />
    <operator name="is before or equal to" value="lessis" set="time" />
    <operator name="is after" value="more" set="time" />
    <operator name="is after or equal to" value="moreis" set="time" />
    <operator name="is" value="is" set="bool" />
    <operator name="is" value="is" set="enum" />
    <operator name="is not" value="not" set="enum" />
  </operators>
</DotNetNancy>')
go

INSERT INTO [GenericDotNetRulesStore].[dbo].[RuleDefinition]
           ([RuleID]
           ,[TypeID]
           ,[ApplicationID]
           ,[RuleName]
           ,[Definition]
           ,[Paused]
           ,[DateCreated]
           ,[CreatedBy]
           ,[Deleted]
           ,[DateUpdated]
           ,[UpdatedBy])
     VALUES
           ('3ff2afda-7f63-4c17-a11b-314a083dd987',
			'9b6df0f2-c552-4e32-a7ec-684613c9985d',
			'3428a138-4738-4324-bd1f-134732b26ce0',
			'all fields',
			'<DotNetNancy><rule><parentheses number="0"><field number="1" propertyName="AirDepartureCity" dataType="string" /><operator number="2" type="is" dataType="string" /><value number="3" dataType="string">ATL</value></parentheses><clause number="4" type="and" /><field number="5" propertyName="AirDestinationCity" dataType="string" /><operator number="6" type="is" dataType="string" /><value number="7" dataType="string">VAN</value><clause number="8" type="or" /><parentheses number="9"><field number="10" propertyName="HasAirline" dataType="string" /><operator number="11" type="contains" dataType="string" /><value number="12" dataType="string">DL</value><newline number="13" /><clause number="14" type="and" /><field number="15" propertyName="AirArrivalDate" dataType="date" /><operator number="16" type="lessis" dataType="date" /><value number="17" dataType="date">03/25/2009</value><clause number="18" type="or" /><parentheses number="19"><field number="20" propertyName="AirArrivalTime" dataType="time" /><operator number="21" type="more" dataType="time" /><newline number="22" /><value number="23" dataType="time">3:26:00</value><clause number="24" type="or" /><parentheses number="25"><field number="26" propertyName="AirDepartureDate" dataType="date" /><operator number="27" type="is" dataType="date" /><value number="28" dataType="date">03/10/2009</value><clause number="29" type="and" /><parentheses number="30"><field number="31" propertyName="AirDepartureTime" dataType="time" /><newline number="32" /><operator number="33" type="not" dataType="time" /><value number="34" dataType="time">1:00:00</value><clause number="35" type="and" /><field number="36" propertyName="HasClassOfService" dataType="string" /><operator number="37" type="starts" dataType="string" /><value number="38" dataType="string">First</value><clause number="39" type="or" /><field number="40" propertyName="AirTripLength" dataType="string" /><newline number="41" /><operator number="42" type="not" dataType="string" /><value number="43" dataType="string">3</value></parentheses><clause number="44" type="or" /><field number="45" propertyName="HotelCheckInDate" dataType="date" /><operator number="46" type="more" dataType="date" /><value number="47" dataType="date">03/24/2009</value><clause number="48" type="and" /><newline number="49" /><field number="50" propertyName="HotelPropertyNumber" dataType="string" /><operator number="51" type="nostarts" dataType="string" /><value number="52" dataType="string">HOme</value></parentheses><clause number="53" type="and" /><newline number="54" /><field number="55" propertyName="OperatingFlight.FlightNo" dataType="string" /><operator number="56" type="contains" dataType="string" /><value number="57" dataType="string">123</value><clause number="58" type="and" /><field number="59" propertyName="HasFlightSegments" dataType="bool" /><operator number="60" type="is" dataType="bool" /><value number="61" dataType="bool">true</value><clause number="62" type="or" /><field number="63" propertyName="AirStatusCode" dataType="string" /><newline number="64" /><operator number="65" type="ends" dataType="string" /><value number="66" dataType="string">blah</value></parentheses><clause number="67" type="or" /><field number="68" propertyName="RemarksGovFields" dataType="string" /><operator number="69" type="contains" dataType="string" /><value number="70" dataType="string">blah</value></parentheses><clause number="71" type="and" /><newline number="72" /><field number="73" propertyName="RemarksGovField1" dataType="string" /><operator number="74" type="nocontains" dataType="string" /><value number="75" dataType="string">to blah</value><newline number="76" /><clause number="77" type="then" /><action number="78" methodName="PreAuthorize" customFieldName="Email" customFieldValue="d@d.com" /></rule></DotNetNancy>',
			0,
			'10/19/2009 12:58:08 PM',
			'eaad5439-08ae-4e12-8307-138afd0b6d9f',
			0,
			'10/19/2009 12:58:08 PM',
			'eaad5439-08ae-4e12-8307-138afd0b6d9f')
go

INSERT INTO [GenericDotNetRulesStore].[dbo].[RuleDefinition]
           ([RuleID]
           ,[TypeID]
           ,[ApplicationID]
           ,[RuleName]
           ,[Definition]
           ,[Paused]
           ,[DateCreated]
           ,[CreatedBy]
           ,[Deleted]
           ,[DateUpdated]
           ,[UpdatedBy])
     VALUES
           ('7e158953-b249-46e8-b6db-e1ee5cf19635',
			'9b6df0f2-c552-4e32-a7ec-684613c9985d',
			'3428a138-4738-4324-bd1f-134732b26ce0',
			'PreAuthorize Rule',
			'<DotNetNancy><rule><field number="0" propertyName="AirDepartureCity" dataType="string" /><operator number="1" type="is" dataType="string" /><value number="2" dataType="string">ATL</value><clause number="3" type="then" /><action number="4" methodName="PreAuthorize" customFieldName="Email" customFieldValue="phani@phani.com" /></rule></DotNetNancy>',
			0,
			'10/19/2009 12:58:08 PM',
			'eaad5439-08ae-4e12-8307-138afd0b6d9f',
			0,
			'10/19/2009 12:58:08 PM',
			'eaad5439-08ae-4e12-8307-138afd0b6d9f')
go


INSERT INTO [GenericDotNetRulesStore].[dbo].[RuleDefinition]
           ([RuleID]
           ,[TypeID]
           ,[ApplicationID]
           ,[RuleName]
           ,[Definition]
           ,[Paused]
           ,[DateCreated]
           ,[CreatedBy]
           ,[Deleted]
           ,[DateUpdated]
           ,[UpdatedBy])
     VALUES
           ('9e557ca9-0de7-4486-8001-bd45cdc4d2dc',
			'9b6df0f2-c552-4e32-a7ec-684613c9985d',
			'3428a138-4738-4324-bd1f-134732b26ce0',
			'Air Segments',
			'<DotNetNancy><rule><parentheses number="0"><field number="1" propertyName="HasFlightSegments" dataType="bool" /><operator number="2" type="is" dataType="bool" /><value number="3" dataType="bool">true</value></parentheses><clause number="4" type="then" /><action number="5" methodName="Report" /></rule></DotNetNancy>',
			0,
			'10/19/2009 12:58:08 PM',
			'eaad5439-08ae-4e12-8307-138afd0b6d9f',
			0,
			'10/19/2009 12:58:08 PM',
			'eaad5439-08ae-4e12-8307-138afd0b6d9f')
go

INSERT INTO [GenericDotNetRulesStore].[dbo].[Application_Type]
           ([ApplicationID]
           ,[TypeID])
     VALUES
           ('3428a138-4738-4324-bd1f-134732b26ce0',
			'9b6df0f2-c552-4e32-a7ec-684613c9985d')
go

INSERT INTO [GenericDotNetRulesStore].[dbo].[Application_User]
           ([ApplicationID]
           ,[UserID])
     VALUES
           ('3428a138-4738-4324-bd1f-134732b26ce0',
			'eaad5439-08ae-4e12-8307-138afd0b6d9f')
go

INSERT INTO [GenericDotNetRulesStore].[dbo].[Type_User]
           ([TypeID]
           ,[UserID])
     VALUES
           ('9b6df0f2-c552-4e32-a7ec-684613c9985d',
			'eaad5439-08ae-4e12-8307-138afd0b6d9f')
go




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
    <field propertyName="AirTripLength" displayName="trip length" dataType="numeric" />
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
	Set ConfigurationFile = @XML
	Where ConfigurationTypeID = 1
	and TypeID = '9B6DF0F2-C552-4E32-A7EC-684613C9985D'
	and ApplicationID = '3428A138-4738-4324-BD1F-134732B26CE0'
GO









