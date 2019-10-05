namespace HammingCodeCSharp
{
   using System;
   class Program
   {
      static void Main( string[ ] aoArgs )
      {
         bool kbRun = true;
         string    koLine = "";
         string[ ] koArgs;

         MUsage( );

         while( kbRun )
         {
            koLine = Console.ReadLine( );
            koArgs = koLine.Split( new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries );
            
            if( koArgs.Length > 0 )
            {
               switch( koArgs[ 0 ] )
               {
                  case "h":
                  case "ham":
                  {
                     int kiCode = MHam( koArgs[ 1 ][ 0 ] );
                     Console.WriteLine( "Code: " );
                     MPrintCode( kiCode, 11 );
                     Console.WriteLine( );
                     break;
                  }
                  case "d":
                  case "deham":
                  {
                     int kiCode = Convert.ToInt32( koArgs[ 1 ], 2 );
                     char kcChar = MDeham( kiCode );
                     Console.WriteLine( "Character: " + kcChar );
                     break;
                  }
                  case "b":
                  case "burst":
                  {
                     string koOut = MBurst( koLine.Substring( koLine.IndexOf( " " ) + 1 ) );
                     Console.WriteLine( "Received String: " + koOut );
                     break;
                  }
                  default:
                  {
                     MUsage( );
                     break;
                  }
               }
            }
            else
            {
               MUsage( );
            }
         }
      }

      static void MUsage( )
      {
         Console.WriteLine( "Usage:" );
         Console.WriteLine( "   h <c>        Outputs the hamming code for given character, c" );
         Console.WriteLine( "   d <c>        Outputs the character for the given hamming code, c" );
         Console.WriteLine( "   b <s>        Input string, s, is hammed, then de-hammed and printed" );
      }

      static void MPrintCode( int aiCode, int aiBits )
      {
         Console.Write( "   " );
         for( int i = ( aiBits - 1 ); i >= 0; i-- )
         {
            Console.Write( ( aiCode & ( 0x00000001 << i ) ) >> i );
         }
      }

      static int MHam( char acChar )
      {
         int[ ] kiBit = new int[ 11 ];
         int    kiCode = 0;

         kiBit[ 0 ] = ( ( acChar & 0x01 ) == 0 ) ? 0 : 1; // 11
         kiBit[ 1 ] = ( ( acChar & 0x02 ) == 0 ) ? 0 : 1; // 10
         kiBit[ 2 ] = ( ( acChar & 0x04 ) == 0 ) ? 0 : 1; // 9
         kiBit[ 4 ] = ( ( acChar & 0x08 ) == 0 ) ? 0 : 1; // 7
         kiBit[ 5 ] = ( ( acChar & 0x10 ) == 0 ) ? 0 : 1; // 6
         kiBit[ 6 ] = ( ( acChar & 0x20 ) == 0 ) ? 0 : 1; // 5
         kiBit[ 8 ] = ( ( acChar & 0x40 ) == 0 ) ? 0 : 1; // 3
         
         kiBit[  3 ] = kiBit[ 0 ] ^ kiBit[ 1 ] ^ kiBit[ 2 ]; // 8
         kiBit[  7 ] = kiBit[ 4 ] ^ kiBit[ 5 ] ^ kiBit[ 6 ];                           // 4
         kiBit[  9 ] = kiBit[ 0 ] ^ kiBit[ 1 ] ^ kiBit[ 4 ] ^ kiBit[ 5 ] ^ kiBit[ 8 ]; // 2
         kiBit[ 10 ] = kiBit[ 0 ] ^ kiBit[ 2 ] ^ kiBit[ 4 ] ^ kiBit[ 6 ] ^ kiBit[ 8 ]; // 1

         // 1 2 3 4 5 6 7 8 9 A B
         // 0 1 2 3 4 5 6 7 8 9 A
         // C C D C D D D C D D D
         for( int i = 0; i < 11; i++ )
         {
            kiCode |= ( kiBit[ i ] << i );
         }

         return( kiCode );
      }

      static char MDeham( int aiCode )
      {
         char   kcChar  = '\0';
         int[ ] kiBit   = new int[ 11 ];
         int    kiCount = 0;

         for( int i = 0; i < 11; i++ )
         {
            kiBit[ i ] = ( aiCode & ( 0x00000001 << i ) ) >> i;
         }

         if( kiBit[ 10 ] != ( kiBit[ 0 ] ^ kiBit[ 2 ] ^ kiBit[ 4 ] ^ kiBit[ 6 ] ^ kiBit[ 8 ] ) )
         {
            kiCount += 1;
         }

         if( kiBit[ 9 ] != ( kiBit[ 0 ] ^ kiBit[ 1 ] ^ kiBit[ 4 ] ^ kiBit[ 5 ] ^ kiBit[ 8 ] ) )
         {
            kiCount += 2;
         }

         if( kiBit[ 7 ] != ( kiBit[ 4 ] ^ kiBit[ 5 ] ^ kiBit[ 6 ] ) )
         {
            kiCount += 4;
         }

         if( kiBit[ 3 ] != ( kiBit[ 0 ] ^ kiBit[ 1 ] ^ kiBit[ 2 ] ) )
         {
            kiCount += 8;
         }

         // If valid
         if( ( kiCount > 0 ) && ( kiCount < 11 ) )
         {
            kiBit[ 11 - kiCount ] = ( kiBit[ 11 - kiCount ] == 0 ) ? 1 : 0;
         }

         kcChar = ( char )( ( kiBit[ 0 ] << 0 ) | ( kiBit[ 1 ] << 1 ) | ( kiBit[ 2 ] << 2 ) | ( kiBit[ 4 ] << 3 ) |
                            ( kiBit[ 5 ] << 4 ) | ( kiBit[ 6 ] << 5 ) | ( kiBit[ 8 ] << 6 ) );

         return( kcChar );
      }

      static string MBurst( string aoStr )
      {
         string koOut;

         int    kiCode;
         int[ ] kiHam = new int[ aoStr.Length ];
         int    kiMsg;     // One burst message, supports up to 32 character bits
         int[ ] kiRcv = new int[ aoStr.Length ];

         Array.Clear( kiHam, 0, kiRcv.Length );
         Array.Clear( kiRcv, 0, kiRcv.Length );

         // Get hamming code for each character
         Console.WriteLine( "Hamming Code Matrix:" );
         for( int i = 0; i < aoStr.Length; i++ )
         {
            kiHam[ i ] = MHam( aoStr[ i ] );
            MPrintCode( kiHam[ i ], 11 );
            Console.WriteLine( );
         }

         // Transmit one column at a time
         Console.WriteLine( "Transmitting Messages: " );
         for( int kiBit = 0; kiBit < 11; kiBit++ )
         {
            // Build Transmit Message
            kiMsg = 0;
            for( int i = 0; i < kiHam.Length; i++ )
            {
               kiCode = ( kiHam[ i ] & ( 1 << kiBit ) ) >> kiBit;
               kiMsg |= kiCode << i;
            }

            Console.Write( "   Bit {0,2}: ", kiBit );
            MPrintCode( kiMsg, kiHam.Length );
            Console.WriteLine( );

            // Receive Transmit Message
            for( int i = 0; i < kiRcv.Length; i++ )
            {
               kiCode = ( kiMsg & ( 1 << i ) ) >> i;
               kiRcv[ i ] |= kiCode << kiBit;
            }
         }

         Console.WriteLine( "Received Hamming Code Matrix:" );
         for( int i = 0; i < aoStr.Length; i++ )
         {
            MPrintCode( kiRcv[ i ], 11 );
            Console.WriteLine( );
         }

         // Build received message
         koOut = "";
         for( int i = 0; i < kiRcv.Length; i++ )
         {
            koOut += MDeham( kiRcv[ i ] );
         }

         return( koOut );
      }
   }
}
