using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommandLineSwitchParser;
using MixERP.Net.VCards;
using MixERP.Net.VCards.Serializer;
using MixERP.Net.VCards.Types;
using vCards;
using VCardMerge.Helpers;

namespace VCardMerge
{
    public class Program
    {
        public static void Main( string[] args )
        {
            var option = CommandLineSwitch.Parse<Options>( ref args );
            if (string.IsNullOrWhiteSpace( option.InputDir ) ||
                string.IsNullOrWhiteSpace( option.OutputFile ))
            {
                Console.WriteLine( Options.GetHelp() );
                return;
            }

            Merge( option.InputDir, option.OutputFile );
        }

        private static void Merge( string inputDir, string outputfile )
        {
            if (!Directory.Exists( inputDir ))
                throw new ArgumentException( "inputDir" );
            Encoding.RegisterProvider( CodePagesEncodingProvider.Instance );

            var incoming = new List<vCard>();

            var inputFiles = Directory.GetFiles( inputDir );
            foreach (var file in inputFiles)
            {
                var fi = new FileInfo( file );
                if (fi.Extension != ".vcf")
                    continue;
                var content = File.ReadAllText( fi.FullName );
                var tr = new StringReader( content );
                var rdr = new vCardStandardReader();
                var card = rdr.Read( tr );
                incoming.Add( card );
            }

            var outgoing = new List<VCard>();
            foreach (var inc in incoming)
            {
                var card = new VCard
                {
                    Version = VCardVersion.V3,
                    UniqueIdentifier = inc.UniqueId,
                    FirstName = inc.GivenName,
                    LastName = inc.FamilyName,
                    NickName = Converter.ToString( inc.Nicknames ),
                    Prefix = inc.NamePrefix,
                    Suffix = inc.NameSuffix,
                    FormattedName = inc.DisplayName,
                    Organization = inc.Organization,
                    Title = inc.Title,
                    Telephones = inc.Phones.Select( Converter.ConvertPhone ),
                    Addresses = inc.DeliveryAddresses.Select( Converter.ConvertAddress ),
                    Emails = inc.EmailAddresses.Select( Converter.ConvertEmail ),
                    Categories = Converter.ToArray( inc.Categories ),
                    Classification = Converter.ConvertClass( inc.AccessClassification ),
                    Source = Converter.ConvertSource( inc.Sources ),

                    Anniversary = null,
                    BirthDay = null,
                    Longitude = 0,
                    Latitude = 0,
                    MiddleName = string.Empty,
                    TimeZone = null,
                    Photo = null,
                    CalendarAddresses = null,
                    CalendarUserAddresses = null,
                    CustomExtensions = null,
                    DeliveryAddress = null,
                    Gender = Gender.Unknown,
                    Impps = null,
                    Key = string.Empty,
                    Kind = Kind.Individual,
                    Languages = null,
                    LastRevision = null,
                    Logo = null,
                    Mailer = string.Empty,
                    Note = string.Empty,
                    OrganizationalUnit = string.Empty,
                    Relations = null,
                    Role = inc.Role,
                    SortString = string.Empty,
                    Sound = string.Empty
                };
                outgoing.Add( card );
            }

            var serialized = outgoing.Serialize();
            File.WriteAllText( outputfile, serialized );
        }


        //private static void Merge1( string inputDir, string outputfile )
        //{
        //    if (!Directory.Exists( inputDir ))
        //        throw new ArgumentException( "inputDir" );

        //    Encoding.RegisterProvider( CodePagesEncodingProvider.Instance );

        //    var all = new List<vCard>();

        //    var inputFiles = Directory.GetFiles( inputDir );
        //    foreach (var file in inputFiles)
        //    {
        //        var fi = new FileInfo( file );
        //        if (fi.Extension != ".vcf")
        //            continue;
        //        var content = File.ReadAllText( fi.FullName );
        //        var tr = new StringReader( content );
        //        var rdr = new vCardStandardReader();
        //        var card = rdr.Read( tr );
        //        all.Add( card );
        //    }

        //    using (TextWriter writer = File.CreateText( outputfile ))
        //    {
        //        var wr = new vCardStandardWriter();
        //        foreach (var c in all)
        //        {
        //            wr.Write( c, writer, Encoding.UTF8.WebName );
        //        }
        //    }
        //}

        //private static void Merge2( string inputDir, string outputfile )
        //{
        //    if (!Directory.Exists( inputDir ))
        //        throw new ArgumentException( "inputDir" );

        //    var all = new List<VCard>();

        //    var inputFiles = Directory.GetFiles( inputDir );
        //    foreach (var file in inputFiles)
        //    {
        //        var fi = new FileInfo( file );
        //        if (fi.Extension != ".vcf")
        //            continue;
        //        var vcards = Deserializer.Deserialize( fi.FullName );
        //        all.AddRange( vcards );
        //    }

        //    var serialized = all.Serialize();
        //    File.WriteAllText( outputfile, serialized );
        //}
    }

    public class Options
    {
        public string InputDir { get; set; }
        public string OutputFile { get; set; }

        public static string GetHelp()
        {
            return @"-i --inputdir [directory path]
-o --outputfile [file path]
";
        }
    }
}