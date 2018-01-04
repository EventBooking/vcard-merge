using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CommandLineSwitchParser;
using vCards;

namespace vcard_merge
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

            var all = new List<vCard>();

            var inputFiles = Directory.GetFiles( inputDir );
            foreach (var file in inputFiles)
            {
                var fi = new FileInfo( file );
                if (fi.Extension != ".vcf")
                    continue;
                var content = File.ReadAllText( fi.FullName );
                TextReader tr = new StringReader( content );
                var rdr = new vCardStandardReader();
                var card = rdr.Read( tr );
                all.Add( card );
            }

            using (TextWriter writer = File.CreateText( outputfile ))
            {
                var wr = new vCardStandardWriter();
                foreach (var c in all)
                {
                    wr.Write( c, writer, Encoding.UTF8.WebName );
                }
            }
        }
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