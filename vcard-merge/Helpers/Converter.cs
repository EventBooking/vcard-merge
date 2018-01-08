using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using MixERP.Net.VCards.Models;
using MixERP.Net.VCards.Types;
using vCards;

namespace VCardMerge.Helpers
{
    public static class Converter
    {
        public static string ToString( StringCollection names )
        {
            //var text = string.Join( ',', names );
            var text = new StringBuilder();
            foreach (var x in names)
            {
                text.Append( x );
                text.Append( " " );
            }
            return text.ToString().Trim();
        }

        public static string[] ToArray( StringCollection things )
        {
            return things.Cast<string>().ToArray();
        }

        public static Email ConvertEmail( vCardEmailAddress x )
        {
            var model = new Email
            {
                EmailAddress = x.Address,
                Type = EmailType.Smtp
            };
            return model;
        }

        public static Address ConvertAddress( vCardDeliveryAddress x )
        {
            var model = new Address
            {
                Type = ConvertAddressType( x.AddressType ),
                Street = x.Street,
                Country = x.Country,
                ExtendedAddress = string.Empty,
                Label = string.Empty,
                Locality = x.City,
                PoBox = string.Empty,
                PostalCode = x.PostalCode,
                Region = x.Region,
                Latitude = null,
                Longitude = null,
                Preference = 0,
                TimeZone = TimeZoneInfo.Local
            };
            return model;
        }

        public static Telephone ConvertPhone( vCardPhone x )
        {
            var model = new Telephone
            {
                Type = ConvertPhoneType( x.PhoneType ),
                Number = x.FullNumber
            };
            return model;
        }

        public static TelephoneType ConvertPhoneType( vCardPhoneTypes type )
        {
            switch (type)
            {
                case vCardPhoneTypes.BBS:
                    return TelephoneType.Bbs;

                case vCardPhoneTypes.Car:
                    return TelephoneType.Car;

                case vCardPhoneTypes.Cellular:
                case vCardPhoneTypes.CellularVoice:
                    return TelephoneType.Cell;

                case vCardPhoneTypes.Fax:
                    return TelephoneType.Fax;

                case vCardPhoneTypes.Home:
                    return TelephoneType.Home;

                case vCardPhoneTypes.ISDN:
                    return TelephoneType.Isdn;

                case vCardPhoneTypes.Work:
                case vCardPhoneTypes.WorkVoice:
                    return TelephoneType.Work;

                case vCardPhoneTypes.WorkFax:
                    return TelephoneType.Fax;

                case vCardPhoneTypes.Voice:
                    return TelephoneType.Voice;

                default:
                    return TelephoneType.Work;
            }
        }

        public static AddressType ConvertAddressType( vCardDeliveryAddressTypes type )
        {
            switch (type)
            {
                case vCardDeliveryAddressTypes.Domestic:
                    return AddressType.Domestic;

                case vCardDeliveryAddressTypes.Home:
                    return AddressType.Home;

                case vCardDeliveryAddressTypes.International:
                    return AddressType.International;

                case vCardDeliveryAddressTypes.Parcel:
                    return AddressType.Parcel;

                case vCardDeliveryAddressTypes.Postal:
                    return AddressType.Postal;

                case vCardDeliveryAddressTypes.Work:
                    return AddressType.Work;

                default:
                    return AddressType.Work;
            }
        }

        public static ClassificationType ConvertClass( vCardAccessClassification c )
        {
            switch (c)
            {
                case vCardAccessClassification.Confidential:
                    return ClassificationType.Confidential;

                case vCardAccessClassification.Private:
                    return ClassificationType.Private;

                case vCardAccessClassification.Public:
                    return ClassificationType.Public;

                default:
                    return ClassificationType.Public;
            }
        }

        public static Uri ConvertSource( vCardSourceCollection sources )
        {
            var src = sources.FirstOrDefault();
            return src?.Uri;
        }
    }
}